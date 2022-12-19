using CourseProject.Factories;
using CourseProject.Models.Grid;
using CourseProject.Models.LocalParts;

namespace CourseProject.Tools.Providers;

public class GridComponentsProvider
{
    private readonly MaterialFactory _materialFactory;
    private readonly LinearFunctionsProvider _linearFunctionsProvider;

    public GridComponentsProvider(MaterialFactory materialFactory, LinearFunctionsProvider linearFunctionsProvider)
    {
        _materialFactory = materialFactory;
        _linearFunctionsProvider = linearFunctionsProvider;
    }

    public Element[] CreateElements(Node[] cornerNodes, int numberByWidth, int numberByHeight)
    {
        var elements = new Element[numberByWidth * numberByHeight];

        var width = cornerNodes[1].R - cornerNodes[0].R;
        var height = cornerNodes[1].Z - cornerNodes[0].Z;

        var elementWidth = width / numberByWidth;
        var elementHeight = height / numberByHeight;

        for (var r = 0; r < numberByHeight; r++)
        {
            for (var z = 0; z < numberByWidth; z++)
            {
                var globalNodesNumbers = new[]
                {
                    r * (numberByWidth + 1) + z,
                    r * (numberByWidth + 1) + z + 1,
                    (r + 1) * (numberByWidth + 1) + z,
                    (r + 1) * (numberByWidth + 1) + z + 1
                };
                var material = _materialFactory.CreateMaterial(0);

                var localBasisFunctions = new LocalBasisFunction[]
                {
                    new(_linearFunctionsProvider.CreateFirstFunction(cornerNodes[0].R + elementWidth * (z + 1), elementWidth),
                        _linearFunctionsProvider.CreateFirstFunction(cornerNodes[0].Z + elementHeight * (r + 1), elementHeight)),
                    new(_linearFunctionsProvider.CreateSecondFunction(cornerNodes[0].R + elementWidth * z, elementWidth),
                        _linearFunctionsProvider.CreateFirstFunction(cornerNodes[0].Z + elementHeight * (r + 1), elementHeight)),
                    new(_linearFunctionsProvider.CreateFirstFunction(cornerNodes[0].R + elementWidth * (z + 1), elementWidth),
                        _linearFunctionsProvider.CreateSecondFunction(cornerNodes[0].Z + elementHeight * r, elementHeight)),
                    new(_linearFunctionsProvider.CreateSecondFunction(cornerNodes[0].R + elementWidth * z, elementWidth),
                        _linearFunctionsProvider.CreateSecondFunction(cornerNodes[0].Z + elementHeight * r, elementHeight))
                };

                var element = new Element(globalNodesNumbers, material, localBasisFunctions);

                elements[r * numberByWidth + z] = element;
            }
        }

        return elements;
    }

    public Element[] CreateElements(Node[] cornerNodes, int numberByWidth, int numberByHeight, Material[] materials)
    {
        var elements = new Element[numberByWidth * numberByHeight];

        var width = cornerNodes[1].R - cornerNodes[0].R;
        var height = cornerNodes[1].Z - cornerNodes[0].Z;

        var elementWidth = width / numberByWidth;
        var elementHeight = height / numberByHeight;

        for (var r = 0; r < numberByHeight; r++)
        {
            for (var z = 0; z < numberByWidth; z++)
            {
                var globalNodesNumbers = new[]
                {
                    r * (numberByWidth + 1) + z,
                    r * (numberByWidth + 1) + z + 1,
                    (r + 1) * (numberByWidth + 1) + z,
                    (r + 1) * (numberByWidth + 1) + z + 1
                };
                var material = materials[r * numberByWidth + z];

                var localBasisFunctions = new LocalBasisFunction[]
                {
                    new(_linearFunctionsProvider.CreateFirstFunction(cornerNodes[0].R + elementWidth * (z + 1), elementWidth),
                        _linearFunctionsProvider.CreateFirstFunction(cornerNodes[0].Z + elementHeight * (r + 1), elementHeight)),
                    new(_linearFunctionsProvider.CreateSecondFunction(cornerNodes[0].R + elementWidth * z, elementWidth),
                        _linearFunctionsProvider.CreateFirstFunction(cornerNodes[0].Z + elementHeight * (r + 1), elementHeight)),
                    new(_linearFunctionsProvider.CreateFirstFunction(cornerNodes[0].R + elementWidth * (z + 1), elementWidth),
                        _linearFunctionsProvider.CreateSecondFunction(cornerNodes[0].Z + elementHeight * r, elementHeight)),
                    new(_linearFunctionsProvider.CreateSecondFunction(cornerNodes[0].R + elementWidth * z, elementWidth),
                        _linearFunctionsProvider.CreateSecondFunction(cornerNodes[0].Z + elementHeight * r, elementHeight))
                };

                var element = new Element(globalNodesNumbers, material, localBasisFunctions);

                elements[r * numberByWidth + z] = element;
            }
        }

        return elements;
    }

    public Node[] CreateNodes(Node[] cornerNodes, int numberByWidth, int numberByHeight)
    {
        var nodes = new Node[(numberByWidth + 1) * (numberByHeight + 1)];

        var width = cornerNodes[1].R - cornerNodes[0].R;
        var height = cornerNodes[1].Z - cornerNodes[0].Z;

        var elementWidth = width / numberByWidth;
        var elementHeight = height / numberByHeight;

        for (var r = 0; r < numberByHeight + 1; r++)
        {
            for (var z = 0; z < numberByWidth + 1; z++)
            {
                nodes[r * (numberByWidth + 1) + z] = new Node(cornerNodes[0].R + elementWidth * z, cornerNodes[0].Z + elementHeight * r);
            }
        }
        return nodes;
    }
}