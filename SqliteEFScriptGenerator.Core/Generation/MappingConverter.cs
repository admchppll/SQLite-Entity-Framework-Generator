// Copyright (c) 2019 Adam Chappell
// This code is licensed under MIT license (see LICENSE for details)
using SqliteEFScriptGenerator.Core.Configuration;
using System;

namespace SqliteEFScriptGenerator.Core.Generation
{
    /// <summary>
    /// Convert an input type to that specified in the config file
    /// </summary>
    public class MappingConverter
    {
        #region Methods

        /// <summary>
        /// Find the SQLite data type that the current Type maps to.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> to find the mapping to</param>
        /// <returns>The SQLite data type</returns>
        /// <exception cref="MappingConversionException"></exception>
        public static string ConvertType(Type type)
        {
            SQLiteMappingConfig config = SQLiteMappingConfig.GetConfig();

            foreach (Mapping map in config.Mappings)
            {
                if (type.FullName == map.From)
                {
                    return map.To;
                }
            }

            //Couldn't find a mapping
            throw new MappingConversionException($"No mapping exists in the configuration for: {type.FullName}");
        }

        /// <summary>
        /// Find the SQLite data type that the current Type maps to.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> to find the mapping to</param>
        /// <returns>The SQLite data type</returns>
        /// <exception cref="MappingConversionException"></exception>
        public static string ConvertString(string typeName)
        {
            if (string.IsNullOrWhiteSpace(typeName))
            {
                throw new MappingConversionException("typeName cannot be null, empty or whitespace.");
            }

            Type type = Type.GetType(typeName, false, true);

            if (type == null)
            {
                throw new MappingConversionException($"{typeName} type could not be found or loaded.");
            }

            return ConvertType(type);
        }

        #endregion Methods

        #region Exception

        /// <summary>
        /// An exception for the MappingConverter class
        /// </summary>
        public class MappingConversionException : Exception
        {
            public MappingConversionException()
            {
            }

            public MappingConversionException(string message)
                : base(message)
            { }

            public MappingConversionException(string message, Exception innerException)
                : base(message, innerException)
            { }
        }

        #endregion Exception
    }
}