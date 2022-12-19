using CourseProject;
using CourseProject.Factories;
using CourseProject.Models.GlobalParts;
using CourseProject.Models.Grid;
using CourseProject.Tools.Providers;
using CourseProject.Tools;

namespace CourseProjectTests;

public class SolutionFinderTest
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
    private GlobalVector _globalVector;
    private SolutionFinder _solutionFinder;

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
        _globalVector = new GlobalVector(new[] { 1.0, 2.0, 3.0, 4.0, 5.0, 6.0, 7.0, 8.0, 9.0 });
        _solutionFinder = new SolutionFinder(_grid, _globalVector, _nodeFinder);
    }

    [TestCase(1.0, 0.0, 0.0)]
    [TestCase(3.0, 1.0, 1.0)]
    [TestCase(2.0, 2.0, 0.0)]
    [TestCase(4.0, 0.0, 2.0)]
    [TestCase(5.0, 2.0, 2.0)]
    [TestCase(7.0, 0.0, 4.0)]
    [TestCase(7.0, 3.0, 3.0)]
    public void FindSolutionTest(double actual, double r, double z)
    {
        var node = new Node(r, z);
        var expected = _solutionFinder.FindSolution(node);
        Assert.That(actual, Is.EqualTo(expected));
    }
}