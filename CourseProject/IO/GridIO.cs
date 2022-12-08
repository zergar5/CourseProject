using CourseProject.Models;

namespace CourseProject.IO;

public class GridIO
{
    private string _path;

    public GridIO() { }

    public GridIO(string path)
    {
        _path = path;
    }

    public Node[] ReadCoordinateFromConsole()
    {
        Console.WriteLine("Input grid coordinate");
        var cornerNodes = new Node[4];

        Console.Write("Input bottom left point: ");
        var point = Console.ReadLine().Split(" ").Select(double.Parse).ToArray();
        cornerNodes[0] = new Node(point[0], point[1]);

        Console.Write("Input bottom right point: ");
        point = Console.ReadLine().Split(" ").Select(double.Parse).ToArray();
        cornerNodes[1] = new Node(point[0], point[1]);

        Console.Write("Input upper left point: ");
        point = Console.ReadLine().Split(" ").Select(double.Parse).ToArray();
        cornerNodes[2] = new Node(point[0], point[1]);

        Console.Write("Input upper right point: ");
        point = Console.ReadLine().Split(" ").Select(double.Parse).ToArray();
        cornerNodes[3] = new Node(point[0], point[1]);

        return cornerNodes;
        
    }

    public void ReadSizesFromConsole(out int numberByWidth, out int numberByHeight)
    {
        Console.Write("Input number by width: ");
        numberByWidth = int.Parse(Console.ReadLine());
        Console.Write("Input number by height: ");
        numberByHeight = int.Parse(Console.ReadLine());
    }
}