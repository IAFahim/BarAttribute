using UnityEngine;

namespace BarAttribute
{
    public class BarAttribute : PropertyAttribute
    {
        public string MaxValueField { get; private set; }
        public BarColor Color { get; private set; }
        public Color BackgroundColor { get; private set; }
        public bool IsVertical { get; private set; }
        public float BarHeight { get; private set; }

        public BarAttribute(BarColor color = BarColor.Red, bool isVertical = false, float barHeight = 18f,
            float r = 0.2f, float g = 0.2f, float b = 0.2f, float a = 1f)
        {
            Color = color;
            IsVertical = isVertical;
            BarHeight = barHeight;
            BackgroundColor = new Color(r, g, b, a);
        }

        public BarAttribute(string maxValueField, BarColor color = BarColor.Red, bool isVertical = false, float barHeight = 18f,
            float r = 0.2f, float g = 0.2f, float b = 0.2f, float a = 1f)
            : this(color, isVertical, barHeight, r, g, b, a)
        {
            MaxValueField = maxValueField;
        }
    }
}