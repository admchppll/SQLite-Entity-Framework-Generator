// Copyright (c) 2019 Adam Chappell
// This code is licensed under MIT license (see LICENSE for details)
using SqliteEFScriptGenerator.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SqliteEFScriptGenerator.Core.Search
{
    /// <summary>
    /// Class of static methods to search a <see cref="Type"/> object
    /// </summary>
    public static class TypeSearch
    {
        #region ScriptProperty Search

        /*
         *  Note: This functionality is near identical to ScriptForeignKey region. Consider changes in those functions as well, if necessary.
         */

        /// <summary>
        /// Get fields on a type with a <see cref="ScriptProperty"/> attribute
        /// </summary>
        /// <param name="type">The <see cref="Type"/> to search for fields</param>
        /// <returns>An <see cref="IEnumerable{T}"/> of <see cref="FieldInfo"/> with the <see cref="ScriptProperty"/> attribute</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IEnumerable<FieldInfo> GetFieldsWithScriptPropertyAttribute(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            Type scriptProperty = Type.GetType("SqliteEFScriptGenerator.Core.Attributes.ScriptProperty");

            foreach (FieldInfo field in type.GetFields())
            {
                object[] attributes = field.GetCustomAttributes(scriptProperty, true);

                if (attributes.Length > 0)
                {
                    foreach (object attr in attributes)
                    {
                        if (((ScriptProperty)attr).IsIncluded)
                        {
                            yield return field;
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Get fields on a type with a <see cref="ScriptProperty"/> attribute
        /// </summary>
        /// <param name="typeName">The name of the <see cref="Type"/> to search</param>
        /// <returns>An <see cref="IEnumerable{T}"/> of <see cref="FieldInfo"/> with the <see cref="ScriptProperty"/> attribute</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public static IEnumerable<FieldInfo> GetFieldsWithScriptPropertyAttribute(string typeName)
        {
            if (string.IsNullOrWhiteSpace(typeName))
            {
                throw new ArgumentException($"{nameof(typeName)} cannot be null, empty or whitespace.");
            }

            Type type = Type.GetType(typeName);
            return GetFieldsWithScriptPropertyAttribute(type);
        }

        /// <summary>
        /// Get properties on a type with a <see cref="ScriptProperty"/> attribute
        /// </summary>
        /// <param name="type">The <see cref="Type"/> to search for properties</param>
        /// <returns>An <see cref="IEnumerable{T}"/> of <see cref="PropertyInfo"/> with the <see cref="ScriptProperty"/> attribute</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IEnumerable<PropertyInfo> GetPropertiesWithScriptPropertyAttribute(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            Type scriptProperty = Type.GetType("SqliteEFScriptGenerator.Core.Attributes.ScriptProperty");

            foreach (PropertyInfo property in type.GetProperties())
            {
                object[] attributes = property.GetCustomAttributes(scriptProperty, true);

                if (attributes.Length > 0)
                {
                    foreach (object attr in attributes)
                    {
                        if (((ScriptProperty)attr).IsIncluded)
                        {
                            yield return property;
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Get properties on a type with a <see cref="ScriptProperty"/> attribute
        /// </summary>
        /// <param name="typeName">The name of the <see cref="Type"/> to search</param>
        /// <returns>An <see cref="IEnumerable{T}"/> of <see cref="PropertyInfo"/> with the <see cref="ScriptProperty"/> attribute</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public static IEnumerable<PropertyInfo> GetPropertiesWithScriptPropertyAttribute(string typeName)
        {
            if (string.IsNullOrWhiteSpace(typeName))
            {
                throw new ArgumentException($"{nameof(typeName)} cannot be null, empty or whitespace.");
            }

            Type type = Type.GetType(typeName);
            return GetPropertiesWithScriptPropertyAttribute(type);
        }

        #endregion ScriptProperty Search

        #region ScriptForeignKey Search

        /*
         *  Note: This functionality is near identical to ScriptProperty region. Consider changes in those functions as well, if necessary.
         */

        /// <summary>
        /// Get fields on a type with a <see cref="ScriptProperty"/> attribute
        /// </summary>
        /// <param name="type">The <see cref="Type"/> to search for fields</param>
        /// <returns>An <see cref="IEnumerable{T}"/> of <see cref="FieldInfo"/> with the <see cref="ScriptForeignKey"/> attribute</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IEnumerable<FieldInfo> GetFieldsWithScriptForeignKeyAttribute(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            Type scriptForeignKey = Type.GetType("SqliteEFScriptGenerator.Core.Attributes.ScriptForeignKey");

            foreach (FieldInfo field in type.GetFields())
            {
                object[] attributes = field.GetCustomAttributes(scriptForeignKey, true);

                if (attributes.Length > 0)
                {
                    foreach (object attr in attributes)
                    {
                        if (((ScriptForeignKey)attr).IsIncluded)
                        {
                            yield return field;
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Get fields on a type with a <see cref="ScriptForeignKey"/> attribute
        /// </summary>
        /// <param name="typeName">The name of the <see cref="Type"/> to search</param>
        /// <returns>An <see cref="IEnumerable{T}"/> of <see cref="FieldInfo"/> with the <see cref="ScriptForeignKey"/> attribute</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public static IEnumerable<FieldInfo> GetFieldsWithScriptForeignKeyAttribute(string typeName)
        {
            if (string.IsNullOrWhiteSpace(typeName))
            {
                throw new ArgumentException($"{nameof(typeName)} cannot be null, empty or whitespace.");
            }

            Type type = Type.GetType(typeName);
            return GetFieldsWithScriptForeignKeyAttribute(type);
        }

        /// <summary>
        /// Get properties on a type with a <see cref="ScriptForeignKey"/> attribute
        /// </summary>
        /// <param name="type">The <see cref="Type"/> to search for properties</param>
        /// <returns>An <see cref="IEnumerable{T}"/> of <see cref="PropertyInfo"/> with the <see cref="ScriptForeignKey"/> attribute</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IEnumerable<PropertyInfo> GetPropertiesWithScriptForeignKeyAttribute(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            Type scriptForeignKey = Type.GetType("SqliteEFScriptGenerator.Core.Attributes.ScriptForeignKey");

            foreach (PropertyInfo property in type.GetProperties())
            {
                object[] attributes = property.GetCustomAttributes(scriptForeignKey, true);

                if (attributes.Length > 0)
                {
                    foreach (object attr in attributes)
                    {
                        if (((ScriptForeignKey)attr).IsIncluded)
                        {
                            yield return property;
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Get properties on a type with a <see cref="ScriptForeignKey"/> attribute
        /// </summary>
        /// <param name="typeName">The name of the <see cref="Type"/> to search</param>
        /// <returns>An <see cref="IEnumerable{T}"/> of <see cref="PropertyInfo"/> with the <see cref="ScriptForeignKey"/> attribute</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public static IEnumerable<PropertyInfo> GetPropertiesWithScriptForeignKeyAttribute(string typeName)
        {
            if (string.IsNullOrWhiteSpace(typeName))
            {
                throw new ArgumentException($"{nameof(typeName)} cannot be null, empty or whitespace.");
            }

            Type type = Type.GetType(typeName);
            return GetPropertiesWithScriptForeignKeyAttribute(type);
        }

        #endregion ScriptForeignKey Search

        #region Primary Key Search

        /// <summary>
        /// Get the name of the primary key field/property
        /// </summary>
        /// <param name="type">The <see cref="Type"/> to search for primary key</param>
        /// <returns>A string name of the primary key field</returns>
        /// <remarks>Based on the premise that in entity framework the <see cref="System.ComponentModel.DataAnnotations.KeyAttribute"/> identifies the Primary Key field. Assumes a single primary key.</remarks>
        /// <exception cref="ArgumentNullException"></exception>
        public static object GetPrimaryKeyFromType(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            //Reference in the project is configured to always include the dll when building
            Type keyAttribute = AssemblySearch.GetTypeFromAssembly(@".\System.ComponentModel.DataAnnotations.dll", "System.ComponentModel.DataAnnotations.KeyAttribute");

            //Check properties for key attribute. (Given structure of EF, more likely to find here)
            if (type.GetProperties().Any(prop => Attribute.IsDefined(prop, keyAttribute)))
            {
                PropertyInfo property;
                property = type.GetProperties()
                            .First(prop => Attribute.IsDefined(prop, keyAttribute));

                return property;
            }

            //Check fields for key attribute
            if (type.GetFields().Any(prop => Attribute.IsDefined(prop, keyAttribute)))
            {
                FieldInfo field;
                field = type.GetFields()
                            .First(prop => Attribute.IsDefined(prop, keyAttribute));

                return field;
            }

            //No primary key
            return null;
        }

        #endregion Primary Key Search
    }
}