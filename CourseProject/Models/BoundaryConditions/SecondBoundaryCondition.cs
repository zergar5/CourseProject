namespace CourseProject.Models.BoundaryConditions;

public class SecondBoundaryCondition
{
    public double[] Thetas { get; set; }
    public int[] GlobalNodesNumbers { get; set; }
    public double H { get; set; }

    private const double Eps = 1.0e-16;

    public SecondBoundaryCondition(int[] globalNodesNumbers, double[] thetas, double h)
    {
        Thetas = thetas;
        GlobalNodesNumbers = globalNodesNumbers;
        H = h;
    }
}