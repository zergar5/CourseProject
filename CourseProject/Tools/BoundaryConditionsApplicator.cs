using CourseProject.Models.BoundaryConditions;
using CourseProject.Models.GlobalParts;
using CourseProject.Models.LocalParts;

namespace CourseProject.Tools;

public class BoundaryConditionsApplicator
{
    private readonly LocalMatrix _localMatrix1;
    private readonly LocalMatrix _localMatrix2;
    private readonly NodeFinder _nodeFinder;
    private const double Eps = 1.0e-16;

    public BoundaryConditionsApplicator(NodeFinder nodeFinder)
    {
        _nodeFinder = nodeFinder;
        _localMatrix1 = new LocalMatrix(2, 2)
        {
            [0, 0] = 2,
            [0, 1] = 1,
            [1, 0] = 1,
            [1, 1] = 2
        };
        _localMatrix2 = new LocalMatrix(2, 2)
        {
            [0, 0] = 1,
            [0, 1] = 1,
            [1, 0] = 1,
            [1, 1] = 3
        };
    }

    public void ApplyFirstCondition(GlobalMatrix globalMatrix, GlobalVector globalVector, FirstBoundaryCondition firstBoundaryCondition)
    {
        for (var i = 0; i < firstBoundaryCondition.GlobalNodesNumbers.Length; i++)
        {
            globalVector[firstBoundaryCondition.GlobalNodesNumbers[i]] = firstBoundaryCondition.Us[i];
            globalMatrix.DI[firstBoundaryCondition.GlobalNodesNumbers[i]] = 1.0;

            for (var j = globalMatrix.IG[firstBoundaryCondition.GlobalNodesNumbers[i]]; j < globalMatrix.IG[firstBoundaryCondition.GlobalNodesNumbers[i] + 1]; j++)
            {
                globalVector[globalMatrix.JG[j]] -= globalMatrix.GG[j] * firstBoundaryCondition.Us[i];
                globalMatrix.GG[j] = 0.0;
            }

            for (var j = firstBoundaryCondition.GlobalNodesNumbers[i] + 1; j < globalMatrix.N; j++)
            {
                var columnIndex = Array.IndexOf(globalMatrix.JG, firstBoundaryCondition.GlobalNodesNumbers[i], globalMatrix.IG[j], globalMatrix.IG[j + 1] - globalMatrix.IG[j]);
                if (columnIndex == -1) continue;

                globalVector[j] -= globalMatrix.GG[columnIndex] * firstBoundaryCondition.Us[i];
                globalMatrix.GG[columnIndex] = 0.0;
            }
        }
    }

    public void ApplySecondCondition(GlobalVector globalVector, SecondBoundaryCondition secondBoundaryCondition)
    {
        var firstNode = _nodeFinder.FindNode(secondBoundaryCondition.GlobalNodesNumbers[0]);
        var secondNode = _nodeFinder.FindNode(secondBoundaryCondition.GlobalNodesNumbers[1]);

        if (CheckParallelism(firstNode.R, secondNode.R))
        {
            var h = CalcH(firstNode.R, secondNode.R);

            var vector1 = new LocalVector(2)
            {
                [0] = 2 * secondBoundaryCondition.Thetas[0] + secondBoundaryCondition.Thetas[1],
                [1] = secondBoundaryCondition.Thetas[0] + 2 * secondBoundaryCondition.Thetas[1]
            };
            vector1 *= h * firstNode.R / 6.0;

            var vector2 = new LocalVector(2)
            {
                [0] = secondBoundaryCondition.Thetas[0] + secondBoundaryCondition.Thetas[1],
                [1] = secondBoundaryCondition.Thetas[0] + 3 * secondBoundaryCondition.Thetas[1]
            };
            vector2 *= h * h / 12.0;

            globalVector.PlaceLocalVector(vector1 + vector2, secondBoundaryCondition.GlobalNodesNumbers);
        }
        else
        {
            var h = CalcH(firstNode.Z, secondNode.Z);

            var vector = new LocalVector(2)
            {
                [0] = 2 * secondBoundaryCondition.Thetas[0] + secondBoundaryCondition.Thetas[1],
                [1] = secondBoundaryCondition.Thetas[0] + 2 * secondBoundaryCondition.Thetas[1]
            };
            vector *= h * firstNode.R / 6.0;

            globalVector.PlaceLocalVector(vector, secondBoundaryCondition.GlobalNodesNumbers);
        }
    }

    public void ApplyThirdCondition(GlobalMatrix globalMatrix, GlobalVector globalVector, ThirdBoundaryCondition thirdBoundaryCondition)
    {
        var firstNode = _nodeFinder.FindNode(thirdBoundaryCondition.GlobalNodesNumbers[0]);
        var secondNode = _nodeFinder.FindNode(thirdBoundaryCondition.GlobalNodesNumbers[1]);

        if (CheckParallelism(firstNode.R, secondNode.R))
        {
            var h = CalcH(firstNode.R, secondNode.R);

            var matrixA = _localMatrix1 * (thirdBoundaryCondition.Beta * h * firstNode.R / 6.0) +
                          _localMatrix2 * (thirdBoundaryCondition.Beta * h * h / 12.0);
            globalMatrix.PlaceLocalMatrix(matrixA, thirdBoundaryCondition.GlobalNodesNumbers);

            var vector1 = new LocalVector(2)
            {
                [0] = 2 * thirdBoundaryCondition.Us[0] + thirdBoundaryCondition.Us[1],
                [1] = thirdBoundaryCondition.Us[0] + 2 * thirdBoundaryCondition.Us[1]
            };
            vector1 *= thirdBoundaryCondition.Beta * h * firstNode.R / 6.0;

            var vector2 = new LocalVector(2)
            {
                [0] = thirdBoundaryCondition.Us[0] + thirdBoundaryCondition.Us[1],
                [1] = thirdBoundaryCondition.Us[0] + 3 * thirdBoundaryCondition.Us[1]
            };
            vector2 *= thirdBoundaryCondition.Beta * h * h / 12.0;

            globalVector.PlaceLocalVector(vector1 + vector2, thirdBoundaryCondition.GlobalNodesNumbers);
        }
        else
        {
            var h = CalcH(firstNode.Z, secondNode.Z);

            var matrixA = _localMatrix1 * (thirdBoundaryCondition.Beta * h * firstNode.R / 6.0);
            globalMatrix.PlaceLocalMatrix(matrixA, thirdBoundaryCondition.GlobalNodesNumbers);

            var vector = new LocalVector(2)
            {
                [0] = 2 * thirdBoundaryCondition.Us[0] + thirdBoundaryCondition.Us[1],
                [1] = thirdBoundaryCondition.Us[0] + 2 * thirdBoundaryCondition.Us[1]
            };
            vector *= thirdBoundaryCondition.Beta * h * firstNode.R / 6.0;

            globalVector.PlaceLocalVector(vector, thirdBoundaryCondition.GlobalNodesNumbers);
        }
    }

    private static bool CheckParallelism(double coordinate1, double coordinate2)
    {
        return Math.Abs(coordinate2 - coordinate1) > Eps;
    }

    private static double CalcH(double coordinate1, double coordinate2)
    {
        return coordinate2 - coordinate1;
    }
}