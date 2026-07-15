using System.ComponentModel;
using System.Runtime.Serialization;

namespace AisIssoEnum
{
    [DataContract]
    public enum ais7DriveDirection
    {
        [EnumMember(Value = "Forward"), Description("Вперед")]
        Forward = 20,
        [EnumMember(Value = "Backward"), Description("Назад")]
        Backward = 30,
        [EnumMember(Value = "Bidirection"), Description("Встречное")]
        Bidirection = 10,
        [EnumMember(Value = "Closed"), Description("Закрыто")]
        Closed = 40
    }
}
