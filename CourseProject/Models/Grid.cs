namespace CourseProject.Models;

public class Grid
{
    public Node[] Nodes { get; set; }
    public Element[] Elements { get; set; }
    public Node[] CornerNodes { get; set; }
    public int NumberByWidth { get; set; }
    public int NumberByHeight { get; set;}

    public Grid(Node[] nodes, Element[] elements, Node[] cornerNodes, int numberByWidth, int numberByHeight)
    {
        Nodes=nodes;
        Elements=elements;
        CornerNodes=cornerNodes;
        NumberByWidth=numberByWidth;
        NumberByHeight=numberByHeight;
    }

    public void CreateGrid(Node[] cornerNodes, int numberByWidth, int numberByHeight)
    {
        NumberByWidth = numberByWidth;
        NumberByHeight = numberByHeight;
        Nodes = new Node[(NumberByWidth + 1) * (NumberByHeight + 1)];
        Elements = new Element[NumberByWidth * NumberByHeight];
        CornerNodes = cornerNodes;

        var width = CornerNodes[1].R - CornerNodes[0].R;
        var height = CornerNodes[2].Z - CornerNodes[0].Z;

        var elementWidth = width / NumberByWidth;
        var elementHeight = height / NumberByHeight;

        for (var i = 0; i < NumberByHeight; i++)
        {
            for (var j = 0; j < NumberByWidth; j++)
            {
                var element = new Element
                {
                    Nodes = new Node[]
                    {
                        new(CornerNodes[0].R + elementWidth * j, CornerNodes[0].Z + elementHeight * i),
                        new(CornerNodes[0].R + elementWidth * (j + 1), CornerNodes[0].Z + elementHeight * i),
                        new(CornerNodes[0].R + elementWidth * j, CornerNodes[0].Z + elementHeight * (i + 1)),
                        new(CornerNodes[0].R + elementWidth * (j + 1), CornerNodes[0].Z + elementHeight * (i + 1))
                    },
                    GlobalNodesNumbers = new[]
                    {
                        i * NumberByWidth + j,
                        i * NumberByWidth + j + 1,
                        (i + 1) * NumberByWidth + j + 1,
                        (i + 1) * NumberByWidth + j + 2
                    },
                    Material = new Material()
                };
                Elements[i * NumberByWidth + j] = element;
            }
        }

        for (var i = 0; i < NumberByHeight + 1; i++)
        {
            for (var j = 0; j < NumberByWidth + 1; j++)
            {
                Nodes[i * (NumberByWidth + 1) + j] = new Node(CornerNodes[0].R + elementWidth * j, CornerNodes[0].Z + elementHeight * i);
            }
        }
    }
}