using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LogShipperConsole.Model
{
    public class RestoreQueueChangeEventArgs : EventArgs
    {
        public string RecoveryDatabase { get; internal set; }
        public RestoreQueueEvent RestoreQueueEvent { get; internal set; }
        public RestoreItem RestoreItem { get; internal set; }

        public RestoreQueueChangeEventArgs(RestoreQueueEvent restoreQueueEvent, string databaseName)
        {
            this.RestoreQueueEvent = restoreQueueEvent;
            this.RecoveryDatabase = databaseName;
        }

        public RestoreQueueChangeEventArgs(RestoreQueueEvent restoreQueueEvent, RestoreItem restoreItem)
        {
            this.RestoreQueueEvent = restoreQueueEvent;
            this.RestoreItem = restoreItem;
        }
    }
}
