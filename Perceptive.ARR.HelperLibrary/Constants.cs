using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Perceptive.ARR.HelperLibrary
{
    public enum MessageProtocol
    {
        All,
        UDP,
        TCP,
        TLS
    }

    public enum UserRole
    {
        Viewer,
        Power,
        Admin,
        Unauthorized,
        Super
    }

    public static class Constants
    {
        public const string Separator = " ";
        public const string NilValue = "-";
        public const int HeaderPartCount = 6;
        public const int MinimumSectionCount = 7;
        public const string PriStart = "<";
        public const string PriEnd = ">";
        public const int StructuredDataPosition = 7;

        public const string UserProfileDelimitor = "!QAZ";
        public const string ServerCertificateThumbPrint = "18a16d69539715df9a7048d9e7eff1e97be75418";

        public static string MethodGet = "GET";
        public static string MethodPost = "POST";

        public const string LogCategoryName_Service = "Service";
        public const string LogCategoryName_Website = "Website";
        public const string NoActiveProgess = "No Server activity is in progress";
        public const string ActivityProgress = "{0} logs";

        public const string AppSetting_KeyColumnName = "Key";
        public const string AppSetting_ValueColumnName = "Value";
        public const string AppSetting_TableName = "AppSettings";
        public const string AppSetting_FileName = "Appsetting.config";
    }
}
