using CourseProject.Models.GlobalParts;
using CourseProject.Models;

namespace CourseProject.Tools;

public class BoundaryConditionApplicator
{
    private readonly LocalMatrix localMatrix;

    public ThirdBoundaryConditionApplicator()
    {
        localMatrix = new LocalMatrix(2, 2)
        {
            [0, 0] = 2,
            [0, 1] = 1,
            [1, 0] = 1,
            [1, 1] = 2
        };
    }

    public void Apply(GlobalMatrix globalMatrix, double beta, double u, double h)
    {

    }
}