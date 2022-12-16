using CourseProject.Factories;
using CourseProject.Models.BoundaryConditions;
using CourseProject.Models.GlobalParts;
using CourseProject.Models.Grid;
using CourseProject.Tools.Providers;
using CourseProject.Tools;

namespace CourseProjectTests;

public class BoundaryConditionsApplicatorTests
{
    private GlobalMatrix _globalMatrix;
    private GlobalVector _globalVector;
    private BoundaryConditionsApplicator _boundConditionsApplicator;
    
    [SetUp]
    public void Setup()
    {
        _globalMatrix = new GlobalMatrix
        {
            N = 5,
            DI = new[] { 10.0, 10.0, 10.0, 10.0, 10.0 },
            IG = new[] { 0, 0, 1, 1, 3, 6 },
            JG = new[] { 0, 0, 1, 0, 1, 3 },
            GG = new[] { 5.0, 5.0, 5.0, 5.0, 5.0, 5.0 }
        };
        _globalVector = new GlobalVector(new[] { 5.0, 5.0, 5.0, 5.0, 5.0 });
        _boundConditionsApplicator = new BoundaryConditionsApplicator();
    }

    [Test]
    public void ApplyFirstConditionTest()
    {
        var globalNodesNumbers = new[] { 1, 2 };
        var us = new[] { 10.0, 5.0 };

        var firstBoundaryCondition = new FirstBoundaryCondition(globalNodesNumbers, us);

        _boundConditionsApplicator.ApplyFirstCondition(_globalMatrix, _globalVector, firstBoundaryCondition);

        var actualGlobalMatrix = new GlobalMatrix
        {
            N = 5,
            DI = new[] { 10.0, 1.0, 1.0, 10.0, 10.0 },
            IG = new[] { 0, 0, 1, 1, 3, 6 },
            JG = new[] { 0, 0, 1, 0, 1, 3 },
            GG = new[] { 0.0, 5.0, 0.0, 5.0, 0.0, 5.0 }
        };
        var actualGlobalVector = new GlobalVector(new[] { -45.0, 10.0, 5.0, -45.0, -45.0 });

        CollectionAssert.AreEqual(_globalMatrix.DI, actualGlobalMatrix.DI);
        CollectionAssert.AreEqual(_globalMatrix.GG, actualGlobalMatrix.GG);
        CollectionAssert.AreEqual(_globalVector.VectorArray, actualGlobalVector.VectorArray);
    }

    [Test]
    public void ApplySecondConditionTest()
    {
        var globalNodesNumbers = new[] { 3, 4 };
        var us = new[] { 10.0, 5.0 };

        var secondBoundaryCondition = new SecondBoundaryCondition(globalNodesNumbers, us, 2.0);

        _boundConditionsApplicator.ApplySecondCondition(_globalVector, secondBoundaryCondition);

        var actualGlobalVector = new GlobalVector(new[] { 5.0, 5.0, 5.0, 13.333333333333332, 11.66666666666666666666 });

        CollectionAssert.AreEqual(_globalVector.VectorArray, actualGlobalVector.VectorArray);
    }

    [Test]
    public void ApplyThirdConditionTest()
    {
        var globalNodesNumbers = new[] { 0, 3 };
        var us = new[] { 10.0, 5.0 };
        const double beta = 2.0;

        var thirdBoundaryCondition = new ThirdBoundaryCondition(globalNodesNumbers, beta, us, 2.0);

        _boundConditionsApplicator.ApplyThirdCondition(_globalMatrix, _globalVector, thirdBoundaryCondition);

        var actualGlobalMatrix = new GlobalMatrix
        {
            N = 5,
            DI = new[] { 10.0 + 8.0/6.0, 10.0, 10.0, 10.0 + 8.0/6.0, 10.0 },
            IG = new[] { 0, 0, 1, 1, 3, 6 },
            JG = new[] { 0, 0, 1, 0, 1, 3 },
            GG = new[] { 5.0, 5.0 + 4.0/6.0, 5.0, 5.0, 5.0, 5.0 }
        };
        var actualGlobalVector = new GlobalVector(new[] { 21.666666666666664, 5.0, 5.0, 18.333333333333332, 5.0 });

        CollectionAssert.AreEqual(_globalMatrix.DI, actualGlobalMatrix.DI);
        CollectionAssert.AreEqual(_globalMatrix.GG, actualGlobalMatrix.GG);
        CollectionAssert.AreEqual(_globalVector.VectorArray, actualGlobalVector.VectorArray);
    }
}