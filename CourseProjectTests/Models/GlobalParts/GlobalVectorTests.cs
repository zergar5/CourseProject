namespace CourseProjectTests.Models.GlobalParts;

public class GlobalVectorTests
{
    private GlobalVector _globalVector;

    [SetUp]
    public void Setup()
    {
        var vector1 = new[] { 1.0, 2.0, 3.0, 4.0, 5.0 };
        _globalVector = new GlobalVector(vector1);
    }

    [Test]
    public void PlaceLocalVectorTest()
    {
        var vector = new[] { 1.0, 1.0, 1.0, 1.0 };
        var localVector = new LocalVector(vector);
        var globalNodesNumbers = new[] { 0, 1, 3, 4 };
        _globalVector.PlaceLocalVector(localVector, globalNodesNumbers);

        var actualVector = new[] { 2.0, 3.0, 3.0, 5.0, 6.0 };
        var actualGlobalVector = new GlobalVector(actualVector);

        CollectionAssert.AreEqual(_globalVector.VectorArray, actualGlobalVector.VectorArray);
    }

    [Test]
    public void VectorAdditionTest()
    {
        var vector1 = new[] { 6.0, 8.0, 10.0, 12.0, 14.0 };
        var actualVector = new GlobalVector(vector1);
        var vector2 = new[] { 5.0, 6.0, 7.0, 8.0, 9.0 };
        var localVector2 = new LocalVector(vector2);
        var expectedVector = _globalVector + localVector2;
        CollectionAssert.AreEqual(expectedVector.VectorArray, actualVector.VectorArray);
    }

    [TestCase(5.0)]
    public void MultiplyingVectorOnNumberTest(double coefficient)
    {
        var vector = new[] { 5.0, 10.0, 15.0, 20.0, 25.0 };
        var actualVector = new GlobalVector(vector);
        var expectedVector = _globalVector * coefficient;
        CollectionAssert.AreEqual(expectedVector.VectorArray, actualVector.VectorArray);
    }

    [TestCase(7.4161984870956629)]
    public void CalcNormTest(double actualNorm)
    {
        var expectedNorm = _globalVector.CalcNorm();
        Assert.That(actualNorm, Is.EqualTo(expectedNorm));
    }

    [Test]
    public void CloneVectorTest()
    {
        var expectedVector = (GlobalVector)_globalVector.Clone();
        CollectionAssert.AreEqual(expectedVector.VectorArray, _globalVector.VectorArray);
    }
}