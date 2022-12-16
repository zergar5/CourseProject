using CourseProject.Models.LocalParts;

namespace CourseProject.Tools.Calculators;

public class DerivativeCalculator
{
    private const double Delta = 1.0e-3;

    public static double CalcDerivative(LocalBasisFunction localBasisFunction, double r, double z, char variableChar)
    {
        double result;
        if (variableChar == 'r')
        {
            result = (localBasisFunction.CalcFunction(r + Delta, z) - localBasisFunction.CalcFunction(r - Delta, z)) / (2.0 * Delta);
        }
        else
        {
            result = (localBasisFunction.CalcFunction(r, z + Delta) - localBasisFunction.CalcFunction(r, z - Delta)) / (2.0 * Delta);
        }
        return result;
    }
}