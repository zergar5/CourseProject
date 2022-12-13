using CourseProject.Models.Grid;

namespace CourseProject.Models.BoundaryConditions;

public class ThirdBoundaryCondition
{
    public double[] Betas { get; set; }
    public double[] Us { get; set; }
    public int[] GlobalNodesNumbers { get; set; }
    public double H { get; set; }

    private const double Eps = 1.0e-16;

    public ThirdBoundaryCondition(double[] betas, double[] us, int[] globalNodesNumbers, Node[] nodes)
    {
        Betas = betas;
        Us = us;
        GlobalNodesNumbers = globalNodesNumbers;
        H = Math.Abs(nodes[globalNodesNumbers[1]].R - nodes[globalNodesNumbers[0]].R) < Eps
            ? Math.Abs(nodes[globalNodesNumbers[1]].Z - nodes[globalNodesNumbers[0]].Z)
            : Math.Abs(nodes[GlobalNodesNumbers[1]].R - nodes[GlobalNodesNumbers[0]].R);
    }
}