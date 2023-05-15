﻿using CourseProject.Core.Global;
using CourseProject.Core.GridComponents;
using CourseProject.Core;
using CourseProject.TwoDimensional.Assembling.Global;
using CourseProject.Calculus;

namespace CourseProject.Time.Schemes.Explicit;

public class FourLayer
{
    private readonly SymmetricSparseMatrix _stiffnessMatrix;
    private readonly SymmetricSparseMatrix _sigmaMassMatrix;
    private readonly SymmetricSparseMatrix _chiMassMatrix;
    private readonly TimeDeltasCalculator _timeDeltasCalculator;

    public FourLayer(GlobalAssembler<Node2D> globalAssembler, Grid<Node2D> grid, TimeDeltasCalculator timeDeltasCalculator)
    {
        _stiffnessMatrix = globalAssembler.AssembleStiffnessMatrix(grid);
        _sigmaMassMatrix = globalAssembler.AssembleSigmaMassMatrix(grid);
        _chiMassMatrix = globalAssembler.AssembleChiMassMatrix(grid);
        _timeDeltasCalculator = timeDeltasCalculator;
    }

    public Equation<SymmetricSparseMatrix> BuildEquation
    (
        GlobalVector rightPart,
        GlobalVector previousSolution,
        GlobalVector twoLayersBackSolution,
        GlobalVector threeLayersBackSolution,
        double currentTime,
        double previousTime,
        double twoLayersBackTime,
        double threeLayersBackTime
    )
    {
        var (delta0, delta1, delta2, delta3, delta4, delta5) =
            _timeDeltasCalculator.CalculateForFourLayer(currentTime, previousTime, twoLayersBackTime,
                threeLayersBackTime);

        var matrixA =
            SymmetricSparseMatrix.Sum
            (
                delta3 * delta4 / (delta0 * delta1 * delta2) * _sigmaMassMatrix,
                (2 * delta3 + 2 * delta4) / (delta0 * delta1 * delta2) * _chiMassMatrix
            );
        var q = new GlobalVector(matrixA.CountRows);
        var b =
            GlobalVector.Sum
            (
                rightPart,
                GlobalVector.Subtract
                (
                    GlobalVector.Sum
                    (
                        GlobalVector.Sum
                        (
                            (-delta0 * delta3 + (2 * -delta0 + 2 * delta3)) /
                            (-delta2 * -delta4 * -delta5) * threeLayersBackSolution,
                            (-delta0 * delta4 + (2 * -delta0 + 2 * delta4)) /
                            (-delta1 * -delta3 * -delta5) * twoLayersBackSolution
                        ),
                        (delta4 * (2 * previousTime + delta1) + -delta0 * -delta3 +
                         2 * delta4 + 4 * previousTime + 2 * delta1) /
                        (delta0 * delta1 * delta2) * previousSolution
                    ),
                    _stiffnessMatrix * previousSolution
                )
            );

        return new Equation<SymmetricSparseMatrix>(matrixA, q, b);
    }
}