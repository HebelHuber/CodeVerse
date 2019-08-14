using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace CodeVerse.Common
{
    public static class StaticExtensions
    {
        public static string ToStringWithProps<T>(this T inputO, bool onnewlines = false, int digits = 5)
        {
            Type type = inputO.GetType();
            FieldInfo[] fields = type.GetFields();
            PropertyInfo[] properties = type.GetProperties();
            var instance = inputO;

            string digitString = "N" + digits.ToString();

            List<string> PerPropString = new List<string>();

            PerPropString.Add(type.Name + "---:");

            Array.ForEach(fields, (field) =>
            {
                var fieldVal = field.GetValue(instance);

                if (field.FieldType == typeof(float))
                    PerPropString.Add(field.Name + ":" + ((float)fieldVal).ToString(digitString));
                else if (field.FieldType == typeof(double))
                    PerPropString.Add(field.Name + ":" + ((double)fieldVal).ToString(digitString));
                else
                    PerPropString.Add(field.Name + ":" + fieldVal);
            });

            Array.ForEach(properties, (property) =>
            {
                if (property.CanRead)
                {
                    var propVal = property.GetValue(instance, null);

                    if (property.PropertyType == typeof(float))
                        PerPropString.Add(property.Name + ":" + ((float)propVal).ToString(digitString));
                    else if (property.PropertyType == typeof(double))
                        PerPropString.Add(property.Name + ":" + ((double)propVal).ToString(digitString));
                    else
                        PerPropString.Add(property.Name + ":" + propVal);
                }
            });

            if (onnewlines)
                return String.Join("\n", PerPropString.ToArray());
            else
                return String.Join(" ", PerPropString.ToArray());
        }
    }
}
