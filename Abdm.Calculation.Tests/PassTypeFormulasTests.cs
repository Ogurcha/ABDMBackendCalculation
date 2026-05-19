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

    [Test]
    public void DistanceBetweenTrajectoryCenterAndAxles_GroupsWheelsByHalfDistance()
    {
        var axles = new[]
        {
            new Axle { WheelsDistance = [2.0f, 2.0f] },
            new Axle { WheelsDistance = [4.0f] }
        };

        var result = PassTypeFormulas.DistanceBetweenTrajectoryCenterAndAxles(axles);

        Assert.That(result, Has.Count.EqualTo(2));
        Assert.That(result[1.0], Is.EqualTo(2));
        Assert.That(result[2.0], Is.EqualTo(1));
    }
}
