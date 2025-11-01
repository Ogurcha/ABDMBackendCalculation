using Abdm.Calculation.Maths.Models;

namespace Abdm.Calculation.SteelConcrete.Models
{
    internal class RecordContainer
    {
        internal Record[] Records { get; set; }

        internal RecordContainer()
        {
            Records =
            [
                new Record()
                {
                    As = 0,
                    Vectors =
                    [
                        new Vector2D(1, 1), new Vector2D(1, 0.98), new Vector2D(1, 0.94),
                        new Vector2D(1, 0.9), new Vector2D(1, 0.87), new Vector2D(1, 0.81),
                        new Vector2D(1, 0.75), new Vector2D(0.98, 0.67), new Vector2D(0.96, 0.58),
                        new Vector2D(0.95, 0.45), new Vector2D(0.92, 0.28), new Vector2D(0.88, 0.52),
                        new Vector2D(0.83, 0.68), new Vector2D(0.75, 0.76), new Vector2D(0.63, 0.82)
                    ]
                },
                new Record()
                {
                    As = 0.2,
                    Vectors =
                    [
                        new Vector2D(1, 1), new Vector2D(1, 0.97), new Vector2D(1, 0.92),
                        new Vector2D(1.02, 0.87), new Vector2D(1.03, 0.8), new Vector2D(1.04, 0.7),
                        new Vector2D(1.05, 0.57), new Vector2D(1.06, 0.38), new Vector2D(1.07, 0.49),
                        new Vector2D(1.06, 0.61), new Vector2D(1.05, 0.72), new Vector2D(1.02, 0.82),
                        new Vector2D(0.99, 0.91), new Vector2D(0.9, 0.99), new Vector2D(0.75, 1.05)
                    ]
                },
                new Record()
                {
                    As = 0.4,
                    Vectors =
                    [
                        new Vector2D(1, 1), new Vector2D(1.04, 0.9), new Vector2D(1.08, 0.8),
                        new Vector2D(1.12, 0.67), new Vector2D(1.14, 0.52), new Vector2D(1.16, 0.34),
                        new Vector2D(1.19, 0.53), new Vector2D(1.2, 0.68), new Vector2D(1.21, 0.84),
                        new Vector2D(1.2, 0.98), new Vector2D(1.18, 1.12), new Vector2D(1.16, 1.22),
                        new Vector2D(1.13, 1.3), new Vector2D(1.09, 1.38), new Vector2D(1.04, 1.42)
                    ]
                },
                new Record()
                {
                    As = 0.6,
                    Vectors =
                    [
                        new Vector2D(1, 1), new Vector2D(1.1, 0.84), new Vector2D(1.19, 0.64),
                        new Vector2D(1.28, 0.4), new Vector2D(1.35, 0.56), new Vector2D(1.4, 0.75),
                        new Vector2D(1.44, 0.95), new Vector2D(1.46, 1.13), new Vector2D(1.47, 1.3),
                        new Vector2D(1.46, 1.45), new Vector2D(1.45, 1.58), new Vector2D(1.42, 1.69),
                        new Vector2D(1.39, 1.76), new Vector2D(1.35, 1.84), new Vector2D(1.3, 1.9)
                    ]
                },
                new Record()
                {
                    As = 0.8,
                    Vectors =
                    [
                        new Vector2D(1, 1), new Vector2D(1.2, 0.61), new Vector2D(1.39, 0.51),
                        new Vector2D(1.55, 0.84), new Vector2D(1.7, 1.12), new Vector2D(1.83, 1.36),
                        new Vector2D(1.93, 1.6), new Vector2D(1.98, 1.86), new Vector2D(2, 2.08),
                        new Vector2D(2.02, 2.29), new Vector2D(2.01, 2.47), new Vector2D(1.99, 2.52),
                        new Vector2D(1.97, 2.5), new Vector2D(1.91, 2.46), new Vector2D(1.84, 2.38)
                    ]
                },
                new Record()
                {
                    As = 1,
                    Vectors =
                    [
                        new Vector2D(1, 1), new Vector2D(1.29, 1.29), new Vector2D(1.63, 1.63),
                        new Vector2D(2.04, 2.04), new Vector2D(2.47, 2.47), new Vector2D(2.86, 2.86),
                        new Vector2D(3.2, 3.2), new Vector2D(3.38, 3.38), new Vector2D(3.49, 3.49),
                        new Vector2D(3.56, 3.56), new Vector2D(3.57, 3.57), new Vector2D(3.53, 3.53),
                        new Vector2D(3.43, 3.43), new Vector2D(3.29, 3.29), new Vector2D(3.05, 3.05)
                    ]
                },
            ];
        }
    }
}


