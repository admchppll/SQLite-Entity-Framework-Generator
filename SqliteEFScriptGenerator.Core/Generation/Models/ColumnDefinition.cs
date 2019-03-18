// Copyright (c) 2019 Adam Chappell
// This code is licensed under MIT license (see LICENSE for details)
using System;
using System.Reflection;

namespace SqliteEFScriptGenerator.Core.Generation.Models
{
    /// <summary>
    /// Represents a column in a sqlite database
    /// </summary>
    public class ColumnDefinition
    {
        #region Properties

        /// <summary>
        /// Name of the column
        /// </summary>
        public string Name;

        /// <summary>
        /// The type of the column
        /// </summary>
        public string Type;

        /// <summary>
        /// Flags whether the current column is the table's primary key
        /// </summary>
        public bool PrimaryKey;

        #endregion Properties

        #region Constructors

        public ColumnDefinition(string name, string type, bool primaryKey = false)
        {
            Name = name;
            Type = type;
            PrimaryKey = primaryKey;
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Creates a column definition from a <see cref="FieldInfo"/> object
        /// </summary>
        /// <param name="field">The <see cref="FieldInfo"/> object</param>
        /// <returns>A <see cref="ColumnDefinition"/></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static ColumnDefinition CreateFromFieldInfo(FieldInfo field, bool isPrimaryKey = false)
        {
            if (field == null || field.Name == null)
            {
                throw new ArgumentNullException(nameof(field));
            }

            return new ColumnDefinition(field.Name, field.FieldType.FullName, isPrimaryKey);
        }

        /// <summary>
        /// Creates a column definition from a <see cref="PropertyInfo"/> object
        /// </summary>
        /// <param name="property">The <see cref="PropertyInfo"/> object</param>
        /// <returns>A <see cref="ColumnDefinition"/></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static ColumnDefinition CreateFromPropertyInfo(PropertyInfo property, bool isPrimaryKey = false)
        {
            if (property == null || property.Name == null)
            {
                throw new ArgumentNullException(nameof(property));
            }

            return new ColumnDefinition(property.Name, property.PropertyType.FullName, isPrimaryKey);
        }

        #endregion Methods
    }
}