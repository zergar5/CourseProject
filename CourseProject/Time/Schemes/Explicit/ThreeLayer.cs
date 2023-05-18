using CourseProject.Core.Global;

namespace CourseProject.Time.Schemes.Explicit;

public class ThreeLayer
{
    private readonly SymmetricSparseMatrix _stiffnessMatrix;
    private readonly SymmetricSparseMatrix _sigmaMassMatrix;
    private readonly SymmetricSparseMatrix _chiMassMatrix;
    private readonly TimeDeltasCalculator _timeDeltasCalculator;

    public ThreeLayer(SymmetricSparseMatrix stiffnessMatrix, SymmetricSparseMatrix sigmaMassMatrix,
        SymmetricSparseMatrix chiMassMatrix, TimeDeltasCalculator timeDeltasCalculator)
    {
        _stiffnessMatrix = stiffnessMatrix;
        _sigmaMassMatrix = sigmaMassMatrix;
        _chiMassMatrix = chiMassMatrix;
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
        var (delta01, delta02, delta12) = _timeDeltasCalculator.CalculateForThreeLayer(currentTime, previousTime, twoLayersBackTime);

        var matrixA =
            SymmetricSparseMatrix.Sum
            (
                delta12 / (delta01 * delta02) * _sigmaMassMatrix,
                2d / (delta01 * delta02) * _chiMassMatrix
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
                            GlobalVector.Multiply((-delta01 + delta12) / (delta01 * delta12), _sigmaMassMatrix * previousSolution),
                            GlobalVector.Multiply(2 / (delta01 * delta12), _chiMassMatrix * previousSolution)
                        ),
                        GlobalVector.Sum
                        (
                            GlobalVector.Multiply(delta01 / (delta02 * delta12), _sigmaMassMatrix * twoLayersBackSolution),
                            GlobalVector.Multiply(-2 / (delta02 * delta12), _chiMassMatrix * twoLayersBackSolution)
                        )
                    ),
                    _stiffnessMatrix * previousSolution
                )
            );

        return new Equation<SymmetricSparseMatrix>(matrixA, q, b);
    }
}