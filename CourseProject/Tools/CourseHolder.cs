using CourseProject.Models.Grid;
using System.Globalization;
using System.Text;

namespace CourseProject.Tools;

public class CourseHolder
{
    private static readonly CultureInfo _culture = CultureInfo.CreateSpecificCulture("en-US");
    public static void GetInfo(int iteration, double residual)
    {
        var stringBuilder = new StringBuilder();

        stringBuilder.Append("Iteration number: " + iteration + ", ");

        var info = "residual: " + residual;
        stringBuilder.Append(info.Replace(',', '.'));

        stringBuilder.Append("                                   \r");

        Console.Write(stringBuilder.ToString());
    }

    public static void WriteSolution(Node node, double result)
    {
        Console.WriteLine($"Function value at the point ({node.R}, {node.Z}) = {result.ToString("0.00000000000000e+00", _culture)}");
    }
}