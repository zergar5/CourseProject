using CourseProject.Models.LocalParts;

namespace CourseProjectTests.Models.LocalParts;

public class LocalMatrixTests
{
    private LocalMatrix _localMatrix;

    [SetUp]
    public void Setup()
    {
        var matrix = new [,]
        {
            { 1.0, 2.0, 3.0 },
            { 4.0, 5.0, 6.0 },
            { 7.0, 8.0, 9.0 }
        };
        _localMatrix = new LocalMatrix(matrix);
    }

    [Test]
    public void MatrixAdditionTest()
    {
        var matrix = new[,]
        {
            { 1.0, 2.0, 3.0 },
            { 4.0, 5.0, 6.0 },
            { 7.0, 8.0, 9.0 }
        };
        var localMatrix = new LocalMatrix(matrix);
        var actualMatrix = new[,]
        {
            { 2.0, 4.0, 6.0 },
            { 8.0, 10.0, 12.0 },
            { 14.0, 16.0, 18.0 }
        };
        var actualLocalMatrix = new LocalMatrix(actualMatrix);
        var expectedMatrix = _localMatrix + localMatrix;
        CollectionAssert.AreEqual(expectedMatrix.Matrix, actualLocalMatrix.Matrix);
    }

    [TestCase(5.0)]
    public void MultiplyingMatrixOnNumberTest(double coefficient)
    {
        var actualMatrix = new[,]
        {
            { 5.0, 10.0, 15.0 },
            { 20.0, 25.0, 30.0 },
            { 35.0, 40.0, 45.0 }
        };
        var actualLocalMatrix = new LocalMatrix(actualMatrix);
        var expectedMatrix = _localMatrix * coefficient;
        CollectionAssert.AreEqual(expectedMatrix.Matrix, actualLocalMatrix.Matrix);
    }

    [TestCase(5.0)]
    public void MultiplyingMatrixOnVectorTest(double coefficient)
    {
        var vector = new[] { 1.0, 2.0, 3.0 };
        var localVector = new LocalVector(vector);
        var expectedVector = _localMatrix * localVector;

        var actualVector = new[] { 14.0, 32.0, 50.0 };
        var actualLocalVector = new LocalVector(actualVector);
        CollectionAssert.AreEqual(expectedVector.VectorArray, actualLocalVector.VectorArray);
    }
}