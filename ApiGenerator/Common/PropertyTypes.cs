using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel; 

namespace ApiGenerator.Common;


public enum PrimitiveType
{
    [Description("int")]
    Int,

    [Description("long")]
    Long,

    [Description("float")]
    Float,

    [Description("double")]
    Double,

    [Description("decimal")]
    Decimal,

    [Description("bool")]
    Bool,

    [Description("string")]
    String,

    [Description("DateTime")]
    DateTime,
}
public static class EnumExtensions
{
    public static string GetDescription(this Enum value)
    {
        var field = value.GetType().GetField(value.ToString());
        var attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;

        return attribute == null ? value.ToString().ToLower() : attribute.Description;
    }
}