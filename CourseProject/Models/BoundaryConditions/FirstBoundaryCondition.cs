using CourseProject.Models.Grid;

namespace CourseProject.Models.BoundaryConditions;

public class FirstBoundaryCondition
{
    public double[] Us { get; set; }
    public int[] GlobalNodesNumbers { get; set; }

    private const double Eps = 1.0e-16;

    public FirstBoundaryCondition(double[] us, int[] globalNodesNumbers)
    {
        Us = us;
        GlobalNodesNumbers = globalNodesNumbers;
    }
}