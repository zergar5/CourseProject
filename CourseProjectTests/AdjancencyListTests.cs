using CourseProject.Factories;
using CourseProject.Models.Grid;
using CourseProject.Tools.Providers;

namespace CourseProjectTests;

public class AdjacencyListTest
{
    private MaterialFactory _materialFactory;
    private LinearFunctionsProvider _linearFunctionsProvider;
    private GridComponentsProvider _gridComponentsProvider;
    private GridFactory _gridFactory;
    private Node[] _cornerNodes;
    private int _numberByWidth;
    private int _numberByHeight;
    private Grid _grid;

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
    }

    [Test]
    public void CreateAdjacencyTest()
    {
        var actualAdjacencyList = new List<SortedSet<int>>
        {
            new(),
            new() { 0 },
            new() { 1 },
            new() { 0, 1 },
            new() { 0, 1, 2, 3 },
            new() { 1, 2, 4 },
            new() { 3, 4 },
            new() { 3, 4, 5, 6 },
            new() { 4, 5, 7 }
        };

        var expectedAdjacencyList = new AdjacencyList(_grid);
        expectedAdjacencyList.CreateAdjacencyList();

        CollectionAssert.AreEqual(expectedAdjacencyList.List, actualAdjacencyList);
    }
}