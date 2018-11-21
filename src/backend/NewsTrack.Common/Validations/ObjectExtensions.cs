using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace NewsTrack.Common.Validations
{
    public static class ObjectExtensions
    {
        public static bool HasProperty(this object value, string propertyName)
        {
            if (value is ExpandoObject model)
            {
                var dictionary = (IDictionary<string, object>) model;
                return dictionary.ContainsKey(propertyName);
            }

            return value.GetType().GetProperties().Any(p => p.Name == propertyName);
        }
    }
}