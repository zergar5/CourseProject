using CourseProject.Models.Grid;

namespace CourseProject.Models.BoundaryConditions;

public class SecondBoundaryCondition
{
    public double[] Tetas { get; set; }
    public int[] GlobalNodesNumbers { get; set; }
    public double H { get; set; }

    private const double Eps = 1.0e-16;

    public SecondBoundaryCondition(double[] tetas, int[] globalNodesNumbers, Node[] nodes)
    {
        Tetas = tetas;
        GlobalNodesNumbers = globalNodesNumbers;
        H = Math.Abs(nodes[globalNodesNumbers[1]].R - nodes[globalNodesNumbers[0]].R) < Eps
            ? Math.Abs(nodes[globalNodesNumbers[1]].Z - nodes[globalNodesNumbers[0]].Z)
            : Math.Abs(nodes[GlobalNodesNumbers[1]].R - nodes[GlobalNodesNumbers[0]].R);
    }
}