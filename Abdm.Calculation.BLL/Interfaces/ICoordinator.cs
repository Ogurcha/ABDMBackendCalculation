using Abdm.Calculation.BLL.Models.DataTransfer;

namespace Abdm.Calculation.BLL.Interfaces
{
    public interface ICoordinator<Param, Result> where Result : class
    {
        Task<ResultExceptionContainer<Result>> Run(Param param, CancellationToken cancellationToken);

        Result GetFailedResult(Param param);

        string InfoMsg(Param param);

        string ErrorMsg(Param param);

        string ExceptionMsg(Param param);
    }
}
