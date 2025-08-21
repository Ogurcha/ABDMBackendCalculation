using System.Threading.Tasks;
using Abdm.Calculation.PassTypeCalculation.DTO;

namespace Abdm.Calculation.ColumnCalculation
{

    /// <summary>
    /// todo: Обработка сообщения
    /// валидация
    /// Расчет колонок
    /// Расчет напряжения
    /// Отправление сообщения в брокер
    /// </summary>
    public class PTCProcessor : IPTCProcessor
    {
        
        public async Task<PTCResultMessage> Process(PTCRequestMessage data)
        {
            var a = data.Surface.SurfacePoints.Length;

            return await Task.FromResult<PTCResultMessage>(new PTCResultMessage());
        }
    }
}
