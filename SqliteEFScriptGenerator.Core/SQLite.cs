// Copyright (c) 2019 Adam Chappell
// This code is licensed under MIT license (see LICENSE for details)
using System;

namespace SqliteEFScriptGenerator.Core
{
    public class SQLite
    {
        /// <summary>
        /// An array of the datatypes SQLite supports
        /// </summary>
        public static readonly string[] DataTypes =
        {
            "int",
            "integer",
            "tinyint",
            "smallint",
            "mediumint",
            "bigint",
            "unsigned big int",
            "int2",
            "int8",
            "text",
            "character",
            "varchar",
            "varying character",
            "nchar",
            "native character",
            "nvarchar",
            "clob",
            "blob",
            "real",
            "double",
            "double precision",
            "float",
            "numeric",
            "decimal",
            "boolean",
            "date",
            "datetime"
        };

        /// <summary>
        /// Check whether a data type is valid for SQLite
        /// </summary>
        /// <param name="dataType">The data type to check</param>
        /// <returns>True, if the data type provided is valid in SQLite. Otherwise, false.</returns>
        public static bool IsDataType(string dataType)
        {
            if (Array.FindIndex(DataTypes, t => t.IndexOf(dataType, StringComparison.InvariantCultureIgnoreCase) >= 0) < 0)
            {
                return false;
            }
            return true;
        }
    }
}