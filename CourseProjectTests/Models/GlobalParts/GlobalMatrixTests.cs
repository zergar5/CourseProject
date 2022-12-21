using CourseProject.Factories;
using CourseProject.Models.GlobalParts;
using CourseProject.Models.Grid;
using CourseProject.Models.LocalParts;
using CourseProject.Tools.Providers;

namespace CourseProjectTests.Models.GlobalParts;

public class GlobalMatrixTests
{
    private MaterialFactory _materialFactory;
    private LinearFunctionsProvider _linearFunctionsProvider;
    private GridComponentsProvider _gridComponentsProvider;
    private GridFactory _gridFactory;
    private Node[] _cornerNodes;
    private int _numberByWidth;
    private int _numberByHeight;
    private CourseProject.Models.Grid.Grid _grid;
    private AdjacencyList _adjacencyList;
    private GlobalMatrix _globalMatrix;

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

        _adjacencyList = new AdjacencyList(_grid);
        _adjacencyList.CreateAdjacencyList();
        _globalMatrix = new GlobalMatrix(_adjacencyList);
    }

    [TestCase(20, 9)]
    public void GlobalMatrixConstructorTest(int actualGGLenght, int actualDILenght)
    {
        var actualGlobalMatrixIG = new[] { 0, 0, 1, 2, 4, 8, 11, 13, 17, 20 };
        var actualGlobalMatrixJG = new[] { 0, 1, 0, 1, 0, 1, 2, 3, 1, 2, 4, 3, 4, 3, 4, 5, 6, 4, 5, 7 };

        CollectionAssert.AreEqual(_globalMatrix.IG, actualGlobalMatrixIG);
        CollectionAssert.AreEqual(_globalMatrix.JG, actualGlobalMatrixJG);
        Assert.Multiple(() =>
        {
            Assert.That(actualGGLenght, Is.EqualTo(_globalMatrix.GG.Length));
            Assert.That(actualDILenght, Is.EqualTo(_globalMatrix.DI.Length));
        });
    }

    [Test]
    public void PlaceLocalMatrix0134Test()
    {
        var matrix = new[,]
        {
            { 1.0, 1.0, 1.0, 1.0 },
            { 1.0, 1.0, 1.0, 1.0 },
            { 1.0, 1.0, 1.0, 1.0 },
            { 1.0, 1.0, 1.0, 1.0 }
        };
        var localMatrix = new LocalMatrix(matrix);

        var globalNodesNumber = new[] { 0, 1, 3, 4 };

        _globalMatrix.PlaceLocalMatrix(localMatrix, globalNodesNumber);

        var actualDI = new[] { 1.0, 1.0, 1.0, 1.0 };
        var actualGG = new[] { 1.0, 1.0, 1.0, 1.0, 1.0, 1.0 };
        Assert.Multiple(() =>
        {
            Assert.That(actualDI[0], Is.EqualTo(_globalMatrix.DI[0]));
            Assert.That(actualDI[1], Is.EqualTo(_globalMatrix.DI[1]));
            Assert.That(actualDI[2], Is.EqualTo(_globalMatrix.DI[3]));
            Assert.That(actualDI[3], Is.EqualTo(_globalMatrix.DI[4]));
            Assert.That(actualGG[0], Is.EqualTo(_globalMatrix.GG[0]));
            Assert.That(actualGG[1], Is.EqualTo(_globalMatrix.GG[2]));
            Assert.That(actualGG[2], Is.EqualTo(_globalMatrix.GG[3]));
            Assert.That(actualGG[3], Is.EqualTo(_globalMatrix.GG[4]));
            Assert.That(actualGG[4], Is.EqualTo(_globalMatrix.GG[5]));
            Assert.That(actualGG[5], Is.EqualTo(_globalMatrix.GG[7]));
        });
    }

    [Test]
    public void PlaceLocalMatrix1245Test()
    {
        var matrix = new[,]
        {
            { 1.0, 1.0, 1.0, 1.0 },
            { 1.0, 1.0, 1.0, 1.0 },
            { 1.0, 1.0, 1.0, 1.0 },
            { 1.0, 1.0, 1.0, 1.0 }
        };
        var localMatrix = new LocalMatrix(matrix);

        var globalNodesNumber = new[] { 0, 1, 3, 4 };

        _globalMatrix.PlaceLocalMatrix(localMatrix, globalNodesNumber);

        matrix = new[,]
        {
            { 1.0, 1.0, 1.0, 1.0 },
            { 1.0, 1.0, 1.0, 1.0 },
            { 1.0, 1.0, 1.0, 1.0 },
            { 1.0, 1.0, 1.0, 1.0 }
        };
        localMatrix = new LocalMatrix(matrix);

        globalNodesNumber = new[] { 1, 2, 4, 5 };

        var actualDI = new[] { 2.0, 1.0, 2.0, 1.0 };
        var actualGG = new[] { 1.0, 2.0, 1.0, 1.0, 1.0, 1.0 };

        _globalMatrix.PlaceLocalMatrix(localMatrix, globalNodesNumber);

        Assert.Multiple(() =>
        {
            Assert.That(actualDI[0], Is.EqualTo(_globalMatrix.DI[1]));
            Assert.That(actualDI[1], Is.EqualTo(_globalMatrix.DI[2]));
            Assert.That(actualDI[2], Is.EqualTo(_globalMatrix.DI[4]));
            Assert.That(actualDI[3], Is.EqualTo(_globalMatrix.DI[5]));
            Assert.That(actualGG[0], Is.EqualTo(_globalMatrix.GG[1]));
            Assert.That(actualGG[1], Is.EqualTo(_globalMatrix.GG[5]));
            Assert.That(actualGG[2], Is.EqualTo(_globalMatrix.GG[6]));
            Assert.That(actualGG[3], Is.EqualTo(_globalMatrix.GG[8]));
            Assert.That(actualGG[4], Is.EqualTo(_globalMatrix.GG[9]));
            Assert.That(actualGG[5], Is.EqualTo(_globalMatrix.GG[10]));
        });
    }

    [Test]
    public void MultiplyingMatrixOnVectorTest()
    {
        var globalMatrix = new GlobalMatrix
        {
            N = 3,
            IG = new[] { 0, 0, 0, 1 },
            JG = new[] { 0 },
            DI = new[] { 1.0, 1.0, 1.0 },
            GG = new[] { 1.0 }
        };
        var vector = new[] { 1.0, 1.0, 1.0 };
        var globalVector = new GlobalVector(vector);
        var expectedVector = globalMatrix * globalVector;

        vector = new[] { 2.0, 1.0, 2.0 };
        var actualVector = new GlobalVector(vector);

        CollectionAssert.AreEqual(expectedVector.VectorArray, actualVector.VectorArray);
    }
}