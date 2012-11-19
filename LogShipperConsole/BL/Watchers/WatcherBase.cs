using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Configuration;
using System.Threading;

namespace LogShipperConsole.BL.Watchers
{
    public class WatcherBase
    {
        protected FileSystemWatcher _watcher;

        public WatcherBase(string watchPath)
        {
            InitializeWatcher(watchPath);
        }

        private void InitializeWatcher(string watchPath)
        {
            _watcher = new FileSystemWatcher();
            _watcher.Path = watchPath;
            _watcher.Changed += new FileSystemEventHandler(OnChanged);
            _watcher.Created += new FileSystemEventHandler(OnCreated);
            _watcher.Deleted += new FileSystemEventHandler(OnDeleted);
            _watcher.Renamed += new RenamedEventHandler(OnRenamed);
        }

        public void Begin(object obj)
        {
            _watcher.EnableRaisingEvents = true;
        }

        public void End(object obj)
        {
            _watcher.EnableRaisingEvents = false;
        }

        // Define the event handlers. 
        protected virtual void OnCreated(object source, FileSystemEventArgs e) { throw new NotImplementedException(); }

        protected virtual void OnChanged(object source, FileSystemEventArgs e) { throw new NotImplementedException(); }

        protected virtual void OnDeleted(object source, FileSystemEventArgs e) { throw new NotImplementedException(); }

        protected virtual void OnRenamed(object source, RenamedEventArgs e) { throw new NotImplementedException(); }
    }
}
