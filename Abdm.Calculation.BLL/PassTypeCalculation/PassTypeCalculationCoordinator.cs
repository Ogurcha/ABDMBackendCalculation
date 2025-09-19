using Abdm.Calculation.BLL.Enums;
using Abdm.Calculation.BLL.Interfaces;
using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.BLL.PassTypeCalculation.PassTypeConditions;
using Abdm.Calculation.DAL.Entities;
using Abdm.Calculation.Graphics;
using Abdm.Calculation.Graphics.Models;

namespace Abdm.Calculation.BLL.PassTypeCalculation
{
    public class PassTypeCalculationCoordinator (
        IPassageIntervalService passageIntervalManager,
        ISurfaceDataService surfaceDataService,
        IMeshManager meshManager,
        IRoadRulesFactory roadRulesFactory,
        IStrainService strainManager
        ) : IPassTypeCalculationCoordinator
    {
        private const string meshErrorMessage = "Mesh construction failed";
        private const string passageIntervalErrorMessage = "Passage intervals for this isso have not been found";
        private const string surfaceDataNotFound = "Surface data for given isso and checkpoint was not found";
        private const string roadRulesNotFound = "Road rules for given lading were not found";

        /// <summary>
        /// Коэффициент при динамическом движении на иссо
        /// TODO: ABDMP-359 - реализация сервиса расчётов динамического/статического коеффициента
        /// На самом деле он не статический
        /// </summary>
        public static double DynamicCoefficient = 1.3d;

        public List<(IPassTypeCondition condition, PassTypeEnum passType)> PassTypeConditions =
            new List<(IPassTypeCondition condition, PassTypeEnum passType)>
            {
                (new NoLimitCondition(), PassTypeEnum.NoLimit),
                (new WithoutPedestrianCondition(), PassTypeEnum.WithoutPedestian),
                (new Speed10Condition(), PassTypeEnum.MaxSpeed10),
                (new SingleAutoOnlyCondition(), PassTypeEnum.SingleAutoOnly)
            };

        public async Task<ResultExceptionContainer<PTCResultMessage>> GetPassType(PTCRequestMessage data)
        {
            var intervals = await passageIntervalManager.GetPassageIntervals(data.IssoId);
            if (intervals?.Any() != true)
            {
                return new ResultExceptionContainer<PTCResultMessage>(new Exception(passageIntervalErrorMessage));
            }
            var surfaceData = await surfaceDataService.GetSurfaceData(data.IssoId, data.CPNumber);
            //TODO: ABDMP-357 - Реализация триангуляции, если ничего не пришло. Запись новой триангуляции обратно в бд
            if (surfaceData?.Triangles == null)
            {
                return new ResultExceptionContainer<PTCResultMessage>(new Exception(surfaceDataNotFound));
            }

            //TODO: ABDMP-360 - реализация кастомных нагрузок LadingSchema.Id, подгрузка их из бд
            var roadRulesNullable = roadRulesFactory.CreateRoadRuleStrategy(data.LadingSchema.Id);
            if (!(roadRulesNullable is RoadRules roadRules))
            {
                return new ResultExceptionContainer<PTCResultMessage>(new Exception(roadRulesNotFound));
            }

            var mesh = meshManager.GetMeshFromPoints(surfaceData.Points, surfaceData.Triangles);
            if (mesh?.Data?.DistinctXs == null || mesh.Data.DistinctYs == null)
            {
                return new ResultExceptionContainer<PTCResultMessage>(new Exception(meshErrorMessage));
            }

            var columnList = new List<ColumnModel>();
            foreach (var interval in intervals)
            {
                var column = new ColumnModel(interval);
                columnList.Add(column);

                column.Xs = passageIntervalManager.CalculateDistinctXPositionsIncludingWheelOffsets(
                mesh.Data.DistinctXs,
                interval,
                data.LadingSchema.Axles,
                data.LadingSchema.Width ?? roadRules.MinColumnDistance
                );
                column.Points = new SmoothPoints[column.Xs.Length];
                column.Strain = new double[column.Xs.Length];
                column.StrainOneAuto = new double[column.Xs.Length];
                
                for (var i = 0; i < column.Xs.Length; i++)
                {
                    var X = column.Xs[i];

                    var profileYZ = meshManager.MakeProfileYZ(mesh, X);
                    if (!(profileYZ?.Any() == true))
                    {
                        continue;
                    }

                    var smoothPoints = meshManager.CreateSmoothPoints(profileYZ.ToArray());
                    column.Points[i] = smoothPoints;

                    var strainList = mesh.Data.DistinctYs
                        .Select(Y => strainManager.GetStrain(data, smoothPoints, Y))
                        .Order().ToList();

                    //TODO: ABDMP-358 - Учитывать расстояние между авто. Пока будем считать, что они могут стоять друг на друге. Пока забьем на расстояние между ними, и то, что они все не поместятся на иссо, так как это в любом случае не приведёт к ложно положительному прогнозу
                    for (int j = 0; j < roadRules.MaxAutoInColumn; j++)
                    {
                        var highestStrain = strainList.Last();
                        if (j == 0)
                        {
                            column.StrainOneAuto[i] += highestStrain;
                        }

                        column.Strain[i] += highestStrain;
                        strainList.Remove(highestStrain);
                    }
                }
            }

            var resultPassType = GetPassType(data, roadRules, columnList);

            PTCResultMessage response = ComposeMessage(resultPassType, data, intervals);

            return new ResultExceptionContainer<PTCResultMessage>(response);
        }

        private PassTypeEnum GetPassType(PTCRequestMessage data, RoadRules roadRules, List<ColumnModel> columnList)
        { 
            columnList = columnList.OrderByDescending(c => c.Strain).ToList();

            foreach (var c in PassTypeConditions)
            {
                if (c.condition.CanPassCondition(columnList, data.Surface, roadRules))
                {
                    return c.passType;
                }
            }

            return PassTypeEnum.Denied;
        }

        private PTCResultMessage ComposeMessage(PassTypeEnum resultPassType, PTCRequestMessage data, PassageInterval[] intervals)
        {
            AllowedEnum allowed = resultPassType switch
            {
                PassTypeEnum.NoLimit => AllowedEnum.Allowed,
                PassTypeEnum.WithoutPedestian 
                or PassTypeEnum.MaxSpeed10 
                or PassTypeEnum.SingleAutoOnly 
                or PassTypeEnum.SingleOnlyAndPlace => AllowedEnum.Restricted,
                PassTypeEnum.Denied => AllowedEnum.Denied,
                PassTypeEnum.Unknown or _ => AllowedEnum.Denied,
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

        public PTCResultMessage GetFailedResponse(PTCRequestMessage? data)
        {
            if (data == null)
            {
                return new PTCResultMessage
                {
                    IssoId = default,
                    CPNumber = default,
                    Allowed = AllowedEnum.Undefined,
                    Intervals = [],
                    LadingId = default,
                    Direction = default,
                    Snip = default,
                    PassType = PassTypeEnum.Unknown
                };
            }
            else
            {
                return new PTCResultMessage
                {
                    IssoId = data.IssoId,
                    CPNumber = data.CPNumber,
                    Allowed = AllowedEnum.Undefined,
                    Intervals = [],
                    LadingId = data.LadingId,
                    Direction = data.Direction,
                    Snip = data.Snip,
                    PassType = PassTypeEnum.Unknown
                };
            } 
        }
    }
}
