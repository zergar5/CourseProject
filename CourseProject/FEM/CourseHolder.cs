using CourseProject.Core.GridComponents;

namespace CourseProject.FEM;

public class CourseHolder
{
    public static void GetInfo(int iteration, double residual)
    {
        Console.Write($"Iteration: {iteration}, residual: {residual:E14}                                   \r");
    }

    public static void WriteSolution(Node2D point, double sValue)
    {
        Console.WriteLine($"({point.X},{point.Y}) {sValue:E14}");
    }

    public static void WriteAreaInfo()
    {
        Console.WriteLine("Point not in area");
    }
}