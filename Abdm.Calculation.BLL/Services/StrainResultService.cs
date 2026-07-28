using Abdm.Calculation.BLL.Interfaces;
using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.BLL.Models.Strain;
using static Npgsql.Replication.PgOutput.Messages.RelationMessage;

namespace Abdm.Calculation.BLL.Services
{
    public class StrainResultService(
        IStrainCalculator strainCalculator,
        IStrainSelector strainSelector,
        IStrainResultPopulator strainResultPopulator) : IStrainResultService
    {
        public StrainResult[] GetStrainResults(
            VehicleRollingBigModel data,
            IEnumerable<IntervalModel> intervals)
        {
            var strainMaps = intervals.SelectMany(i => strainCalculator.GenerateStrainsMap(i, data)).ToArray();

            var unpopulated = strainSelector.SelectBestStrainResult(strainMaps, data).ToArray();

            var populated = strainResultPopulator.PopulateStrainResults(unpopulated, data.Data);

            return ApplyStripedCoefficient(populated, data.Data);
        }

        private StrainResult[] ApplyStripedCoefficient(List<StrainResult> populated, VehicleRollingSmallModel data)
        {
            var strainResults = populated.ToArray();

            foreach (var strainResult in strainResults)
            {
                strainResult.VehicleColumnStrains = strainResult.VehicleColumnStrains.OrderDescending().ToArray();
                for (int i = 0; i < strainResult.VehicleColumnStrains.Length; i++)
                {
                    var column = strainResult.VehicleColumnStrains[i];
                    var lambda = column.VehicleStrains.First().LambdaSmall;
                    var coefficients = data.CoefficientProvider.GetStripeCoefficient(lambda);
                    var coefficientToPick = Math.Min(4, i);
                    column.StripeCoefficient = coefficients[coefficientToPick];
                    if (column.TrafficJamStrain != null && data.CoefficientProvider.TrafficJamStrainCoefficientProvider != null)
                    {
                        var trafficjamCoefficients = data.CoefficientProvider.TrafficJamStrainCoefficientProvider.GetStripeCoefficient(lambda);
                        column.TrafficJamStripeCoefficient = trafficjamCoefficients[coefficientToPick];
                    }

                    column.TotalStrain = column.VehicleStrains.Sum(x => x.TotalStrain) * column.StripeCoefficient + (column.TrafficJamStrain?.TotalStrain * column.TrafficJamStripeCoefficient ?? 0d);
                }
            }

            return strainResults;
        }
    }
}
