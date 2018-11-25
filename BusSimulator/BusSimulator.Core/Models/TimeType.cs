using BusSimulator.Core.Utils;

using System.ComponentModel;

namespace BusSimulator.Core.Models
{
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum TimeType
    {
        [Description("Night")]
        Night,

        [Description("Special")]
        Special,

        [Description("Normal")]
        Normal,

        [Description("Weekend")]
        Weekend
    }
}
