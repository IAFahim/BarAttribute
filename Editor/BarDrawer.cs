using BarAttribute;
using UnityEditor;
using UnityEngine;

namespace BarAttributeEditor
{
    [CustomPropertyDrawer(typeof(BarAttribute.BarAttribute))]
    public class BarDrawer : PropertyDrawer
    {
        private const float PropertyHeight = 18f;
        private const float MinBarHeight = 18f;
        private const float Spacing = 2f;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var barAttribute = (BarAttribute.BarAttribute)attribute;

            // Property field
            Rect propertyRect = new Rect(position.x, position.y, position.width, PropertyHeight);
            EditorGUI.PropertyField(propertyRect, property, label);

            // Bar
            float barHeight = Mathf.Max(barAttribute.BarHeight, MinBarHeight);
            Rect barRect;

            if (barAttribute.IsVertical)
            {
                barRect = new Rect(position.x, position.y + PropertyHeight + Spacing, barHeight, position.height - PropertyHeight - Spacing);
            }
            else
            {
                barRect = new Rect(position.x, position.y + PropertyHeight + Spacing, position.width, barHeight);
            }

            float currentValue, maxValue;

            if (property.propertyType == SerializedPropertyType.Vector2)
            {
                Vector2 vec = property.vector2Value;
                currentValue = vec.x;
                maxValue = vec.y;
            }
            else
            {
                currentValue = GetPropertyValue(property);
                if (!string.IsNullOrEmpty(barAttribute.MaxValueField))
                {
                    var maxValueProperty = property.serializedObject.FindProperty(barAttribute.MaxValueField);
                    maxValue = GetPropertyValue(maxValueProperty);
                }
                else
                {
                    maxValue = currentValue;
                }
            }

            float fillPercentage = maxValue != 0 ? currentValue / maxValue : 0;

            string barLabel = $"{currentValue:F2}/{maxValue:F2}";

            DrawEnhancedBar(barRect, fillPercentage, barLabel, GetColor(barAttribute.Color), barAttribute.BackgroundColor, barAttribute.IsVertical);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var barAttribute = (BarAttribute.BarAttribute)attribute;
            float barHeight = Mathf.Max(barAttribute.BarHeight, MinBarHeight);
            return PropertyHeight + barHeight + Spacing * 2;
        }

        private float GetPropertyValue(SerializedProperty property)
        {
            switch (property.propertyType)
            {
                case SerializedPropertyType.Integer:
                    return property.intValue;
                case SerializedPropertyType.Float:
                    return property.floatValue;
                default:
                    return 0f;
            }
        }

        private void DrawEnhancedBar(Rect position, float fillPercent, string label, Color barColor, Color backgroundColor, bool isVertical)
        {
            if (Event.current.type != EventType.Repaint)
                return;

            // Draw background
            EditorGUI.DrawRect(position, backgroundColor);

            // Draw fill
            Rect fillRect;
            if (isVertical)
            {
                fillRect = new Rect(position.x, position.y + position.height * (1 - fillPercent), position.width, position.height * fillPercent);
            }
            else
            {
                fillRect = new Rect(position.x, position.y, position.width * fillPercent, position.height);
            }
            EditorGUI.DrawRect(fillRect, barColor);

            // Draw border
            Rect borderRect = new Rect(position.x, position.y, position.width, position.height);
            EditorGUI.DrawRect(borderRect, new Color(0.5f, 0.5f, 0.5f, 0.5f));

            // Draw label
            GUIStyle labelStyle = new GUIStyle(EditorStyles.boldLabel)
            {
                alignment = TextAnchor.MiddleCenter,
                normal = { textColor = Color.white }
            };
            EditorGUI.DropShadowLabel(position, label, labelStyle);
        }

        private Color GetColor(BarColor color)
        {
            switch (color)
            {
                case BarColor.Red:
                    return new Color32(255, 0, 63, 255);
                case BarColor.Pink:
                    return new Color32(255, 152, 203, 255);
                case BarColor.Orange:
                    return new Color32(255, 128, 0, 255);
                case BarColor.Yellow:
                    return new Color32(255, 211, 0, 255);
                case BarColor.Green:
                    return new Color32(102, 255, 0, 255);
                case BarColor.Blue:
                    return new Color32(0, 135, 189, 255);
                case BarColor.Indigo:
                    return new Color32(75, 0, 130, 255);
                case BarColor.Violet:
                    return new Color32(127, 0, 255, 255);
                default:
                    return Color.white;
            }
        }
    }
}