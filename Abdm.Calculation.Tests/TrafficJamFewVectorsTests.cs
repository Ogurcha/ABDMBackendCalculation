using System.Collections.Generic;
using System.Linq;
using Abdm.Calculation.BLL.Helpers;
using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.BLL.Services.LowLevelCalculation;
using Abdm.Calculation.Maths.Helpers;
using Abdm.Calculation.Maths.Models;
using NUnit.Framework;

namespace Abdm.Calculation.Tests;

[TestFixture]
public class TrafficJamFewVectorsTests
{
    /// <summary>
    /// Profile after diagonal cleanup: (-ε,0), (y0,z), (y1,0), (y1+ε,0).
    /// Area under the positive piece must stay positive — exact YStart/YFinish hits
    /// must not drop both endpoints and leave a single sample.
    /// </summary>
    [Test]
    public void CalculateZAreaAroundY_TriangleToZeroProfile_ReturnsPositiveArea()
    {
        const double eps = 0.5e-10;
        var vectors = new[]
        {
            new Vector2D(-eps, 0),
            new Vector2D(0, 2),
            new Vector2D(18, 0),
            new Vector2D(18 + eps, 0)
        };

        var interval = new Interval { Start = -eps, End = 18 };
        var Y = (interval.Start + interval.End) / 2;
        var radius = interval.Length / 2;

        var provider = new TestableVolumetricProvider(new DoubleEqualityComparer());
        var area = provider.PublicCalculateZAreaAroundY(vectors, Y, radius, out _);

        Assert.That(area, Is.GreaterThan(1.0), $"expected triangle area ~18, got {area}");
        Assert.That(area, Is.EqualTo(18).Within(0.1));
    }

    [Test]
    public void GetTrafficJamStrainForOneProfile_TriangleToZero_ReturnsNonZeroStrain()
    {
        const double eps = 0.5e-10;
        var sorted = new[]
        {
            new Vector2D(-eps, 0),
            new Vector2D(0, 2),
            new Vector2D(18, 0),
            new Vector2D(18 + eps, 0)
        };

        var axle = new Axle
        {
            Position = 0,
            Weight = 100,
            WheelWidth = 0.5,
            WheelLength = 0.3,
            WheelsDistance = [2.0]
        };

        var profile = new ProfileYZExtended
        {
            X = 20,
            SortedVectors = sorted,
            Extremums = [new Vector2D(0, 2)],
            MaximumIndexes = [0],
            PositivePieces = [new Interval { Start = -eps, End = 18 }],
            PositivePieceMap = new Dictionary<double, Interval>(),
            FootprintLength = new Dictionary<Axle, double> { [axle] = 0.5 },
            FootprintWidth = new Dictionary<Axle, double> { [axle] = 0.7 },
            VolumetricProfiles = new Dictionary<Axle, ProfileYZBase[]>
            {
                [axle] =
                [
                    new ProfileYZBase { X = 19.65, SortedVectors = sorted },
                    new ProfileYZBase { X = 20.35, SortedVectors = sorted }
                ]
            }
        };

        var load = new LoadModel
        {
            Width = 2,
            Length = 5,
            Distance = 10,
            Axles = [axle],
            WheelOffsetsMap = PassTypeFormulas.DistanceBetweenTrajectoryCenterAndAxles([axle])
        };

        var provider = new VehicleStrainProviderVolumetric(new DoubleEqualityComparer());
        var (volume, strain) = provider.GetTrafficJamStrainForOneProfile(
            profile, load, profile.PositivePieces[0], wheelOffset: 1.0);

        Assert.That(volume, Is.GreaterThan(0.01));
        Assert.That(strain, Is.GreaterThan(0.01));
    }

    private sealed class TestableVolumetricProvider(IEqualityComparer<double> equalityComparer)
        : VehicleStrainProviderVolumetric(equalityComparer)
    {
        public double PublicCalculateZAreaAroundY(
            Vector2D[] vectors,
            double Y,
            double radius,
            out (int indexLeft, int indexRight) indexes)
            => CalculateZAreaAroundY(vectors, Y, radius, out indexes);
    }
}
