using CourseProject.Models.BoundaryConditions;
using CourseProject.Models.Grid;
using CourseProject.Tools;

namespace CourseProject.Factories;

public class BoundaryConditionFactory
{
    private readonly NodeFinder _nodeFinder;
    private const double Eps = 1.0e-16;

    public BoundaryConditionFactory(NodeFinder nodeFinder)
    {
        _nodeFinder = nodeFinder;
    }

    public FirstBoundaryCondition CreateFirstBoundaryCondition(int[] globalNodesNumbers, double[] us)
    {
        return new FirstBoundaryCondition(globalNodesNumbers, us);
    }

    public SecondBoundaryCondition CreateSecondBoundaryCondition(int[] globalNodesNumbers, double[] thetas)
    {
        var firstNode = _nodeFinder.FindNode(globalNodesNumbers[0]);
        var secondNode = _nodeFinder.FindNode(globalNodesNumbers[1]);

        var h = CalcH(firstNode, secondNode);

        return new SecondBoundaryCondition(globalNodesNumbers, thetas, h);
    }

    public ThirdBoundaryCondition CreateThirdBoundaryCondition(int[] globalNodesNumbers, double beta, double[] us)
    {
        var firstNode = _nodeFinder.FindNode(globalNodesNumbers[0]);
        var secondNode = _nodeFinder.FindNode(globalNodesNumbers[1]);

        var h = CalcH(firstNode, secondNode);

        return new ThirdBoundaryCondition(globalNodesNumbers, beta, us, h);
    }

    private static double CalcH(Node firstNode, Node secondNode)
    {
        var h = Math.Abs(secondNode.R - firstNode.R) < Eps
            ? Math.Abs(secondNode.Z - firstNode.Z)
            : Math.Abs(secondNode.R - firstNode.R);

        return h;
    }
}