namespace CourseProject.Models.Grid;

public class Grid
{
    public Node[] Nodes { get; set; }
    public Element[] Elements { get; set; }
    public Node[] CornerNodes { get; set; }
    public double Width { get; set; }
    public double Height { get; set; }
    public int NumberByWidth { get; set; }
    public int NumberByHeight { get; set; }

    public Grid(Node[] nodes, Element[] elements, Node[] cornerNodes, double width, double height, int numberByWidth, int numberByHeight)
    {
        Nodes = nodes;
        Elements = elements;
        CornerNodes = cornerNodes;
        Width = width;
        Height = height;
        NumberByWidth = numberByWidth;
        NumberByHeight = numberByHeight;
    }
}