using Abdm.Calculation.BLL.Enums;
using Abdm.Calculation.BLL.Interfaces;
using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.Maths.Models;

namespace Abdm.Calculation.BLL.Services.SurfaceData.Parsers
{
    public class BaseSurfaceBinaryParser : ISurfaceBinaryParser
    {
        public virtual IList<StrainCalculationGroupTypeEnum> StrainCalculationTypes => 
        [
            StrainCalculationGroupTypeEnum.Default
        ];

        public virtual SurfaceDataDto ParseData(SurfaceDataDto surface, BinaryReader reader, PassageInterval[] intervals)
        {
            SkipSomeBytes(reader);
            surface.IsSymmetric = reader.ReadBoolean();
            surface.IsGridRegular = reader.ReadBoolean();
            surface.PointsCount = reader.ReadInt32();
            surface.Points = ReadPoints3D(reader, surface.PointsCount).ToArray();
            surface.TrianglesCount = reader.ReadInt32();
            surface.Triangles = surface.TrianglesCount > 0
                ? ReadTriangles(reader, surface.TrianglesCount, surface.PointsCount).ToArray()
                : null;

            return surface;
        }

        protected IEnumerable<Vector3D> ReadPointsYZ(BinaryReader reader, int pointsToRead)
        {
            for (int i = 0; i < pointsToRead; i++)
            {
                yield return new((double)default, reader.ReadDouble(), reader.ReadDouble());
            }
        }

        protected IEnumerable<Vector3D> ReadPoints3D(BinaryReader reader, int pointsToRead)
        {
            for (int i = 0; i < pointsToRead; i++)
            {
                yield return new(reader.ReadDouble(), reader.ReadDouble(), reader.ReadDouble());
            }
        }

        protected IEnumerable<Vector3I> ReadTriangles(BinaryReader reader, int trianglesToRead, int pointsCount)
        {
            for (int i = 0; i < trianglesToRead; i++)
            {
                var p1 = reader.ReadInt32();
                var p2 = reader.ReadInt32();
                var p3 = reader.ReadInt32();
                if (IsValidTriangle(pointsCount, p1, p2, p3))
                {
                    yield return (p1, p2, p3);
                }
            }
        }

        protected bool IsValidTriangle(int pointsCount, int p1, int p2, int p3)
        {
            return p1 != p2 && p2 != p3 && p3 != p1 && p1 < pointsCount && p2 < pointsCount && p3 < pointsCount;
        }

        protected void SkipSomeBytes(BinaryReader reader)
        {
            var strainType = reader.ReadInt16();
            var bytesCount = reader.ReadInt32();
        }
    }
}
