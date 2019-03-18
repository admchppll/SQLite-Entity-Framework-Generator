// Copyright (c) 2019 Adam Chappell
// This code is licensed under MIT license (see LICENSE for details)
using SqliteEFScriptGenerator.Core.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLiteTestAssembly.Folder
{
    [ScriptTable]
    internal class AnotherTestClass
    {
        [Key]
        public int ID;

        [ScriptProperty(false)]
        public string Ignored;

        [ScriptProperty]
        public string NotIgnored;

        [ScriptProperty]
        public int Included;

        [ScriptProperty]
        public DateTime DateIncluded;

        [ScriptProperty]
        [ScriptForeignKey("TestClass")]
        public int TestClassId { get; set; }

        public virtual TestClass TestClass { get; set; }
    }
}