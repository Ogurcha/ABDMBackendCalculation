using System.Collections.Generic;
using System.Linq;
using Abdm.Calculation.Graphics;
using Abdm.Calculation.Maths.Helpers;
using Abdm.Calculation.Maths.Models;
using NUnit.Framework;

namespace Abdm.Calculation.Tests;

[TestFixture]
public class PillarTrafficJamVolumeTests
{
    /// <summary>
    /// Extruded pillar quad: profiles at different X must be identical in YZ
    /// (no X-dependent triangulation-diagonal midpoints).
    /// </summary>
    [Test]
    public void PlaneCut_OnExtrudedPillar_ProfilesIdenticalAcrossX()
    {
        var meshManager = new MeshManager(new DoubleEqualityComparer());

        const double start = 0;
        const double finish = 40;
        var yz = new (double y, double z)[] { (0, 2), (18, 2) };
        var points = yz.SelectMany(p => new[]
        {
            new Vector3D(start, p.y, p.z),
            new Vector3D(finish, p.y, p.z)
        }).ToArray();

        var triangles = new List<Vector3I>();
        for (var i = 0; i < points.Length - 2; i++)
        {
            triangles.Add((i, i + 1, i + 2));
        }

        var mesh = meshManager.GetMeshFromPoints(points, triangles.ToArray(), out _);

        var leftPts = meshManager.GetIntersectionVectors(mesh, 19)!.OrderBy(p => p.y).ToArray();
        var rightPts = meshManager.GetIntersectionVectors(mesh, 21)!.OrderBy(p => p.y).ToArray();

        TestContext.Out.WriteLine("Left:  " + string.Join(", ", leftPts.Select(p => $"({p.y},{p.z})")));
        TestContext.Out.WriteLine("Right: " + string.Join(", ", rightPts.Select(p => $"({p.y},{p.z})")));

        Assert.That(leftPts.Length, Is.EqualTo(2), "diagonal midpoint should be removed");
        Assert.That(rightPts.Length, Is.EqualTo(2));
        Assert.That(leftPts[0].y, Is.EqualTo(rightPts[0].y).Within(1e-12));
        Assert.That(leftPts[0].z, Is.EqualTo(rightPts[0].z).Within(1e-12));
        Assert.That(leftPts[1].y, Is.EqualTo(rightPts[1].y).Within(1e-12));
        Assert.That(leftPts[1].z, Is.EqualTo(rightPts[1].z).Within(1e-12));
    }

    [Test]
    public void PlaneCut_OnSlantedExtrudedPillar_KeepsEndpointsOnly()
    {
        var meshManager = new MeshManager(new DoubleEqualityComparer());

        const double start = 0;
        const double finish = 40;
        var yz = new (double y, double z)[] { (0, 1), (18, 3) };
        var points = yz.SelectMany(p => new[]
        {
            new Vector3D(start, p.y, p.z),
            new Vector3D(finish, p.y, p.z)
        }).ToArray();

        var triangles = new List<Vector3I>();
        for (var i = 0; i < points.Length - 2; i++)
        {
            triangles.Add((i, i + 1, i + 2));
        }

        var mesh = meshManager.GetMeshFromPoints(points, triangles.ToArray(), out _);

        var a = meshManager.GetIntersectionVectors(mesh, 10)!.OrderBy(p => p.y).ToArray();
        var b = meshManager.GetIntersectionVectors(mesh, 30)!.OrderBy(p => p.y).ToArray();

        Assert.That(a.Length, Is.EqualTo(2));
        Assert.That(b.Length, Is.EqualTo(2));
        Assert.That(a[0].z, Is.EqualTo(1).Within(1e-12));
        Assert.That(a[1].z, Is.EqualTo(3).Within(1e-12));
        Assert.That(a[0].y, Is.EqualTo(b[0].y).Within(1e-12));
        Assert.That(a[1].y, Is.EqualTo(b[1].y).Within(1e-12));
        Assert.That(a[0].z, Is.EqualTo(b[0].z).Within(1e-12));
        Assert.That(a[1].z, Is.EqualTo(b[1].z).Within(1e-12));
    }
}
