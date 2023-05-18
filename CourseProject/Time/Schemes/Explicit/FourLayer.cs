using CourseProject.Core.Global;

namespace CourseProject.Time.Schemes.Explicit;

public class FourLayer
{
    private readonly SymmetricSparseMatrix _stiffnessMatrix;
    private readonly SymmetricSparseMatrix _sigmaMassMatrix;
    private readonly SymmetricSparseMatrix _chiMassMatrix;
    private readonly TimeDeltasCalculator _timeDeltasCalculator;

    public FourLayer(SymmetricSparseMatrix stiffnessMatrix, SymmetricSparseMatrix sigmaMassMatrix,
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
        GlobalVector threeLayersBackSolution,
        double currentTime,
        double previousTime,
        double twoLayersBackTime,
        double threeLayersBackTime
    )
    {
        var (delta01, delta02, delta03, delta12, delta13, delta23) =
            _timeDeltasCalculator.CalculateForFourLayer(currentTime, previousTime, twoLayersBackTime,
                threeLayersBackTime);
        var matrixA =
            SymmetricSparseMatrix.Sum
            (
                delta12 * delta13 / (delta01 * delta02 * delta03) * _sigmaMassMatrix,
                2 * (delta12 + delta13) / (delta01 * delta02 * delta03) * _chiMassMatrix
            );

        var q = new GlobalVector(matrixA.CountRows);
        var b = GlobalVector.Sum
        (
            rightPart,
            GlobalVector.Sum
            (
                GlobalVector.Sum
                (
                    GlobalVector.Sum
                    (
                        GlobalVector.Multiply((delta13 * (delta01 - delta12) + delta01 * delta12) / -(delta01 * delta12 * delta13), _sigmaMassMatrix * previousSolution),
                        GlobalVector.Multiply(2 * (-delta13 + (delta01 - delta12)) / -(delta01 * delta12 * delta13), _chiMassMatrix * previousSolution)
                    ),
                    GlobalVector.Sum
                    (
                        GlobalVector.Multiply(delta01 * delta13 / (delta02 * delta12 * delta23), _sigmaMassMatrix * twoLayersBackSolution),
                        GlobalVector.Multiply(2 * (delta01 - delta13) / (delta02 * delta12 * delta23), _chiMassMatrix * twoLayersBackSolution)
                    )
                ),
                GlobalVector.Subtract
                (
                    GlobalVector.Sum
                    (
                        GlobalVector.Multiply(delta01 * delta12 / -(delta03 * delta13 * delta23), _sigmaMassMatrix * threeLayersBackSolution),
                        GlobalVector.Multiply(2 * (delta01 - delta12) / -(delta03 * delta13 * delta23), _chiMassMatrix * threeLayersBackSolution)
                    ),
                    _stiffnessMatrix * previousSolution
                )
            )
        );

        return new Equation<SymmetricSparseMatrix>(matrixA, q, b);
    }
}