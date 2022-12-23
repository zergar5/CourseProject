using CourseProject.Models.BoundaryConditions;
using CourseProject.Models.Grid;
using CourseProject.Tools;

namespace CourseProject.Factories;

public class BoundaryConditionFactory
{
    public FirstBoundaryCondition CreateFirstBoundaryCondition(int[] globalNodesNumbers, double[] us)
    {
        return new FirstBoundaryCondition(globalNodesNumbers, us);
    }

    public SecondBoundaryCondition CreateSecondBoundaryCondition(int[] globalNodesNumbers, double[] thetas)
    {
        return new SecondBoundaryCondition(globalNodesNumbers, thetas);
    }

    public ThirdBoundaryCondition CreateThirdBoundaryCondition(int[] globalNodesNumbers, double beta, double[] us)
    {
        return new ThirdBoundaryCondition(globalNodesNumbers, beta, us);
    }
}