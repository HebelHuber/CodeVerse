using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CodeVerse.Common
{
    public static class StaticExtensions
    {
        public static string ToStringWithProps<T>(this T inputO, bool onnewlines = false, int NumberDigits = 3, bool printclassname = true, int ClassNamePadding = 15)
        {
            Type type = inputO.GetType();
            FieldInfo[] fields = type.GetFields();
            fields = fields.Reverse().ToArray();
            PropertyInfo[] properties = type.GetProperties();
            properties = properties.Reverse().ToArray();
            var instance = inputO;

            List<string> PerPropString = new List<string>();

            if (printclassname)
            {
                string classNameString = type.Name;

                if (ClassNamePadding != 0)
                {
                    while (classNameString.Length < ClassNamePadding)
                        classNameString += "-";
                }

                PerPropString.Add(classNameString + ":");
            }

            Array.ForEach(fields, (field) =>
            {
                var fieldVal = field.GetValue(instance);

                if (field.FieldType == typeof(float))
                    PerPropString.Add(field.Name + ":" + ((float)fieldVal).ToFixedLengthString(NumberDigits));
                else if (field.FieldType == typeof(double))
                    PerPropString.Add(field.Name + ":" + ((double)fieldVal).ToFixedLengthString(NumberDigits));
                else
                    PerPropString.Add(field.Name + ":" + fieldVal);
            });

            Array.ForEach(properties, (property) =>
            {
                if (property.CanRead)
                {
                    var propVal = property.GetValue(instance, null);

                    if (property.PropertyType == typeof(float))
                        PerPropString.Add(property.Name + ":" + ((float)propVal).ToFixedLengthString(NumberDigits));
                    else if (property.PropertyType == typeof(double))
                        PerPropString.Add(property.Name + ":" + ((double)propVal).ToFixedLengthString(NumberDigits));
                    else
                        PerPropString.Add(property.Name + ":" + propVal);
                }
            });

            if (onnewlines)
                return String.Join("\n", PerPropString.ToArray());
            else
                return String.Join(" ", PerPropString.ToArray());
        }

        public static string ToFixedLengthString(this float val, int length)
        {
            string digitString = "N" + length.ToString();
            return val.ToString(digitString);
            //return String.Format("{0:00000}", val);
            //return val.ToString("000:00000");
        }

        public static string ToFixedLengthString(this double val, int length)
        {
            return Convert.ToSingle(val).ToFixedLengthString(length);
        }

        private static Random random = null;

        public static float randomNormalizedFloat
        {
            get
            {
                if (random == null) random = new Random();
                return Convert.ToSingle(random.NextDouble());
            }
        }

        public static int GetRandomWeightedIndex(params float[] weights)
        {
            if (random == null) random = new Random();

            if (weights == null || weights.Length == 0) return -1;

            // add up all the weights
            float total = 0f;
            for (int i = 0; i < weights.Length; i++)
            {
                if (float.IsPositiveInfinity(weights[i])) return i;
                else if (weights[i] >= 0f && !float.IsNaN(weights[i])) total += weights[i];
            }

            float randomFloat = Convert.ToSingle(random.NextDouble());
            float movingChance = 0f;

            // now step through the weights and check if we hit it
            for (int i = 0; i < weights.Length; i++)
            {
                if (float.IsNaN(weights[i]) || weights[i] <= 0f) continue;

                movingChance += weights[i] / total;
                if (movingChance >= randomFloat) return i;
            }

            return -1;
        }
    }
}
