using Abdm.Calculation.BLL.Interfaces;
using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.DAL.Entities;

namespace Abdm.Calculation.BLL.Services
{
    public class PillarDataService : IPillarDataService
    {

        public void UpdateSurfaceDataFromPillarData(SurfaceDataDto surface, PassageInterval[] passageIntervals)
        {
            if (surface.StrainCalculationType != DAL.Enums.StrainCalculationTypeEnum.st70)
            {
                return;
            }
            var start = passageIntervals.Select(x => x.AbsolutePositionLeft).Min();
            var finish = passageIntervals.Select(x => x.AbsolutePositionRight).Max();

            surface.Points = surface.Points.Select(p => (start, p.X, p.Y)).Concat(surface.Points.Select(p => (finish, p.X, p.Y))).ToArray();
            var triangles = new List<(int, int, int)>();
            for (var i = 0; i < surface.Points.Length - 2; i++)
            {
                triangles.Add((i, i + 1, i + 2));
            }
            surface.Triangles = triangles.ToArray();
        }
    }
}
