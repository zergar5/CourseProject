using CourseProject.Models.LocalParts;
using CourseProject.Tools.Calculators;
using CourseProject.Tools.Providers;

namespace CourseProjectTests.Calculators;

public class DerivativeCalculatorTests
{
    private LinearFunctionsProvider _linearFunctionsProvider;

    [SetUp]
    public void Setup()
    {
        _linearFunctionsProvider = new LinearFunctionsProvider();
    }

    //Поменять на сравнение с эпсилоном

    [TestCase(-1.0000000000001119, 4.0, -4.0)]
    [TestCase(-0.33333333333329662, 3.0, 0.0)]
    [TestCase(0.0, 2.0, 2.0)]
    public void CalcRDerivativeTest(double actual, double r, double z)
    {
        var localBasisFunction = new LocalBasisFunction(_linearFunctionsProvider.CreateFirstFunction(3.0, 3.0),
            _linearFunctionsProvider.CreateFirstFunction(2.0, 2.0));
        var expected = DerivativeCalculator.CalcDerivative(localBasisFunction, r, z, 'r');
        Assert.That(expected, Is.EqualTo(actual));
    }

    [TestCase(-0.99999999999988987, -3.0, 2.0)]
    [TestCase(-0.50000000000005596, 0.0, 4.0)]
    [TestCase(0.0, 3.0, 2.0)]
    public void CalcZDerivativeTest(double actual, double r, double z)
    {
        var localBasisFunction = new LocalBasisFunction(_linearFunctionsProvider.CreateFirstFunction(3.0, 3.0),
            _linearFunctionsProvider.CreateFirstFunction(2.0, 2.0));
        var expected = DerivativeCalculator.CalcDerivative(localBasisFunction, r, z, 'z');
        Assert.That(expected, Is.EqualTo(actual));
    }
}