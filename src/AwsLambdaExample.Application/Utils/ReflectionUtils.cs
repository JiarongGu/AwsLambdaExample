using System;

namespace AwsLambdaExample.Application.Utils
{
    public static class ReflectionUtils
    {
        public static bool IsNullableType(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        public static Type GetUnderlyingType(Type type) 
        {
            return IsNullableType(type) ? Nullable.GetUnderlyingType(type)! : type;
        }
    }
}
