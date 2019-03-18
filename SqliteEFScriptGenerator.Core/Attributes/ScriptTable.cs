// Copyright (c) 2019 Adam Chappell
// This code is licensed under MIT license (see LICENSE for details)
using System;

namespace SqliteEFScriptGenerator.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class,
                    AllowMultiple = false,
                    Inherited = true)]
    public class ScriptTable : Attribute
    {
        #region Properties

        private readonly bool _include;

        public bool IsIncluded
        {
            get
            {
                return _include;
            }
        }

        #endregion Properties

        #region Constructors

        public ScriptTable(bool include = true)
        {
            this._include = include;
        }

        #endregion Constructors
    }
}