using Abdm.Calculation.BLL.Models;

namespace Abdm.Calculation.BLL.Services.PassTypes.PassTypeConditions
{
    public interface IPassTypeCondition
    {
        bool CanPassCondition(List<StrainResult> columnList, Surface surface);
    }
}
