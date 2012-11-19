using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Configuration;
using System.Threading;
using LogShipperConsole.Configuration;
using LogShipperConsole.Model;
using LogShipperConsole.BL.Queue;

namespace LogShipperConsole.BL.Watchers
{
    public class DirectoryWatcher : WatcherBase
    {
        private ManagedDatabase _managedDatabase { get; set; }
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public DirectoryWatcher(ManagedDatabase managedDatabase)
            : base(managedDatabase.LogDirectory)
        {
            _managedDatabase = managedDatabase;
            _watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite;

            ProcessExisting();
            base.Begin(null);
            Log.Info(string.Format("Log directory watcher bugun at {0}", _managedDatabase.LogDirectory));
        }

        private void ProcessExisting()
        {
            var directoryInfo = new DirectoryInfo(_managedDatabase.LogDirectory);
            var fileInfos = directoryInfo.GetFiles();

            var sortedFileInfos = fileInfos.OrderBy(x => x.CreationTime);

            foreach (var sortedFileInfo in sortedFileInfos)
            {
                Log.Info(string.Format("Found existing file, {0}. Dispatching to restore queue.", sortedFileInfo.FullName));
                ThreadPool.QueueUserWorkItem(RestoreQueue.TryQueue,
                    new RestoreItem { ManagedDatabase = _managedDatabase, File = new FileInfo(sortedFileInfo.FullName), IsManaged = false });
            }
        }

        // Define the event handlers. 
        protected override void OnCreated(object source, FileSystemEventArgs e)
        {
            ThreadPool.QueueUserWorkItem(RestoreQueue.TryQueue, new RestoreItem { ManagedDatabase = _managedDatabase, File = new FileInfo(e.FullPath), IsManaged = false });
        }

        protected override void OnChanged(object source, FileSystemEventArgs e)
        {
            ThreadPool.QueueUserWorkItem(RestoreQueue.TryQueue, new RestoreItem { ManagedDatabase = _managedDatabase, File = new FileInfo(e.FullPath), IsManaged = false });
        }

        protected override void OnDeleted(object source, FileSystemEventArgs e)
        {
            return;
        }

        protected override void OnRenamed(object source, RenamedEventArgs e)
        {
            return;
        }
    }
}
