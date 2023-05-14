namespace CourseProjectTests;

public class NodeFinderTests
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
    }

    [Test]
    public void FindNodeTest1()
    {
        var actualNode = new Node(0.0, 0.0);
        var expectedNode = _nodeFinder.FindNode(0);
        Assert.That(actualNode, Is.EqualTo(expectedNode));
    }

    [Test]
    public void FindNodeTest2()
    {
        var actualNode = new Node(2.0, 2.0);
        var expectedNode = _nodeFinder.FindNode(4);
        Assert.That(actualNode, Is.EqualTo(expectedNode));
    }

    [Test]
    public void FindNodeTest3()
    {
        var actualNode = new Node(0.0, 4.0);
        var expectedNode = _nodeFinder.FindNode(6);
        Assert.That(actualNode, Is.EqualTo(expectedNode));
    }
}