using Abdm.Calculation.BLL.Interfaces;
using Microsoft.Extensions.Logging;

namespace Abdm.Calculation.BLL.Coordinators
{
    public class WorkerWrapper<T, Param, Result>(
        T coordinator,
        ILogger<T> logger) : ICanWork<Param, Result> 
        where T : class, ICoordinator<Param, Result> where Result : class
    {
        public async Task<Result> Run(Param param, CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation(coordinator.InfoMsg(param));
                var workerThreadTask = () => coordinator.Run(param, cancellationToken);
                var result = await Task.Run(workerThreadTask, cancellationToken);
                if (result.IsSuccess && result.Result != null)
                {
                    return result.Result;
                }
                else
                {
                    if (result.Exception != null)
                    {
                        logger.LogError(
                            result.Exception,
                            coordinator.ErrorMsg(param));
                    }
                    return coordinator.GetFailedResult(param);
                }
            }
            catch (Exception e)
            {
                logger.LogError(coordinator.ExceptionMsg(param));
                logger.LogError(e.StackTrace);
                return coordinator.GetFailedResult(param);
            }
        }
    }
}
