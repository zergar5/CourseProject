using CourseProject.Core.GridComponents;
using CourseProject.TwoDimensional.Parameters;

namespace CourseProject.TwoDimensional.Assembling.Local;

public class LambdaInterpolateProvider
{
    private readonly LocalBasisFunction[] _localBasisFunctions;
    private readonly MaterialFactory _materialFactory;

    public LambdaInterpolateProvider(LocalBasisFunctionsProvider localBasisFunctionsProvider, MaterialFactory materialFactory)
    {
        _materialFactory = materialFactory;
        _localBasisFunctions = localBasisFunctionsProvider.GetBiquadraticFunctions();
    }

    public Func<Node2D, double> GetLambdaInterpolate(Element element)
    {
        var lambdas = _materialFactory.GetById(element.MaterialId).Lambdas;

        return p =>
                    lambdas[0] * _localBasisFunctions[0].Calculate(p) +
                    lambdas[1] * _localBasisFunctions[1].Calculate(p) +
                    lambdas[2] * _localBasisFunctions[2].Calculate(p) +
                    lambdas[3] * _localBasisFunctions[3].Calculate(p) +
                    lambdas[4] * _localBasisFunctions[4].Calculate(p) +
                    lambdas[5] * _localBasisFunctions[5].Calculate(p) +
                    lambdas[6] * _localBasisFunctions[6].Calculate(p) +
                    lambdas[7] * _localBasisFunctions[7].Calculate(p) +
                    lambdas[8] * _localBasisFunctions[8].Calculate(p);
    }
}