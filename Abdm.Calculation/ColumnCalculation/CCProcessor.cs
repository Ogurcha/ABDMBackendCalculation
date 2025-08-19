using System.Threading.Tasks;
using Abdm.Calculation.Models;

namespace Abdm.Calculation.ColumnCalculation
{

    /// <summary>
    /// todo: Обработка сообщения
    /// валидация
    /// Расчет колонок
    /// Расчет напряжения
    /// Отправление сообщения в брокер
    /// </summary>
    public class CCProcessor : ICCProcessor
    {
        
        public async Task<CCResultMessage> Process(CCRequestMessage data)
        {
            return await Task.FromResult<CCResultMessage>(new CCResultMessage());
        }
    }
}
