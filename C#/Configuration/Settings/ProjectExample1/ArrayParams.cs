using System;
using System.Configuration;

namespace ConfigAppNetFramework
{
    public class MyArrayParametrSection : ConfigurationSection
    {
        [ConfigurationProperty("SomeParametrString1", IsRequired = true)]
        public string SomeParametrString1
        {
            get { return (string)this["SomeParametrString1"]; }
            set { this["SomeParametrString1"] = value; }
        }

        [ConfigurationProperty("SomeParametrInt2", IsRequired = true)]
        public int SomeParametrInt2
        {
            get { return (int)this["SomeParametrInt2"]; }
            set { this["SomeParametrInt2"] = value; }
        }

        [ConfigurationProperty("SomeParametrBool3", IsRequired = true)]
        public bool SomeParametrBool3
        {
            get { return (bool)this["SomeParametrBool3"]; }
            set { this["SomeParametrBool3"] = value; }
        }
    }
}

