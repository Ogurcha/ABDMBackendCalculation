namespace Abdm.Calculation.BLL.Interfaces
{
    public interface ICanWork<Param, Result>
    {
        Task<Result> Run(Param param, CancellationToken cancellationToken);
    }
}
