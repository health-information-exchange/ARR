using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Globalization;
using System.Data;
using System.Xml;
using System.Reflection;
using System.IO;
using Perceptive.ARR.DataModel;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Perceptive.ARR.HelperLibrary
{
    public class AppsettingManager
    {
        private static AppsettingManager appSettingInstance;
        private static object lockObj = new object();
        private Dictionary<string, string> configurationValues;
        private ConcurrentQueue<RepositoryRequest> messageQueue;
        private bool logIntoMessageQueue;

        # region Private Methods

        private static void PopulateAppSetting()
        {
            appSettingInstance = new AppsettingManager();
            appSettingInstance.configurationValues = new Dictionary<string, string>();
            using (var configModel = new PerceptiveARR_ConfigEntities(DatabaseConnector.GetEntityConnectionString(DatabaseType.Config)))
            {
                foreach (var settings in configModel.AppSettings)
                {
                    appSettingInstance.configurationValues.Add(settings.Key, settings.Value);
                }
            }
        }
        
        private void SetAppSetting(string key, string value)
        {
            using (var configModel = new PerceptiveARR_ConfigEntities(DatabaseConnector.GetEntityConnectionString(DatabaseType.Config)))
            {
                var setting = configModel.AppSettings.FirstOrDefault(s => s.Key.Equals(key, StringComparison.OrdinalIgnoreCase));
                if (setting != null)
                {
                    setting.Value = value;
                    configModel.SaveChanges();
                }
            }
        }

        private void SendQueuedMessagesToDatabase()
        {
            Task.Factory.StartNew(() =>
                {
                    RepositoryRequest dequeuedRequest;
                    bool result;

                    while (messageQueue.TryDequeue(out dequeuedRequest))
                    {
                        result = new DBLogger().RecordAuditTrail(dequeuedRequest);
                        if(!result)
                            Helper.LogMessage(Encoding.UTF8.GetString(dequeuedRequest.Data), Constants.LogCategoryName_Service);
                    }
                });
        }

        # endregion

        # region Protected Constructor

        protected AppsettingManager()
        {
            configurationValues = new Dictionary<string, string>();
            messageQueue = new ConcurrentQueue<RepositoryRequest>();
        }

        # endregion

        # region Public Methods

        # region Static

        public static AppsettingManager Instance
        {
            get
            {
                if (appSettingInstance == null)
                {
                    lock (lockObj)
                    {
                        if (appSettingInstance == null)
                            PopulateAppSetting();
                    }
                }

                return appSettingInstance;
            }
        }

        public static void ResetAppSetting()
        {
            lock (lockObj)
            {
                try
                {
                    XmlDocument doc = new XmlDocument();
                    string path = Assembly.GetExecutingAssembly().CodeBase.Substring(8);
                    doc.Load(Path.Combine(new FileInfo(path).DirectoryName, Constants.AppSetting_FileName));
                    using (var configModel = new PerceptiveARR_ConfigEntities(DatabaseConnector.GetEntityConnectionString(DatabaseType.Config)))
                    {
                        configModel.Database.ExecuteSqlCommand(string.Format(CultureInfo.InvariantCulture, "TRUNCATE TABLE {0}", Constants.AppSetting_TableName));

                        foreach (XmlNode childNode in doc.FirstChild.ChildNodes)
                        {
                            var appSetting = new AppSetting();
                            appSetting.AppSettingsID = Guid.NewGuid();
                            appSetting.Key = childNode.Attributes["key"].Value;
                            appSetting.Value = childNode.Attributes["value"].Value;

                            configModel.AppSettings.Add(appSetting);
                        }

                        configModel.SaveChanges();
                    }
                    

                    appSettingInstance = null;

                }
                catch (Exception ex)
                {
                    Helper.LogMessage(ex.ToString(), Constants.LogCategoryName_Service);
                }
            }
        }

        # endregion

        # region Non Static

        public string this[string key]
        {
            get
            {
                if (configurationValues.ContainsKey(key))
                    return configurationValues[key];
                else
                    return null;
            }
            set
            {
                if (!configurationValues[key].Equals(value, StringComparison.OrdinalIgnoreCase))
                {
                    configurationValues[key] = value;
                    SetAppSetting(key, value);
                }
            }
        }

        public Dictionary<string, string> GetCurrentSetting()
        {
            return new Dictionary<string, string>(configurationValues);
        }

        public bool LogIntoMessageQueue
        {
            get { return logIntoMessageQueue; }
            set
            {
                logIntoMessageQueue = value;
                if (!value)
                    SendQueuedMessagesToDatabase();
            }
        }

        public void AddToQueue(RepositoryRequest request)
        {
            messageQueue.Enqueue(request);
        }

        public List<MenuItem> GetMenu()
        {
            var menu = new List<MenuItem>();
            menu.Add(new MenuItem() { Name = "Record Viewer", id = 1, Access = UserAccess.General });
            menu.Add(new MenuItem() { Name = "Action Center", id = 2, Access = UserAccess.Admin });
            menu.Add(new MenuItem() { Name = "Supported Logs", id = 3, Access = UserAccess.Admin });
            menu.Add(new MenuItem() { Name = "Port Management", id = 4, Access = UserAccess.Admin });
            menu.Add(new MenuItem() { Name = "Period Selection", id = 5, Access = UserAccess.General });

            return menu;
        }

        # endregion

        # endregion
    }
}
