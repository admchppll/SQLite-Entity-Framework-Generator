// Copyright (c) 2019 Adam Chappell
// This code is licensed under MIT license (see LICENSE for details)
using System;
using System.Collections.Generic;
using System.Configuration;

namespace SqliteEFScriptGenerator.Core.Configuration
{
    public class SQLiteMappingConfig : ConfigurationSection
    {
        [ConfigurationProperty("mappings")]
        [ConfigurationCollection(typeof(Mappings), AddItemName = "mapping")]
        public Mappings Mappings
        {
            get
            {
                object o = this["mappings"];
                return o as Mappings;
            }
        }

        /// <summary>
        /// Retrieve the SQLite Mappings from the config
        /// </summary>
        /// <returns>SQLite <see cref="Mapping"/>'s</returns>
        public static SQLiteMappingConfig GetConfig()
        {
            return (SQLiteMappingConfig)ConfigurationManager.GetSection("sqliteMappings") ?? new SQLiteMappingConfig();
        }

        /// <summary>
        /// Validate the SQLite Mappings from the config
        /// </summary>
        /// <returns>True, if SQLite types specified in the config are valid. Otherwise, false.</returns>
        public static bool ValidateConfig()
        {
            SQLiteMappingConfig config = GetConfig();

            foreach (Mapping map in config.Mappings)
            {
                if (!SQLite.IsDataType(map.To))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Lists all validation errors for the SQLite mappings in the config
        /// </summary>
        /// <param name="outputToConsole">Whether the errors should be outputted to the console as well. Defaults to false.</param>
        /// <returns>All the errors for each mapping in the config file.</returns>
        public static IEnumerable<string> ListConfigErrors()
        {
            SQLiteMappingConfig config = GetConfig();

            foreach (Mapping map in config.Mappings)
            {
                if (!SQLite.IsDataType(map.To))
                {
                    string errorString = $"{map.From}->{map.To} : {map.To} is not a valid SQLite data type";
                    yield return errorString;
                }
            }
        }

        /// <summary>
        /// Output all configuration errors to the console
        /// </summary>
        /// <remarks>Uses <see cref="ListConfigErrors"/> to retrieve a list of errors</remarks>
        public static void OutputConfigErrorsToConsole()
        {
            Console.WriteLine("There are errors in the SQLite Configuration. Please correct the below issues");

            foreach (string error in ListConfigErrors())
            {
                Console.WriteLine(error);
            }
        }
    }
}