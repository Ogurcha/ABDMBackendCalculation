using Abdm.Calculation.BLL.Interfaces;
using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.DAL;
using Mapster;

namespace Abdm.Calculation.BLL.Services
{
    public class PassageIntervalService(IPassageIntervalRepository passageIntervalRepository) : IPassageIntervalService
    {
        /// <summary>
        /// Возвращает абсолютные значения  интервалов для данного иссо
        /// </summary>
        public async Task<PassageInterval[]> GetPassageIntervals(long issoId, 
            double globalPositionShift, 
            CancellationToken cancellationToken)
        {
            var queryResult = await passageIntervalRepository.GetPassageIntervals(issoId, cancellationToken);
            var passageIntervals = queryResult.Adapt<PassageInterval[]>();

            var filteredIntervals = FilterIntervals(passageIntervals);

            double rightSideExtraShift = default;
            var right = filteredIntervals.Where(x => x.Type == Enums.PassageIntervalTypeEnum.RightInterval).FirstOrDefault();
            if (right != null)
            {
                var fenceSize = right.AbsolutePositionLeft;
                var leftSideSize = filteredIntervals.Where(x => x.Type == Enums.PassageIntervalTypeEnum.LeftInterval)
                    .First().TotalWidth;
                rightSideExtraShift = fenceSize + leftSideSize;
            }

            foreach (var intervalModel in filteredIntervals)
            {
                if (intervalModel.Type != Enums.PassageIntervalTypeEnum.RightInterval)
                {
                    intervalModel.AbsolutePositionLeft = globalPositionShift;
                    intervalModel.AbsolutePositionRight = globalPositionShift + intervalModel.TotalWidth;
                }
                else
                {
                    intervalModel.AbsolutePositionLeft = globalPositionShift + rightSideExtraShift;
                    intervalModel.AbsolutePositionRight = globalPositionShift + rightSideExtraShift + intervalModel.TotalWidth;
                }
            }

            return filteredIntervals;
        }

        /// <summary>
        /// Фильтрация, чтобы избавиться от дублей:
        /// в бд странно хранятся интервалы. 
        /// Один и тот же промежуток может быть записан два раза
        /// (Как одинарный интервал и как сумма двух интервалов). 
        /// Есть догадка, что дубли связаны с тем, 
        /// что если интервал содержит две полосы 
        /// и на нем нет ограждений, то этот интервал 
        /// можно использовать как двуполосное движение 
        /// для маеленьких машин, так и однополосное для больших
        /// </summary>
        private PassageInterval[] FilterIntervals(PassageInterval[]? passageIntervals)
        {
            if (passageIntervals?.Any() != true)
            {
                return [];
            }
            var left = passageIntervals.Where(x => x.Type == Enums.PassageIntervalTypeEnum.LeftInterval);
            var right = passageIntervals.Where(x => x.Type == Enums.PassageIntervalTypeEnum.RightInterval);
            var whole = passageIntervals.Where(x => x.Type == Enums.PassageIntervalTypeEnum.WholeInterval);
            if (
                left.Count() > 0 && 
                left.Count() == right.Count()
                )
            {
                return left.Concat(right).ToArray();
            }
            return whole.ToArray();
        }
    }
}
