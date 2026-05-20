using System.Collections.Generic;
using Abdm.Calculation.Maths.Helpers;
using Abdm.Calculation.Maths.Models;
using NUnit.Framework;

namespace Abdm.Calculation.Tests;

[TestFixture]
public class FormulasTests
{
    [Test]
    public void TrapezoidArea_CalculatesSignedAreaUnderSegment()
    {
        var area = Formulas.TrapezoidArea(new Vector2D(0, 0), new Vector2D(2, 4));

        Assert.That(area, Is.EqualTo(4));
    }

    [Test]
    public void FindBetweenValues_ReturnsBoundaryValues_WhenTargetIsOutsideRange()
    {
        var sorted = new SortedList<int, string>
        {
            { 1, "a" },
            { 3, "b" },
            { 5, "c" }
        };

        Assert.That(sorted.FindBetweenValues(0), Is.EqualTo(("a", "a")));
        Assert.That(sorted.FindBetweenValues(10), Is.EqualTo(("c", "c")));
    }

    [Test]
    public void FindBetweenValues_ReturnsNeighbors_WhenTargetIsInsideRange()
    {
        var sorted = new SortedList<int, string>
        {
            { 1, "a" },
            { 3, "b" },
            { 5, "c" }
        };

        Assert.That(sorted.FindBetweenValues(4), Is.EqualTo(("b", "c")));
    }

    [TestCase(1, true)]
    [TestCase(2, false)]
    [TestCase(3, true)]
    public void IsOdd_DetectsOddNumbers(int number, bool expected)
    {
        Assert.That(Formulas.IsOdd(number), Is.EqualTo(expected));
    }

    [Test]
    public void GetOrdinat_InterpolatesLinearlyBetweenPoints()
    {
        var value = Formulas.GetOrdinat(new Vector2D(0, 0), new Vector2D(4, 8), 2);

        Assert.That(value, Is.EqualTo(4));
    }

    [Test]
    public void GetYValueByX_UsesSortedListInterpolation()
    {
        var sorted = new SortedList<double, Vector2D>
        {
            { 0, new Vector2D(0, 0) },
            { 2, new Vector2D(2, 4) }
        };

        Assert.That(sorted.GetYValueByX(1), Is.EqualTo(2));
    }
}
