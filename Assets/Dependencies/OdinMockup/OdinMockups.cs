#if USE_ODIN_MOCKUP
using System;
using System.Diagnostics;

namespace Sirenix.OdinInspector
{
    [Conditional("UNITY_EDITOR")]
    public sealed class ProgressBarAttribute : Attribute
    {
        public ProgressBarAttribute(double min, double max)
        {
        }
    }
    
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
    [Conditional("UNITY_EDITOR")]
    public sealed class ReadOnlyAttribute : Attribute
    {
    }
    
    [Conditional("UNITY_EDITOR")]
    public class EnumToggleButtonsAttribute : Attribute
    {
    }
    
    [AttributeUsage(AttributeTargets.All)]
    [Conditional("UNITY_EDITOR")]
    public class HideLabelAttribute : Attribute
    {
    }

    [Conditional("UNITY_EDITOR")]
    public sealed class PropertyRangeAttribute : Attribute
    {
        public PropertyRangeAttribute(string min, string max)
        {
        }
    }

    [Conditional("UNITY_EDITOR")]
    public class GUIColorAttribute : Attribute
    {
        public GUIColorAttribute(float r, float g, float b)
        {
        }
    }

    [Conditional("UNITY_EDITOR")]
    public sealed class TitleGroupAttribute : Attribute
    {
        public TitleGroupAttribute(string s1)
        {
        }
        
        public TitleGroupAttribute(string s1, string s2)
        {
        }
    }
    
    [Conditional("UNITY_EDITOR")]
    public sealed class VerticalGroupAttribute : Attribute
    {
        public VerticalGroupAttribute(string s1)
        {
        }
    }
    
    [Conditional("UNITY_EDITOR")]
    public sealed class HorizontalGroupAttribute : Attribute
    {
        public int LabelWidth;
        public float Width;
        
        public HorizontalGroupAttribute(string s1)
        {
        }
    }
    
    [Conditional("UNITY_EDITOR")]
    public sealed class ButtonAttribute : Attribute
    {
        public string Name;
        public ButtonStyle Style;
        
        public ButtonAttribute(ButtonSizes size)
        {
        }
        
        public ButtonAttribute(string s1, ButtonSizes size)
        {
        }
    }
    
    public enum ButtonStyle
    {
        CompactBox,
        FoldoutButton,
        Box,
    }
    
    public enum ButtonSizes
    {
        Small = 0,
        Medium = 1,
        Large = 2,
        Gigantic = 3,
    }
    
    [Conditional("UNITY_EDITOR")]
    public sealed class OnValueChangedAttribute : Attribute
    {
        public OnValueChangedAttribute(string s1)
        {
        }
    }
    
    [Conditional("UNITY_EDITOR")]
    public sealed class EnableIfAttribute : Attribute
    {
        public EnableIfAttribute(string s1)
        {
        }
    }
    
    [Conditional("UNITY_EDITOR")]
    public sealed class PropertySpaceAttribute : Attribute
    {
        public int SpaceAfter;
    }
}
#endif