using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace LogShipperConsole.Configuration
{
    [ConfigurationCollection(typeof(ManagedDatabase),
        CollectionType = ConfigurationElementCollectionType.AddRemoveClearMap)]
    public class ManagedDatabaseElementCollection : ConfigurationElementCollection
    {
        static ManagedDatabaseElementCollection()
        {
            m_properties = new ConfigurationPropertyCollection();
        }

        public ManagedDatabaseElementCollection()
        {
        }

        private static ConfigurationPropertyCollection m_properties;

        protected override ConfigurationPropertyCollection Properties
        {
            get { return m_properties; }
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.AddRemoveClearMap; }
        }

        public ManagedDatabase this[int index]
        {
            get { return (ManagedDatabase)base.BaseGet(index); }
            set
            {
                if (base.BaseGet(index) != null)
                {
                    base.BaseRemoveAt(index);
                }
                base.BaseAdd(index, value);
            }
        }

        public ManagedDatabase this[string name]
        {
            get { return (ManagedDatabase)base.BaseGet(name); }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new ManagedDatabase();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return (element as ManagedDatabase).Name;
        }
    }
}
