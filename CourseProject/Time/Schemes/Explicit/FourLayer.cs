using CourseProject.Calculus;
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
        _sigmaMassMatrix = stiffnessMatrix;
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
        var (delta0, delta1, delta2, delta3, delta4, delta5) =
            _timeDeltasCalculator.CalculateForFourLayer(currentTime, previousTime, twoLayersBackTime,
                threeLayersBackTime);
        var matrixA =
            SymmetricSparseMatrix.Sum
            (
                delta3 * delta4 / (delta0 * delta1 * delta2) * _sigmaMassMatrix,
                2 * (delta3 + delta4) / (delta0 * delta1 * delta2) * _chiMassMatrix
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
                        GlobalVector.Multiply((delta4 * (delta0 - delta3) + delta0 * delta3) / -(delta0 * delta3 * delta4), _sigmaMassMatrix * previousSolution),
                        GlobalVector.Multiply(2 * (-delta4 + (delta0 - delta3)) / -(delta0 * delta3 * delta4), _chiMassMatrix * previousSolution)
                    ),
                    GlobalVector.Sum
                    (
                        GlobalVector.Multiply(delta0 * delta4 / (delta1 * delta3 * delta5), _sigmaMassMatrix * twoLayersBackSolution),
                        GlobalVector.Multiply(2 * (delta0 - delta4) / (delta1 * delta3 * delta5), _chiMassMatrix * twoLayersBackSolution)
                    )
                ),
                GlobalVector.Subtract
                (
                    GlobalVector.Sum
                    (
                        GlobalVector.Multiply(delta0 * delta3 / -(delta2 * delta4 * delta5), _sigmaMassMatrix * threeLayersBackSolution),
                        GlobalVector.Multiply(2 * (delta0 - delta3) / -(delta2 * delta4 * delta5), _chiMassMatrix * threeLayersBackSolution)
                    ),
                    _stiffnessMatrix * previousSolution
                )
            )
        );

        return new Equation<SymmetricSparseMatrix>(matrixA, q, b);
    }
}