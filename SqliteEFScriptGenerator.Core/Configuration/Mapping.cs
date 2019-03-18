// Copyright (c) 2019 Adam Chappell
// This code is licensed under MIT license (see LICENSE for details)
using System.Configuration;

namespace SqliteEFScriptGenerator.Core.Configuration
{
    public class Mapping : ConfigurationElement
    {
        [ConfigurationProperty("from", IsRequired = true)]
        [StringValidator(InvalidCharacters = "~!@#$%^&*()[]{}/;'\"|\\")]
        public string From
        {
            get
            {
                return this["from"] as string;
            }
        }

        [ConfigurationProperty("to", IsRequired = true)]
        [StringValidator(InvalidCharacters = "~!@#$%^&*()[]{}/;'\"|\\")]
        public string To
        {
            get
            {
                return this["to"] as string;
            }
        }
    }
}