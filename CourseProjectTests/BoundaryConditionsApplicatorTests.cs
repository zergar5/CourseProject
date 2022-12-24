using CourseProject.Factories;
using CourseProject.Models.BoundaryConditions;
using CourseProject.Models.GlobalParts;
using CourseProject.Models.Grid;
using CourseProject.Tools;
using CourseProject.Tools.Providers;

namespace CourseProjectTests;

public class BoundaryConditionsApplicatorTests
{
    private GlobalMatrix _globalMatrix;
    private GlobalVector _globalVector;

    private BoundaryConditionsApplicator _boundConditionsApplicator;
    private MaterialFactory _materialFactory;
    private LinearFunctionsProvider _linearFunctionsProvider;
    private GridComponentsProvider _gridComponentsProvider;
    private GridFactory _gridFactory;
    private Node[] _cornerNodes;
    private int _numberByWidth;
    private int _numberByHeight;
    private CourseProject.Models.Grid.Grid _grid;
    private NodeFinder _nodeFinder;

    [SetUp]
    public void Setup()
    {
        var lambdas = new List<double[]>
        {
            new[] { 1.0, 2.0, 3.0, 4.0, 5.0, 6.0, 7.0, 8.0, 9.0 },
            new[] { 1.0, 2.0, 3.0, 4.0, 5.0, 4.0, 3.0, 2.0, 1.0 },
            new[] { 9.0, 8.0, 7.0, 6.0, 5.0, 4.0, 3.0, 2.0, 1.0 }
        };
        var gammas = new List<double>
        {
            1.0,
            2.0,
            3.0
        };
        _materialFactory = new MaterialFactory(lambdas, gammas);

        _linearFunctionsProvider = new LinearFunctionsProvider();

        _gridComponentsProvider = new GridComponentsProvider(_materialFactory, _linearFunctionsProvider);
        _gridFactory = new GridFactory(_gridComponentsProvider);

        _cornerNodes = new Node[]
        {
            new (2.0, 2.0),
            new (4.0, 4.0)
        };
        _numberByWidth = 1;
        _numberByHeight = 1;
        _grid = _gridFactory.CreateGrid(_cornerNodes, _numberByWidth, _numberByHeight);

        _nodeFinder = new NodeFinder(_grid);

        _globalMatrix = new GlobalMatrix
        {
            N = 4,
            DI = new[] { 10.0, 10.0, 10.0, 10.0 },
            IG = new[] { 0, 0, 0, 1, 4 },
            JG = new[] { 0, 0, 1, 2 },
            GG = new[] { 5.0, 5.0, 5.0, 5.0 }
        };
        _globalVector = new GlobalVector(new[] { 5.0, 5.0, 5.0, 5.0 });

        _boundConditionsApplicator = new BoundaryConditionsApplicator(_nodeFinder);
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
            N = 4,
            DI = new[] { 10.0, 1.0, 1.0, 10.0 },
            IG = new[] { 0, 0, 0, 1, 4 },
            JG = new[] { 0, 0, 1, 2 },
            GG = new[] { 0.0, 5.0, 0.0, 0.0 }
        };
        var actualGlobalVector = new GlobalVector(new[] { -20.0, 10.0, 5.0, -70.0 });

        CollectionAssert.AreEqual(_globalMatrix.DI, actualGlobalMatrix.DI);
        CollectionAssert.AreEqual(_globalMatrix.GG, actualGlobalMatrix.GG);
        CollectionAssert.AreEqual(_globalVector.VectorArray, actualGlobalVector.VectorArray);
    }

    [Test]
    public void ApplySecondRConditionTest()
    {
        var globalNodesNumbers = new[] { 2, 3 };
        var thetas = new[] { 1.0, 1.0 };

        var secondBoundaryCondition = new SecondBoundaryCondition(globalNodesNumbers, thetas);

        _boundConditionsApplicator.ApplySecondCondition(_globalVector, secondBoundaryCondition);

        var actualGlobalVector = new GlobalVector(new[] { 5.0, 5.0, 7.6666666666666661, 8.3333333333333321 });

        CollectionAssert.AreEqual(_globalVector.VectorArray, actualGlobalVector.VectorArray);
    }

    [Test]
    public void ApplySecondZConditionTest()
    {
        var globalNodesNumbers = new[] { 1, 3 };
        var thetas = new[] { 1.0, 1.0 };

        var secondBoundaryCondition = new SecondBoundaryCondition(globalNodesNumbers, thetas);

        _boundConditionsApplicator.ApplySecondCondition(_globalVector, secondBoundaryCondition);

        var actualGlobalVector = new GlobalVector(new[] { 5.0, 9.0, 5.0, 9.0 });

        CollectionAssert.AreEqual(_globalVector.VectorArray, actualGlobalVector.VectorArray);
    }

    [Test]
    public void ApplyThirdRConditionTest()
    {
        var globalNodesNumbers = new[] { 2, 3 };
        var us = new[] { 1.0, 1.0 };
        const double beta = 2.0;

        var thirdBoundaryCondition = new ThirdBoundaryCondition(globalNodesNumbers, beta, us);

        _boundConditionsApplicator.ApplyThirdCondition(_globalMatrix, _globalVector, thirdBoundaryCondition);

        var actualGlobalMatrix = new GlobalMatrix
        {
            N = 4,
            DI = new[] { 10.0, 10.0, 13.333333333333332, 44.0/3.0 },
            IG = new[] { 0, 0, 0, 1, 4 },
            JG = new[] { 0, 0, 1, 2 },
            GG = new[] { 5.0, 5.0, 5.0, 7.0 }
        };
        var actualGlobalVector = new GlobalVector(new[] { 5.0, 5.0, 10.333333333333332, 35.0/3.0 });

        CollectionAssert.AreEqual(_globalMatrix.DI, actualGlobalMatrix.DI);
        CollectionAssert.AreEqual(_globalMatrix.GG, actualGlobalMatrix.GG);
        CollectionAssert.AreEqual(_globalVector.VectorArray, actualGlobalVector.VectorArray);
    }

    [Test]
    public void ApplyThirdZConditionTest()
    {
        var globalNodesNumbers = new[] { 1, 3 };
        var us = new[] { 1.0, 1.0 };
        const double beta = 2.0;

        var thirdBoundaryCondition = new ThirdBoundaryCondition(globalNodesNumbers, beta, us);

        _boundConditionsApplicator.ApplyThirdCondition(_globalMatrix, _globalVector, thirdBoundaryCondition);

        var actualGlobalMatrix = new GlobalMatrix
        {
            N = 4,
            DI = new[] { 10.0, 10.0 + 16/3.0, 10.0, 10.0 + 16/3.0 },
            IG = new[] { 0, 0, 0, 1, 4 },
            JG = new[] { 0, 0, 1, 2 },
            GG = new[] { 5.0, 5.0, 5.0 + 16.0/6.0, 5.0 }
        };
        var actualGlobalVector = new GlobalVector(new[] { 5.0, 13.0, 5.0, 13.0 });

        CollectionAssert.AreEqual(_globalMatrix.DI, actualGlobalMatrix.DI);
        CollectionAssert.AreEqual(_globalMatrix.GG, actualGlobalMatrix.GG);
        CollectionAssert.AreEqual(_globalVector.VectorArray, actualGlobalVector.VectorArray);
    }
}