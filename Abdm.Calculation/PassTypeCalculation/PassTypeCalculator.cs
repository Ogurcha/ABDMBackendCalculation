using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abdm.Calculation.BLL.IntervalCalculation;
using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.BLL.RoadRules;
using Abdm.Calculation.BLL.StrainCalculation;
using Abdm.Calculation.DAL.Entities;
using Abdm.Calculation.Graphics;

namespace Abdm.Calculation.ColumnCalculation
{
    public class PassTypeCalculator (
        IPassageIntervalManager passageIntervalManager,
        IMeshManager meshManager,
        IRoadRulesManager roadRulesManager,
        IStrainManager strainManager
        ) : IPassTypeCalculator
    {
        private const string meshErrorMessage = "Mesh construction failed";
        private const string passageIntervalErrormessage = "Passage intervals for this isso have not been found";

        /// <summary>
        /// Коэффициент при динамическом движении на иссо
        /// </summary>
        private const double DynamicCoefficient = 1.3d;

        public async Task<PTCResultMessage> CalculatePassType(PTCRequestMessage data)
        {
            var intervals = await passageIntervalManager.GetPassageIntervals(data.IssoId);
            if (intervals?.Any() != true)
            {
                throw new Exception(passageIntervalErrormessage);
            }

            var roadRules = roadRulesManager.RefreshRoadRules(data.IssoId, data.LadingSchema.Id);

            var mesh = meshManager.GetMeshFromPoints(data.Surface.SurfacePoints);
            if (mesh?.Data?.DistinctXs == null || mesh.Data.DistinctYs == null)
            {
                throw new Exception(meshErrorMessage);
            }

            var columnList = new List<Column>();
            foreach (var interval in intervals)
            {
                var column = new Column(interval);
                columnList.Add(column);

                column.Xs = passageIntervalManager.GetDistinctXsWithWheels(
                mesh.Data.DistinctXs,
                interval,
                data.LadingSchema.Axles,
                data.LadingSchema.Width
                );
                column.Points = new Graphics.Entities.SmoothPoints[column.Xs.Length];
                column.Strain = new double[column.Xs.Length];
                column.StrainOneAuto = new double[column.Xs.Length];
                
                for (var i = 0; i < column.Xs.Length; i++)
                {
                    var X = column.Xs[i];

                    var profileYZ = meshManager.MakeProfileYZ(mesh, X);

                    var smoothPoints = meshManager.CreateSmoothPoints(profileYZ.ToArray());
                    column.Points[i] = smoothPoints;

                    var strainList = mesh.Data.DistinctYs
                        .Select(Y => strainManager.GetStrain(data, smoothPoints, Y))
                        .OrderDescending().ToList();

                    //TODO: Учитывать расстояние между авто. Пока будем считать, что они могут стоять друг на друге. Пока забьем на расстояние между ними, и то, что они все не поместятся на иссо, так как это в любом случае не приведёт к ложно положительному прогнозу
                    for (int j = 0; j < roadRules.MaxAutoInColumn; i++)
                    {
                        if (j == 0)
                        {
                            column.StrainOneAuto[i] += strainList.First();
                        }

                        column.Strain[i] += strainList.First();
                        strainList.RemoveAt(0);
                    }
                }
            }

            var resultPassType = GetPassType(data, roadRules, columnList);

            PTCResultMessage response = ComposeMessage(resultPassType, data, intervals);

            return response;
        }

        private PassTypeEnum GetPassType(PTCRequestMessage data, RoadRules roadRules, List<Column> columnList)
        { 
            columnList = columnList.OrderByDescending(c => c.Strain).ToList();
            if (CheckNoLimitCondition(columnList, data.Surface, roadRules))
            {
                return PassTypeEnum.NoLimit;
            }
            if (CheckWithoutPedestianCondition(columnList, data.Surface, roadRules))
            {
                return PassTypeEnum.WithoutPedestian;
            }
            if (CheckMaxSpeed10Condition(columnList, data.Surface, roadRules))
            {
                return PassTypeEnum.MaxSpeed10;
            }
            if (CheckSingleAutoOnlyCondition(columnList, data.Surface, roadRules))
            {
                return PassTypeEnum.SingleAutoOnly;
            }

            return PassTypeEnum.Denied;
        }

        private bool CheckNoLimitCondition(
            List<Column> columnList, 
            Surface surface, 
            RoadRules roadRules)
        {
            var totalColumns = Math.Min(roadRules.MaxColumnCount, columnList.Count);

            var dynamicLoad = columnList.Take(totalColumns).Sum(c => c.Strain?.Max());

            dynamicLoad *= DynamicCoefficient;

            return surface.MyStrength > surface.СonstLoad + surface.PedestrianLoad + surface.OtherLoad + dynamicLoad;
        }

        private bool CheckWithoutPedestianCondition(List<Column> columnList, Surface surface, RoadRules roadRules)
        {
            var totalColumns = Math.Min(roadRules.MaxColumnCount, columnList.Count);

            var dynamicLoad = columnList.Take(totalColumns).Sum(c => c.Strain?.Max());

            dynamicLoad *= DynamicCoefficient;

            return surface.MyStrength > surface.СonstLoad + surface.OtherLoad + dynamicLoad;
        }

        private bool CheckMaxSpeed10Condition(List<Column> columnList, Surface surface, RoadRules roadRules)
        {
            var totalColumns = Math.Min(roadRules.MaxColumnCount, columnList.Count);

            var dynamicLoad = columnList.Take(totalColumns).Sum(c => c.Strain?.Max());

            return surface.MyStrength > surface.СonstLoad + surface.OtherLoad + dynamicLoad;
        }

        private bool CheckSingleAutoOnlyCondition(List<Column> columnList, Surface surface, RoadRules roadRules)
        {
            var totalColumns = Math.Min(roadRules.MaxColumnCount, columnList.Count);

            var dynamicLoad = columnList.Take(totalColumns).Sum(c => c.StrainOneAuto?.Max());

            return surface.MyStrength > surface.СonstLoad + surface.OtherLoad + dynamicLoad;
        }

        private PTCResultMessage ComposeMessage(PassTypeEnum resultPassType, PTCRequestMessage data, PassageInterval[] intervals)
        {
            AllowedEnum? allowed = resultPassType switch
            {
                PassTypeEnum.NoLimit => AllowedEnum.Allowed,
                PassTypeEnum.WithoutPedestian 
                or PassTypeEnum.MaxSpeed10 
                or PassTypeEnum.SingleAutoOnly 
                or PassTypeEnum.SingleOnlyAndPlace => AllowedEnum.Restricted,
                PassTypeEnum.Denied => AllowedEnum.Denied,
                PassTypeEnum.Unknown or _ => null
            };

            return new PTCResultMessage
            {
                Allowed = allowed,
                CPNumber = data.CPNumber,
                Direction = data.Direction,
                Intervals = intervals.SelectMany(i => i?.SafeInterval ?? []).ToArray(),
                IssoId = data.IssoId,
                PassType = resultPassType,
                LadingId = data.LadingId
            };
        }
    }
}
