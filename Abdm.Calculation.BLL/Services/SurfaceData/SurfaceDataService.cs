using Abdm.Calculation.BLL.Interfaces;
using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.BLL.Models.DataTransfer;
using Abdm.Calculation.DAL;
using Mapster;

namespace Abdm.Calculation.BLL.Services.SurfaceData
{
    public class SurfaceDataService(ISurfaceRepository repository, 
        ISurfaceBinaryParserFactory surfaceBinaryParserFactory) : ISurfaceDataService
    {
        /// <summary>
        /// старый "толстый клиент" сохраняет в первых 10-ти байтах: 
        /// "0x02" или "0x01" в UTF-8 - это их версии парсера, плюс Int16 число
        /// </summary>
        private const int UsefulDataStartingPosition = 10;

        /// <summary>
        /// Проверка списанная со старого клиента
        /// </summary>
        private const int OldClientFormatCondition = 10;

        private const string UnsupportedBinaryTypeStr = "Unsupported binary format";

        private const string CantFindParserStr = "Can't find binary parser for this surface data format";

        /// <summary>
        /// Расшифровывает байт массив и получает информацию о поверхности влияния
        /// </summary>
        public async Task<ResultExceptionContainer<SurfaceDataDto>> GetSurfaceData(long issoId,
            int checkpointNumber,
            PassageInterval[] intervals,
            CancellationToken cancellationToken)
        {
            var data = await repository.GetSurfaceData(issoId, checkpointNumber, cancellationToken);
            if (data?.data == null|| data?.data.Length <= UsefulDataStartingPosition)
            {
                return new ResultExceptionContainer<SurfaceDataDto>(new Exception(UnsupportedBinaryTypeStr));
            }
            using MemoryStream stream = new MemoryStream(data!.data);
            using BinaryReader reader = new BinaryReader(stream);
            if (reader.ReadInt32() > OldClientFormatCondition)
            {
                return new ResultExceptionContainer<SurfaceDataDto>(new Exception(UnsupportedBinaryTypeStr));
            }

            var surface = data.Adapt<SurfaceDataDto>();
            stream.Position = UsefulDataStartingPosition;

            var parser = surfaceBinaryParserFactory.GetParser(surface.StrainCalculationType);
            if (parser == null)
            {
                return new ResultExceptionContainer<SurfaceDataDto>(new Exception(CantFindParserStr));
            }
            parser.ParseData(surface, reader, intervals);

            return new ResultExceptionContainer<SurfaceDataDto>(surface);
        }
    }
}
