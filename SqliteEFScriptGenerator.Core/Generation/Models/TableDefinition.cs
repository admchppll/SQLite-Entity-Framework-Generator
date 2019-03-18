// Copyright (c) 2019 Adam Chappell
// This code is licensed under MIT license (see LICENSE for details)
using SqliteEFScriptGenerator.Core.Exceptions;
using SqliteEFScriptGenerator.Core.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SqliteEFScriptGenerator.Core.Generation.Models
{
    /// <summary>
    /// Represents a table in a sqlite database
    /// </summary>
    public class TableDefinition
    {
        #region Properties

        /// <summary>
        /// Name of the table
        /// </summary>
        public string Name;

        /// <summary>
        /// Namespace of the type in C#
        /// </summary>
        public string ClassNamespace;

        /// <summary>
        /// Columns on the table
        /// </summary>
        public List<ColumnDefinition> Columns;

        /// <summary>
        /// Columns on the table
        /// </summary>
        public List<ForeignKeyDefinition> ForeignKeys;

        /// <summary>
        /// The full namespace including class name
        /// </summary>
        public string FullClassNamespace => $"{ClassNamespace}.{Name}";

        #endregion Properties

        #region Constructors

        public TableDefinition()
        {
            Columns = new List<ColumnDefinition>();
            ForeignKeys = new List<ForeignKeyDefinition>();
        }

        public TableDefinition(string name, string classNamespace)
        {
            Name = name;
            ClassNamespace = classNamespace;
            Columns = new List<ColumnDefinition>();
            ForeignKeys = new List<ForeignKeyDefinition>();
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Add a <see cref="ColumnDefinition"/> to the table
        /// </summary>
        /// <param name="name">Name of the column</param>
        /// <param name="type">Type of the column</param>
        /// <exception cref="ArgumentNullException"></exception>
        public void AddColumn(string name, string type, bool primaryKey)
        {
            AddColumn(new ColumnDefinition(name, type, primaryKey));
        }

        /// <summary>
        /// Add a <see cref="ColumnDefinition"/> to the table
        /// </summary>
        /// <paramm name="column">The <see cref="ColumnDefinition"/> to add to <see cref="Columns"/></paramm>
        /// <exception cref="ArgumentNullException"></exception>
        public void AddColumn(ColumnDefinition column)
        {
            if (column == null)
            {
                throw new ArgumentNullException(nameof(column));
            }

            // Can't have multiple columns of same name
            if (!Columns.Exists(c => c.Name == column.Name))
            {
                Columns.Add(column);
            }
        }

        /// <summary>
        /// Add a <see cref="ForeignKeyDefinition"/> to the table
        /// </summary>
        /// <param name="columnName">Foreign Key column name</param>
        /// <param name="foreignTable">Table the foreign key links to</param>
        /// <exception cref="ArgumentNullException"></exception>
        public void AddForeignKey(string columnName, string foreignTable)
        {
            ForeignKeys.Add(new ForeignKeyDefinition(Name, columnName, foreignTable));
        }

        /// <summary>
        /// Add a <see cref="ForeignKeyDefinition"/> to the table
        /// </summary>
        /// <paramm name="foreignKey">The <see cref="ForeignKeyDefinition"/> to add to <see cref="ForeignKeys"/></paramm>
        /// <exception cref="ArgumentNullException"></exception>
        public void AddForeignKey(ForeignKeyDefinition foreignKey)
        {
            if (foreignKey == null)
            {
                throw new ArgumentNullException(nameof(foreignKey));
            }

            ForeignKeys.Add(foreignKey);
        }

        /// <summary>
        /// Retrieves all the columns for the table
        /// </summary>
        /// <param name="assemblyFilePath">The assembly file to search</param>
        public void GetColumns(string assemblyFilePath)
        {
            // Get primary key for table
            GetPrimaryKey(assemblyFilePath);

            Type type = AssemblySearch.GetTypeFromAssembly(assemblyFilePath, FullClassNamespace);

            //Search fields
            foreach (var field in TypeSearch.GetFieldsWithScriptPropertyAttribute(type) ?? new FieldInfo[0])
            {
                AddColumn(ColumnDefinition.CreateFromFieldInfo(field));
            }

            //Search properties
            foreach (var property in TypeSearch.GetPropertiesWithScriptPropertyAttribute(type) ?? new PropertyInfo[0])
            {
                AddColumn(ColumnDefinition.CreateFromPropertyInfo(property));
            }

#if DEBUG
            foreach (ColumnDefinition column in Columns)
            {
                Console.WriteLine($"Table: {Name} - Column: {column.Name}, {column.Type}, PK:{column.PrimaryKey}");
            }
#endif
        }

        /// <summary>
        /// Retrieves all foreigns keys for the table
        /// </summary>
        /// <param name="assemblyFilePath">The assembly file to search</param>
        /// <remarks><see cref="CompleteForeignKeyDefinitions"/> should be called after this function.</remarks>
        public void GetForeignKeys(string assemblyFilePath)
        {
            Type type = AssemblySearch.GetTypeFromAssembly(assemblyFilePath, FullClassNamespace);

            //Search fields
            foreach (var field in TypeSearch.GetFieldsWithScriptForeignKeyAttribute(type) ?? new FieldInfo[0])
            {
                AddForeignKey(ForeignKeyDefinition.CreateFromFieldInfo(field, Name));
            }

            //Search properties
            foreach (var property in TypeSearch.GetPropertiesWithScriptForeignKeyAttribute(type) ?? new PropertyInfo[0])
            {
                AddForeignKey(ForeignKeyDefinition.CreateFromPropertyInfo(property, Name));
            }

#if DEBUG
            foreach (ForeignKeyDefinition fk in ForeignKeys)
            {
                Console.WriteLine($"Table: {Name} - Foreign Key: {fk.ColumnName}, {fk.ForeignTable}");
            }
#endif
        }

        /// <summary>
        /// Completes the <see cref="ForeignKeyDefinition"/> by updating the ForeignColumn properties
        /// </summary>
        /// <param name="tables">The complete list of tables in the database to find the foreign key</param>
        /// <remarks>Should be run after <see cref="GetForeignKeys"/>, otherwise no foreign keys will be generated in the script</remarks>
        public void CompleteForeignKeyDefinitions(ref List<TableDefinition> tables)
        {
            //Only need to handle this if there any foreign keys
            if (ForeignKeys != null && ForeignKeys.Any())
            {
                foreach (ForeignKeyDefinition foreignKey in ForeignKeys)
                {
                    foreignKey.SetForeignColumn(ref tables);
                }
            }
        }

        /// <summary>
        /// Retrieves the primary key for the table
        /// </summary>
        /// <param name="assemblyFilePath">The assembly file to search</param>
        /// <remarks>Assumes only 1 primary key defined on class</remarks>
        public void GetPrimaryKey(string assemblyFilePath)
        {
            Type type = AssemblySearch.GetTypeFromAssembly(assemblyFilePath, FullClassNamespace);
            object primaryKey = TypeSearch.GetPrimaryKeyFromType(type);

            //if null, there is no primary key
            if (primaryKey != null)
            {
                try
                {
                    PropertyInfo property = (PropertyInfo)primaryKey;
                    AddColumn(ColumnDefinition.CreateFromPropertyInfo(property, true));
                }
                catch (InvalidCastException)
                {
                    //If not a PropertyInfo type or null, it must be FieldInfo. Otherwise, allow exceptions.
                    FieldInfo field = (FieldInfo)primaryKey;
                    AddColumn(ColumnDefinition.CreateFromFieldInfo(field, true));
                }
            }
        }

        /// <summary>
        /// Gets the primary key of the current table
        /// </summary>
        /// <returns>The <see cref="ColumnDefinition"/> of the primary key. Otherwise, null.</returns>
        /// <remarks>Assumes that there is only a single primary key, therefore will only return the first column which it finds that is marked as a primary key.</remarks>
        public ColumnDefinition PrimaryKey => Columns.Find(c => c.PrimaryKey == true);

        /// <summary>
        /// Creates a SQLite database script to create the table
        /// </summary>
        /// <returns>A string of valid SQLite to create database tables</returns>
        /// <exception cref="SQLiteGeneratorException"></exception>
        public string GenerateSQLiteCreateTable()
        {
            if (Columns == null ||
                Columns.Count == 0)
            {
                throw new SQLiteGeneratorException("Cannot create a SQLite database table that has no columns.");
            }

            StringBuilder script = new StringBuilder();

            //Start create table
            script.AppendLine($"CREATE TABLE [IF NOT EXISTS] {this.Name} (");

            //Add column to table definition
            foreach (ColumnDefinition column in Columns)
            {
                script.AppendLine($"\t{column.Name} {column.Type}{(column.PrimaryKey ? " PRIMARY KEY" : "")},");
            }

            //Add foreign key constraints if any exist
            if (ForeignKeys.Count > 0)
            {
                foreach (var foreignKey in ForeignKeys)
                {
                    if (foreignKey.Scriptable)
                    {
                        script.AppendLine($"\tFOREIGN KEY ({foreignKey.ColumnName}) REFERENCES {foreignKey.ForeignTable}({foreignKey.ForeignPrimaryKey})");
                    }
                }
            }

            //Remove last comma from string and re-add line break
            script.Length = script.Length - 3;
            script.AppendLine("");

            //close and return create table
            script.AppendLine($");");
            return script.ToString();
        }

        #endregion Methods
    }
}