using CourseProject.Models;
using CourseProject.Models.Grid;

namespace CourseProject.IO;

public class GridIO
{
    private string _path;

    public GridIO() { }

    public GridIO(string path)
    {
        _path = path;
    }

    public void ReadCoordinateFromConsole(out Node[] cornerNodes, out double width, out double height)
    {
        Console.WriteLine("Input grid coordinate");
        cornerNodes = new Node[2];

        Console.Write("Input bottom left point: ");
        var point = Console.ReadLine().Split(" ").Select(double.Parse).ToArray();
        cornerNodes[0] = new Node(point[0], point[1]);

        Console.Write("Input upper right point: ");
        point = Console.ReadLine().Split(" ").Select(double.Parse).ToArray();
        cornerNodes[1] = new Node(point[0], point[1]);

        Console.Write("Input width of grid: ");
        width = double.Parse(Console.ReadLine());

        Console.Write("Input height of grid: ");
        height = double.Parse(Console.ReadLine());
    }

    public void ReadSizesFromConsole(out int numberByWidth, out int numberByHeight)
    {
        Console.Write("Input number by width: ");
        numberByWidth = int.Parse(Console.ReadLine());
        Console.Write("Input number by height: ");
        numberByHeight = int.Parse(Console.ReadLine());
    }
}