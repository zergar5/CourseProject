using CourseProject.Calculus;
using CourseProject.Core;
using CourseProject.Core.Global;
using CourseProject.Core.GridComponents;
using CourseProject.TwoDimensional.Assembling.Global;

namespace CourseProject.Time.Schemes.Explicit;

public class ThreeLayer
{
    private readonly SymmetricSparseMatrix _stiffnessMatrix;
    private readonly SymmetricSparseMatrix _sigmaMassMatrix;
    private readonly SymmetricSparseMatrix _chiMassMatrix;
    private readonly TimeDeltasCalculator _timeDeltasCalculator;

    public ThreeLayer(GlobalAssembler<Node2D> globalAssembler, Grid<Node2D> grid, TimeDeltasCalculator timeDeltasCalculator)
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
        double currentTime,
        double previousTime,
        double twoLayersBackTime
    )
    {
        var (delta0, delta1, delta2) = _timeDeltasCalculator.CalculateForThreeLayer(currentTime, previousTime, twoLayersBackTime);
        var matrixA =
            SymmetricSparseMatrix.Sum
            (
                delta2 / (delta0 * delta1) * _sigmaMassMatrix,
                2d / (delta0 * delta1) * _chiMassMatrix
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
                            GlobalVector.Multiply(-(delta0 - delta2) / (delta0 * delta2), _sigmaMassMatrix * previousSolution),
                            GlobalVector.Multiply(2 / (delta0 * delta2), _chiMassMatrix * previousSolution)
                        ),
                        GlobalVector.Sum
                        (
                            GlobalVector.Multiply(delta0 / (delta1 * delta2), _sigmaMassMatrix * twoLayersBackSolution),
                            GlobalVector.Multiply( -2 / (delta1 * delta2), _chiMassMatrix * twoLayersBackSolution)
                        )
                    ),
                    _stiffnessMatrix * previousSolution
                )
            );

        //(delta0 - 2) / (delta1 * delta2) * twoLayersBackSolution,
        //(2 - (delta0 - delta2)) / (delta0 * delta2) * previousSolution

        return new Equation<SymmetricSparseMatrix>(matrixA, q, b);
    }
}