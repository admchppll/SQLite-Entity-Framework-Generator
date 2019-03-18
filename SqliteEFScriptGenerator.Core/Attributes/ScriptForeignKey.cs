// Copyright (c) 2019 Adam Chappell
// This code is licensed under MIT license (see LICENSE for details)
using System;

namespace SqliteEFScriptGenerator.Core.Attributes
{
    /// <summary>
    /// An attribute for signaling a foreign key to be included in a generated SQLite script
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field,
        AllowMultiple = true,
        Inherited = true)]
    public class ScriptForeignKey : Attribute
    {
        #region Properties

        private readonly bool _include;
        public readonly string ForeignTable;

        public bool IsIncluded
        {
            get
            {
                return _include;
            }
        }

        #endregion Properties

        #region Constructors

        public ScriptForeignKey(string foreignTable, bool include = true)
        {
            this._include = include;
            this.ForeignTable = foreignTable;
        }

        #endregion Constructors
    }
}