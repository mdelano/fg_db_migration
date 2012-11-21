using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Configuration;
using LogShipperConsole.BL.Watchers;
using LogShipperConsole.Configuration;
using System.Threading;
using LogShipperConsole.BL;
using LogShipperConsole.BL.Queue;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace LogShipperConsole
{
    class Program
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static void Main()
        {
            Log.Info("Beginning Log Shipper Console");
            Run();
        }

        public static void Run()
        {
            SetThreadPool();

            DispatchWatchers();

            ListenForCommand();
        }

        // Leaves the command prompt ready for input
        private static void ListenForCommand()
        {
            Console.WriteLine("Press \'quit!\' to quit the console.");

            var command = Console.ReadLine();

            while(command != "quit!")
            {
                if (command == "Recover")
                {
                    NotifyRecovery();
                    command = "";
                    continue;
                }

                if (command != "")
                    Console.WriteLine("UNRECOGNIZED COMMAND");

                command = Console.ReadLine();
            }
        }

        // On user instruction, request the recovery of a database
        private static void NotifyRecovery()
        {
            Console.WriteLine("Which database would you like to recover?");
            var databaseName = Console.ReadLine();
            RestoreQueue.RecoverDatabase = databaseName;
        }

        private static void SetThreadPool()
        {
            var workerThreadsMax = Int32.Parse(ConfigurationManager.AppSettings[CommonConstants.WorkerThreadsMax]);
            var completionPortThreadsMax = Int32.Parse(ConfigurationManager.AppSettings[CommonConstants.CompletionPortThreadsMax]);
            ThreadPool.SetMaxThreads(workerThreadsMax, completionPortThreadsMax);
        }

        // Begin the file system watchers on the configured database log directories
        private static void DispatchWatchers()
        {
            Log.Info("Fetching configuration for restore databases.");
            var restoreConfiguration = ConfigurationManager.GetSection("restoreConfiguration") as RestoreConfiguration;
            Log.Info(string.Format("Found {0} configuration(s)", restoreConfiguration.ManagedDatabases.Count));

            var restoreCoordinator = new RestoreWorkDispatcher();
            foreach (var managedDatabaseConfiguration in restoreConfiguration.ManagedDatabases)
            {
                ThreadPool.QueueUserWorkItem(Watch, managedDatabaseConfiguration);
            }
        }

        private static void Watch(object managedDatabaseConfiguration)
        {
            var managedDatabase = (ManagedDatabase)managedDatabaseConfiguration;
            Log.Info(string.Format("Dispatched log directory watcher for database {0} at {1} .", managedDatabase.Name, managedDatabase.LogDirectory));
            var directoryWatcher = new DirectoryWatcher(managedDatabase);
        }
    }
}
