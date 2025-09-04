using g4;

namespace Abdm.Calculation.Graphics.Entities
{
    public class Mesh
    {
        public required DMeshAABBTree3 Tree { get; set; }

        /// <summary>
        /// Вычисляемые данные по кэшу. Здесь происходит денормализация и дублирование во имя оптимизации
        /// </summary>
        public MeshData? Data { get; set; }
    }

    
}
