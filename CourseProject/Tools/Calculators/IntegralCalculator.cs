using CourseProject.Models;
using CourseProject.Models.LocalParts;

namespace CourseProject.Tools.Calculators;

public class IntegralCalculator
{
    private static readonly double[] InterpolationNodes = { -0.5773503, 0.5773503 };

    private static readonly double[] Weights = { 1.0, 1.0 };

    public static double CalcDoubleIntegralForStiffnessMatrix(double rUpperLimit, double rDownLimit, double zUpperLimit, double zDownLimit, LocalBasisFunction PsiI, LocalBasisFunction PsiJ, double lambda)
    {
        var hz = zUpperLimit - zDownLimit;
        var hr = rUpperLimit - rDownLimit;
        var firstIntegralValue = 0.0;
        for (var i = 0; i < 2; i++)
        {
            var zI = (zDownLimit + zUpperLimit) / 2.0 + InterpolationNodes[i] * hz / 2.0;
            var rI = (rUpperLimit + rDownLimit) / 2.0 + InterpolationNodes[i] * hr / 2.0;
            firstIntegralValue += Weights[i] / 2.0 * hz * (DerivativeCalculator.CalcDerivative(PsiI, rI, zI, 'r') *
                                                           DerivativeCalculator.CalcDerivative(PsiJ, rI, zI, 'r') +
                                                           DerivativeCalculator.CalcDerivative(PsiI, rI, zI, 'z') *
                                                           DerivativeCalculator.CalcDerivative(PsiJ, rI, zI, 'z'));
        }

        var secondIntegralValue = 0.0;
        for (var i = 0; i < 2; i++)
        {
            var rI = (rUpperLimit + rDownLimit) / 2.0 + InterpolationNodes[i] * hr / 2.0;
            secondIntegralValue += Weights[i] / 2.0 * hr * rI * firstIntegralValue;
        }

        return lambda * secondIntegralValue;
    }

    public static double CalcDoubleIntegralForMassMatrix(double rUpperLimit, double rDownLimit, double zUpperLimit, double zDownLimit, LocalBasisFunction PsiI, LocalBasisFunction PsiJ)
    {
        var hz = zUpperLimit - zDownLimit;
        var hr = rUpperLimit - rDownLimit;
        var firstIntegralValue = 0.0;
        for (var i = 0; i < 2; i++)
        {
            var zI = (zDownLimit + zUpperLimit) / 2.0 + InterpolationNodes[i] * hz / 2.0;
            var rI = (rUpperLimit + rDownLimit) / 2.0 + InterpolationNodes[i] * hr / 2.0;
            firstIntegralValue += Weights[i] / 2.0 * hz * (PsiI.CalcFunction(rI, zI) * PsiJ.CalcFunction(rI, zI));
        }

        var secondIntegralValue = 0.0;
        for (var i = 0; i < 2; i++)
        {
            var rI = (rUpperLimit + rDownLimit) / 2.0 + InterpolationNodes[i] * hr / 2.0;
            secondIntegralValue += Weights[i] / 2.0 * hr * rI * firstIntegralValue;
        }

        return secondIntegralValue;
    }
}