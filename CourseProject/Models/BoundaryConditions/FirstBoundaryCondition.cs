namespace CourseProject.Models.BoundaryConditions;

public class FirstBoundaryCondition
{
    public double[] Us { get; set; }
    public int[] GlobalNodesNumbers { get; set; }

    public FirstBoundaryCondition(int[] globalNodesNumbers, double[] us)
    {
        GlobalNodesNumbers = globalNodesNumbers;
        Us = us;
    }
}