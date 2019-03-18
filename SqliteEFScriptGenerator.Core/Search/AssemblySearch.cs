// Copyright (c) 2019 Adam Chappell
// This code is licensed under MIT license (see LICENSE for details)
using SqliteEFScriptGenerator.Core.Attributes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace SqliteEFScriptGenerator.Core.Search
{
    /// <summary>
    /// Class of static methods to search an <see cref="Assembly"/> object
    /// </summary>
    public class AssemblySearch
    {
        /// <summary>
        /// Get the specified type from the specified assembly file
        /// </summary>
        /// <param name="assemblyFilePath">Assembly file to search</param>
        /// <param name="typeName">Type to retrieve</param>
        /// <returns>The <see cref="Type"/> object from <see cref="typeName"/></returns>
        public static Type GetTypeFromAssembly(string assemblyFilePath, string typeName)
        {
            Assembly assembly = Assembly.LoadFrom(assemblyFilePath);
            return GetTypeFromAssembly(assembly, typeName);
        }

        /// <summary>
        /// Get the specified type from the specified assembly
        /// </summary>
        /// <param name="assembly">Assembly file to search</param>
        /// <param name="typeName">Type to retrieve</param>
        /// <returns>The <see cref="Type"/> object from <see cref="typeName"/></returns>
        public static Type GetTypeFromAssembly(Assembly assembly, string typeName)
        {
            return assembly.GetType(typeName);
        }

        /// <summary>
        /// Search assembly for a given attribute
        /// </summary>
        /// <param name="assembly">Assembly to search</param>
        /// <param name="attributeType">Attribute to search for</param>
        /// <returns>List of Types with the <see cref="ScriptTable"/> attribute</returns>
        public static IEnumerable<Type> GetTypesByAttribute(Assembly assembly, Type attributeType)
        {
            foreach (Type type in assembly.GetTypes())
            {
                if (type.GetCustomAttributes(attributeType, true).Length > 0)
                {
                    yield return type;
                }
            }
        }

        /// <summary>
        /// Search assembly for a given attribute
        /// </summary>
        /// <param name="assemblyFilePath">File path of the Assembly to search</param>
        /// <param name="attributeType">Attribute to search for</param>
        /// <returns>List of Types with the attribute specified</returns>
        public static IEnumerable<Type> GetTypesByAttribute(string assemblyFilePath, Type attributeType)
        {
            if (!File.Exists(assemblyFilePath))
            {
                throw new FileNotFoundException($"{assemblyFilePath} does not exist.");
            }

            Assembly assembly = Assembly.LoadFrom(assemblyFilePath);
            return GetTypesByAttribute(assembly, attributeType);
        }

        /// <summary>
        /// Search assembly for <see cref="ScriptTable"/> attribute
        /// </summary>
        /// <param name="assemblyFilePath">The file path of the assembly to search</param>
        /// <returns>List of Types with the <see cref="ScriptTable"/> attribute</returns>
        public static IEnumerable<Type> GetClassesWithScriptTableAttribute(string assemblyFilePath)
        {
            if (!File.Exists(assemblyFilePath))
            {
                throw new FileNotFoundException($"{assemblyFilePath} does not exist.");
            }

            Assembly assembly = Assembly.LoadFrom(assemblyFilePath);

            return GetClassesWithScriptTableAttribute(assembly);
        }

        /// <summary>
        /// Search assembly for <see cref="ScriptTable"/> attribute
        /// </summary>
        /// <param name="assembly">The assembly to search</param>
        /// <returns>List of Types with the <see cref="ScriptTable"/> attribute</returns>
        public static IEnumerable<Type> GetClassesWithScriptTableAttribute(Assembly assembly)
        {
            Type scriptTable = Type.GetType("SqliteEFScriptGenerator.Core.Attributes.ScriptTable");
            return GetTypesByAttribute(assembly, scriptTable);
        }
    }
}