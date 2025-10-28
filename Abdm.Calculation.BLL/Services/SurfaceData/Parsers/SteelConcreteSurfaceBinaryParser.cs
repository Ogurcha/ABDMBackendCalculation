using Abdm.Calculation.BLL.Interfaces;
using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.BLL.Models.SteelConcrete;
using Abdm.Calculation.DAL.Enums;

namespace Abdm.Calculation.BLL.Services.SurfaceData.Parsers
{
    public class SteelConcreteSurfaceBinaryParser : BaseSurfaceBinaryParser, ISurfaceBinaryParser
    {
        public override IList<StrainCalculationTypeEnum> StrainCalculationTypes =>
           new List<StrainCalculationTypeEnum> {
                 StrainCalculationTypeEnum.st40
           };

        public override SurfaceDataDto ParseData(SurfaceDataDto surface, BinaryReader reader, PassageInterval[] intervals)
        {
            base.ParseData(surface, reader, intervals);
            SkipSomeBytes(reader);

            surface.StrainTypeSpecificData = new SteelConcreteData
            {
                Rectangles = ReadRectangles(reader).ToArray(),
                Corners = ReadCorners(reader).ToArray()
            };

            return surface;
        }

        private IEnumerable<SteelConcreteDataRectangle> ReadRectangles(BinaryReader reader) {
            var count = reader.ReadInt16();
            for (int i = 0; i < count; i++)
            {
                yield return new SteelConcreteDataRectangle
                {
                    Width = reader.ReadDouble(),
                    Height = reader.ReadDouble(),
                    DHeight = reader.ReadDouble(),
                    Material = (SteelConcreteMaterialTypeEnum)reader.ReadInt16(),
                    Ar = reader.ReadDouble(),
                    dYr = reader.ReadDouble()
                };
            }
        }

        private IEnumerable<SteelConcreteDataCorner> ReadCorners(BinaryReader reader)
        {
            var count = reader.ReadInt16();
            for (int i = 0; i < count; i++)
            {
                yield return new SteelConcreteDataCorner
                {
                    Location = (SteelConcreteCornerLocationEnum)reader.ReadInt16(),
                    Width = reader.ReadDouble(),
                    Height = reader.ReadDouble(),
                    H2 = reader.ReadDouble(),
                };
            }
        }
    }
}
