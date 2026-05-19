using System.Collections.Generic;
using System.Linq;
using Abdm.Calculation.BLL.Models;
using Abdm.Calculation.Maths.Extensions;
using Abdm.Calculation.Maths.Models;
using NUnit.Framework;

namespace Abdm.Calculation.Tests;

[TestFixture]
public class MathExtensionsTests
{
    [Test]
    public void GetIntersectionWithY_ReturnsNull_WhenSegmentIsVertical()
    {
        var result = MathExtensions.GetIntersectionWithY(new Vector2D(1, 0), new Vector2D(1, 2));

        Assert.That(result, Is.Null);
    }

    [Test]
    public void GetIntersectionWithY_ReturnsXWhereYEqualsZero()
    {
        var result = MathExtensions.GetIntersectionWithY(new Vector2D(0, 1), new Vector2D(2, -1));

        Assert.That(result, Is.EqualTo(1).Within(1e-9));
    }

    [Test]
    public void CalculateAreaUnderCurve_ReturnsZero_WhenLessThanTwoPoints()
    {
        Assert.That(MathExtensions.CalculateAreaUnderCurve([]), Is.Zero);
        Assert.That(MathExtensions.CalculateAreaUnderCurve([new Vector2D(0, 1)]), Is.Zero);
    }

    [Test]
    public void CalculateAreaUnderCurve_SumsPositiveTrapezoidAreasOnly()
    {
        var points = new List<Vector2D>
        {
            new(0, 0),
            new(1, 2),
            new(2, 0),
            new(3, -4)
        };

        var area = MathExtensions.CalculateAreaUnderCurve(points);

        Assert.That(area, Is.EqualTo(1).Within(1e-9));
    }

    [Test]
    public void Max_ReturnsGreaterComparableValue()
    {
        Assert.That(MathExtensions.Max(1, 2), Is.EqualTo(2));
        Assert.That(MathExtensions.Max(3.5, 3.5), Is.EqualTo(3.5));
    }

    [Test]
    public void ToDecimal_RoundsToTwoDecimalPlaces()
    {
        Assert.That(MathExtensions.ToDecimal(1.23456), Is.EqualTo(1.23m));
        Assert.That(MathExtensions.ToDecimal(1.235), Is.EqualTo(1.24m));
    }

    [Test]
    public void FindExtremumsAndPositives_DetectsMaximumAndPositiveInterval()
    {
        var points = new[]
        {
            new Vector2D(0, -1),
            new Vector2D(1, 2),
            new Vector2D(2, -1)
        };

        var (extremums, maximums, positivePieces, positivePiecesMap) =
            MathExtensions.FindExtremumsAndPositives(points);

        Assert.That(extremums, Has.Count.EqualTo(1));
        Assert.That(extremums[0].X, Is.EqualTo(1));
        Assert.That(maximums, Is.EqualTo(new[] { 0 }));
        Assert.That(positivePieces, Has.Count.EqualTo(1));
        Assert.That(positivePiecesMap, Has.Count.EqualTo(1));
        Assert.That(positivePieces[0].Length, Is.GreaterThan(0));
    }

    [Test]
    public void FindExtremumsAndPositives_ReturnsEmpty_WhenLessThanThreePoints()
    {
        var (extremums, maximums, positivePieces, positivePiecesMap) =
            MathExtensions.FindExtremumsAndPositives(
            [
                new Vector2D(0, 1),
                new Vector2D(1, 2)
            ]);

        Assert.That(extremums, Is.Empty);
        Assert.That(maximums, Is.Empty);
        Assert.That(positivePieces, Is.Empty);
        Assert.That(positivePiecesMap, Is.Empty);
    }

    [Test]
    public void CalculateTruncatedSquarePyramidVolume_ReturnsZero_ForInvalidInput()
    {
        Assert.That(MathExtensions.CalculateTruncatedSquarePyramidVolume(0, 1, 1), Is.Zero);
        Assert.That(MathExtensions.CalculateTruncatedSquarePyramidVolume(1, -1, 1), Is.Zero);
    }

    [Test]
    public void CalculateTruncatedSquarePyramidVolume_UsesFrustumFormula()
    {
        const double height = 2;
        const double topWidth1 = 1;
        const double topWidth2 = 1;

        var volume = MathExtensions.CalculateTruncatedSquarePyramidVolume(height, topWidth1, topWidth2);

        const double baseWidth = topWidth1 + 2 * height;
        const double expected = height / 3.0 * (baseWidth * baseWidth + topWidth1 * topWidth2 + baseWidth * topWidth1);

        Assert.That(volume, Is.EqualTo(expected).Within(1e-9));
    }

    [Test]
    public void GetPositvePieces_YieldsIntervalBetweenZeroCrossings()
    {
        var function = new List<Vector2D>
        {
            new(-1, -1),
            new(0, 1),
            new(2, 1),
            new(3, -1)
        };

        var pieces = MathExtensions.GetPositvePieces(function).ToList();

        Assert.That(pieces, Has.Count.EqualTo(1));
        Assert.That(pieces[0].X, Is.LessThan(0));
        Assert.That(pieces[0].Y, Is.GreaterThan(2));
    }
}
