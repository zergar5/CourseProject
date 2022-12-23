namespace CourseProject.Models.BoundaryConditions;

public class ThirdBoundaryCondition
{
    public double Beta { get; set; }
    public double[] Us { get; set; }
    public int[] GlobalNodesNumbers { get; set; }

    public ThirdBoundaryCondition(int[] globalNodesNumbers, double beta, double[] us)
    {
        GlobalNodesNumbers = globalNodesNumbers;
        Beta = beta;
        Us = us;
    }
}