using CourseProject.Models.LocalParts;

namespace CourseProjectTests.Models.LocalParts;

public class LocalVectorTests
{
    private LocalVector _localVector;

    [SetUp]
    public void Setup()
    {
        var vector1 = new[] { 1.0, 2.0, 3.0, 4.0, 5.0 };
        _localVector = new LocalVector(vector1);
    }

    [Test]
    public void VectorAdditionTest()
    {
        var vector1 = new[] { 6.0, 8.0, 10.0, 12.0, 14.0 };
        var actualVector = new LocalVector(vector1);
        var vector2 = new[] { 5.0, 6.0, 7.0, 8.0, 9.0 };
        var localVector2 = new LocalVector(vector2);
        var expectedVector = _localVector + localVector2;
        CollectionAssert.AreEqual(expectedVector.VectorArray, actualVector.VectorArray);
    }

    [TestCase(5.0)]
    public void MultiplyingVectorOnNumberTest(double coefficient)
    {
        var vector = new[] { 5.0, 10.0, 15.0, 20.0, 25.0 };
        var actualVector = new LocalVector(vector);
        var expectedVector = _localVector * coefficient;
        CollectionAssert.AreEqual(expectedVector.VectorArray, actualVector.VectorArray);
    }
}