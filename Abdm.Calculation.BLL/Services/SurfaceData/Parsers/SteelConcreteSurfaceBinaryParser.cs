using Abdm.Calculation.BLL.Enums;
using Abdm.Calculation.BLL.Interfaces;
using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.SteelConcrete.Enums;
using Abdm.Calculation.SteelConcrete.Helpers;
using Abdm.Calculation.SteelConcrete.Models;
using Abdm.Calculation.SteelConcrete.SteelConcrete;

namespace Abdm.Calculation.BLL.Services.SurfaceData.Parsers
{
    public class SteelConcreteSurfaceBinaryParser : BaseSurfaceBinaryParser, ISurfaceBinaryParser
    {
        public override IList<StrainCalculationGroupTypeEnum> StrainCalculationTypes =>
        [
            StrainCalculationGroupTypeEnum.SteelConcrete
        ];

        public override SurfaceDataDto ParseData(SurfaceDataDto surface, BinaryReader reader, PassageInterval[] intervals)
        {
            base.ParseData(surface, reader, intervals);
            SkipSomeBytes(reader);

            surface.StrainTypeSpecificData = new SteelConcreteData
            { 
                CrossSection = new CrossSection
                {
                    Rectangles = ReadRectangles(reader).ToArray(),
                    Corners = ReadCorners(reader).ToArray()
                },
            };

            return surface;
        }

        private IEnumerable<Rectangle> ReadRectangles(BinaryReader reader) {
            var count = reader.ReadInt16();
            for (int i = 0; i < count; i++)
            {
                yield return new Rectangle
                {
                    Width = reader.ReadDouble(),
                    Height = reader.ReadDouble(),
                    DHeight = reader.ReadDouble(),
                    Material = (MaterialEnum)reader.ReadInt16(),
                    Ar = reader.ReadDouble(),
                    dYr = reader.ReadDouble()
                };
            }
        }

        private IEnumerable<Corner> ReadCorners(BinaryReader reader)
        {
            var count = reader.ReadInt16();
            for (int i = 0; i < count; i++)
            {
                yield return new Corner
                {
                    Location = (CornerLocationEnum)reader.ReadInt16(),
                    Width = reader.ReadDouble(),
                    Height = reader.ReadDouble(),
                    H2 = reader.ReadDouble(),
                };
            }
        }
    }
}
