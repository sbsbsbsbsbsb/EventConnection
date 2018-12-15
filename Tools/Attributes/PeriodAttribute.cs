using System;

namespace Tools.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class PeriodAttribute : Attribute
    {
        public int Period { get; private set; }

        public PeriodAttribute(int period)
        {
            Period = period;
        }
    }
}
