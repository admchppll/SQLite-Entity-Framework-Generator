// Copyright (c) 2019 Adam Chappell
// This code is licensed under MIT license (see LICENSE for details)
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using SqliteEFScriptGenerator.Core.Attributes;

namespace SqliteEFScriptGenerator.Core.Generation.Models
{
    /// <summary>
    /// Represents a foreign key constraint in sqlite database
    /// </summary>
    public class ForeignKeyDefinition
    {
        #region Properties

        /// <summary>
        /// The table name the foreign key belongs to
        /// </summary>
        public string TableName;

        /// <summary>
        /// The column on the table to apply foreign key to.
        /// </summary>
        public string ColumnName;

        /// <summary>
        /// The foreign table the key belongs to.
        /// </summary>
        public string ForeignTable;

        /// <summary>
        /// The foreign table's primary key column
        /// </summary>
        public string ForeignPrimaryKey;

        /// <summary>
        /// Flags whether all required fields are present for the foreign key to be scripted in a create table statement
        /// </summary>
        public bool Scriptable => !string.IsNullOrWhiteSpace(ColumnName) &&
                                       !string.IsNullOrWhiteSpace(ForeignTable) &&
                                       !string.IsNullOrWhiteSpace(ForeignPrimaryKey);

        #endregion Properties

        #region Constructors

        public ForeignKeyDefinition(string tableName, string columnName, string foreignTable)
        {
            TableName = tableName;
            ColumnName = columnName;
            ForeignTable = foreignTable;
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Creates a column definition from a <see cref="FieldInfo"/> object
        /// </summary>
        /// <param name="field">The <see cref="FieldInfo"/> object</param>
        /// <returns>A <see cref="ColumnDefinition"/></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static ForeignKeyDefinition CreateFromFieldInfo(FieldInfo field, string tableName)
        {
            if (field == null || field.Name == null)
            {
                throw new ArgumentNullException(nameof(field));
            }

            return new ForeignKeyDefinition(tableName, field.Name, GetForeignTable(field));
        }

        /// <summary>
        /// Creates a Foreign Key definition from a <see cref="PropertyInfo"/> object
        /// </summary>
        /// <param name="property">The <see cref="PropertyInfo"/> object</param>
        /// <returns>A <see cref="ForeignKeyDefinition"/></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static ForeignKeyDefinition CreateFromPropertyInfo(PropertyInfo property, string tableName)
        {
            if (property == null || property.Name == null)
            {
                throw new ArgumentNullException(nameof(property));
            }

            return new ForeignKeyDefinition(tableName, property.Name, GetForeignTable(property));
        }

        /// <summary>
        /// Gets the table the Foreign Key is dependent on
        /// </summary>
        /// <param name="field">The field which holds the <see cref="ScriptForeignKey"/> attribute</param>
        /// <returns>The name of the table for the foreign key</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="NullReferenceException"></exception>
        private static string GetForeignTable(FieldInfo field)
        {
            if (field == null || field.Name == null)
            {
                throw new ArgumentNullException(nameof(field));
            }

            Type scriptForeignKey = Type.GetType("SqliteEFScriptGenerator.Core.Attributes.ScriptForeignKey");
            object[] attributes = field.GetCustomAttributes(scriptForeignKey, true);

            if (attributes.Length > 0)
            {
                foreach (object attr in attributes)
                {
                    if (attr.GetType() == scriptForeignKey)
                    {
                        return ((ScriptForeignKey)attr).ForeignTable;
                    }
                }
            }

            //No foreign key found (Shouldn't get to this state.
            throw new NullReferenceException("Could not find a foreign table in the ScriptForeignKey attribute");
        }

        /// <summary>
        /// Gets the table the Foreign Key is dependent on
        /// </summary>
        /// <param name="property">The property which holds the <see cref="ScriptForeignKey"/> attribute</param>
        /// <returns>The name of the table for the foreign key</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="NullReferenceException"></exception>
        private static string GetForeignTable(PropertyInfo property)
        {
            if (property == null || property.Name == null)
            {
                throw new ArgumentNullException(nameof(property));
            }

            Type scriptForeignKey = Type.GetType("SqliteEFScriptGenerator.Core.Attributes.ScriptForeignKey");
            object[] attributes = property.GetCustomAttributes(scriptForeignKey, true);

            if (attributes.Length > 0)
            {
                foreach (object attr in attributes)
                {
                    if (attr.GetType() == scriptForeignKey)
                    {
                        return ((ScriptForeignKey)attr).ForeignTable;
                    }
                }
            }

            //No foreign key found (Shouldn't get to this state.
            throw new NullReferenceException("Could not find a foreign table in the ScriptForeignKey attribute");
        }

        /// <summary>
        /// Sets the ForeignColumn property
        /// </summary>
        /// <param name="tables">The <see cref="List{T}"/> of <see cref="TableDefinition"/>'s to search for the foreign key</param>
        /// <remarks>Requires that the <see cref="ForeignTable"/> property is set.</remarks>
        /// <exception cref="NullReferenceException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public void SetForeignColumn(ref List<TableDefinition> tables)
        {
            string columnName;

            //Check ForeignTable is set
            if (string.IsNullOrWhiteSpace(ForeignTable))
            {
                throw new NullReferenceException("Foreign table cannot be null, empty or whitespace");
            }

            //Check the tables parameter is passed
            if (tables == null)
            {
                throw new ArgumentNullException(nameof(tables));
            }

            //Find primary key for table in a list of table definitions and set ForeignPrimary key field.
            //Allow exceptions to filter out of the function
            columnName = tables
                .Find(t => String.Equals(t.Name, ForeignTable, StringComparison.CurrentCultureIgnoreCase))
                .PrimaryKey.Name;

            ForeignPrimaryKey = columnName;
        }

        #endregion Methods
    }
}