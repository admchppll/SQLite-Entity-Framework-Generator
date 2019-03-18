// Copyright (c) 2019 Adam Chappell
// This code is licensed under MIT license (see LICENSE for details)
using SqliteEFScriptGenerator.Core.Generation.Models;
using SqliteEFScriptGenerator.Core.Search;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SqliteEFScriptGenerator.Core.Generation
{
    /// <summary>
    /// Class to generate a sqlite database script
    /// </summary>
    public class SQLiteGenerator
    {
        #region Properties

        /// <summary>
        /// The assembly file used to generate the script
        /// </summary>
        private readonly string _assemblyFilePath;

        /// <summary>
        /// List of the tables to generate
        /// </summary>
        private List<TableDefinition> _tables;

        //Tuple<Class/Table, Column/Field, Type>
        ///public List<Tuple<string, string, string>> ProblemColumns;
        //TODO: Add functionality to list all issues occurring whilst generating the script to output/log

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Initialize the generator
        /// </summary>
        /// <param name="assemblyFilePath">The assembly file path to generate a database from</param>
        public SQLiteGenerator(string assemblyFilePath)
        {
            _assemblyFilePath = assemblyFilePath;
            //ProblemColumns = new List<Tuple<string, string, string>>();
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Generates the sqlite script to create a database.
        /// </summary>
        public string GenerateNewDatabase()
        {
            PopulateTables();
            PopulateColumns();

            return GenerateCreateScript();
        }

        /// <summary>
        /// Finds the tables that are to be generated.
        /// </summary>
        private void PopulateTables()
        {
            //Create new list of table definitions each time
            _tables = new List<TableDefinition>();

            //Search types for script-able tables
            foreach (Type type in AssemblySearch.GetClassesWithScriptTableAttribute(_assemblyFilePath) ?? new List<Type>())
            {
                _tables.Add(new TableDefinition(type.Name, type.Namespace));
            }
        }

        /// <summary>
        /// Finds and add columns for the table definitions
        /// </summary>
        private void PopulateColumns()
        {
            Parallel.ForEach(_tables, (currentTable) =>
            {
                currentTable.GetColumns(_assemblyFilePath);
                currentTable.GetForeignKeys(_assemblyFilePath);
            });
        }

        /// <summary>
        /// Get the primary key of specified table
        /// </summary>
        /// <param name="tableName">The name of the table to search</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        private string GetTablePrimaryKey(string tableName)
        {
            if (string.IsNullOrWhiteSpace(tableName))
            {
                throw new ArgumentException($"{nameof(tableName)} cannot be null, empty or whitespace");
            }

            TableDefinition table = _tables.Find(t => t.Name.ToLowerInvariant() == tableName.ToLowerInvariant());
            return table.PrimaryKey.Name;
        }

        /// <summary>
        /// Creates a sqlite script to generate the complete database
        /// </summary>
        /// <returns>A sqlite script</returns>
        private string GenerateCreateScript()
        {
            StringBuilder databaseScipt = new StringBuilder();

            foreach (TableDefinition table in _tables)
            {
                databaseScipt.AppendLine(table.GenerateSQLiteCreateTable());
            }

            return databaseScipt.ToString();
        }

        #endregion Methods
    }
}