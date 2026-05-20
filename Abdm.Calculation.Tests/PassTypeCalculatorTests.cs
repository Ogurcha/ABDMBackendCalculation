using System.Threading;
using System.Threading.Tasks;
using Abdm.Calculation.BLL.Coordinators;
using Abdm.Calculation.BLL.Enums;
using Abdm.Calculation.BLL.Models.DataTransfer;
using NUnit.Framework;

namespace Abdm.Calculation.Tests;

[TestFixture]
public class PassTypeCalculatorTests
{
    private PassTypeCalculationCoordinator _coordinator = null!;
    private PassTypeCalculationParameters _request = null!;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        var surfaceBinary = CalculationTestFixture.LoadSurfaceBinary();
        _coordinator = CalculationTestFixture.CreatePassTypeCoordinator(surfaceBinary);
        _request = PassTypeCalculatorTestData.TestRequestMessage;
    }

    [Test]
    public async Task Run_ReturnsExpectedDeniedPassType_ForReferenceScenario()
    {
        var expected = PassTypeCalculatorTestData.TestResultMessage;

        var resultContainer = await _coordinator.Run(_request, CancellationToken.None);

        Assert.That(resultContainer.IsSuccess, Is.True, resultContainer.Exception?.Message);
        Assert.That(resultContainer.Result, Is.Not.Null);

        var actual = resultContainer.Result!;
        Assert.That(actual.IssoId, Is.EqualTo(expected.IssoId));
        Assert.That(actual.CPNumber, Is.EqualTo(expected.CPNumber));
        Assert.That(actual.LoadId, Is.EqualTo(expected.LoadId));
        Assert.That(actual.Direction, Is.EqualTo(expected.Direction));
        Assert.That(actual.Snip, Is.EqualTo(expected.Snip));
        Assert.That(actual.PassType, Is.EqualTo(expected.PassType));
        Assert.That(actual.Allowed, Is.EqualTo(AllowedEnum.Denied));
        Assert.That(actual.Data, Is.Not.Null.And.Not.Empty);
    }

    [Test]
    public void GetFailedResult_ReturnsUnknownPassType_WhenParametersAreNull()
    {
        var result = _coordinator.GetFailedResult(null);

        Assert.That(result.PassType, Is.EqualTo(PassTypeEnum.Unknown));
        Assert.That(result.Allowed, Is.EqualTo(AllowedEnum.Undefined));
        Assert.That(result.IssoId, Is.Zero);
    }

    [Test]
    public void GetFailedResult_PreservesRequestIdentifiers_WhenParametersAreProvided()
    {
        var result = _coordinator.GetFailedResult(_request);

        Assert.That(result.IssoId, Is.EqualTo(_request.IssoId));
        Assert.That(result.CPNumber, Is.EqualTo(_request.CheckPointNumber));
        Assert.That(result.LoadId, Is.EqualTo(_request.LoadId));
        Assert.That(result.PassType, Is.EqualTo(PassTypeEnum.Unknown));
    }
}
