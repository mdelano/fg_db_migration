using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using LogShipperConsole.Model;

namespace LogShipperConsole.BL.Queue
{
    public static class RestoreQueue
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static string _recoverDatabase;
        public static string RecoverDatabase
        {

            get { return _recoverDatabase; }
            set
            {
                if (_recoverDatabase != value)
                {
                    _recoverDatabase = value;
                    if (_recoverDatabase != string.Empty)
                        OnQueueChange(null, new RestoreQueueChangeEventArgs(RestoreQueueEvent.Recover, _recoverDatabase));
                }
            }
        }

        private static List<RestoreItem> _managedFiles;

        public static List<RestoreItem> QueuedFiles
        {
            get {
                if (_managedFiles == null)
                    _managedFiles = new List<RestoreItem>();

                return _managedFiles;
            }
            private set
            {
                _managedFiles = value;
            }
        }

        public static void TryQueue(object restoreItem)
        {
            var _restoreItem = (RestoreItem)restoreItem;

            lock (QueuedFiles)
            {
                var databaseQueue = QueuedFiles.Where(resItem => resItem.ManagedDatabase.Name == _restoreItem.ManagedDatabase.Name);

                if (databaseQueue == null)
                {
                    QueuedFiles.Add(_restoreItem);
                    OnQueueChange(null, new RestoreQueueChangeEventArgs(RestoreQueueEvent.Restore, _restoreItem));
                    
                }

                if (!databaseQueue.Any(x => x.File.FullName == _restoreItem.File.FullName))
                {
                    QueuedFiles.Add(_restoreItem);
                    OnQueueChange(null, new RestoreQueueChangeEventArgs(RestoreQueueEvent.Restore, _restoreItem));
                }
            }
        }

        public static void TryUnqueue(object restoreItem)
        {
            var _restoreItem = (RestoreItem)restoreItem;

            lock (QueuedFiles)
            {
                if (!QueuedFiles.Any(resItem => resItem.File.FullName == _restoreItem.File.FullName))
                    return;

                var item = QueuedFiles.Select(x => new RestoreItem { ManagedDatabase = x.ManagedDatabase, File = x.File, IsManaged = x.IsManaged }).Where(resItem => resItem.File.FullName == _restoreItem.File.FullName).FirstOrDefault();
                QueuedFiles.RemoveAll(resItem => resItem.File.FullName == _restoreItem.File.FullName);
                //OnQueueChange(item, new RestoreQueueChangeEventArgs(RestoreQueueChangeState.Delete));
            }
        }

        public static List<RestoreItem> RetrieveQueue(string databaseName)
        {
            lock (QueuedFiles)
            {
                return QueuedFiles.Where(restoreItem => restoreItem.ManagedDatabase.Name == databaseName).ToList();
            }
        }

        #region events
        public delegate void PropertyChangeHandler(object sender, RestoreQueueChangeEventArgs data);

        public static event PropertyChangeHandler QueueChange;

        public static void OnQueueChange(object sender, RestoreQueueChangeEventArgs data)
        {
            // Check if there are any Subscribers
            if (QueueChange != null)
            {
                Log.Info(string.Format("Received new queue event. Notifying dispatcher."));
                // Call the Event
                QueueChange(sender, data);
            }
        }


        #endregion
    }
}
