using System.Numerics;

namespace Abdm.Calculation.Models
{
    public class Surface
    {
        required public Vector3[] Surface_data { get; set; }
        /*[
			{
				"x": 0.0,
				"y": 0.0,
				"z": 0.0002673736965817054
			},
			{
				"x": 1.61,
				"y": 0.0,
				"z": 0.0
			},*/

        public object[] Line_data { get; set; }

        public float MaxX { get; set; } // 10.79

        public float MinX { get; set; } //-0.69

        public float MaxY { get; set; } // 10.84

        public float MinY { get; set; } //0.0

        public float MaxZ { get; set; }	//1.452982986171758

        public float MinZ { get; set; }

        /// <summary>
        /// айдишник от ais7EnumCpTypePs в старом клиенте
        /// </summary>
        public long CpVid { get; set; }

        /// <summary>
        /// Проектная устойчивость структуры
        /// </summary>
        public float MyStrength { get; set; } //53.0

        /// <summary>
        /// Фиксированная нагрузка
        /// </summary>
        public float СonstLoad { get; set; } //15.79

        /// <summary>
        /// Нагрузка от пешеходов
        /// </summary>
        public float СonstPesh { get; set; } //0.0

        /// <summary>
        /// Другая нагрузка
        /// </summary>
        public float СonstOther { get; set; } //0.0

        public float KStrength { get; set; } //1.0
    }
}
