using CourseProject.Models.GlobalParts;
using CourseProject.Models;
using CourseProject.Models.BoundaryConditions;
using CourseProject.Models.LocalParts;

namespace CourseProject.Tools;

public class BoundaryConditionsApplicator
{
    private readonly LocalMatrix _localMatrix;

    public BoundaryConditionsApplicator()
    {
        _localMatrix = new LocalMatrix(2, 2)
        {
            [0, 0] = 2,
            [0, 1] = 1,
            [1, 0] = 1,
            [1, 1] = 2
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
        var vector = new LocalVector(2)
        {
            [0] = 2 * secondBoundaryCondition.Tetas[0] + secondBoundaryCondition.Tetas[1],
            [1] = secondBoundaryCondition.Tetas[0] + 2 * secondBoundaryCondition.Tetas[1]
        };
        vector *= secondBoundaryCondition.H / 6.0;
        globalVector.PlaceLocalVector(vector, secondBoundaryCondition.GlobalNodesNumbers);
    }

    public void ApplyThirdCondition(GlobalMatrix globalMatrix, GlobalVector globalVector, ThirdBoundaryCondition thirdBoundaryCondition)
    {
        var matrixA = _localMatrix * (thirdBoundaryCondition.Beta * thirdBoundaryCondition.H / 6.0);
        globalMatrix.PlaceLocalMatrix(matrixA, thirdBoundaryCondition.GlobalNodesNumbers);

        var vectorB = new LocalVector(2)
        {
            [0] = 2 * thirdBoundaryCondition.Us[0] + thirdBoundaryCondition.Us[1],
            [1] = thirdBoundaryCondition.Us[0] + 2 * thirdBoundaryCondition.Us[1]
        };

        vectorB *= thirdBoundaryCondition.Beta * thirdBoundaryCondition.H / 6.0;
        globalVector.PlaceLocalVector(vectorB, thirdBoundaryCondition.GlobalNodesNumbers);
    }
}