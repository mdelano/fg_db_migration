﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace LogShipperConsole.Configuration
{
    public class ManagedDatabase : ConfigurationElement
    {
        static ManagedDatabase()
        {
            s_propName = new ConfigurationProperty(
                "name",
                typeof(string),
                null,
                ConfigurationPropertyOptions.IsRequired
            );

            s_propLogDirectory = new ConfigurationProperty(
                "logDirectory",
                typeof(string),
                "",
                ConfigurationPropertyOptions.None
            );

            s_propFullDirectory = new ConfigurationProperty(
                "fullDirectory",
                typeof(string),
                "",
                ConfigurationPropertyOptions.None
            );

            s_propIsActive = new ConfigurationProperty(
                "isActive",
                typeof(bool),
                "true",
                ConfigurationPropertyOptions.None
            );

            s_propDestinationName = new ConfigurationProperty(
                "destinationName",
                typeof(string),
                "",
                ConfigurationPropertyOptions.None
            );

            s_propServerName = new ConfigurationProperty(
                "serverName",
                typeof(string),
                "",
                ConfigurationPropertyOptions.None
            );

            s_propConnectionString = new ConfigurationProperty(
                "connectionString",
                typeof(string),
                "",
                ConfigurationPropertyOptions.None
            );

            s_properties = new ConfigurationPropertyCollection();

            s_properties.Add(s_propName);
            s_properties.Add(s_propLogDirectory);
            s_properties.Add(s_propFullDirectory);
            s_properties.Add(s_propIsActive);
            s_properties.Add(s_propDestinationName);
            s_properties.Add(s_propServerName);
            s_properties.Add(s_propConnectionString);
        }

        private static ConfigurationProperty s_propName;
        private static ConfigurationProperty s_propLogDirectory;
        private static ConfigurationProperty s_propIsActive;
        private static ConfigurationProperty s_propFullDirectory;
        private static ConfigurationProperty s_propDestinationName;
        private static ConfigurationProperty s_propServerName;
        private static ConfigurationProperty s_propConnectionString;
        private static ConfigurationPropertyCollection s_properties;

        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get { return (string)base[s_propName]; }
        }

        [ConfigurationProperty("logDirectory")]
        public string LogDirectory
        {
            get { return (string)base[s_propLogDirectory]; }
        }

        [ConfigurationProperty("fullDirectory")]
        public string FullDirectory
        {
            get { return (string)base[s_propFullDirectory]; }
        }

        [ConfigurationProperty("destinationName")]
        public string DestinationName
        {
            get { return (string)base[s_propDestinationName]; }
        }

        [ConfigurationProperty("serverName")]
        public string ServerName
        {
            get { return (string)base[s_propServerName]; }
        }

        [ConfigurationProperty("connectionString")]
        public string ConnectionString
        {
            get { return (string)base[s_propConnectionString]; }
        }

        [ConfigurationProperty("isActive")]
        public string IsActive
        {
            get { return (string)base[s_propIsActive]; }
        }
        protected override ConfigurationPropertyCollection Properties
        {
            get { return s_properties; }
        }
    }
}
