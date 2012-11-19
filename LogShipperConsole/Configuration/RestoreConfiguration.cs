using System;
using System.Configuration;

namespace LogShipperConsole.Configuration
{
    public class RestoreConfiguration: ConfigurationSection
    {
        static RestoreConfiguration()
        {
            s_propElements = new ConfigurationProperty(
                "managedDatabases",
                typeof(ManagedDatabaseElementCollection),
                null,
                ConfigurationPropertyOptions.IsRequired
            );

            s_properties = new ConfigurationPropertyCollection();
            
            s_properties.Add(s_propElements);
        }

        private static ConfigurationProperty s_propElements;

        private static ConfigurationPropertyCollection s_properties;

        [ConfigurationProperty("managedDatabases")]
        public ManagedDatabaseElementCollection ManagedDatabases
        {
            get { return (ManagedDatabaseElementCollection)base[s_propElements]; }
        }

        protected override ConfigurationPropertyCollection Properties
        {
            get { return s_properties; }
        }
    }
}