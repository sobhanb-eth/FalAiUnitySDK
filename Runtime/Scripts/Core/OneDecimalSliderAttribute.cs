// Runtime/Scripts/Core/OneDecimalSliderAttribute.cs
using UnityEngine;
using UnityEditor;

namespace TripoSR.SDK
{
    /// <summary>
    /// Attribute to define a slider with one decimal place.
    /// </summary>
    public class OneDecimalSliderAttribute : PropertyAttribute
    {
        public readonly float Min;
        public readonly float Max;

        public OneDecimalSliderAttribute(float min, float max)
        {
            Min = min;
            Max = max;
        }
    }
}
