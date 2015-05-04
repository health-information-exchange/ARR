using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Activation;
using Perceptive.ARR.HelperLibrary;
using System.ServiceModel.Web;

namespace Perceptive.ARR.RestService
{
    [ServiceContract]
    public interface IRepositoryManager
    {
        [WebInvoke(UriTemplate = "GetLogs", Method = "POST", ResponseFormat = WebMessageFormat.Json)]
        List<SearchResult> GetAuditLogs(SearchFilter filter);

        [WebGet(UriTemplate = "GetLogs/{logId}/UserId/{userId}", ResponseFormat = WebMessageFormat.Json)]
        AuditLogData GetAuditLog(string logId, string userId);

        [WebGet(UriTemplate = "DeleteLogs/{days}", ResponseFormat = WebMessageFormat.Json)]
        bool DeleteLogs(string days);

        [WebGet(UriTemplate = "Scheduler/{start}", ResponseFormat = WebMessageFormat.Json)]
        bool StartScheduler(string start);

        [WebGet(UriTemplate = "SupportedActorElement", ResponseFormat = WebMessageFormat.Json)]
        List<SupportedLogElement> GetSupportedActorElementList();

        [WebInvoke(UriTemplate = "SupportedActorElement", Method = "POST", ResponseFormat = WebMessageFormat.Json)]
        bool SetSupportedActorElementList(List<SupportedLogElement> modifiedList);

        [WebGet(UriTemplate = "Scheduler", ResponseFormat = WebMessageFormat.Json)]
        SchedulerProperty GetScheduler();

        [WebInvoke(UriTemplate = "Scheduler", Method = "POST", ResponseFormat = WebMessageFormat.Json)]
        bool SetScheduler(SchedulerProperty property);

        [WebGet(UriTemplate = "AuthenticateUser/{userId}/{password}", ResponseFormat = WebMessageFormat.Json)]
        UserInfo AuthenticateUser(string userId, string password);

        [WebGet(UriTemplate = "ArchiveLogs/{userInitiated}", ResponseFormat = WebMessageFormat.Json)]
        bool ArchiveLogs(string userInitiated);

        [WebGet(UriTemplate = "AddSupportedLogType/Code/{code}/DisplayName/{displayName}", ResponseFormat = WebMessageFormat.Json)]
        bool AddLogType(string code, string displayName);

        [WebGet(UriTemplate = "DeleteSupportedLogType/{id}", ResponseFormat = WebMessageFormat.Json)]
        bool DeleteLogType(string id);

        [WebGet(UriTemplate = "GetSupportedLogTypes", ResponseFormat = WebMessageFormat.Json)]
        List<SupportedLog> GetLogTypes();

        [WebGet(UriTemplate = "GetAppSetting", ResponseFormat = WebMessageFormat.Json)]
        List<AppSettingItem> GetAppSetting();

        [WebInvoke(UriTemplate = "SetAppSetting", Method = "POST", ResponseFormat = WebMessageFormat.Json)]
        bool SetAppSetting(List<AppSettingItem> modifiedSetting);

        [WebInvoke(UriTemplate = "ModifyUser", Method = "POST", ResponseFormat = WebMessageFormat.Json)]
        bool ModifyUser(User user);

        [WebGet(UriTemplate = "DeleteUser/{userId}", ResponseFormat = WebMessageFormat.Json)]
        bool DeleteUser(string userId);

        [WebInvoke(UriTemplate = "GetUsers", Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        List<User> GetUsers();

        [WebGet(UriTemplate = "Databases/{userId}/ActiveDatabase/{databaseName}", ResponseFormat = WebMessageFormat.Json)]
        bool SetUserDatabase(string userId, string databaseName);

        [WebGet(UriTemplate = "Databases/{userId}", ResponseFormat = WebMessageFormat.Json)]
        List<Db> GetDatabaseList(string userId);        

        [WebInvoke(UriTemplate = "SupportedLogTypes", Method = "POST", ResponseFormat = WebMessageFormat.Json)]
        bool SetLogTypes(List<SupportedLog> logTypes);

        [WebGet(UriTemplate = "PortNumbers", ResponseFormat = WebMessageFormat.Json)]
        PortNumber GetPortNumbers();

        [WebInvoke(UriTemplate = "PortNumbers", ResponseFormat = WebMessageFormat.Json, Method = "POST")]
        bool SetPortNumbers(PortNumber ports);

        [WebGet(UriTemplate = "GetMenu/{userName}", ResponseFormat = WebMessageFormat.Json)]
        List<MenuItem> GetMenu(string userName);
    }
}
