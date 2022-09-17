using System;
using System.ComponentModel;
using System.Linq;

namespace PinguApps.Alchemy.NftApiClient.Extensions
{
    internal static class EnumExtensions
    {
        public static string GetDescription(this Enum enumValue)
        {
            var enumType = enumValue.GetType();

            var memberInfo = enumType.GetMember(enumValue.ToString());

            if ((memberInfo != null && memberInfo.Length > 0))
            {
                if (memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false) is DescriptionAttribute[] attributes && attributes.Count() > 0)
                {
                    return attributes.ElementAt(0).Description;
                }
            }

            return enumValue.ToString();
        }
    }
}
