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
            { 0.49999999999992695, -0.1666666666666978, -9.2130079606955739E-15, -0.33333333333321968 },
            { -0.1666666666666978, 0.83333333333335968, -0.33333333333321979, -0.33333333333344195 },
            { -9.2130079606955739E-15, -0.33333333333321979, 0.49999999999992706, -0.16666666666669783 },
            { -0.33333333333321968, -0.33333333333344195, -0.16666666666669783, 0.8333333333333599 }
        };
        _element.CalcStiffnessMatrix();
        CollectionAssert.AreEqual(_element.StiffnessMatrix.Matrix, actualLocalMatrix);
    }

    [Test]
    public void CalcMassMatrixTest()
    {
        var actualLocalMatrix = new[,]
        {
            { 0.22222222222219962, 0.22222222222219973, 0.11111111111106595, 0.11111111111106589 },
            { 0.22222222222219973, 0.66666666666687036, 0.11111111111106589, 0.33333333333333337 },
            { 0.11111111111106595, 0.11111111111106589, 0.22222222222219967, 0.22222222222219978 },
            { 0.11111111111106589, 0.33333333333333337, 0.22222222222219978, 0.66666666666687047 }
        };
        _element.CalcMassMatrix();
        CollectionAssert.AreEqual(_element.MassMatrix.Matrix, actualLocalMatrix);
    }

    [Test]
    public void CalcRightPartTest()
    {
        var actualVector = new[] { 0.44444444444426356, 1.3333333333333335, 0.88888888888879913, 2.6666666666674819 };
        _element.CalcMassMatrix();
        _element.CalcRightPart(_pComponentsProvider);
        CollectionAssert.AreEqual(_element.RightPart.VectorArray, actualVector);
    }

    [Test]
    public void CalcAMatrixTest()
    {
        var matrix1 = new[,]
        {
            { 0.49999999999992695, -0.1666666666666978, -9.2130079606955739E-15, -0.33333333333321968 },
            { -0.1666666666666978, 0.83333333333335968, -0.33333333333321979, -0.33333333333344195 },
            { -9.2130079606955739E-15, -0.33333333333321979, 0.49999999999992706, -0.16666666666669783 },
            { -0.33333333333321968, -0.33333333333344195, -0.16666666666669783, 0.8333333333333599 }
        };
        var localMatrix1 = new LocalMatrix(matrix1);

        matrix1 = new[,]
        {
            { 0.22222222222219962, 0.22222222222219973, 0.11111111111106595, 0.11111111111106589 },
            { 0.22222222222219973, 0.66666666666687036, 0.11111111111106589, 0.33333333333333337 },
            { 0.11111111111106595, 0.11111111111106589, 0.22222222222219967, 0.22222222222219978 },
            { 0.11111111111106589, 0.33333333333333337, 0.22222222222219978, 0.66666666666687047 }
        };
        var localMatrix2 = new LocalMatrix(matrix1);

        var actualLocalMatrix = localMatrix1 + localMatrix2;

        _element.CalcStiffnessMatrix();
        _element.CalcMassMatrix();
        _element.CalcAMatrix();

        CollectionAssert.AreEqual(_element.LocalMatrixA.Matrix, actualLocalMatrix.Matrix);
    }
}