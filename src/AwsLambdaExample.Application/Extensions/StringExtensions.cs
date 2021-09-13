using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AwsLambdaExample.Application
{
    public static class StringExtensions
    {
        public static string GetSubstringOrEmpty(this string? value, int startIndex, int length)
        {
            if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }

            var maxLength = value.Length - startIndex;

            if (maxLength < 0) {
                return string.Empty;
            }

            if (maxLength > length) 
            {
                return value.Substring(startIndex, length);
            }

            return value.Substring(startIndex, maxLength);
        }

        public static IEnumerable<string> Chunk(this string? value, int length) 
        {
            if (string.IsNullOrEmpty(value)) 
            {
                return Enumerable.Empty<string>();
            }

            var results = new List<string>();

            for (var i = 0; i < value.Length; i+=length) 
            {
                results.Add(value.GetSubstringOrEmpty(i, length));
            }

            return results;
        }
    }
}
