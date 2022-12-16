using CourseProject.Models.LocalParts;

namespace CourseProject.Tools.Calculators;

public class IntegralCalculator
{
    private static readonly double[] InterpolationNodes = { -0.5773503, 0.5773503 };

    private static readonly double[] Weights = { 1.0, 1.0 };

    private const int GaussMethodNumber = 2;

    public static double CalcDoubleIntegralForStiffnessMatrix(double rUpperLimit, double rDownLimit, double zUpperLimit, double zDownLimit, LocalBasisFunction PsiI, LocalBasisFunction PsiJ, double lambda)
    {
        var hr = rUpperLimit - rDownLimit;
        var hz = zUpperLimit - zDownLimit;

        var outerIntegralValue = 0.0;

        for (var i = 0; i < GaussMethodNumber; i++)
        {
            var innerIntegralValue = 0.0;

            var rI = (rDownLimit + rUpperLimit) / 2.0 + InterpolationNodes[i] * hr / 2.0;

            for (var j = 0; j < GaussMethodNumber; j++)
            {
                var zJ = (zDownLimit + zUpperLimit) / 2.0 + InterpolationNodes[j] * hz / 2.0;
                innerIntegralValue += Weights[j] * rI * (DerivativeCalculator.CalcDerivative(PsiI, rI, zJ, 'r') *
                                                         DerivativeCalculator.CalcDerivative(PsiJ, rI, zJ, 'r') +
                                                         DerivativeCalculator.CalcDerivative(PsiI, rI, zJ, 'z') *
                                                         DerivativeCalculator.CalcDerivative(PsiJ, rI, zJ, 'z'));
            }

            outerIntegralValue += Weights[i] * hz / 2.0 * innerIntegralValue;
        }

        return lambda * hr / 2.0 * outerIntegralValue;
    }

    public static double CalcDoubleIntegralForMassMatrix(double rUpperLimit, double rDownLimit, double zUpperLimit, double zDownLimit, LocalBasisFunction PsiI, LocalBasisFunction PsiJ)
    {
        var hr = rUpperLimit - rDownLimit;
        var hz = zUpperLimit - zDownLimit;

        var outerIntegralValue = 0.0;

        for (var i = 0; i < GaussMethodNumber; i++)
        {
            var innerIntegralValue = 0.0;

            var rI = (rDownLimit + rUpperLimit) / 2.0 + InterpolationNodes[i] * hr / 2.0;

            for (var j = 0; j < GaussMethodNumber; j++)
            {
                var zJ = (zDownLimit + zUpperLimit) / 2.0 + InterpolationNodes[j] * hz / 2.0;
                innerIntegralValue += Weights[j] * rI * PsiI.CalcFunction(rI, zJ) * PsiJ.CalcFunction(rI, zJ);
            }

            outerIntegralValue += Weights[i] * hz / 2.0 * innerIntegralValue;
        }

        return hr / 2.0 * outerIntegralValue;
    }
}