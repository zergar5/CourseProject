using CourseProject.TwoDimensional.Assembling.Local;

namespace CourseProject.Calculus;

public class DerivativeCalculator
{
    private const double Delta = 1.0e-3;

    public static double CalcDerivative(LocalBasisFunction localBasisFunction, double r, double z, char variableChar)
    {
        double result;
        if (variableChar == 'r')
        {
            result = (localBasisFunction.Calculate(r + Delta, z) - localBasisFunction.Calculate(r - Delta, z)) / (2.0 * Delta);
        }
        else
        {
            result = (localBasisFunction.Calculate(r, z + Delta) - localBasisFunction.Calculate(r, z - Delta)) / (2.0 * Delta);
        }
        return result;
    }
}