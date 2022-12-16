using CourseProject.Tools.Providers;

namespace CourseProjectTests.Providers;

public class LinearFunctionProviderTests
{
    private LinearFunctionsProvider _linearFunctionsProvider;

    [SetUp]
    public void Setup()
    {
        _linearFunctionsProvider = new LinearFunctionsProvider();
    }

    [TestCase(1.0)]
    public void CreateFirstFunctionXTest(double actual)
    {
        var function = _linearFunctionsProvider.CreateFirstFunction(3.0, 3.0);
        var expected = function(0.0);
        Assert.That(actual, Is.EqualTo(expected));
    }

    [TestCase(0.5)]
    public void CreateFirstFunctionYTest(double actual)
    {
        var function = _linearFunctionsProvider.CreateFirstFunction(2.0, 2.0);
        var expected = function(1.0);
        Assert.That(actual, Is.EqualTo(expected));
    }

    [TestCase(1.0)]
    public void CreateSecondFunctionXTest(double actual)
    {
        var function = _linearFunctionsProvider.CreateSecondFunction(0.0, 3.0);
        var expected = function(3.0);
        Assert.That(actual, Is.EqualTo(expected));
    }

    [TestCase(1.0)]
    public void CreateSecondFunctionYTest(double actual)
    {
        var function = _linearFunctionsProvider.CreateSecondFunction(0.0, 2.0);
        var expected = function(2.0);
        Assert.That(actual, Is.EqualTo(expected));
    }
}