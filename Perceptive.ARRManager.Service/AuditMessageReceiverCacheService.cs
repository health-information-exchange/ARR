using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Timers;
using Perceptive.ARR.HelperLibrary;
using Perceptive.ARR.ProtocolClassLibrary;
using System.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ServiceModel.Activation;
using System.ServiceModel;
using Perceptive.ARR.DataModel;
using System.Globalization;
using System.DirectoryServices.AccountManagement;

namespace Perceptive.ARR.RestService
{
    public sealed class AuditMessageReceiverCacheService : INotifyPropertyChanged, IDisposable
    {
        # region Private Variables

        private Timer scheduler;
        private SchedulerProperty schedulerProperty;
        private List<User> users;
        private List<SupportedLog> supportedLogTypes;
        private List<SupportedLogElement> supportedLogElementTypes;
        private object lockObject;

        private TCPClientHandler tcpHandler;
        private UDPClientHandler udpHandler;
        private SSLClientHandler sslHandler;

        public event PropertyChangedEventHandler PropertyChanged;

        # endregion

        # region Constructor

        private AuditMessageReceiverCacheService()
        {
            lockObject = new object();
            AppsettingManager.ResetAppSetting();
            StartListeners(true);
            PopulateUsers();
            PopulateSupportedLogElementTypes();
            PopulateSupportedLogTypes();
            schedulerProperty = new SchedulerProperty();
            schedulerProperty.ArchiveDays = int.Parse(AppsettingManager.Instance["ArchiveDays"]);
            schedulerProperty.IsRunning = bool.Parse(AppsettingManager.Instance["IsSchedulerRunning"]);
            SetActiveProcessStatus(Constants.NoActiveProgess);
            scheduler = new Timer();
            scheduler.Elapsed += new ElapsedEventHandler(scheduler_Elapsed);
            StartScheduler(true);
        }

        # endregion

        # region Private Methods

        /// <summary>
        /// Starts the Archive Scheduler.
        /// </summary>
        private void StartScheduler(bool resetStart)
        {
            
            if (schedulerProperty.IsRunning)
            {
                scheduler.Enabled = false;
                TimeSpan timeBetween = DateTime.Today.AddDays(1) - DateTime.Now;
                scheduler.Interval = timeBetween.TotalMilliseconds;
                if(resetStart)
                    schedulerProperty.SchedulerStartDateTime = DateTime.Now;
                scheduler.Start();
            }
        }

        /// <summary>
        /// Stops the Archive Scheduler.
        /// </summary>
        private void StopScheduler()
        {
            scheduler.Stop();
        }

        /// <summary>
        /// Scheduler Elapsed event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void scheduler_Elapsed(object sender, ElapsedEventArgs e)
        {
            HandleSchedulerElapsed(false);
        }

        /// <summary>
        /// Method called when scheduler is elapsed.
        /// </summary>
        /// <param name="triggeredFromUser"></param>
        public void HandleSchedulerElapsed(bool triggeredFromUser)
        {
            if (triggeredFromUser || schedulerProperty.ArchiveDays <= DateTime.Now.Date.Subtract(schedulerProperty.SchedulerStartDateTime.Date).TotalDays)
            {
                AppsettingManager.Instance.LogIntoMessageQueue = true;
                StopScheduler();
                Task.Factory.StartNew(() => HandleSchedulerJobs()).ContinueWith(t =>
                    {
                        AppsettingManager.Instance.LogIntoMessageQueue = false;
                        StartScheduler(true);
                    });
            }
            else
            {
                StopScheduler();
                StartScheduler(false);
            }
        }

        /// <summary>
        /// Handles different tasks oncer the timer elapse event fires.
        /// </summary>
        private void HandleSchedulerJobs()
        {
            if (schedulerProperty.ActiveProcessStatus == Constants.NoActiveProgess)
            {
                ProcessJob("ArchiveLogs", 0);
                PropertyChanged += new PropertyChangedEventHandler(AuditMessageReceiverCacheService_PropertyChanged);

                return;
            }
        }

        /// <summary>
        /// Processes specific task.
        /// </summary>
        /// <param name="task"></param>
        /// <param name="days"></param>
        /// <returns></returns>
        private bool ProcessJob(string task, int days)
        {
            var utility = new RestServiceUtility();
            bool result = utility.ProcessRequest<bool>(new WebRequestInput()
            {
                Method = Constants.MethodGet,
                Uri = RestServiceUtility.ProcessLogsUrl(task, days)
            });
            return result;
        }

        /// <summary>
        /// Used to perform scheduler export after scheduler archive is done.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AuditMessageReceiverCacheService_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            PropertyChanged -= new PropertyChangedEventHandler(AuditMessageReceiverCacheService_PropertyChanged);
        }

        private void PopulateSupportedLogElementTypes()
        {
            supportedLogElementTypes = new List<SupportedLogElement>();
            try
            {

                using (var configModel = new PerceptiveARR_ConfigEntities(DatabaseConnector.GetEntityConnectionString(DatabaseType.Config)))
                {
                    foreach (var supportedType in configModel.SupportedActorElements)
                    {
                        supportedLogElementTypes.Add(new SupportedLogElement()
                        {
                            Id = supportedType.ActorElementId,
                            Code = supportedType.Code,
                            SystemName = supportedType.CodeSystemName,
                            DisplayName = supportedType.CodeDisplayName,
                            ElementType = supportedType.UsedAt,
                            AllowLog = supportedType.AllowLog
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Helper.LogMessage(ex.ToString(), Constants.LogCategoryName_Service);
            }

            var comparer = new SupportedLogElementComparer();
            supportedLogElementTypes.Sort(comparer);
        }

        /// <summary>
        /// Populates supported log types from the database.
        /// </summary>
        private void PopulateSupportedLogTypes()
        {
            supportedLogTypes = new List<SupportedLog>();
            try
            {

                using (var configModel = new PerceptiveARR_ConfigEntities(DatabaseConnector.GetEntityConnectionString(DatabaseType.Config)))
                {
                    foreach (var supportedType in configModel.SupportedEventTypes)
                    {
                        supportedLogTypes.Add(new SupportedLog()
                        {
                            Id = supportedType.SupportedEventTypeID.ToString(),
                            Code = supportedType.EventTypeCode,
                            DisplayName = supportedType.EventTypeDisplayName
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Helper.LogMessage(ex.ToString(), Constants.LogCategoryName_Service);
            }
        }

        /// <summary>
        /// Populates website users from the database.
        /// </summary>
        private void PopulateUsers()
        {
            users = new List<User>();
            try
            {
                using (var configModel = new PerceptiveARR_ConfigEntities(DatabaseConnector.GetEntityConnectionString(DatabaseType.Config)))
                {
                    foreach (var clientUser in configModel.ClientUsers)
                    {
                        var user = new User()
                        {
                            UserId = clientUser.UserID,
                            UserName = clientUser.UserName,
                            Role = (UserRole)Enum.ToObject(typeof(UserRole), clientUser.UserRole),
                            Password = clientUser.Password

                        };
                        users.Add(user);
                    }
                }                
            }
            catch (Exception ex)
            {
                Helper.LogMessage(ex.ToString(), Constants.LogCategoryName_Service);
            }
        }        

        /// <summary>
        /// Starts tcp tls and udp listeneres at specified ports.
        /// </summary>
        /// <param name="serviceStart"></param>
        private void StartListeners(bool serviceStart)
        {
            if (serviceStart)
            {
                tcpHandler = new TCPClientHandler(Convert.ToInt32(AppsettingManager.Instance["tcpPort"]));
                sslHandler = new SSLClientHandler(Convert.ToInt32(AppsettingManager.Instance["tlsPort"]), AppsettingManager.Instance["ServerCertificateThumbPrint"]);
                udpHandler = new UDPClientHandler(Convert.ToInt32(AppsettingManager.Instance["udpPort"]));
            }
            else
            {
                if (tcpHandler.Port != Convert.ToInt32(AppsettingManager.Instance["tcpPort"]))
                {
                    tcpHandler.Dispose();
                    tcpHandler = new TCPClientHandler(Convert.ToInt32(AppsettingManager.Instance["tcpPort"]));
                }

                if (sslHandler.Port != Convert.ToInt32(AppsettingManager.Instance["tlsPort"]))
                {
                    sslHandler.Dispose();
                    sslHandler = new SSLClientHandler(Convert.ToInt32(AppsettingManager.Instance["tlsPort"]), AppsettingManager.Instance["ServerCertificateThumbPrint"]);
                }

                if (udpHandler.Port != Convert.ToInt32(AppsettingManager.Instance["udpPort"]))
                {
                    udpHandler.Dispose();
                    udpHandler = new UDPClientHandler(Convert.ToInt32(AppsettingManager.Instance["udpPort"]));
                }
            }
        }

        # endregion

        # region Public Methods

        public void Start()
        {
            //Do nothing, already intitialized.
        }

        public void Dispose()
        {
            tcpHandler.Dispose();
            sslHandler.Dispose();
            udpHandler.Dispose();
            scheduler.Dispose();
        }

        public bool SetActiveProcessStatus(string status)
        {
            lock (lockObject)
            {
                if(string.IsNullOrEmpty(schedulerProperty.ActiveProcessStatus) || 
                    (schedulerProperty.ActiveProcessStatus.Equals(Constants.NoActiveProgess) && status != Constants.NoActiveProgess) ||
                    (!schedulerProperty.ActiveProcessStatus.Equals(Constants.NoActiveProgess) && status == Constants.NoActiveProgess))
                {
                    schedulerProperty.ActiveProcessStatus = status;
                    if (this.PropertyChanged != null)
                        this.PropertyChanged(this, new PropertyChangedEventArgs(status));
                    
                    return true;
                }
            }
            return false;
        }

        public bool StartScheduler(string start)
        {
            lock (lockObject)
            {
                bool toRunService = start == "1";
                if (toRunService && !scheduler.Enabled)
                    StartScheduler(true);
                else if (!toRunService && scheduler.Enabled)
                    StopScheduler();
            }

            return true;
        }

        public SchedulerProperty GetScheduler()
        {
            lock (lockObject)
            {
                return schedulerProperty;
            }
        }

        public bool SetScheduler(SchedulerProperty property)
        {
            lock (lockObject)
            {
                if (schedulerProperty.ActiveProcessStatus == Constants.NoActiveProgess)
                {
                    schedulerProperty.ArchiveDays = property.ArchiveDays;
                    schedulerProperty.IsRunning = property.IsRunning;
                    StartScheduler(schedulerProperty.IsRunning ? "1" : "0");
                    return true;
                }
                else
                    return false;
            }
        }

        public bool SetSupportedActorElementList(List<SupportedLogElement> modifiedList)
        {
            lock (lockObject)
            {
                try
                {
                    using (var configModel = new PerceptiveARR_ConfigEntities(DatabaseConnector.GetEntityConnectionString(DatabaseType.Config)))
                    {   
                        foreach (var element in modifiedList)
                        {
                            var serverElement = configModel.SupportedActorElements.SingleOrDefault(s => s.ActorElementId.Equals(element.Id));
                            if (serverElement != null)
                                serverElement.AllowLog = element.AllowLog;
                        }

                        configModel.SaveChanges();
                    }

                    PopulateSupportedLogElementTypes();

                    return true;
                }
                catch (Exception ex)
                {
                    Helper.LogMessage(ex.ToString(), Constants.LogCategoryName_Service);
                    return false;
                }
            }
        }

        public bool SetSupportedLogs(List<SupportedLog> logTypes)
        {
            lock (lockObject)
            {
                try
                {
                    using (var configModel = new PerceptiveARR_ConfigEntities(DatabaseConnector.GetEntityConnectionString(DatabaseType.Config)))
                    {
                        foreach (var type in logTypes)
                        {
                            var currentMemoryObj = supportedLogTypes.FirstOrDefault(s => s.Id.Equals(type.Id, StringComparison.OrdinalIgnoreCase));                            

                            if (currentMemoryObj == null)
                            {
                                if (!type._destroy)
                                {
                                    //newly added object
                                    Guid id = Guid.NewGuid();

                                    configModel.SupportedEventTypes.Add(
                                        new SupportedEventType()
                                        {
                                            SupportedEventTypeID = id,
                                            EventTypeCode = type.Code,
                                            EventTypeDisplayName = type.DisplayName
                                        });

                                    supportedLogTypes.Add(new SupportedLog() { Id = id.ToString(), Code = type.Code, DisplayName = type.DisplayName });
                                }
                            }
                            else
                            {
                                //Check if deleted
                                if (type._destroy)
                                {
                                    string queryString = string.Format(CultureInfo.InvariantCulture, 
                                        "DELETE FROM [SupportedEventType] WHERE [SupportedEventTypeID] like '{0}'", type.Id);

                                    configModel.Database.ExecuteSqlCommand(queryString);

                                    supportedLogTypes.Remove(currentMemoryObj);
                                }
                                else if (!currentMemoryObj.DisplayName.Equals(type.DisplayName, StringComparison.OrdinalIgnoreCase) || 
                                    !currentMemoryObj.Code.Equals(type.Code, StringComparison.OrdinalIgnoreCase))
                                {
                                    //Check if updated
                                    var currentDbObj = configModel.SupportedEventTypes.First(s => s.SupportedEventTypeID.ToString().Equals(type.Id,
                                    StringComparison.OrdinalIgnoreCase));

                                    currentMemoryObj.Code = currentDbObj.EventTypeCode = type.Code;
                                    currentMemoryObj.DisplayName = currentDbObj.EventTypeDisplayName = type.DisplayName;
                                }
                            }
                        }

                        configModel.SaveChanges();
                    }

                    return true;
                }
                catch (Exception ex)
                {
                    Helper.LogMessage(ex.ToString(), Constants.LogCategoryName_Service);
                    return false;
                }
            }
        }

        public bool AuthenticateUser(string userId, string password)
        {
            using(var context = new PrincipalContext(ContextType.Machine))
            {
                return context.ValidateCredentials(userId, password, ContextOptions.Negotiate);
            }
        }

        public User AuthenticateUser(User modifiedUser)
        {
            lock (lockObject)
            {
                var user = users.Find(u => u.UserName.Equals(modifiedUser.UserName) && u.Password.Equals(modifiedUser.Password));
                if (user == null)
                    return new User() { Role = UserRole.Unauthorized };
                else
                    return user;
            }
        }

        public bool AddLogType(string code, string displayName)
        {
            try
            {
                lock (lockObject)
                {
                    using (var configModel = new PerceptiveARR_ConfigEntities(DatabaseConnector.GetEntityConnectionString(DatabaseType.Config)))
                    {
                        //newly added object
                        Guid id = Guid.NewGuid();

                        configModel.SupportedEventTypes.Add(
                            new SupportedEventType()
                            {
                                SupportedEventTypeID = id,
                                EventTypeCode = code,
                                EventTypeDisplayName = displayName
                            });

                        supportedLogTypes.Add(new SupportedLog() { Id = id.ToString(), Code = code, DisplayName = displayName });

                        configModel.SaveChanges();
                    }
                    
                    return true;
                }
            }
            catch (Exception ex)
            {
                Helper.LogMessage(ex.ToString(), Constants.LogCategoryName_Service);
                return false;
            }
        }

        public bool DeleteLogType(string id)
        {
            try
            {
                lock (lockObject)
                {
                    using (var configModel = new PerceptiveARR_ConfigEntities(DatabaseConnector.GetEntityConnectionString(DatabaseType.Config)))
                    {
                        string queryString = string.Format(CultureInfo.InvariantCulture,
                                        "DELETE FROM [SupportedEventType] WHERE [SupportedEventTypeID] like '{0}'", id);

                        configModel.Database.ExecuteSqlCommand(queryString);
                        var item = supportedLogTypes.FirstOrDefault(s => s.Id.Equals(id));
                        if(item != null)
                            supportedLogTypes.Remove(item);
                        configModel.SaveChanges();
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                Helper.LogMessage(ex.ToString(), Constants.LogCategoryName_Service);
                return false;
            }
        }

        public List<SupportedLog> GetLogTypes()
        {
            lock (lockObject)
            {
                return supportedLogTypes;
            }
        }

        public List<SupportedLogElement> GetSupportedLogElementTypes()
        {
            lock(lockObject)
            {
                return supportedLogElementTypes;
            }
        }

        public List<AppSettingItem> GetAppSetting()
        {
            lock (lockObject)
            {
                List<AppSettingItem> items = new List<AppSettingItem>();
                foreach (KeyValuePair<string, string> pair in AppsettingManager.Instance.GetCurrentSetting())
                    items.Add(new AppSettingItem() { Key = pair.Key, Value = pair.Value });

                return items;
            }
        }

        public bool SetAppSetting(List<AppSettingItem> modifiedSetting)
        {
            try
            {
                lock (lockObject)
                {
                    foreach (AppSettingItem item in modifiedSetting)
                        AppsettingManager.Instance[item.Key] = item.Value;
                    StartListeners(false);
                }
                return true;
            }
            catch (Exception ex)
            {
                Helper.LogMessage(ex.ToString(), Constants.LogCategoryName_Service);
                return false;
            }
        }

        public bool ModifyUser(User user)
        {
            try
            {
                lock (lockObject)
                {
                    string commandText;
                    if (user.UserId == default(Guid))
                    {
                        //Check if user exists with same name and password
                        if (users.Exists(u => u.UserName == user.UserName && u.Password == user.Password))
                        {
                            Helper.LogMessage("User already exists with same name and password", Constants.LogCategoryName_Service);
                            return false;
                        }

                        commandText = "INSERT INTO dbo.[User] (UserId, UserName, UserRole, Password) VALUES (@UserId, @UserName, @UserRole, @Password);";
                        user.UserId = Guid.NewGuid();
                        users.Add(user);
                    }
                    else
                    {
                        commandText = "UPDATE dbo.[User] SET UserName = @UserName, UserRole = @UserRole, Password = @Password WHERE UserId = @UserId;";

                        var cacheUser = users.First(u => u.UserId == user.UserId);

                        //Required if admin changes rights. And admin is not expected to know the password once modified by the actual user.
                        if (string.IsNullOrEmpty(user.Password))
                            user.Password = cacheUser.Password;

                        cacheUser.UserName = user.UserName;
                        cacheUser.Password = user.Password;
                        cacheUser.Role = user.Role;
                    }

                    //Perform the DB Operation.
                    using (SqlConnection conn = new SqlConnection(Helper.GetConnectionString()))
                    {
                        conn.Open();
                        using (IDbCommand comm = new SqlCommand(commandText, conn))
                        {
                            comm.Parameters.Add(new SqlParameter("@UserName", user.UserName));
                            comm.Parameters.Add(new SqlParameter("@UserRole", (int)user.Role));
                            comm.Parameters.Add(new SqlParameter("@Password", user.Password));
                            comm.Parameters.Add(new SqlParameter("@UserId", user.UserId));

                            comm.ExecuteNonQuery();
                        }
                        conn.Close();
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                Helper.LogMessage(ex.ToString(), Constants.LogCategoryName_Service);
                return false;
            }
        }

        public bool DeleteUser(string userId)
        {
            try
            {
                lock (lockObject)
                {
                    string commandText = "DELETE FROM dbo.[User] WHERE UserId = @UserId;";

                    users.RemoveAll(u => u.UserId == new Guid(userId));

                    //Perform the DB Operation.
                    using (SqlConnection conn = new SqlConnection(Helper.GetConnectionString()))
                    {
                        conn.Open();
                        using (IDbCommand comm = new SqlCommand(commandText, conn))
                        {
                            comm.Parameters.Add(new SqlParameter("@UserId", new Guid(userId)));
                            comm.ExecuteNonQuery();
                        }
                        conn.Close();
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                Helper.LogMessage(ex.ToString(), Constants.LogCategoryName_Service);
                return false;
            }
        }

        public List<User> GetUsers()
        {
            lock (lockObject)
            {
                List<User> usersToSend = new List<User>();
                foreach (var u in users)
                    usersToSend.Add(new User() { Role = u.Role, UserId = u.UserId, UserName = u.UserName });

                return usersToSend;
            }
        }

        # endregion

        public static AuditMessageReceiverCacheService Instance
        {
            get { return AuditMessageReceiverCacheManager.instance; }
        }

        private class AuditMessageReceiverCacheManager
        {
            static AuditMessageReceiverCacheManager() { }
            internal static readonly AuditMessageReceiverCacheService instance = new AuditMessageReceiverCacheService();
        }
    }
}