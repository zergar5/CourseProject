using CourseProject.Models.Grid;

namespace CourseProject.IOs;

public class NodeIO
{
    public NodeIO() { }

    public Node ReadNodeFromConsole()
    {
        Console.Write("Input a point to find function value in it : ");
        var point = Console.ReadLine().Replace('.', ',').Split(' ').Select(double.Parse).ToArray();
        var node = new Node(point[0], point[1]);
        return node;
    }
}