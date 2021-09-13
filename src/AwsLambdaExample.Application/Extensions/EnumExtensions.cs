using AwsLambdaExample.Application.Utils;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace AwsLambdaExample.Application
{
    public class DescriptionAttribute : Attribute
    {
        public DescriptionAttribute(string description)
        {
            Description = description;
        }

        public string Description { get; set; }
    }

    public static class EnumExtensions
    {
        public static ConcurrentDictionary<Type, Dictionary<object, string>> _enumDescriptions = new ConcurrentDictionary<Type, Dictionary<object, string>>();
        public static ConcurrentDictionary<Type, Dictionary<string, object>> _descriptionEnums = new ConcurrentDictionary<Type, Dictionary<string, object>>();

        public static string GetEnumDescription<T>(this T value) where T : Enum
        {
            return GetEnumDescription(value, ReflectionUtils.GetUnderlyingType(typeof(T)));
        }

        public static string GetEnumDescription(object value, Type enumType)
        {
            if (!enumType.IsEnum)
            {
                throw new ArgumentException($"Type: ${enumType.Name} is not enum");
            }

            var dictionary = GetEnumDescriptionDictionary(enumType);

            if (dictionary.enumDescription.ContainsKey(value))
            {
                return dictionary.enumDescription[value];
            }

            throw new ArgumentException($"Enum: ${enumType.Name} does not have description for value ${value}");
        }

        /// <summary>
        /// convert description to enum
        /// </summary>
        /// <typeparam name="T">enum type</typeparam>
        /// <exception cref="ArgumentException">not one of the description constants defined for the enumeration.</exception>
        public static T ParseEnumDescription<T>(this string description) where T : Enum
        {
            return (T)ParseEnumDescription(description, ReflectionUtils.GetUnderlyingType(typeof(T)));
        }

        public static object ParseEnumDescription(string description, Type enumType) 
        {
            if (!enumType.IsEnum)
            {
                throw new ArgumentException($"Type: ${enumType.Name} is not enum");
            }

            var dictionary = GetEnumDescriptionDictionary(enumType);

            if (dictionary.descriptionEnum.ContainsKey(description))
            {
                return dictionary.descriptionEnum[description];
            }

            throw new ArgumentException($"Description: ${description} does not exist in enum ${enumType.Name}");
        }

        private static (Dictionary<object, string> enumDescription, Dictionary<string, object> descriptionEnum) GetEnumDescriptionDictionary(Type type)
        {
            if (!_enumDescriptions.ContainsKey(type))
            {
                var enumDescription = new Dictionary<object, string>();

                foreach (var name in Enum.GetNames(type))
                {
                    var memberInfo = type.GetMember(name).FirstOrDefault(m => m.DeclaringType == type);
                    var description = (DescriptionAttribute?)memberInfo.GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault();

                    if (description != null)
                    {
                        enumDescription.Add(Enum.Parse(type, name), description.Description);
                    }
                }

                _enumDescriptions[type] = enumDescription;
                _descriptionEnums[type] = enumDescription.ToDictionary(x => x.Value, x => x.Key);
            }

            return (_enumDescriptions[type], _descriptionEnums[type]);
        }
    }
}
