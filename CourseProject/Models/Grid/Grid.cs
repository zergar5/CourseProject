using System.Collections;

namespace CourseProject.Models.Grid;

public class Grid
{
    public Node[] Nodes { get; set; }
    public Element[] Elements { get; set; }
    public Node[] CornerNodes { get; set; }
    public int NumberByWidth { get; set; }
    public int NumberByHeight { get; set; }

    public Grid(Node[] nodes, Element[] elements, Node[] cornerNodes, int numberByWidth, int numberByHeight)
    {
        Nodes = nodes;
        Elements = elements;
        CornerNodes = cornerNodes;
        NumberByWidth = numberByWidth;
        NumberByHeight = numberByHeight;
    }
}