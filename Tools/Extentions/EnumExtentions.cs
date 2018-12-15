using System;
using System.Reflection;
using Tools.Attributes;
using Tools.ResourcesSupport;

namespace Wukker.Tools
{
    public static class EnumExtentions
    {
        public static string GetLocalisedDescription(this Enum value)
        {
            FieldInfo fi = value.GetType().GetTypeInfo().GetDeclaredField(value.ToString());

            DisplayAttribute[] attributes =
                (DisplayAttribute[])fi.GetCustomAttributes(
                typeof(DisplayAttribute),
                false);

            if (attributes != null &&
                attributes.Length > 0)
                return AppResources.GetResource(attributes[0].Name);

            return value.ToString();
        }

        public static int GetPeriod(this Enum value)
        {
            FieldInfo fi = value.GetType().GetTypeInfo().GetDeclaredField(value.ToString());

            PeriodAttribute[] attributes =
                (PeriodAttribute[])fi.GetCustomAttributes(
                typeof(PeriodAttribute),
                false);

            if (attributes != null &&
                attributes.Length > 0)
                return attributes[0].Period;

            return 0;
        }
    }
}
