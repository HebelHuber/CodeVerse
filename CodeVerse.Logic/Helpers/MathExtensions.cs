using System;
using System.Collections.Generic;
using System.Text;

namespace CodeVerse.Common
{
    public static class MathExtensions
    {
        public static float Remap(this float value, float InputRangeStart, float InputRangeEnd, float OutputRangeStart, float OutputRangeEnd, bool clamp = false)
        {
            float remapped = (value - InputRangeStart) / (InputRangeEnd - InputRangeStart) * (OutputRangeEnd - OutputRangeStart) + OutputRangeStart;

            if (clamp)
            {
                if (OutputRangeStart < OutputRangeEnd)
                {
                    if (remapped > OutputRangeEnd) remapped = OutputRangeEnd;
                    if (remapped < OutputRangeStart) remapped = OutputRangeStart;
                }
                else
                {
                    if (remapped < OutputRangeEnd) remapped = OutputRangeEnd;
                    if (remapped > OutputRangeStart) remapped = OutputRangeStart;
                }
            }

            return remapped;
        }
    }
}
