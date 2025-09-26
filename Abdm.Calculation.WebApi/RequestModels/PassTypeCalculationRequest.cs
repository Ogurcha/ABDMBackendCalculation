namespace Abdm.Calculation.WebApi.RequestModels
{
    /// <summary>
    /// реквест сообщение для начала расчётов
    /// </summary>
    public class PassTypeCalculationRequest
    {
        /// <summary>
        /// идентификатор искусственного сооружения
        /// </summary>
        public long c_isso { get; set; }

        /// <summary>
        /// Номер чекпоинта данного сооружения
        /// </summary>
        public int number { get; set; }

        /// <summary>
        /// идентификатор нагрузки на сооружение
        /// </summary>
        public int c_nagruzka { get; set; }

        /// <summary>
        /// номер выбранного снипа, по которому пойдут расчет
        /// </summary>
        public int snip { get; set; }

        /// <summary>
        /// Направление физичесrого воздействия
        /// </summary>
        public int direction { get; set; }

        /// <summary>
        /// Подробные характеристики нагрузки на данное сооружение
        /// </summary>
        public LadingSchemaRequestModel? load_schema { get; set; }

        /// <summary>
        /// Характеристики "поверхности влияния" иссо
        /// </summary>
        public SurfaceRequestModel? surface { get; set; }

        /// <summary>
        /// Характеристики пути
        /// </summary>
        public RoadwayRequestModel? roadway { get; set; }
    }
}
