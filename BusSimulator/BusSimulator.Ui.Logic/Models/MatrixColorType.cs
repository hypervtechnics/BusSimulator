using BusSimulator.Core.Utils;

using System.ComponentModel;

namespace BusSimulator.Ui.Logic.Models
{
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum MatrixColorType
    {
        [Description("Green")]
        Green,

        [Description("Yellow")]
        Yellow,

        [Description("Orange")]
        Orange,

        [Description("White")]
        White,

        [Description("Light blue")]
        LightBlue
    }
}
