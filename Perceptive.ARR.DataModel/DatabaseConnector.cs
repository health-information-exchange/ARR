using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;
using System.Globalization;
using System.Data.Entity.Core.EntityClient;

namespace Perceptive.ARR.DataModel
{
    public enum DatabaseType
    {
        Active,
        Config
    }

    public class DatabaseConnector
    {
        public static string GetEntityConnectionString(DatabaseType type, string loggedInUser = "service")
        {
            // Set Sql Connection Properties.
            var sqlBuilder = new SqlConnectionStringBuilder();

            sqlBuilder.DataSource = ConfigurationManager.AppSettings["ARRServer"];

            if (type == DatabaseType.Config)
                sqlBuilder.InitialCatalog = ConfigurationManager.AppSettings["ConfigDatabase"];
            else
            {
                bool entryFound = false;
                using (var configModel = new PerceptiveARR_ConfigEntities(DatabaseConnector.GetEntityConnectionString(DatabaseType.Config)))
                {
                    var entry = configModel.UserActiveDatabases.FirstOrDefault(u => u.UserName.Equals(loggedInUser, StringComparison.OrdinalIgnoreCase));
                    if (entry != null)
                    {
                        sqlBuilder.InitialCatalog = entry.ActiveDatabase;
                        entryFound = true;
                    }
                }

                if(!entryFound)
                    sqlBuilder.InitialCatalog = ConfigurationManager.AppSettings["ARRDatabase"];
            }

            sqlBuilder.MultipleActiveResultSets = true;

            string userId = ConfigurationManager.AppSettings["UserId"];
            string password = ConfigurationManager.AppSettings["Password"];

            if (!string.IsNullOrEmpty(userId) && !string.IsNullOrEmpty(password))
            {
                sqlBuilder.IntegratedSecurity = false;
                sqlBuilder.UserID = userId;
                sqlBuilder.Password = password;
            }
            else
                sqlBuilder.IntegratedSecurity = true;

            // Set Entity Connection Properties.
            var entityBuilder = new EntityConnectionStringBuilder();
            entityBuilder.Provider = "System.Data.SqlClient";
            entityBuilder.ProviderConnectionString = sqlBuilder.ToString();
            entityBuilder.Metadata = string.Format(CultureInfo.InvariantCulture, @"res://*/{0}.csdl|res://*/{0}.ssdl|res://*/{0}.msl",
                (type == DatabaseType.Active) ? "RepositoryData" : "RepositoryConfiguration");

            return entityBuilder.ToString();
        }
    }
}
