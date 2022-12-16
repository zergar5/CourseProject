using CourseProject.Factories;
using CourseProject.Models.Grid;
using CourseProject.Models.LocalParts;
using CourseProject.Tools;
using CourseProject.Tools.Calculators;
using CourseProject.Tools.Providers;
using System;

namespace CourseProjectTests.Models.Grid;

public class ElementTests
{
    private MaterialFactory _materialFactory;
    private LinearFunctionsProvider _linearFunctionsProvider;
    private GridComponentsProvider _gridComponentsProvider;
    private GridFactory _gridFactory;
    private Element _element;
    private Node[] _cornerNodes;
    private int _numberByWidth;
    private int _numberByHeight;
    private CourseProject.Models.Grid.Grid _grid;
    private NodeFinder _nodeFinder;
    private PComponentsProvider _pComponentsProvider;

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
        double F(double x, double y) => x * y;
        _pComponentsProvider = new PComponentsProvider(F, _nodeFinder);

        _element = _grid.Elements[0];
    }

    //Поменять на сравнение с эпсилоном

    [Test]
    public void CalcStiffnessMatrixTest()
    {
        var actualLocalMatrix = new[,]
        {
            { 0.49999999999990874, -0.16666666666671631, -0.99999999999982614, 0.66666666666663388 },
            { -0.16666666666671631, 0.83333333333330406, 0.66666666666663377, -1.3333333333332216 },
            { -0.99999999999982614, 0.66666666666663377, 2.4999999999995781, -2.1666666666663867 },
            { 0.66666666666663388, -1.3333333333332216, -2.1666666666663867, 2.8333333333329755 }
        };
        _element.CalcStiffnessMatrix();
        CollectionAssert.AreEqual(_element.StiffnessMatrix.Matrix, actualLocalMatrix);
    }

    [Test]
    public void CalcMassMatrixTest()
    {
        var actualLocalMatrix = new[,]
        {
            { 0.22222222222219967, 0.22222222222219976, -0.55555555555546543, -0.55555555555546499 },
            { 0.22222222222219978, 0.66666666666687047, -0.55555555555546499, -1.6666666666670735 },
            { -0.55555555555546521, -0.55555555555546499, 1.5555555555552627, 1.5555555555552614 },
            { -0.55555555555546499, -1.6666666666670735, 1.5555555555552614, 4.6666666666676848 }
        };
        _element.CalcMassMatrix();
        CollectionAssert.AreEqual(_element.MassMatrix.Matrix, actualLocalMatrix);
    }

    [Test]
    public void CalcRightPartTest()
    {
        var actualVector = new[] { -2.2222222222218599, -6.6666666666682941, 6.2222222222210455, 18.666666666670739 };
        _element.CalcMassMatrix();
        _element.CalcRightPart(_pComponentsProvider);
        CollectionAssert.AreEqual(_element.RightPart.VectorArray, actualVector);
    }

    [Test]
    public void CalcAMatrixTest()
    {
        var matrix1 = new[,]
        {
            { 0.49999999999990874, -0.16666666666671631, -0.99999999999982614, 0.66666666666663388 },
            { -0.16666666666671631, 0.83333333333330406, 0.66666666666663377, -1.3333333333332216 },
            { -0.99999999999982614, 0.66666666666663377, 2.4999999999995781, -2.1666666666663867 },
            { 0.66666666666663388, -1.3333333333332216, -2.1666666666663867, 2.8333333333329755 }
        };
        var localMatrix1 = new LocalMatrix(matrix1);

        matrix1 = new[,]
        {
            { 0.22222222222219967, 0.22222222222219976, -0.55555555555546543, -0.55555555555546499 },
            { 0.22222222222219978, 0.66666666666687047, -0.55555555555546499, -1.6666666666670735 },
            { -0.55555555555546521, -0.55555555555546499, 1.5555555555552627, 1.5555555555552614 },
            { -0.55555555555546499, -1.6666666666670735, 1.5555555555552614, 4.6666666666676848 }
        };
        var localMatrix2 = new LocalMatrix(matrix1);

        var actualLocalMatrix = localMatrix1 + localMatrix2;

        _element.CalcStiffnessMatrix();
        _element.CalcMassMatrix();
        _element.CalcAMatrix();

        CollectionAssert.AreEqual(_element.LocalMatrixA.Matrix, actualLocalMatrix.Matrix);
    }
}