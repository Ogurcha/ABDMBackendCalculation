using System.ComponentModel;
using System.Runtime.Serialization;

namespace Abdm.Calculation.BLL.Enums
{
    [DataContract]
    public enum DriveDirectionEnum
    {
        [EnumMember(Value = "Bidirection"), Description("Встречное")]
        Bidirection = 0,
        [EnumMember(Value = "Forward"), Description("Вперед")]
        Forward = 1,
        [EnumMember(Value = "Backward"), Description("Назад")]
        Backward = 2,
        [EnumMember(Value = "Closed"), Description("Закрыто")]
        Closed = 3
    }
}
