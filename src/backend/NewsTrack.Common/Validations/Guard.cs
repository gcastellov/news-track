using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace NewsTrack.Common.Validations
{
    public static class Guard
    {
        public static void CheckIfNull(this string value, string propertyName)
        {
            if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException(propertyName);
            }
        }

        public static void CheckIfNull(this object value, string propertyName)
        {
            if (value == null)
            {
                throw new ArgumentNullException(propertyName);
            }
        }

        public static void Check(this Uri value, string propertyName)
        {
            if (!value.IsWellFormedOriginalString())
            {
                throw new ArgumentNullException(propertyName);
            }
        }

        public static void CheckIfNull<T>(this IEnumerable<T> value, string propertyName)
        {
            if (value == null || !value.Any())
            {
                throw new ArgumentNullException(propertyName);
            }
        }

        public static void CheckIfNull(this IDictionary value, string propertyName)
        {
            if (value == null || value.Count == 0)
            {
                throw new ArgumentNullException(propertyName);
            }
        }
    }
}