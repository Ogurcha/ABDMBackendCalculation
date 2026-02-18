namespace Abdm.Calculation.BLL.Interfaces
{
    public interface IWorkerWrapper<Param, Result>
    {
        Task<Result> Run(Param param, CancellationToken cancellationToken);
    }
}
