using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Perceptive.IHE.AuditTrail;

namespace Perceptive.ARR.HelperLibrary
{
    [DataContract]
    [KnownType(typeof(SearchFilter))]
    [KnownType(typeof(SyslogMessage))]
    public class SearchFilter
    {
        # region Logged in User Id

        [DataMember]
        public string UserName { get; set; }

        # endregion        

        # region LogRecorder

        [DataMember]
        public string IPAddress { get; set; }

        [DataMember]
        public MessageProtocol Protocol { get; set; }       

        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        public DateTime LoggedFrom { get; set; }

        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        public DateTime LoggedTill { get; set; }

        [DataMember]
        public bool IsValid { get; set; }

        # endregion

        # region Global Search

        [DataMember]
        public string SearchText { get; set; }

        # endregion

        # region ValidLogs

        [DataMember]
        public string HostName { get; set; }

        [DataMember]
        public string AppName { get; set; }

        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        public DateTime SentAfter { get; set; }

        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        public DateTime SentBefore { get; set; }

        # endregion

        # region Dataset count

        [DataMember]
        public int LastStartingRowNumber { get; set; }

        [DataMember]
        public bool RetrieveNext { get; set; }

        [DataMember]
        public int MaxLogsToRetrieve { get; set; }

        # endregion

        # region Event Identification

        # region Event ID

        [DataMember]
        public string EventId_Code { get; set; }

        [DataMember]
        public string EventId_CodeSystemName { get; set; }

        [DataMember]
        public string EventId_DisplayName { get; set; }

        # endregion

        [DataMember]
        public string EventActionCode { get; set; }

        [DataMember]
        public string EventDateTime { get; set; }

        [DataMember]
        public string EventOutcomeIndicator { get; set; }

        # region Event Type Code

        [DataMember]
        public string EventTypeCode_Code { get; set; }

        [DataMember]
        public string EventTypeCode_CodeSystemName { get; set; }

        [DataMember]
        public string EventTypeCode_DisplayName { get; set; }

        # endregion

        # endregion

        # region Audit Source Identification

        [DataMember]
        public string AuditSourceId { get; set; }

        [DataMember]
        public string AuditEnterpriseSiteId { get; set; }

        [DataMember]
        public string AuditSourceTypeCode { get; set; }

        [DataMember]
        public string AuditSourceCodeSystemName { get; set; }

        [DataMember]
        public string AuditSourceOriginalText { get; set; }

        # endregion

        # region Active Participant

        [DataMember]
        public string SourceUserId { get; set; }

        [DataMember]
        public string SourceNetworkAccessPointId { get; set; }

        [DataMember]
        public string HumanRequestorUserId { get; set; }

        [DataMember]
        public string DestinationUserId { get; set; }

        [DataMember]
        public string DestinationNetworkAccessPointId { get; set; }

        # endregion

        # region Participant Object

        [DataMember]
        public string ParticipantObjectId { get; set; }

        # endregion
    }

    [DataContract]
    [KnownType(typeof(SearchResult))]
    [Serializable]
    public class SearchResult
    {
        [DataMember]
        public Guid LogId { get; set; }

        [DataMember]
        public bool IsValid { get; set; }

        [DataMember]
        public string RemoteIP { get; set; }

        [DataMember]
        public DateTime DateTime { get; set; }

        [DataMember]
        public string LogType { get; set; }

        [DataMember]
        public string IDDescription { get; set; }

        [DataMember]
        public string EventType { get; set; }

        [DataMember]
        public string ActionCode { get; set; }
    }

    [DataContract]
    [KnownType(typeof(SchedulerProperty))]
    public class SchedulerProperty
    {
        [DataMember]
        public bool IsRunning { get; set; }

        [DataMember]
        public int ArchiveDays { get; set; }

        [DataMember]
        public string ActiveProcessStatus { get; set; }

        [DataMember]
        public DateTime SchedulerStartDateTime { get; set; }
    }

    [DataContract]
    [KnownType(typeof(User))]
    [KnownType(typeof(UserRole))]
    [Serializable]
    public class User
    {
        [DataMember]
        public Guid UserId { get; set; }

        [DataMember]
        public string UserName { get; set; }

        [DataMember]
        public string Password { get; set; }

        [DataMember]
        public UserRole Role { get; set; }
    }

    [DataContract]
    [KnownType(typeof(SupportedLog))]
    public class SupportedLog
    {
        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public string Code { get; set; }

        [DataMember]
        public string DisplayName { get; set; }

        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        public bool _destroy { get; set; }
    }

    [DataContract]
    [KnownType(typeof(SupportedLogElement))]
    public class SupportedLogElement
    {
        [DataMember]
        public Guid Id { get; set; }

        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        public string Code { get; set; }

        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        public string SystemName { get; set; }

        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        public string DisplayName { get; set; }

        [DataMember(IsRequired = false, EmitDefaultValue = false)]
        public string ElementType { get; set; }

        [DataMember]
        public bool AllowLog { get; set; }
    }

    [DataContract]
    [KnownType(typeof(Db))]
    [Serializable]
    public class Db
    {
        [DataMember]
        public string Name;

        [DataMember]
        public bool IsActive;

        [DataMember]
        public string Type;
    }

    [DataContract]
    [KnownType(typeof(AppSettingItem))]
    [Serializable]
    public class AppSettingItem
    {
        [DataMember]
        public string Key { get; set; }

        [DataMember]
        public string Value { get; set; }
    }

    [DataContract]
    [KnownType(typeof(PortNumber))]
    [Serializable]
    public class PortNumber
    {
        [DataMember]
        public string TCP { get; set; }

        [DataMember]
        public string TLS { get; set; }

        [DataMember]
        public string UDP { get; set; }
    }

    [DataContract]
    [KnownType(typeof(AuditLogData))]
    [KnownType(typeof(AuditMessageDicom))]
    [Serializable]
    public class AuditLogData
    {
        [DataMember]
        public bool IsValid { get; set; }

        [DataMember]
        public string RawMessage { get; set; }

        [DataMember]
        public AuditMessageResult Message { get; set; }
    }

    [DataContract]
    [KnownType(typeof(UserInfo))]
    [Serializable]
    public class UserInfo
    {
        [DataMember]
        public string username { get; set; }

        [DataMember]
        public string password { get; set; }

        [DataMember]
        public bool validuser { get; set; }
    }

    [DataContract]
    [KnownType(typeof(MenuItem))]
    [KnownType(typeof(UserAccess))]
    public class MenuItem
    {
        [DataMember]
        public int id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public UserAccess Access { get; set; }
    }

    public enum UserAccess
    {
        Admin,
        General
    }
}
