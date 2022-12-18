using CourseProject.Factories;
using CourseProject.Models.Grid;
using CourseProject.Models.LocalParts;
using CourseProject.Tools.Providers;

namespace CourseProjectTests.Providers;

public class GridComponentsProviderTests
{
    private MaterialFactory _materialFactory;
    private LinearFunctionsProvider _linearFunctionsProvider;
    private GridComponentsProvider _gridComponentsProvider;
    private Node[] _cornerNodes;
    private int _numberByWidth;
    private int _numberByHeight;

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

        _cornerNodes = new Node[]
        {
            new (0.0, 0.0),
            new (4.0, 4.0)
        };
        _numberByWidth = 2;
        _numberByHeight = 2;
    }

    [Test]
    public void CreateElementsTest()
    {
        var width = _cornerNodes[1].R - _cornerNodes[0].R;
        var height = _cornerNodes[1].Z - _cornerNodes[0].Z;

        var elementWidth = width / _numberByWidth;
        var elementHeight = height / _numberByHeight;

        var actualElements = new[]
        {
            new Element(
                new[] { 0, 1, 3, 4 },
                _materialFactory.CreateMaterial(0),
                new LocalBasisFunction[]
                {
                    new(_linearFunctionsProvider.CreateFirstFunction(2.0, elementWidth),
                        _linearFunctionsProvider.CreateFirstFunction(2.0, elementHeight)),
                    new(_linearFunctionsProvider.CreateFirstFunction(2.0, elementWidth),
                        _linearFunctionsProvider.CreateSecondFunction(0.0, elementHeight)),
                    new(_linearFunctionsProvider.CreateSecondFunction(0.0, elementWidth),
                        _linearFunctionsProvider.CreateFirstFunction(2.0, elementHeight)),
                    new(_linearFunctionsProvider.CreateSecondFunction(0.0, elementWidth),
                        _linearFunctionsProvider.CreateSecondFunction(0.0, elementHeight))
                }
            ),
            new Element(
                new[] { 1, 2, 4, 5 },
                _materialFactory.CreateMaterial(0),
                new LocalBasisFunction[]
                {
                    new(_linearFunctionsProvider.CreateFirstFunction(4.0, elementWidth),
                        _linearFunctionsProvider.CreateFirstFunction(2.0, elementHeight)),
                    new(_linearFunctionsProvider.CreateFirstFunction(4.0, elementWidth),
                        _linearFunctionsProvider.CreateSecondFunction(0.0, elementHeight)),
                    new(_linearFunctionsProvider.CreateSecondFunction(2.0, elementWidth),
                        _linearFunctionsProvider.CreateFirstFunction(2.0, elementHeight)),
                    new(_linearFunctionsProvider.CreateSecondFunction(0.0, elementWidth),
                        _linearFunctionsProvider.CreateSecondFunction(0.0, elementHeight))
                }
            ),
            new Element(
                new[] { 3, 4, 6, 7 },
                _materialFactory.CreateMaterial(0),
                new LocalBasisFunction[]
                {
                    new(_linearFunctionsProvider.CreateFirstFunction(2.0, elementWidth),
                        _linearFunctionsProvider.CreateFirstFunction(4.0, elementHeight)),
                    new(_linearFunctionsProvider.CreateFirstFunction(2.0, elementWidth),
                        _linearFunctionsProvider.CreateSecondFunction(2.0, elementHeight)),
                    new(_linearFunctionsProvider.CreateSecondFunction(0.0, elementWidth),
                        _linearFunctionsProvider.CreateFirstFunction(4.0, elementHeight)),
                    new(_linearFunctionsProvider.CreateSecondFunction(0.0, elementWidth),
                        _linearFunctionsProvider.CreateSecondFunction(2.0, elementHeight))
                }
            ),
            new Element(
                new[] { 4, 5, 7, 8 },
                _materialFactory.CreateMaterial(0),
                new LocalBasisFunction[]
                {
                    new(_linearFunctionsProvider.CreateFirstFunction(4.0, elementWidth),
                        _linearFunctionsProvider.CreateFirstFunction(4.0, elementHeight)),
                    new(_linearFunctionsProvider.CreateFirstFunction(4.0, elementWidth),
                        _linearFunctionsProvider.CreateSecondFunction(2.0, elementHeight)),
                    new(_linearFunctionsProvider.CreateSecondFunction(2.0, elementWidth),
                        _linearFunctionsProvider.CreateFirstFunction(4.0, elementHeight)),
                    new(_linearFunctionsProvider.CreateSecondFunction(2.0, elementWidth),
                        _linearFunctionsProvider.CreateSecondFunction(2.0, elementHeight))
                }
            )
        };
        var expectedElements =
            _gridComponentsProvider.CreateElements(_cornerNodes, _numberByWidth, _numberByHeight);

        CollectionAssert.AreEqual(expectedElements, actualElements);
    }

    [Test]
    public void CreateNodesTest()
    {
        var actualNodes = new Node[]
        {
            new(0.0, 0.0),
            new(2.0, 0.0),
            new(4.0, 0.0),
            new(0.0, 2.0),
            new(2.0, 2.0),
            new(4.0, 2.0),
            new(0.0, 4.0),
            new(2.0, 4.0),
            new(4.0, 4.0)
        };

        var expectedNodes =
            _gridComponentsProvider.CreateNodes(_cornerNodes, _numberByWidth, _numberByHeight);

        CollectionAssert.AreEqual(expectedNodes, actualNodes);
    }
}