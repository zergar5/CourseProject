using CourseProject.Factories;
using CourseProject.Models.BoundaryConditions;
using CourseProject.Models.Grid;
using CourseProject.Tools;
using CourseProject.Tools.Providers;

namespace CourseProjectTests.Factories;

public class BoundaryConditionFactoryTests
{
    private MaterialFactory _materialFactory;
    private LinearFunctionsProvider _linearFunctionsProvider;
    private GridComponentsProvider _gridComponentsProvider;
    private GridFactory _gridFactory;
    private Node[] _cornerNodes;
    private int _numberByWidth;
    private int _numberByHeight;
    private Grid _grid;
    private NodeFinder _nodeFinder;
    private BoundaryConditionFactory _boundaryConditionFactory;

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
            new (0.0, 0.0),
            new (4.0, 4.0)
        };
        _numberByWidth = 2;
        _numberByHeight = 2;
        _grid = _gridFactory.CreateGrid(_cornerNodes, _numberByWidth, _numberByHeight);
        _nodeFinder = new NodeFinder(_grid);
        _boundaryConditionFactory = new BoundaryConditionFactory(_nodeFinder);
    }

    [Test]
    public void CreateFirstBoundaryConditionTest()
    {
        var globalNodesNumbers = new[] { 0, 1 };

        var us = new[] { 0.0, 2.0 };

        var expectedFirstBoundaryCondition =
            _boundaryConditionFactory.CreateFirstBoundaryCondition(globalNodesNumbers, us);
        var actualFirstBoundaryCondition = new FirstBoundaryCondition(globalNodesNumbers, us);

        CollectionAssert.AreEqual(expectedFirstBoundaryCondition.GlobalNodesNumbers, actualFirstBoundaryCondition.GlobalNodesNumbers);
        CollectionAssert.AreEqual(expectedFirstBoundaryCondition.Us, actualFirstBoundaryCondition.Us);
    }

    [Test]
    public void CreateSecondBoundaryConditionTest()
    {
        var globalNodesNumbers = new[] { 2, 5 };

        var thetas = new[] { 8.0, 6.0 };

        var expectedSecondBoundaryCondition =
            _boundaryConditionFactory.CreateSecondBoundaryCondition(globalNodesNumbers, thetas);
        var actualSecondBoundaryCondition = new SecondBoundaryCondition(globalNodesNumbers, thetas, 2.0);

        CollectionAssert.AreEqual(expectedSecondBoundaryCondition.GlobalNodesNumbers, actualSecondBoundaryCondition.GlobalNodesNumbers);
        CollectionAssert.AreEqual(expectedSecondBoundaryCondition.Thetas, actualSecondBoundaryCondition.Thetas);
        Assert.That(expectedSecondBoundaryCondition.H, Is.EqualTo(actualSecondBoundaryCondition.H));
    }

    [Test]
    public void CreateThirdBoundaryConditionTest()
    {
        var globalNodesNumbers = new[] { 6, 7 };

        var us = new[] { 0.0, 0.5 };

        const double beta = 3.0;

        var expectedThirdBoundaryCondition =
            _boundaryConditionFactory.CreateThirdBoundaryCondition(globalNodesNumbers, beta, us);
        var actualThirdBoundaryCondition = new ThirdBoundaryCondition(globalNodesNumbers, beta, us, 2.0);

        CollectionAssert.AreEqual(expectedThirdBoundaryCondition.GlobalNodesNumbers, actualThirdBoundaryCondition.GlobalNodesNumbers);
        CollectionAssert.AreEqual(expectedThirdBoundaryCondition.Us, actualThirdBoundaryCondition.Us);
        Assert.That(expectedThirdBoundaryCondition.Beta, Is.EqualTo(actualThirdBoundaryCondition.Beta));
        Assert.That(expectedThirdBoundaryCondition.H, Is.EqualTo(actualThirdBoundaryCondition.H));
    }
}