using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace backend.Domain.Extensions
{
    public static class EnumExtension
    {
        public static string GetEnumMemberValue(this Enum enumValue)
        {
            var fieldInfo = enumValue.GetType().GetField(enumValue.ToString());
            var attribute = fieldInfo?.GetCustomAttributes(typeof(EnumMemberAttribute), false)
                .FirstOrDefault() as EnumMemberAttribute;

            return attribute?.Value ?? enumValue.ToString();
        }
    }
}
