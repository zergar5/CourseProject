using CourseProject.Factories;
using CourseProject.Models.Grid;
using CourseProject.Models.LocalParts;
using CourseProject.Tools.Providers;
using CourseProject.Tools;

namespace CourseProjectTests;

public class LocalBasisFunctionTests
{
    private LinearFunctionsProvider _linearFunctionsProvider;
    
    [SetUp]
    public void Setup()
    {
        _linearFunctionsProvider = new LinearFunctionsProvider();
    }

    [TestCase(1.0, 0.0, 0.0)]
    [TestCase(0.0, 3.0, 0.0)]
    [TestCase(0.0, 0.0, 2.0)]
    [TestCase(0.0, 3.0, 2.0)]
    public void CalcFirstFirstFunctionTest(double actual, double r, double z)
    {
        var localBasisFunction = new LocalBasisFunction(_linearFunctionsProvider.CreateFirstFunction(3.0, 3.0),
            _linearFunctionsProvider.CreateFirstFunction(2.0, 2.0));
        var expected = localBasisFunction.CalcFunction(r, z);
        Assert.That(expected, Is.EqualTo(actual));
    }

    [TestCase(0.0, 0.0, 0.0)]
    [TestCase(0.0, 3.0, 0.0)]
    [TestCase(1.0, 0.0, 2.0)]
    [TestCase(0.0, 3.0, 2.0)]
    public void CalcFirstSecondFunctionTest(double actual, double r, double z)
    {
        var localBasisFunction = new LocalBasisFunction(_linearFunctionsProvider.CreateFirstFunction(3.0, 3.0),
            _linearFunctionsProvider.CreateSecondFunction(0.0, 2.0));
        var expected = localBasisFunction.CalcFunction(r, z);
        Assert.That(expected, Is.EqualTo(actual));
    }

    [TestCase(0.0, 0.0, 0.0)]
    [TestCase(0.0, 3.0, 0.0)]
    [TestCase(0.0, 0.0, 2.0)]
    [TestCase(1.0, 3.0, 2.0)]
    public void CalcSecondSecondFunctionTest(double actual, double r, double z)
    {
        var localBasisFunction = new LocalBasisFunction(_linearFunctionsProvider.CreateSecondFunction(0.0, 3.0),
            _linearFunctionsProvider.CreateSecondFunction(0.0, 2.0));
        var expected = localBasisFunction.CalcFunction(r, z);
        Assert.That(expected, Is.EqualTo(actual));
    }
}