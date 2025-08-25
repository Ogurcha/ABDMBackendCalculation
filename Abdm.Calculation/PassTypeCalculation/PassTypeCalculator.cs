using System.Linq;
using System.Threading.Tasks;
using Abdm.Calculation.DAL;
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
    public class PassTypeCalculator (
        IPassageIntervalRepository passageIntervalRepository
        ) : IPassTypeCalculator
    {
        
        public async Task<PTCResultMessage> CalculatePassType(PTCRequestMessage data)
        {
            var interval = await passageIntervalRepository.GetPassageIntervals(3800031);

            return await Task.FromResult<PTCResultMessage>(new PTCResultMessage());
        }


    }
}
