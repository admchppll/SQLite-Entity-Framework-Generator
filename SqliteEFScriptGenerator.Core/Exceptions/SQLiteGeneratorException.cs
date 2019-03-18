// Copyright (c) 2019 Adam Chappell
// This code is licensed under MIT license (see LICENSE for details)
using System;

namespace SqliteEFScriptGenerator.Core.Exceptions
{
    /// <summary>
    /// An exception for the SQLiteGenerator class
    /// </summary>
    public class SQLiteGeneratorException : Exception
    {
        public SQLiteGeneratorException()
        {
        }

        public SQLiteGeneratorException(string message)
            : base(message)
        { }

        public SQLiteGeneratorException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}