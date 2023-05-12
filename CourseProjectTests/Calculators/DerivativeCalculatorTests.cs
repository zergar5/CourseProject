using CourseProject.Calculus;
using CourseProject.Models.LocalParts;
using CourseProject.Tools.Providers;

namespace CourseProjectTests.Calculators;

public class DerivativeCalculatorTests
{
    private LinearFunctionsProvider _linearFunctionsProvider;
    private const double _eps = 1.0e-12;

    [SetUp]
    public void Setup()
    {
        _linearFunctionsProvider = new LinearFunctionsProvider();
    }

    [TestCase(-1.0, 4.0, -4.0)]
    [TestCase(-0.3333333333333333, 3.0, 0.0)]
    [TestCase(0.0, 2.0, 2.0)]
    public void CalcRDerivativeTest(double actual, double r, double z)
    {
        var localBasisFunction = new LocalBasisFunction(_linearFunctionsProvider.CreateFirstFunction(3.0, 3.0),
            _linearFunctionsProvider.CreateFirstFunction(2.0, 2.0));
        var expected = DerivativeCalculator.CalcDerivative(localBasisFunction, r, z, 'r');
        Assert.That(Math.Abs(actual - expected), Is.LessThanOrEqualTo(_eps));
    }

    [TestCase(-1.0, -3.0, 2.0)]
    [TestCase(-0.5, 0.0, 4.0)]
    [TestCase(0.0, 3.0, 2.0)]
    public void CalcZDerivativeTest(double actual, double r, double z)
    {
        var localBasisFunction = new LocalBasisFunction(_linearFunctionsProvider.CreateFirstFunction(3.0, 3.0),
            _linearFunctionsProvider.CreateFirstFunction(2.0, 2.0));
        var expected = DerivativeCalculator.CalcDerivative(localBasisFunction, r, z, 'z');
        Assert.That(Math.Abs(actual - expected), Is.LessThanOrEqualTo(_eps));
    }
}