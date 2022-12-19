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

    public void ReadParametersFromConsole(out Node[] cornerNodes, out int numberByWidth, out int numberByHeight)
    {
        Console.WriteLine("Input grid coordinate");
        cornerNodes = new Node[2];

        Console.Write("Input bottom left point: ");
        var point = Console.ReadLine().Replace('.', ',').Split(' ').Select(double.Parse).ToArray();
        cornerNodes[0] = new Node(point[0], point[1]);

        Console.Write("Input upper right point: ");
        point = Console.ReadLine().Replace('.', ',').Split(' ').Select(double.Parse).ToArray();
        cornerNodes[1] = new Node(point[0], point[1]);

        Console.Write("Input number by width: ");
        numberByWidth = int.Parse(Console.ReadLine());

        Console.Write("Input number by height: ");
        numberByHeight = int.Parse(Console.ReadLine());
    }

    public void ReadParametersFromFile(string fileName, out Node[] cornerNodes, out int numberByWidth, out int numberByHeight)
    {
        try
        {
            using var streamReader = new StreamReader(_path + fileName);
            cornerNodes = new Node[2];

            var point = streamReader.ReadLine().Replace('.', ',').Split(' ').Select(double.Parse).ToArray();
            cornerNodes[0] = new Node(point[0], point[1]);

            point = streamReader.ReadLine().Replace('.', ',').Split(' ').Select(double.Parse).ToArray();
            cornerNodes[1] = new Node(point[0], point[1]);

            var numberByWidthAndHeight = streamReader.ReadLine().Replace('.', ',').Split(' ').Select(int.Parse).ToArray();
            numberByWidth = numberByWidthAndHeight[0];
            numberByHeight = numberByWidthAndHeight[1];
        }
        catch (Exception)
        {
            throw new Exception("Can't read parameters from file");
        }
    }

    public void WriteGridToJson(string fileName, Grid grid)
    {
        using var fileStream = new FileStream(_path + fileName, FileMode.OpenOrCreate);
        JsonSerializer.Serialize(fileStream, grid, new JsonSerializerOptions { WriteIndented = true });
    }
}