using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Common;
using System.IO;
using LogShipperConsole.Model;
using System.Data.SqlClient;
using LogShipperConsole.Configuration;
using System.Configuration;

namespace LogShipperConsole.BL
{
    public class LogService
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public void NativeRestore(RestoreItem restoreItem)
        {
            Log.Info(string.Format("Restore starting for {0}", restoreItem.File.FullName));

            try
            {
                var sqlConnection = new SqlConnection(restoreItem.ManagedDatabase.ConnectionString);
                var serverConnection = new ServerConnection(sqlConnection);
                Server server = new Server(serverConnection);
                Restore restore = new Restore();
                restore.Action = RestoreActionType.Log;
                restore.Database = restoreItem.ManagedDatabase.Name;
                BackupDeviceItem bkpDevice = new BackupDeviceItem(restoreItem.File.FullName, DeviceType.File);
                restore.Devices.Add(bkpDevice);
                restore.ReplaceDatabase = true;
                restore.NoRecovery = true;
                restore.SqlRestore(server);
            }
            catch (Exception e)
            {
                Log.Error(e);

                return;
            }

            Log.Info(string.Format("Restore completed for {0}", restoreItem.File.FullName));
        }

        public void ScriptedRestore(RestoreItem restoreItem)
        {
            Log.Info(string.Format("Restore starting for {0}", restoreItem.File.FullName));

            var process = new Process();
            try
            {
                //string targetDir;
                //targetDir = string.Format(@"C:\Documents and  Settings\mmeuse\Desktop\CallBatchFile\BatchFile");

                process.StartInfo.UseShellExecute = false;
                //process.StartInfo.WorkingDirectory = targetDir;
                var restoreConfiguration = ConfigurationManager.GetSection("restoreConfiguration") as RestoreConfiguration;
                process.StartInfo.FileName = restoreConfiguration.RestoreScriptName;

                process.StartInfo.Arguments = string.Format("{0} {1} {2} {3}",
                    restoreItem.File.FullName,
                    restoreItem.ManagedDatabase.DecryptionKey,
                    restoreItem.ManagedDatabase.DestinationName,
                    restoreItem.ManagedDatabase.DatabaseInstance);
                process.StartInfo.CreateNoWindow = false;
                process.Start();
                process.WaitForExit();
            }
            catch (Exception e)
            {
                Log.Error(e);
            }

            Log.Info(string.Format("Restore completed for {0}", restoreItem.File.FullName));
        }

        public void Recover(ManagedDatabase managedDatabase)
        {
            Log.Info(string.Format("Recovery starting for {0}", managedDatabase.DestinationName));

            try
            {
                var sqlConnection = new SqlConnection(managedDatabase.ConnectionString);
                var sqlCommand = string.Format("RESTORE DATABASE {0} WITH RECOVERY", managedDatabase.DestinationName);
                SqlCommand command = new SqlCommand(sqlCommand, sqlConnection);
                sqlConnection.Open();
                command.ExecuteNonQuery();
                sqlConnection.Close();
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
                Log.Error(e.StackTrace);

                return;
            }

            Log.Info(string.Format("Recovery completed for {0}", managedDatabase.DestinationName));
        }
    }
}
