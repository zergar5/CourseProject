using CourseProject.Models.GlobalParts;

namespace CourseProjectTests.Calculators;

public class ScalarProductCalculatorTests
{
    private GlobalVector _globalVector1;
    private GlobalVector _globalVector2;

    [SetUp]
    public void Setup()
    {
        var vector1 = new[] { 1.0, 2.0, 3.0, 4.0, 5.0 };
        _globalVector1 = new GlobalVector(vector1);
        var vector2 = new[] { 1.0, 2.0, 3.0, 4.0, 5.0 };
        _globalVector2 = new GlobalVector(vector2);
    }

    [TestCase(55.0)]
    public void CalcScalarProductTest(double actual)
    {
        var expected = ScalarProductCalculator.CalcScalarProduct(_globalVector1, _globalVector2);
        Assert.That(expected, Is.EqualTo(actual));
    }
}