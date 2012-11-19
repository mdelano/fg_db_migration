using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LogShipperConsole.Model;
using System.Threading;
using System.IO;
using LogShipperConsole.Configuration;

namespace LogShipperConsole.BL
{
    public class RestoreWorker
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool Working { get; set; }
        public bool Recovery { get; private set; }
        public ManagedDatabase ManagedDatabase { get; private set; }
        private List<RestoreItem> RestoreItems { get; set; }
        private LogService _logService { get; set; }

        public RestoreWorker(ManagedDatabase managedDatabase)
        {
            RestoreItems = new List<RestoreItem>();
            ManagedDatabase = managedDatabase;
            _logService = new LogService();
        }

        public void AssignRestoreItem(object restoreItem)
        {
            var _restoreItem = (RestoreItem)restoreItem;
            if (!RestoreItems.Any(x => x.File.FullName == _restoreItem.File.FullName))
            {
                RestoreItems.Add(_restoreItem);
                Log.Info(string.Format("{0} has been queued for restore.", _restoreItem.File.FullName));
            }

            if (!Working)
                ThreadPool.QueueUserWorkItem(WaitForWork, null);
        }

        private void Restore(RestoreItem restoreItem)
        {
            _logService.Restore(restoreItem);
        }

        private void Recover()
        {
            _logService.Recover(ManagedDatabase);
        }

        public void WaitForWork(object o)
        {
            Working = true;
            while (true)
            {
                RestoreItem nextRestoreItem;

                lock (RestoreItems)
                {
                    nextRestoreItem = RestoreItems.OrderBy(x => x.File.CreationTime).FirstOrDefault();
                }

                if (nextRestoreItem != null)
                {
                    Work(nextRestoreItem);
                    RestoreItems.Remove(nextRestoreItem);
                }

                if (Recovery)
                {
                    Recovery = false;
                    Recover();
                    break;
                }

                Thread.Sleep(5000);
            }
            Working = false;
        }

        public void RequestRecovery(object o)
        {
            Recovery = true;
            if (!Working)
                ThreadPool.QueueUserWorkItem(WaitForWork, null);
        }

        private void Work(RestoreItem nextRestoreItem)
        {
            var originalFileName = nextRestoreItem.File.FullName;
            var newFileName = nextRestoreItem.File.FullName + ".logwtch";

            // Attepmt to rename the file in order to see if it is locked by another process
            var processed = false;
            while (!processed)
            {
                try
                {
                    File.Move(originalFileName, newFileName);
                }
                catch (IOException e)
                {
                    processed = false;
                    continue;
                }

                processed = true;
            }

            // The file is not locked. We can work with it now.
            File.Move(newFileName, originalFileName);
            Restore(nextRestoreItem);
        }
    }
}
