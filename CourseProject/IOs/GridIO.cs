using CourseProject.Models.Grid;
using System.Text.Json;

namespace CourseProject.IOs;

public class GridIO
{
    private readonly string _path;

    public GridIO() { }

    public GridIO(string path)
    {
        _path = path;
    }

    public void ReadParametersFromConsole(out Node[] cornerNodes, out double width, out double height, out int numberByWidth, out int numberByHeight)
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

        Console.Write("Input number by width: ");
        numberByWidth = int.Parse(Console.ReadLine());

        Console.Write("Input number by height: ");
        numberByHeight = int.Parse(Console.ReadLine());
    }

    public void ReadParametersFromFile(string fileName, out Node[] cornerNodes, out double width, out double height, out int numberByWidth, out int numberByHeight)
    {
        try
        {
            using var streamReader = new StreamReader(_path + fileName);
            cornerNodes = new Node[2];

            var point = streamReader.ReadLine().Split(" ").Select(double.Parse).ToArray();
            cornerNodes[0] = new Node(point[0], point[1]);

            point = streamReader.ReadLine().Split(" ").Select(double.Parse).ToArray();
            cornerNodes[1] = new Node(point[0], point[1]);

            var widthAndHeight = streamReader.ReadLine().Split(" ").Select(double.Parse).ToArray();
            width = widthAndHeight[0];
            height = widthAndHeight[1];

            var numberByWidthAndHeight = streamReader.ReadLine().Split(" ").Select(int.Parse).ToArray();
            numberByWidth = numberByWidthAndHeight[0];
            numberByHeight = numberByWidthAndHeight[1];
        }
        catch (Exception)
        {
            throw new Exception("Can't read parameters from file");
        }
    }

    public Grid ReadGridFromJson(string fileName)
    {
        using var fileStream = new FileStream(_path + fileName, FileMode.OpenOrCreate);
        var grid = JsonSerializer.Deserialize<Grid>(fileStream);
        if (grid == null) throw new Exception("Can't read grid from file");
        return grid;
    }

    public void WriteGridToJson(string fileName, Grid grid)
    {
        using var fileStream = new FileStream(_path + fileName, FileMode.OpenOrCreate);
        JsonSerializer.Serialize(fileStream, grid);
    }
}