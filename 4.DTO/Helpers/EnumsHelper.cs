using System;
using System.ComponentModel;
using System.Reflection;

namespace _4.DTO.Helpers
{
    public static class EnumsHelper
    {
        public static string GetDescription<T>(this T enumerationValue) where T : Enum
        {
            Type type = enumerationValue.GetType();

            MemberInfo[] memberInfo = type.GetMember(enumerationValue.ToString());
            if (memberInfo != null && memberInfo.Length > 0)
            {
                object[] attrs = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attrs != null && attrs.Length > 0)
                {
                    return ((DescriptionAttribute)attrs[0]).Description;
                }
            }
            
            return enumerationValue.ToString();
        }
    }
}
