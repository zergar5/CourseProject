namespace CourseProject.Models.Grid;

public class Node : IEquatable<Node>
{
    public double R { get; set; }
    public double Z { get; set; }

    public Node(double r, double z)
    {
        R = r;
        Z = z;
    }

    public bool Equals(Node? other)
    {
        return R.Equals(other.R) && Z.Equals(other.Z);
    }
}