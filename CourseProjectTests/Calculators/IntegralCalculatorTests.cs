using CourseProject.Models.LocalParts;
using CourseProject.Tools.Calculators;
using CourseProject.Tools.Providers;

namespace CourseProjectTests.Calculators;

public class IntegralCalculatorTests
{
    private LinearFunctionsProvider _linearFunctionsProvider;

    [SetUp]
    public void Setup()
    {
        _linearFunctionsProvider = new LinearFunctionsProvider();
    }

    //Поменять на сравнение с эпсилоном

    [TestCase(0.5)]
    public void CalcDoubleIntegralForStiffnessTest11(double actual)
    {
        var localBasisFunction = new LocalBasisFunction(_linearFunctionsProvider.CreateFirstFunction(2.0, 2.0),
            _linearFunctionsProvider.CreateFirstFunction(2.0, 2.0));
        var expected =
            IntegralCalculator.CalcDoubleIntegralForStiffnessMatrix(2, 0, 2, 0, localBasisFunction, localBasisFunction,
                1.0);
        Assert.That(expected, Is.EqualTo(actual));
    }

    [TestCase(0.222222222222222)]
    public void CalcDoubleIntegralForMassTest11(double actual)
    {
        var localBasisFunction = new LocalBasisFunction(_linearFunctionsProvider.CreateFirstFunction(2.0, 2.0),
            _linearFunctionsProvider.CreateFirstFunction(2.0, 2.0));
        var expected = IntegralCalculator.CalcDoubleIntegralForMassMatrix(2, 0, 2, 0, localBasisFunction,
            localBasisFunction);
        Assert.That(expected, Is.EqualTo(actual));
    }

    [TestCase(-0.166666666666667)]
    public void CalcDoubleIntegralForStiffnessTest12(double actual)
    {
        var localBasisFunctionI = new LocalBasisFunction(_linearFunctionsProvider.CreateFirstFunction(2.0, 2.0),
            _linearFunctionsProvider.CreateFirstFunction(2.0, 2.0));
        var localBasisFunctionJ = new LocalBasisFunction(_linearFunctionsProvider.CreateSecondFunction(0.0, 2.0),
            _linearFunctionsProvider.CreateFirstFunction(2.0, 2.0));
        var expected =
            IntegralCalculator.CalcDoubleIntegralForStiffnessMatrix(2, 0, 2, 0, localBasisFunctionI, localBasisFunctionJ,
                1.0);
        Assert.That(expected, Is.EqualTo(actual));
    }

    [TestCase(0.222222222222222)]
    public void CalcDoubleIntegralForMassTest12(double actual)
    {
        var localBasisFunctionI = new LocalBasisFunction(_linearFunctionsProvider.CreateFirstFunction(2.0, 2.0),
            _linearFunctionsProvider.CreateFirstFunction(2.0, 2.0));
        var localBasisFunctionJ = new LocalBasisFunction(_linearFunctionsProvider.CreateSecondFunction(0.0, 2.0),
            _linearFunctionsProvider.CreateFirstFunction(2.0, 2.0));
        var expected = IntegralCalculator.CalcDoubleIntegralForMassMatrix(2, 0, 2, 0, localBasisFunctionI,
            localBasisFunctionJ);
        Assert.That(expected, Is.EqualTo(actual));
    }
}