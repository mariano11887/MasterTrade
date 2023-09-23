using System.ComponentModel;

namespace _4.DTO.Enums
{
    public enum Comparer
    {
        [Description("igual a")]
        Equal = 1,

        [Description("menor que")]
        Lower = 2,

        [Description("menor o igual a")]
        LowerOrEqual = 3,

        [Description("mayor que")]
        Greater = 4,

        [Description("mayor o igual a")]
        GreaterOrEqual = 5
    }
}
