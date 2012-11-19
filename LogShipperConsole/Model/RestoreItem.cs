using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using LogShipperConsole.Configuration;

namespace LogShipperConsole.Model
{
    public class RestoreItem
    {
        public FileInfo File { get; set; }
        public bool IsManaged { get; set; }
        public ManagedDatabase ManagedDatabase { get; set; }
    }
}
