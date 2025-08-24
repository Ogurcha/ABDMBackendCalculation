using System.Linq;
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
            var sp = data.Surface.SurfacePoints;

            var b = sp.Where(p => p.X == 1.85f).ToList();

            return await Task.FromResult<PTCResultMessage>(new PTCResultMessage());
        }
    }
}
