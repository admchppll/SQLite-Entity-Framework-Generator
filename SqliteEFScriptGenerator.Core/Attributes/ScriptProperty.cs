// Copyright (c) 2019 Adam Chappell
// This code is licensed under MIT license (see LICENSE for details)
using System;

namespace SqliteEFScriptGenerator.Core.Attributes
{
    /// <summary>
    /// An attribute for signaling a property to be included in a generated SQLite script
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field,
                    AllowMultiple = true,
                    Inherited = true)]
    public class ScriptProperty : Attribute
    {
        #region Properties

        private readonly bool _include;
        private readonly string _type;

        /// <summary>
        /// Whether the field/property should be included in a script generation
        /// </summary>
        public bool IsIncluded
        {
            get
            {
                return _include;
            }
        }

        /// <summary>
        /// The SQLite type the field/property will be held as in the db.
        /// </summary>
        /// <remarks>If this is set, this will override the mapping</remarks>
        public string Type
        {
            get
            {
                return _type;
            }
        }

        #endregion Properties

        #region Constructors

        public ScriptProperty()
        {
            this._include = true;
        }

        public ScriptProperty(bool include = true)
        {
            this._include = include;
        }

        public ScriptProperty(bool include = true, string type = "")
        {
            this._include = include;
            this._type = type;
        }

        #endregion Constructors
    }
}