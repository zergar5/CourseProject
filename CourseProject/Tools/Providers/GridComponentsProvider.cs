using CourseProject.Models;
using CourseProject.Models.Grid;
using CourseProject.Models.LocalParts;

namespace CourseProject.Tools.Providers;

public class GridComponentsProvider
{
    private readonly MaterialProvider _materialProvider;
    private readonly LinearFunctionsProvider _linearFunctionsProvider;

    public GridComponentsProvider(MaterialProvider materialProvider, LinearFunctionsProvider linearFunctionsProvider)
    {
        _materialProvider = materialProvider;
        _linearFunctionsProvider = linearFunctionsProvider;
    }
    public Element[] CreateElements(Node[] cornerNodes, double width, double height, int numberByWidth, int numberByHeight)
    {
        var elements = new Element[numberByWidth * numberByHeight];

        var elementWidth = width / numberByWidth;
        var elementHeight = height / numberByHeight;

        for (var i = 0; i < numberByHeight; i++)
        {
            for (var j = 0; j < numberByWidth; j++)
            {
                var nodes = new Node[]
                {
                    new(cornerNodes[0].R + elementWidth * j, cornerNodes[0].Z + elementHeight * i),
                    new(cornerNodes[0].R + elementWidth * (j + 1), cornerNodes[0].Z + elementHeight * i),
                    new(cornerNodes[0].R + elementWidth * j, cornerNodes[0].Z + elementHeight * (i + 1)),
                    new(cornerNodes[0].R + elementWidth * (j + 1), cornerNodes[0].Z + elementHeight * (i + 1))
                };
                var globalNodesNumbers = new[]
                {
                    i * (numberByWidth + 1) + j,
                    i * (numberByWidth + 1) + j + 1,
                    (i + 1) * (numberByWidth + 1) + j,
                    (i + 1) * (numberByWidth + 1) + j + 1
                };
                var material = _materialProvider.CreateMaterial(0);

                var localBasisFunctions = new LocalBasisFunction[]
                {
                    new(_linearFunctionsProvider.CreateFirstFunction(nodes[1].R, elementWidth),
                        _linearFunctionsProvider.CreateFirstFunction(nodes[2].Z, elementHeight)),
                    new(_linearFunctionsProvider.CreateFirstFunction(nodes[1].R, elementWidth),
                        _linearFunctionsProvider.CreateSecondFunction(nodes[0].Z, elementHeight)),
                    new(_linearFunctionsProvider.CreateSecondFunction(nodes[0].R, elementWidth),
                        _linearFunctionsProvider.CreateFirstFunction(nodes[2].Z, elementHeight)),
                    new(_linearFunctionsProvider.CreateSecondFunction(nodes[0].R, elementWidth),
                        _linearFunctionsProvider.CreateSecondFunction(nodes[0].Z, elementHeight))
                };

                var element = new Element(nodes, globalNodesNumbers, material, localBasisFunctions);

                elements[i * numberByWidth + j] = element;
            }
        }

        return elements;
    }

    public Node[] CreateNodes(Node[] cornerNodes, double width, double height, int numberByWidth, int numberByHeight)
    {
        var nodes = new Node[(numberByWidth + 1) * (numberByHeight + 1)];

        var elementWidth = width / numberByWidth;
        var elementHeight = height / numberByHeight;

        for (var i = 0; i < numberByHeight + 1; i++)
        {
            for (var j = 0; j < numberByWidth + 1; j++)
            {
                nodes[i * (numberByWidth + 1) + j] = new Node(cornerNodes[0].R + elementWidth * j, cornerNodes[0].Z + elementHeight * i);
            }
        }
        return nodes;
    }
}