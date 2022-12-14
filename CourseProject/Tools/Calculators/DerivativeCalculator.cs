using CourseProject.Models.LocalParts;

namespace CourseProject.Tools.Calculators;

public class DerivativeCalculator
{
    private const double Delta = 0.001;

    public static double CalcDerivative(LocalBasisFunction localBasisFunction, double r, double z, char variableChar)
    {
        double result;
        if (variableChar == 'r')
        {
            result = (localBasisFunction.CalcFunction(r - Delta, z) - localBasisFunction.CalcFunction(r + Delta, z)) / 2.0 * Delta;
        }
        else
        {
            result = (localBasisFunction.CalcFunction(r, z - Delta) - localBasisFunction.CalcFunction(r, z + Delta)) / 2.0 * Delta;
        }
        return result;
    }
}