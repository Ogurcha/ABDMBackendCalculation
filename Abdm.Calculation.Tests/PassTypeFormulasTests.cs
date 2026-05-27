using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.Maths.Helpers;
using NUnit.Framework;

namespace Abdm.Calculation.Tests;

[TestFixture]
public class PassTypeFormulasTests
{
    [Test]
    public void DistanceBetweenIntervalEdgeAndTrajectoryCenter_UsesMaxOfVehicleAndRoadRule()
    {
        var loadModel = new LoadModel
        {
            Width = 4,
            Interval = 1,
            Axles = [],
            Length = 3,
            Distance = 3
        };
        var roadRules = new[]
        {
            new RoadRule { MinTrajectoryDistance = 3 },
            new RoadRule { MinTrajectoryDistance = 5 }
        };

        var distance = PassTypeFormulas.DistanceBetweenIntervalEdgeAndTrajectoryCenter(loadModel, roadRules);

        Assert.That(distance, Is.EqualTo(2.5));
    }
}
