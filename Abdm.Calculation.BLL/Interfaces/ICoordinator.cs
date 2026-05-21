using Abdm.Calculation.BLL.Models.DataTransfer;

namespace Abdm.Calculation.BLL.Interfaces
{
    public interface ICoordinator<Param, Result> : ICanWork<Param, ResultMonad<Result>> where Result : class
    {
        Result GetFailedResult(Param param);

        string InfoMsg(Param param);

        string ErrorMsg(Param param);

        string ExceptionMsg(Param param);
    }
}
