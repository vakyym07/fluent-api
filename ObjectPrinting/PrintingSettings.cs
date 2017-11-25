using System;
using System.Collections.Generic;
using System.Globalization;

namespace ObjectPrinting
{
    public class PrintingSettings
    {
        public PrintingSettings()
        {
            SerializationModesForTypes = new Dictionary<Type, Func<object, string>>();
            SerializationModesForProperties = new Dictionary<string, Func<object, string>>();
            Culture = new Dictionary<Type, CultureInfo>();
            TrimmedProperties = new Dictionary<string, Func<string, string>>();
            ExcludingProperties = new HashSet<string>();
            ExcludingTypes = new HashSet<Type>();
        }

        public Dictionary<Type, Func<object, string>> SerializationModesForTypes { get; }
        public Dictionary<string, Func<object, string>> SerializationModesForProperties { get; }
        public Dictionary<Type, CultureInfo> Culture { get; }
        public Dictionary<string, Func<string, string>> TrimmedProperties { get; }
        public HashSet<Type> ExcludingTypes { get; }
        public HashSet<string> ExcludingProperties { get; }
    }
}