// Copyright (c) 2019 Adam Chappell
// This code is licensed under MIT license (see LICENSE for details)
using System.Configuration;

namespace SqliteEFScriptGenerator.Core.Configuration
{
    public class Mappings : ConfigurationElementCollection
    {
        public Mapping this[int index]
        {
            get
            {
                return base.BaseGet(index) as Mapping;
            }
            set
            {
                if (base.BaseGet(index) != null)
                {
                    base.BaseRemoveAt(index);
                }
                this.BaseAdd(index, value);
            }
        }

        public new Mapping this[string responseString]
        {
            get
            {
                return (Mapping)BaseGet(responseString);
            }
            set
            {
                if (BaseGet(responseString) != null)
                {
                    BaseRemoveAt(BaseIndexOf(BaseGet(responseString)));
                }
                BaseAdd(value);
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new Mapping();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((Mapping)element).From;
        }
    }
}