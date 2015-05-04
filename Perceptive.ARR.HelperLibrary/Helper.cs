using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using System.Data.SqlClient;

namespace Perceptive.ARR.HelperLibrary
{
    public class Helper
    {
        public static string JoinString(string[] inputs, int indexesToIgnore = -1)
        {
            string output = string.Empty;

            for (int i = 0; i < inputs.Length; i++)
            {
                if (i <= indexesToIgnore)
                    continue;

                output = string.Concat(output, Constants.Separator, inputs[i]);
            }

            if (string.IsNullOrEmpty(output))
                return output;
            else
                return output.Substring(1);
        }

        /// <summary>
        /// Implement Microsoft Enterprise Library Logging.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="category"></param>
        public static void LogMessage(string message, string category)
        {
            if (string.IsNullOrEmpty(category) || category.Equals(Constants.LogCategoryName_Service))
            {
                if (Convert.ToBoolean(AppsettingManager.Instance["ErrorLogActive"]))
                    Logger.Write(message, Constants.LogCategoryName_Service);
            }
            else if(Convert.ToBoolean(ConfigurationManager.AppSettings["ErrorLogActive"]))
                Logger.Write(message, category);
        }

        /// <summary>
        /// Gets the connection string.
        /// </summary>
        /// <param name="activeServer"></param>
        /// <returns></returns>
        public static string GetConnectionString(bool activeServer = true)
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.MultipleActiveResultSets = true;
            builder.Password = ConfigurationManager.AppSettings["Password"];
            builder.UserID = ConfigurationManager.AppSettings["UserId"];
            builder.IntegratedSecurity = string.IsNullOrEmpty(builder.UserID);
            builder.DataSource = ConfigurationManager.AppSettings["ARRServer"];
            builder.InitialCatalog = ConfigurationManager.AppSettings["ARRDatabase"];

            if (!activeServer)
                builder.InitialCatalog += "_Archive";

            return builder.ConnectionString;
        }

        public static User GetUserData(string data)
        {
            User user = new User();
            int index;
            index = data.IndexOf(Constants.UserProfileDelimitor);
            user.UserName = data.Substring(0, index);
            data = data.Substring(index + Constants.UserProfileDelimitor.Length);
            index = data.IndexOf(Constants.UserProfileDelimitor);
            user.Role = (UserRole)Enum.Parse(typeof(UserRole), data.Substring(0, index));
            data = data.Substring(index + Constants.UserProfileDelimitor.Length);
            index = data.IndexOf(Constants.UserProfileDelimitor);
            user.UserId = new Guid(data.Substring(0, index));
            user.Password = data.Substring(index + Constants.UserProfileDelimitor.Length);
            return user;
        }
    }

    public class SupportedLogElementComparer : IComparer<SupportedLogElement>
    {
        public int Compare(SupportedLogElement x, SupportedLogElement y)
        {
            var output = x.ElementType.CompareTo(y.ElementType);
            
            if(output == 0)
            {
                output = x.Code.CompareTo(y.Code);
                if (output == 0)
                {
                    output = x.SystemName.CompareTo(y.SystemName);
                    if (output == 0)
                    {
                        output = x.DisplayName.CompareTo(y.DisplayName);
                    }
                }                
            }
            
            return output;
        }
    }
}
