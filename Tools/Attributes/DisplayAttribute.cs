using System;
using JetBrains.Annotations;

namespace Tools.Attributes
{
    [AttributeUsage(AttributeTargets.All)]
    public class DisplayAttribute : Attribute
    {
        public string Name { get; private set; }

        public DisplayAttribute([NotNull] string name)
        {
            Name = name;
        }
    }
}
