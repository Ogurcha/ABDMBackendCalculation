using System.Collections.Generic;
using Abdm.Calculation.BLL.Entities;
using Abdm.Calculation.BLL.Enums;
using Abdm.Calculation.BLL.Interfaces;
using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.BLL.Models.Strain;
using Abdm.Calculation.BLL.Services.PassTypes;
using Moq;
using NUnit.Framework;

namespace Abdm.Calculation.Tests;

[TestFixture]
public class PassTypeResolverTests
{
    [Test]
    public void Resolve_ReturnsNoLimit_WhenStrengthExceedsAllLoads()
    {
        var resolver = CreateResolver(coefficient: 1.0);
        var strainResults = new List<StrainResult>
        {
            CreateStrainResult(totalStrain: 0.1, isPedestrianAllowed: true, isDynamicMovement: false),
            CreateStrainResult(totalStrain: 0.1, isPedestrianAllowed: false, isDynamicMovement: true)
        };
        var data = CreateSurfaceData();

        var passType = resolver.Resolve(strainResults, data);

        Assert.That(passType, Is.EqualTo(PassTypeEnum.NoLimit));
    }

    [Test]
    public void Resolve_ReturnsWithoutPedestrian_WhenOnlyPedestrianRuleFails()
    {
        var resolver = CreateResolver(coefficient: 1.0);
        var strainResults = new List<StrainResult>
        {
            CreateStrainResult(totalStrain: 1.5, isPedestrianAllowed: true, isDynamicMovement: false),
            CreateStrainResult(totalStrain: 0.1, isPedestrianAllowed: false, isDynamicMovement: false),
            CreateStrainResult(totalStrain: 0.1, isPedestrianAllowed: false, isDynamicMovement: true)
        };
        var data = CreateSurfaceData();

        var passType = resolver.Resolve(strainResults, data);

        Assert.That(passType, Is.EqualTo(PassTypeEnum.WithoutPedestrian));
    }

    [Test]
    public void Resolve_ReturnsDenied_WhenAllConditionsFail()
    {
        var resolver = CreateResolver(coefficient: 1.0);
        var strainResults = new List<StrainResult>
        {
            CreateStrainResult(totalStrain: 10.0, isPedestrianAllowed: false, isDynamicMovement: false),
            CreateStrainResult(totalStrain: 10.0, isPedestrianAllowed: false, isDynamicMovement: true)
        };
        var data = CreateSurfaceData();

        var passType = resolver.Resolve(strainResults, data);

        Assert.That(passType, Is.EqualTo(PassTypeEnum.Denied));
    }

    private static PassTypeResolver CreateResolver(double coefficient)
    {
        var calculatorMock = new Mock<ICoefficientCalculator>();
        calculatorMock
            .Setup(c => c.Get(It.IsAny<double>(), It.IsAny<LoadGroupTypeEnum>(), It.IsAny<IMaterial?>()))
            .Returns(coefficient);

        var factoryMock = new Mock<IStrainCoefficientFactory>();
        factoryMock
            .Setup(f => f.GetStrainCalculator(StrainCoefficientTypeEnum.DynamicMovement, It.IsAny<StrainCalculationGroupTypeEnum>()))
            .Returns(calculatorMock.Object);

        return new PassTypeResolver(factoryMock.Object);
    }

    private static VehicleRollingSmallModel CreateSurfaceData() =>
        new()
        {
            Surface = new SurfaceModel
            {
                MyStrength = 2.08,
                ConstLoad = 0.45,
                PedestrianLoad = 0.2,
                OtherLoad = 0,
                Lambda = 1,
                StrainCalculationGroupType = StrainCalculationGroupTypeEnum.Default
            },
            Load = new LoadModel
            {
                Width = 3.5,
                Length = 4.4,
                Distance = 0,
                Axles = []
            }
        };

    private static StrainResult CreateStrainResult(
        double totalStrain,
        bool isPedestrianAllowed,
        bool isDynamicMovement) =>
        new()
        {
            RoadRuleRef = new RoadRule
            {
                IsPedestrianAllowed = isPedestrianAllowed,
                IsDynamicMovement = isDynamicMovement
            },
            Strain =
            [
                new VehicleColumnStrain
                {
                    TotalStrain = totalStrain,
                    VehicleTrajectoryRef = CreateTrajectory(),
                    VehicleStrains = []
                }
            ],
            IntervalModelRef = new IntervalModel { PassageIntervalRef = new PassageInterval(), Trajectories = [] }
        };

    private static VehicleTrajectory CreateTrajectory()
    {
        var profile = new ProfileYZ
        {
            SortedVectors = [],
            Extremums = [],
            MaximumIndexes = [],
            PositivePieceMap = [],
            PositivePieces = []
        };

        return new VehicleTrajectory
        {
            Left = [],
            Right = [],
            Center = profile
        };
    }
}
