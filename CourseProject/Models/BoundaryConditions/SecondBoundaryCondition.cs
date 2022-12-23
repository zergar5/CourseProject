namespace CourseProject.Models.BoundaryConditions;

public class SecondBoundaryCondition
{
    public double[] Thetas { get; set; }
    public int[] GlobalNodesNumbers { get; set; }

    public SecondBoundaryCondition(int[] globalNodesNumbers, double[] thetas)
    {
        Thetas = thetas;
        GlobalNodesNumbers = globalNodesNumbers;
    }
}