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

            s_propRestoreScriptName = new ConfigurationProperty(
                "restoreScriptName",
                typeof(string),
                null,
                ConfigurationPropertyOptions.IsRequired
            );

            s_properties = new ConfigurationPropertyCollection();
            
            s_properties.Add(s_propElements);
            s_properties.Add(s_propRestoreScriptName);
        }

        private static ConfigurationProperty s_propElements;
        private static ConfigurationProperty s_propRestoreScriptName;

        private static ConfigurationPropertyCollection s_properties;

        [ConfigurationProperty("managedDatabases")]
        public ManagedDatabaseElementCollection ManagedDatabases
        {
            get { return (ManagedDatabaseElementCollection)base[s_propElements]; }
        }

        [ConfigurationProperty("restoreScriptName")]
        public string RestoreScriptName
        {
            get { return (string)base[s_propRestoreScriptName]; }
        }

        protected override ConfigurationPropertyCollection Properties
        {
            get { return s_properties; }
        }
    }
}