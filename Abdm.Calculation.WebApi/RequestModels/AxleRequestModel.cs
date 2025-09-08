namespace Abdm.Calculation.WebApi.RequestModels
{
    /// <summary>
    /// DTO Информация об осях
    /// </summary>
    public class AxleRequestModel
    {
        public double y { get; set; }

        public double wx { get; set; }

        public double wy { get; set; }

        /// <summary>
        /// Вес колеса
        /// </summary>
        public double weight { get; set; }

        /// <summary>
        /// Абсолютная длина проекции, с учетом текущего колеса и колёс позади
        /// </summary>
        public double absY { get; set; }

        /// <summary>
        /// Габариты колеса (их может быть несколько)
        /// </summary>
        public double[]? wheels { get; set; }
    }
}
