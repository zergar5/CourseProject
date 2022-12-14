using System.Text;

namespace CourseProject.Tools;

public class CourseHolder
{
    public static void GetInfo(int iteration, double residual)
    {
        var stringBuilder = new StringBuilder();
        var info = "residual: " + residual;
        info = info.Replace(',', '.');
        stringBuilder.Append("Iteration number: " + iteration + ", ");
        stringBuilder.Append(info);
        stringBuilder.Append("                                   \r");
        Console.Write(stringBuilder.ToString());
    }
}