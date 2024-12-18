using UnityEngine;
using UnityEditor;

namespace TripoSR.SDK.Editor
{
    /// <summary>
    /// Custom Property Drawer for the OneDecimalSliderAttribute.
    /// </summary>
    [CustomPropertyDrawer(typeof(OneDecimalSliderAttribute))]
    public class OneDecimalSliderDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Get the range limits from the attribute
            OneDecimalSliderAttribute slider = (OneDecimalSliderAttribute)attribute;

            if (property.propertyType == SerializedPropertyType.Float)
            {
                // Draw the slider and round to one decimal place
                EditorGUI.BeginProperty(position, label, property);
                float value = EditorGUI.Slider(position, label, property.floatValue, slider.Min, slider.Max);
                property.floatValue = Mathf.Round(value * 10) / 10f; // Keep one decimal place
                EditorGUI.EndProperty();
            }
            else
            {
                // Handle error case for non-float property
                EditorGUI.LabelField(position, label.text, "Use OneDecimalSlider with float.");
            }
        }
    }
}