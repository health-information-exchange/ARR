using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;

namespace Perceptive.ARR.RestService
{
    public class BackupRestore
    {
        static Server srv;
        static ServerConnection conn;

        public static bool BackupDatabase(string serverName, string databaseName, string filePath)
        {
            conn = GetConnection(serverName);
            srv = new Server(conn);

            bool result = false;

            try
            {
                Backup bkp = new Backup();

                bkp.Action = BackupActionType.Database;
                bkp.Database = databaseName;

                bkp.Devices.AddDevice(filePath, DeviceType.File);
                bkp.Incremental = false;

                bkp.SqlBackup(srv);

                result = true;
            }
            catch (SmoException ex)
            {
                throw new SmoException(ex.Message, ex.InnerException);
            }
            catch (IOException ex)
            {
                throw new IOException(ex.Message, ex.InnerException);
            }
            finally
            {
                conn.Disconnect();
                conn = null;
                srv = null;
            }

            return result;
        }

        public static bool RestoreDatabase(string serverName, string databaseName, string filePath)
        {
            conn = GetConnection(serverName);
            srv = new Server(conn);
            bool restoreSuccessful = false;

            try
            {
                if (srv.Databases.Contains(databaseName))
                    return true;

                Database smoDatabase = new Database(srv, databaseName);
                smoDatabase.Create();
                srv.Refresh();

                Restore res = new Restore();

                res.Devices.AddDevice(filePath, DeviceType.File);

                RelocateFile DataFile = new RelocateFile();
                string MDF = res.ReadFileList(srv).Rows[0][1].ToString();
                DataFile.LogicalFileName = res.ReadFileList(srv).Rows[0][0].ToString();
                DataFile.PhysicalFileName = srv.Databases[databaseName].FileGroups[0].Files[0].FileName;

                RelocateFile LogFile = new RelocateFile();
                string LDF = res.ReadFileList(srv).Rows[1][1].ToString();
                LogFile.LogicalFileName = res.ReadFileList(srv).Rows[1][0].ToString();
                LogFile.PhysicalFileName = srv.Databases[databaseName].LogFiles[0].FileName;

                res.RelocateFiles.Add(DataFile);
                res.RelocateFiles.Add(LogFile);

                res.Database = databaseName;
                res.NoRecovery = false;
                res.ReplaceDatabase = true;
                res.SqlRestore(srv);

                restoreSuccessful = true;
            }
            catch (SmoException ex)
            {
                throw new SmoException(ex.Message, ex.InnerException);
            }
            catch (IOException ex)
            {
                throw new IOException(ex.Message, ex.InnerException);
            }
            finally
            {
                conn.Disconnect();
                conn = null;
                srv = null;
            }

            return restoreSuccessful;
        }

        public static Server Getdatabases(string serverName)
        {
            conn = GetConnection(serverName);
            srv = new Server(conn);
            conn.Disconnect();
            return srv;
        }

        public static bool DeleteDatabase(string serverName, string databaseName)
        {
            conn = GetConnection(serverName);
            srv = new Server(conn);
            bool deleteSuccessful;

            try
            {
                srv.KillDatabase(databaseName);
                srv.Refresh();
                deleteSuccessful = true;
            }
            catch (SmoException ex)
            {
                throw new SmoException(ex.Message, ex.InnerException);
            }
            catch (IOException ex)
            {
                throw new IOException(ex.Message, ex.InnerException);
            }
            finally
            {
                conn.Disconnect();
                conn = null;
                srv = null;
            }

            return deleteSuccessful;
        }

        private static ServerConnection GetConnection(string serverName)
        {
            if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["UserId"]))
                return new ServerConnection(serverName);
            else
                return new ServerConnection(serverName, ConfigurationManager.AppSettings["UserId"], ConfigurationManager.AppSettings["Password"]);
        }
    }
}