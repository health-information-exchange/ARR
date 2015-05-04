using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using System.Timers;
using Perceptive.ARR.HelperLibrary;
using System.Configuration;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
using System.Data.SqlClient;
using System.Data;
using System.Globalization;
using Perceptive.ARR.ProtocolClassLibrary;
using Perceptive.ARR.DataModel;
using Perceptive.IHE.AuditTrail;
using System.Runtime.Serialization;

namespace Perceptive.ARR.RestService
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class RepositoryManager : IRepositoryManager
    {
        # region Public Functions

        /// <summary>
        /// Gets the Audit Logs as per search criteria.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public List<SearchResult> GetAuditLogs(SearchFilter filter)
        {
            List<SearchResult> result = new List<SearchResult>();

            try
            {
                int logsToSkip = (filter.RetrieveNext) ? (filter.LastStartingRowNumber == 0 ? 0 : filter.LastStartingRowNumber + filter.MaxLogsToRetrieve - 1) : filter.LastStartingRowNumber - filter.MaxLogsToRetrieve - 1;
                string protocol = filter.Protocol.ToString();
                using (var entity = new Entities(DatabaseConnector.GetEntityConnectionString(DatabaseType.Active, filter.UserName)))
                {
                    # region LogRecorder Filter

                    var logObjects = entity.LogRecorders.Where(l =>
                        l.IsValid == filter.IsValid &&
                        l.RemoteIP == (string.IsNullOrEmpty(filter.IPAddress) ? l.RemoteIP : filter.IPAddress) &&
                        l.LogType == (filter.Protocol == MessageProtocol.All ? l.LogType : protocol) &&
                        l.LogDateTime >= (filter.LoggedFrom == default(DateTime) ? l.LogDateTime : filter.LoggedFrom) &&
                        l.LogDateTime <= (filter.LoggedTill == default(DateTime) ? l.LogDateTime : filter.LoggedTill)
                        ).ToList();

                    # endregion


                    if (filter.IsValid)
                    {
                        # region Valid Log Filter

                        string sendStartTime = (filter.SentAfter == null || filter.SentAfter == default(DateTime)) ? string.Empty : 
                            filter.SentAfter.ToString("yyyy-MM-ddTHH", CultureInfo.InvariantCulture);

                        string sendEndTime = (filter.SentBefore == null || filter.SentBefore == default(DateTime)) ? string.Empty :
                            filter.SentBefore.ToString("yyyy-MM-ddTHH", CultureInfo.InvariantCulture);
                            
                        logObjects = logObjects.Where(l => 
                            (string.IsNullOrEmpty(filter.HostName) ? true : filter.HostName.Equals(l.ValidLog.HostName)) && 
                            (string.IsNullOrEmpty(filter.AppName) ? true : filter.AppName.Equals(l.ValidLog.AppName)) &&
                            (string.IsNullOrEmpty(sendStartTime) ? true : l.ValidLog.Timestamp.CompareTo(sendStartTime) >= 0) &&
                            (string.IsNullOrEmpty(sendEndTime) ? true : l.ValidLog.Timestamp.CompareTo(sendEndTime) <= 0) && 
                            (string.IsNullOrEmpty(filter.SearchText) ? true : l.ValidLog.Data.Contains(filter.SearchText))
                            ).ToList();

                        # endregion

                        # region Event Identification Filter

                        logObjects = logObjects.Where(l =>
                            (string.IsNullOrEmpty(filter.EventId_Code) ? true : filter.EventId_Code.Equals(l.ValidLog.EventIdentification.ActorElement.Code)) &&
                            (string.IsNullOrEmpty(filter.EventId_CodeSystemName) ? true : filter.EventId_CodeSystemName.Equals(l.ValidLog.EventIdentification.ActorElement.Code)) &&
                            (string.IsNullOrEmpty(filter.EventId_DisplayName) ? true : filter.EventId_DisplayName.Equals(l.ValidLog.EventIdentification.ActorElement.Code)) &&
                            (string.IsNullOrEmpty(filter.EventActionCode) ? true : filter.EventActionCode.Equals(l.ValidLog.EventIdentification.EventActionCode)) &&
                            (string.IsNullOrEmpty(filter.EventOutcomeIndicator) ? true : filter.EventOutcomeIndicator.Equals(l.ValidLog.EventIdentification.EventOutcomeIndicator)) &&
                            (string.IsNullOrEmpty(filter.EventDateTime) ? true : l.ValidLog.EventIdentification.EventDateTime.Contains(filter.EventDateTime)) &&
                            (string.IsNullOrEmpty(filter.EventTypeCode_Code) ? true : (l.ValidLog.EventIdentification.ActorElement1 != null ? l.ValidLog.EventIdentification.ActorElement1.Code.Equals(filter.EventTypeCode_Code) : false)) &&
                            (string.IsNullOrEmpty(filter.EventTypeCode_CodeSystemName) ? true : (l.ValidLog.EventIdentification.ActorElement1 != null ? l.ValidLog.EventIdentification.ActorElement1.CodeSystemName.Equals(filter.EventTypeCode_CodeSystemName) : false)) &&
                            (string.IsNullOrEmpty(filter.EventTypeCode_DisplayName) ? true : (l.ValidLog.EventIdentification.ActorElement1 != null ? l.ValidLog.EventIdentification.ActorElement1.DisplayName.Equals(filter.EventTypeCode_DisplayName) : false))
                           
                            ).ToList();

                        # endregion

                        # region Audit Source Identification Filter

                        logObjects = logObjects.Where(l => 
                            (string.IsNullOrEmpty(filter.AuditSourceId) ? true : filter.AuditSourceId.Equals(l.ValidLog.AuditSourceIdentification.AuditSourceID)) &&
                            (string.IsNullOrEmpty(filter.AuditEnterpriseSiteId) ? true : filter.AuditEnterpriseSiteId.Equals(l.ValidLog.AuditSourceIdentification.AuditEnterpriseSiteID)) &&
                            (string.IsNullOrEmpty(filter.AuditSourceTypeCode) ? true : filter.AuditSourceTypeCode.Equals(l.ValidLog.AuditSourceIdentification.Code)) &&
                            (string.IsNullOrEmpty(filter.AuditSourceCodeSystemName) ? true : filter.AuditSourceCodeSystemName.Equals(l.ValidLog.AuditSourceIdentification.CodeSystemName)) &&
                            (string.IsNullOrEmpty(filter.AuditSourceOriginalText) ? true : filter.AuditSourceOriginalText.Equals(l.ValidLog.AuditSourceIdentification.DisplayName))
                            ).ToList();

                        # endregion

                        # region Active Participant Filter

                        logObjects = logObjects.Where(l => 
                            (string.IsNullOrEmpty(filter.SourceUserId) ? true : 
                                l.ValidLog.ActiveParticipants.FirstOrDefault(a => a.ActorElement.ActorElementTypeID == (int)Perceptive.IHE.AuditTrail.ActorElementType.RoleIDCode && 
                                                                                  (a.ActorElement.DisplayName.Equals("Source") || a.ActorElement.DisplayName.Equals("Application")) &&
                                                                                  a.UserID.Equals(filter.SourceUserId, StringComparison.OrdinalIgnoreCase)) != null) &&
                            (string.IsNullOrEmpty(filter.SourceNetworkAccessPointId) ? true :
                                l.ValidLog.ActiveParticipants.FirstOrDefault(a => a.ActorElement.ActorElementTypeID == (int)Perceptive.IHE.AuditTrail.ActorElementType.RoleIDCode &&
                                                                                  (a.ActorElement.DisplayName.Equals("Source") || a.ActorElement.DisplayName.Equals("Application")) &&
                                                                                  a.NetworkAccesPointID.Equals(filter.SourceNetworkAccessPointId, StringComparison.OrdinalIgnoreCase)) != null) &&
                            (string.IsNullOrEmpty(filter.HumanRequestorUserId) ? true : 
                                l.ValidLog.ActiveParticipants.FirstOrDefault(a => (a.ActorElement == null || (a.ActorElement.ActorElementTypeID == (int)Perceptive.IHE.AuditTrail.ActorElementType.RoleIDCode &&
                                                                                  (a.ActorElement.DisplayName.Equals("Source") || a.ActorElement.DisplayName.Equals("Application") || a.ActorElement.DisplayName.Equals("Destination")) == false)) &&
                                                                                  a.UserID.Equals(filter.HumanRequestorUserId, StringComparison.OrdinalIgnoreCase)) != null) &&
                            (string.IsNullOrEmpty(filter.DestinationUserId) ? true : 
                                l.ValidLog.ActiveParticipants.FirstOrDefault(a => a.ActorElement.ActorElementTypeID == (int)Perceptive.IHE.AuditTrail.ActorElementType.RoleIDCode && 
                                                                                  a.ActorElement.DisplayName.Equals("Destination") &&
                                                                                  a.UserID.Equals(filter.DestinationUserId, StringComparison.OrdinalIgnoreCase)) != null) &&                            
                            (string.IsNullOrEmpty(filter.DestinationNetworkAccessPointId) ? true : 
                                l.ValidLog.ActiveParticipants.FirstOrDefault(a => a.ActorElement.ActorElementTypeID == (int)Perceptive.IHE.AuditTrail.ActorElementType.RoleIDCode &&
                                                                                  a.ActorElement.DisplayName.Equals("Destination") &&
                                                                                  a.NetworkAccesPointID.Equals(filter.DestinationNetworkAccessPointId, StringComparison.OrdinalIgnoreCase)) != null)
                            ).ToList();

                        #endregion

                        # region Participant Object Filter

                        if (!string.IsNullOrEmpty(filter.ParticipantObjectId))
                        {
                            logObjects = logObjects.Where(l =>
                                l.ValidLog.ParticipantObjectIdentifications.FirstOrDefault(p => p.ParticipantObjectID.Equals(filter.ParticipantObjectId, StringComparison.OrdinalIgnoreCase)) != null).ToList();
                        }

                        # endregion
                    }

                    foreach (var obj in logObjects.OrderByDescending(m => m.LogDateTime).Skip(logsToSkip).Take(filter.MaxLogsToRetrieve))
                    {
                        var item = new SearchResult();
                        item.LogId = obj.LogID;
                        item.IsValid = obj.IsValid;
                        item.RemoteIP = obj.RemoteIP;
                        item.LogType = ((MessageProtocol)Enum.Parse(typeof(MessageProtocol), obj.LogType)).ToString();
                        item.DateTime = obj.LogDateTime;
                        if(item.IsValid)
                        {
                            var eventTypeCode = obj.ValidLog.EventIdentification.ActorElement1;

                            item.ActionCode = obj.ValidLog.EventIdentification.EventActionCode;
                            item.IDDescription = obj.ValidLog.EventIdentification.ActorElement.DisplayName;
                            if (eventTypeCode != null)
                                item.EventType = eventTypeCode.DisplayName;
                        }
                        result.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                Helper.LogMessage(ex.ToString(), Constants.LogCategoryName_Service);
            }

            return result;
        }

        /// <summary>
        /// Gets selected audit log data.
        /// </summary>
        /// <param name="logId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public AuditLogData GetAuditLog(string logId, string userId)
        {
            var result = new AuditLogData();
            try
            {
                Guid guidLogId = default(Guid);
                if (!Guid.TryParse(logId, out guidLogId))
                    throw new InvalidDataException("Input logId not in correct Guid format");

                string outputString = string.Empty;

                using (var context = new Entities(DatabaseConnector.GetEntityConnectionString(DatabaseType.Active, userId)))
                {
                    result.IsValid = context.LogRecorders.First(l => l.LogID == guidLogId).IsValid;
                    result.RawMessage = result.IsValid ? context.ValidLogs.First(v => v.LogID == guidLogId).Data : 
                        Encoding.UTF8.GetString(context.InvalidLogs.First(i => i.LogID == guidLogId).Data);                    
                }

                if (result.IsValid)
                {
                    //Try Decoding with RFC 3881 Schema, else with Dicom Format
                    var message = AuditTrail.Deserialize<AuditMessage>(result.RawMessage);
                    if (!string.IsNullOrEmpty(message.Event.EventId.Code))
                        result.Message = new AuditMessageResult(message);
                    else
                    {
                        var dicomMessage = AuditTrail.Deserialize<AuditMessageDicom>(result.RawMessage);
                        result.Message = new AuditMessageResult(dicomMessage);                                             
                    }
                }
            }
            catch (Exception ex)
            {
                Helper.LogMessage(ex.ToString(), Constants.LogCategoryName_Service);                
            }

            return result;
        }

        /// <summary>
        /// Deletes audit logs older than specified days.
        /// </summary>
        /// <param name="days"></param>
        /// <returns></returns>
        public bool DeleteLogs(string days)
        {
            if (SetActiveProcessStatus(string.Format(CultureInfo.InvariantCulture, Constants.ActivityProgress, "Deleting")))
            {
                AppsettingManager.Instance.LogIntoMessageQueue = true;
                Task t = Task.Factory.StartNew(() => DeleteLogs(int.Parse(days)));
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// Starts or stops the scheduler timer.
        /// </summary>
        /// <param name="start"></param>
        public bool StartScheduler(string start)
        {
            return AuditMessageReceiverCacheService.Instance.StartScheduler(start);
        }

        /// <summary>
        /// Gets the scheduler runtime properties.
        /// </summary>
        /// <returns></returns>
        public SchedulerProperty GetScheduler()
        {
            return AuditMessageReceiverCacheService.Instance.GetScheduler();
        }

        /// <summary>
        /// Sets the scheduler runtime properties.
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public bool SetScheduler(SchedulerProperty property)
        {
            return AuditMessageReceiverCacheService.Instance.SetScheduler(property);
        }

        /// <summary>
        /// Sets the supported audit log types.
        /// </summary>
        /// <param name="modifiedList"></param>
        /// <returns></returns>
        public bool SetSupportedActorElementList(List<SupportedLogElement> modifiedList)
        {
            return AuditMessageReceiverCacheService.Instance.SetSupportedActorElementList(modifiedList);
        }

        /// <summary>
        /// Authorizes and authenticates logged in user against known user list.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public UserInfo AuthenticateUser(string userId, string password)
        {
            return new UserInfo()
            {
                username = userId,
                password = password,
                validuser = AuditMessageReceiverCacheService.Instance.AuthenticateUser(userId, password)
            };
        }

        /// <summary>
        /// Authorizes and authenticates logged in user against known user list.
        /// </summary>
        /// <param name="modifiedUser"></param>
        /// <returns></returns>
        public Perceptive.ARR.HelperLibrary.User AuthenticateUser(Perceptive.ARR.HelperLibrary.User modifiedUser)
        {
            return AuditMessageReceiverCacheService.Instance.AuthenticateUser(modifiedUser);
        }

        /// <summary>
        /// Archives audit logs.
        /// </summary>
        /// <param name="days"></param>
        /// <returns></returns>
        public bool ArchiveLogs(string userInitiated)
        {
            if (userInitiated.Equals("1"))
            {
                Task.Factory.StartNew(() => AuditMessageReceiverCacheService.Instance.HandleSchedulerElapsed(true));
                return true;
            }
            else
            {
                if (SetActiveProcessStatus(string.Format(CultureInfo.InvariantCulture, Constants.ActivityProgress, "Archiving")))
                {
                    try
                    {
                        if (ArchiveDatabase())
                            return CleanActiveDatabase();
                        else
                            return false;
                    }
                    catch (Exception ex)
                    {
                        Helper.LogMessage(ex.ToString(), Constants.LogCategoryName_Service);
                        return false;
                    }
                    finally
                    {
                        SetActiveProcessStatus(Constants.NoActiveProgess);
                    }
                }
                else
                    return false;
            }
        }

        /// <summary>
        /// Adds a new log type to the list of supported audit log types.
        /// </summary>
        /// <param name="code"></param>
        /// <param name="displayName"></param>
        /// <returns></returns>
        public bool AddLogType(string code, string displayName)
        {
            return AuditMessageReceiverCacheService.Instance.AddLogType(code, displayName);
        }

        /// <summary>
        /// Deletes a log type from the list of supported audit log types.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteLogType(string id)
        {
            return AuditMessageReceiverCacheService.Instance.DeleteLogType(id);
        }

        /// <summary>
        /// Gets the supported audit log types.
        /// </summary>
        /// <returns></returns>
        public List<SupportedLogElement> GetSupportedActorElementList()
        {
            return AuditMessageReceiverCacheService.Instance.GetSupportedLogElementTypes();
        }

        /// <summary>
        /// Gets the supported audit log types.
        /// </summary>
        /// <returns></returns>
        public List<SupportedLog> GetLogTypes()
        {
            return AuditMessageReceiverCacheService.Instance.GetLogTypes();
        }

        /// <summary>
        /// Returns a shallow copy of the current config settings.
        /// </summary>
        /// <returns></returns>
        public List<AppSettingItem> GetAppSetting()
        {
            return AuditMessageReceiverCacheService.Instance.GetAppSetting();
        }

        /// <summary>
        /// Sets any previously existing setting to its updated value.
        /// </summary>
        /// <param name="modifiedSetting"></param>
        /// <returns></returns>
        public bool SetAppSetting(List<AppSettingItem> modifiedSetting)
        {
            return AuditMessageReceiverCacheService.Instance.SetAppSetting(modifiedSetting);
        }

        /// <summary>
        /// Adds or Edits a user.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool ModifyUser(Perceptive.ARR.HelperLibrary.User user)
        {
            return AuditMessageReceiverCacheService.Instance.ModifyUser(user);
        }

        /// <summary>
        /// Deletes a user.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool DeleteUser(string userId)
        {
            return AuditMessageReceiverCacheService.Instance.DeleteUser(userId);
        }

        /// <summary>
        /// Retrieves all the user credential except the password.
        /// </summary>
        /// <returns></returns>
        public List<Perceptive.ARR.HelperLibrary.User> GetUsers()
        {
            return AuditMessageReceiverCacheService.Instance.GetUsers();
        }

        /// <summary>
        /// Sets current user's Active Database.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="databaseName"></param>
        /// <returns></returns>
        public bool SetUserDatabase(string userId, string databaseName)
        {
            try
            {
                //Delete any previous entry and add a new one, if the value is something other than default.
                using (var configModel = new PerceptiveARR_ConfigEntities(DatabaseConnector.GetEntityConnectionString(DatabaseType.Config)))
                {
                    string deleteQueryString = string.Format(CultureInfo.InvariantCulture, "DELETE FROM [UserActiveDatabase] WHERE [UserName] LIKE '{0}'", userId);
                    configModel.Database.ExecuteSqlCommand(deleteQueryString);

                    if (!databaseName.Equals(ConfigurationManager.AppSettings["ARRDatabase"], StringComparison.OrdinalIgnoreCase))
                    {
                        string serverName = ConfigurationManager.AppSettings["ARRServer"];
                        string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Database", databaseName + ".bak");
                        BackupRestore.RestoreDatabase(serverName, databaseName, filePath);
                        var newEntry = new UserActiveDatabase();
                        newEntry.UserName = userId;
                        newEntry.ActiveDatabase = databaseName;
                        configModel.UserActiveDatabases.Add(newEntry);
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
            finally
            {
                SetActiveProcessStatus(Constants.NoActiveProgess);
            }
        }

        /// <summary>
        /// Gets list of active database and archived backups.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<Db> GetDatabaseList(string userId)
        {
            var list = new List<Db>();
            string activeDatabase;

            try
            {
                var dirInfo = new DirectoryInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Database"));
                var files = dirInfo.GetFiles("*.bak", SearchOption.TopDirectoryOnly);                
                foreach (var f in files.OrderByDescending(p => p.CreationTime))
                {
                    if(f.Name.StartsWith(ConfigurationManager.AppSettings["ARRDatabase"], StringComparison.OrdinalIgnoreCase))
                        continue;

                    list.Add(new Db() { Name = f.Name.Substring(0, f.Name.Length - 4), Type = "Archived" });
                }

                list.Insert(0, new Db() { Name = ConfigurationManager.AppSettings["ARRDatabase"], Type = "Active" });

                using (var configModel = new PerceptiveARR_ConfigEntities(DatabaseConnector.GetEntityConnectionString(DatabaseType.Config)))
                {
                    var entry = configModel.UserActiveDatabases.FirstOrDefault(u => u.UserName.Equals(userId, StringComparison.OrdinalIgnoreCase));
                    if (entry == null)
                        activeDatabase = ConfigurationManager.AppSettings["ARRDatabase"];
                    else
                        activeDatabase = entry.ActiveDatabase;
                }

                list.First(l => l.Name.Equals(activeDatabase, StringComparison.OrdinalIgnoreCase)).IsActive = true;
            }
            catch (Exception ex)
            {
                Helper.LogMessage(ex.ToString(), Constants.LogCategoryName_Service);
            }

            return list;
        }

        /// <summary>
        /// Sets the current log types.
        /// </summary>
        /// <param name="logTypes"></param>
        /// <returns></returns>
        public bool SetLogTypes(List<SupportedLog> logTypes)
        {
            return AuditMessageReceiverCacheService.Instance.SetSupportedLogs(logTypes);
        }

        /// <summary>
        /// Gets the currently opened port numbers.
        /// </summary>
        /// <returns></returns>
        public PortNumber GetPortNumbers()
        {
            var currentSetting = AuditMessageReceiverCacheService.Instance.GetAppSetting();
            var ports = new PortNumber();
            ports.TCP = currentSetting.Find(item => item.Key.Equals("tcpPort", StringComparison.OrdinalIgnoreCase)).Value;
            ports.TLS = currentSetting.Find(item => item.Key.Equals("tlsPort", StringComparison.OrdinalIgnoreCase)).Value;
            ports.UDP = currentSetting.Find(item => item.Key.Equals("udpPort", StringComparison.OrdinalIgnoreCase)).Value;
            return ports;
        }

        /// <summary>
        /// Sets the newly modified port numbers.
        /// </summary>
        /// <param name="ports"></param>
        /// <returns></returns>
        public bool SetPortNumbers(PortNumber ports)
        {
            var modifiedSettings = new List<AppSettingItem>();
            modifiedSettings.Add(new AppSettingItem() { Key = "tcpPort", Value = ports.TCP });
            modifiedSettings.Add(new AppSettingItem() { Key = "tlsPort", Value = ports.TLS });
            modifiedSettings.Add(new AppSettingItem() { Key = "udpPort", Value = ports.UDP });

            return AuditMessageReceiverCacheService.Instance.SetAppSetting(modifiedSettings);
        }

        public List<MenuItem> GetMenu(string userName)
        {
            var menu = AppsettingManager.Instance.GetMenu();

            if (userName == "GuestUser")
                return menu.Where(m => m.Access == UserAccess.General).ToList();
            else
                return menu;
        }

        # endregion

        # region Private Functions

        private bool SetActiveProcessStatus(string status)
        {
            return AuditMessageReceiverCacheService.Instance.SetActiveProcessStatus(status);
        }        

        /// <summary>
        /// Deletes database logs as a part of Delete, Archive or Restore Operation.
        /// </summary>
        /// <param name="days"></param>
        /// <returns></returns>
        private bool DeleteLogs(int days)
        {
            try
            {
                string queryString = string.Format(CultureInfo.InvariantCulture, "DELETE FROM LogRecorder WHERE DATEDIFF(DAY,  [LogDateTime], GETDATE()) >= {0}", days);

                using (var context = new Entities(DatabaseConnector.GetEntityConnectionString(DatabaseType.Active)))
                {
                    context.Database.ExecuteSqlCommand(queryString);
                    context.SaveChanges();
                }

                return true;
            }
            catch (Exception ex)
            {
                Helper.LogMessage(ex.ToString(), Constants.LogCategoryName_Service);
                return false;
            }
            finally
            {
                AppsettingManager.Instance.LogIntoMessageQueue = false;
                SetActiveProcessStatus(Constants.NoActiveProgess);
            }
        }

        /// <summary>
        /// Archives the database.
        /// </summary>
        /// <returns></returns>
        private bool ArchiveDatabase()
        {
            try
            {
                var utcOffset = TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now);
                var utcOffsetValue = ((utcOffset < TimeSpan.Zero) ? "-" : "+") + utcOffset.ToString("hhmm");

                string startDateTime = AuditMessageReceiverCacheService.Instance.GetScheduler().SchedulerStartDateTime.ToString("yyyy-MM-ddTHHmmss") + utcOffsetValue;
                string endDateTime = DateTime.Now.ToString("yyyy-MM-ddTHHmmss") + utcOffsetValue;

                string databaseName = startDateTime + "_" + endDateTime;
                string serverName = ConfigurationManager.AppSettings["ARRServer"];
                string originalDBName = ConfigurationManager.AppSettings["ARRDatabase"];
                string backupFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Database", databaseName + ".bak");

                return BackupRestore.BackupDatabase(serverName, originalDBName, backupFilePath);
            }
            catch (Exception ex)
            {
                Helper.LogMessage(ex.ToString(), Constants.LogCategoryName_Service);
                return false;
            }
        }

        /// <summary>
        /// Deletes data from the active database.
        /// </summary>
        /// <returns></returns>
        private bool CleanActiveDatabase()
        {
            string serverName = ConfigurationManager.AppSettings["ARRServer"];
            string databaseName = ConfigurationManager.AppSettings["ARRDatabase"];
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Database", databaseName + ".bak");

            if (BackupRestore.DeleteDatabase(serverName, databaseName))
                return BackupRestore.RestoreDatabase(serverName, databaseName, filePath);
            else
                return false;
        }

        /// <summary>
        /// Bulk copy operation for Archive and Restore.
        /// </summary>
        /// <param name="days"></param>
        /// <param name="isArchive"></param>
        /// <returns></returns>
        private bool BulkCopy(int days, bool isArchive)
        {
            string sourceParentTable = (isArchive) ? "LogRecorder" : "LogRecorder_Archive";
            string destinationParentTable = (!isArchive) ? "LogRecorder" : "LogRecorder_Archive";
            string sourceChildTable = (isArchive) ? "ValidLogs" : "ValidLogs_Archive";
            string destinationChildTable = (!isArchive) ? "ValidLogs" : "ValidLogs_Archive";
            
            string commandText_LogRecorder = string.Format(CultureInfo.InvariantCulture, 
                "SELECT L.* FROM {0} L INNER JOIN {1} V ON L.LogId = V.LogId WHERE DATEDIFF(DAY,  L.[DateTime], GETDATE()) >= {2}", 
                sourceParentTable, sourceChildTable, days);

            string commandText_ValidLogs = string.Format(CultureInfo.InvariantCulture,
                "SELECT V.* FROM {0} L INNER JOIN {1} V ON L.LogId = V.LogId WHERE DATEDIFF(DAY,  L.[DateTime], GETDATE()) >= {2}", 
                sourceParentTable, sourceChildTable, days);

            SqlConnection conn_Active = null;
            SqlConnection conn_Archive = null;
            IDataReader reader_LogRecorder;
            IDataReader reader_ValidLogs;
            SqlBulkCopy bulkCopy;

            try
            {
                conn_Active = new SqlConnection(Helper.GetConnectionString(isArchive));
                conn_Active.Open();
                conn_Archive = new SqlConnection(Helper.GetConnectionString(!isArchive));
                conn_Archive.Open();

                SqlCommand comm_LogRecorder = new SqlCommand(commandText_LogRecorder, conn_Active);
                reader_LogRecorder = comm_LogRecorder.ExecuteReader();


                SqlCommand comm_ValidLogs = new SqlCommand(commandText_ValidLogs, conn_Active);
                reader_ValidLogs = comm_ValidLogs.ExecuteReader();

                bulkCopy = new SqlBulkCopy(conn_Archive);
                bulkCopy.BatchSize = 1000;
                bulkCopy.BulkCopyTimeout = 0;

                bulkCopy.DestinationTableName = destinationParentTable;
                bulkCopy.WriteToServer(reader_LogRecorder);
                reader_LogRecorder.Close();
                comm_LogRecorder.Dispose();

                bulkCopy.DestinationTableName = destinationChildTable;
                bulkCopy.WriteToServer(reader_ValidLogs);
                reader_ValidLogs.Close();
                comm_ValidLogs.Dispose();

                bulkCopy.Close();

                conn_Active.Close();
                conn_Archive.Close();

                conn_Active = null;
                conn_Archive = null;

                return true;
            }
            catch (Exception ex)
            {
                Helper.LogMessage(ex.ToString(), Constants.LogCategoryName_Service);
                return false;
            }
            finally
            {
                if (conn_Active != null && conn_Active.State == ConnectionState.Open)
                    conn_Active.Close();

                if (conn_Archive != null && conn_Archive.State == ConnectionState.Open)
                    conn_Archive.Close();
            }
        }

        # endregion
    }
}
