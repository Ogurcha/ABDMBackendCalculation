using System.ComponentModel;
using System.Runtime.Serialization;

namespace Abdm.Calculation.BLL.Enums
{
    /// <summary>
    /// Как прокатывать ТС по поверхности влияния
    /// </summary>
    [DataContract]
    public enum DriveDirectionEnum
    {
        /// <summary>
        /// Прокатывать и передом, и задом
        /// </summary>
        [EnumMember(Value = "Bidirection"), Description("Встречное")]
        Bidirection = 0,

        /// <summary>
        /// Прокатывать передом
        /// </summary>
        [EnumMember(Value = "Forward"), Description("Вперед")]
        Forward = 1,

        /// <summary>
        /// Прокатывать задом
        /// </summary>
        [EnumMember(Value = "Backward"), Description("Назад")]
        Backward = 2,

        /// <summary>
        /// Неопределено
        /// </summary>
        [EnumMember(Value = "Closed"), Description("Закрыто")]
        Closed = 3
    }
}
