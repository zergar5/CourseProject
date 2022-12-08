using CourseProject.Models;

namespace CourseProject.Tools;

public class GridComponentsProvider
{
    public Element[] CreateElements(Node[] cornerNodes, int numberByWidth, int numberByHeight)
    {
        var elements = new Element[numberByWidth * numberByHeight];

        var width = cornerNodes[1].R - cornerNodes[0].R;
        var height = cornerNodes[2].Z - cornerNodes[0].Z;

        var elementWidth = width / numberByWidth;
        var elementHeight = height / numberByHeight;

        for (var i = 0; i < numberByHeight; i++)
        {
            for (var j = 0; j < numberByWidth; j++)
            {
                var element = new Element
                {
                    Nodes = new Node[]
                    {
                        new(cornerNodes[0].R + elementWidth * j, cornerNodes[0].Z + elementHeight * i),
                        new(cornerNodes[0].R + elementWidth * (j + 1), cornerNodes[0].Z + elementHeight * i),
                        new(cornerNodes[0].R + elementWidth * j, cornerNodes[0].Z + elementHeight * (i + 1)),
                        new(cornerNodes[0].R + elementWidth * (j + 1), cornerNodes[0].Z + elementHeight * (i + 1))
                    },
                    GlobalNodesNumbers = new[]
                    {
                        i * (numberByWidth + 1) + j,
                        i * (numberByWidth + 1) + j + 1,
                        (i + 1) * (numberByWidth + 1) + j,
                        (i + 1) * (numberByWidth + 1) + j + 1
                    },
                    Material = new Material()
                };
                elements[i * numberByWidth + j] = element;
            }
        }

        return elements;
    }

    public Node[] CreateNodes(Node[] cornerNodes, int numberByWidth, int numberByHeight)
    {
        var nodes = new Node[(numberByWidth + 1) * (numberByHeight + 1)];

        var width = cornerNodes[1].R - cornerNodes[0].R;
        var height = cornerNodes[2].Z - cornerNodes[0].Z;

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