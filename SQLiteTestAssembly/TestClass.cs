// Copyright (c) 2019 Adam Chappell
// This code is licensed under MIT license (see LICENSE for details)
using SqliteEFScriptGenerator.Core.Attributes;
using System;

namespace SQLiteTestAssembly
{
    [ScriptTable]
    public class TestClass
    {
        public string Name;

        [ScriptProperty]
        public int Number;

        [ScriptProperty]
        public DateTime Date;

        [ScriptProperty]
        public string RandomName;
    }
}