using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LogShipperConsole.Model;
using System.Threading;
using LogShipperConsole.BL.Queue;
using LogShipperConsole.BL;


namespace LogShipperConsole.BL
{
    public class RestoreWorkDispatcher
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<RestoreWorker> _restoreWorkers { get; set; }

        public RestoreWorkDispatcher()
        {
            _restoreWorkers = new List<RestoreWorker>();
            RestoreQueue.QueueChange += Dispatch;
        }

        public void Dispatch(object sender, RestoreQueueChangeEventArgs data)
        {
            if (data.RestoreQueueEvent == RestoreQueueEvent.Recover)
            {
                Log.Info("Attempting recovery.");
                DispatchRecovery(sender, data);
            }

            if (data.RestoreQueueEvent == RestoreQueueEvent.Restore)
            {
                Log.Info("Attempting restore.");
                DispatchRestore(sender, data);
            }
        }

        private void DispatchRestore(object sender, RestoreQueueChangeEventArgs data)
        {
            var restoreWorker = GetRestoreWorker(data);

            if (restoreWorker == null)
                return;

            Log.Info("Obtained worker for restore. Queueing work.");
            ThreadPool.QueueUserWorkItem(restoreWorker.AssignRestoreItem, data.RestoreItem);
        }

        private void DispatchRecovery(object sender, RestoreQueueChangeEventArgs data)
        {
            var restoreWorker = GetRestoreWorker(data.RecoveryDatabase);

            if (restoreWorker == null)
                return;

            Log.Info("Obtained worker for recovery. Queueing work.");
            ThreadPool.QueueUserWorkItem(restoreWorker.RequestRecovery, data.RecoveryDatabase);
        }

        private RestoreWorker GetRestoreWorker(RestoreQueueChangeEventArgs data)
        {
            if (data.RestoreItem == null)
            {
                Log.Warn("Cannot get restore worker process going because no restores items have been started");
                return null;
            }

            var restoreWorker = GetRestoreWorker(data.RestoreItem.ManagedDatabase.Name);
            
            if (restoreWorker == null)
            {
                restoreWorker = new RestoreWorker(data.RestoreItem.ManagedDatabase);
                _restoreWorkers.Add(restoreWorker);
            }

            return restoreWorker;
        }

        private RestoreWorker GetRestoreWorker(string databaseName)
        {
            return _restoreWorkers.Where(x => x.ManagedDatabase.Name == databaseName).FirstOrDefault();
        }
    }
}
