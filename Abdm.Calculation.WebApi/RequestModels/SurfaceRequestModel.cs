namespace Abdm.Calculation.WebApi.RequestModels
{
    public class SurfaceRequestModel
    {
        /// <summary>
        /// Массив точек, из которых состоит поверхность влияния
        /// </summary>
        public SurfaceDataItemRequestModel[]? surface_data { get; set; }

        /// <summary>
        /// Данные по опоре. Если чекпоинт не являтся опорой - массив пустой
        /// </summary>
        public double[]? line_data { get; set; }

        /// <summary>
        /// Максимальное значение всех точек по X
        /// </summary>
        public double maxX { get; set; }

        /// <summary>
        /// Минимальное значение всех точек по X
        /// </summary>
        public double minX { get; set; }

        /// <summary>
        /// Максимальное значение всех точек по Y
        /// </summary>
        public double maxY { get; set; }

        /// <summary>
        /// Минимальное значение всех точек по Y
        /// </summary>
        public double minY { get; set; }

        /// <summary>
        /// Максимальное значение всех точек по Z
        /// </summary>
        public double maxZ { get; set; }

        /// <summary>
        /// Перечисление, указывающее на то, как поверхность будет подвергаться нагрузке
        /// CpSubType в старом клиенте
        /// </summary>
        public int cpVid { get; set; }

        /// <summary>
        /// Проектная устойчивость структуры. Без учёта собственного веса
        /// </summary>
        public double myStrength { get; set; }

        /// <summary>
        /// Фиксированная нагрузка от собственного веса
        /// </summary>
        public double constLoad { get; set; }

        /// <summary>
        /// Нагрузка от пешеходов
        /// </summary>
        public double constPesh { get; set; } 

        /// <summary>
        /// Другая нагрузка
        /// </summary>
        public double constOther { get; set; }

        /// <summary>
        /// Коэффициент устойчивости. По дефолту всегда 1.
        /// </summary>
        public double kStrength { get; set; }
    }

    public class SurfaceDataItemRequestModel
    {
        public double x { get; set; }

        public double y { get; set; }

        public double z { get; set; }
    }
}
