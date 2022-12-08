namespace CourseProject.Models;

public class Node
{
    public double R { get; set; }
    public double Z { get; set; }

    public Node(double r, double z)
    {
        R = r;
        Z = z;
    }
}