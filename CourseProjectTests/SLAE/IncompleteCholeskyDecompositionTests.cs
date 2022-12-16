using CourseProject.Models.GlobalParts;
using CourseProject.SLAESolution;

namespace CourseProjectTests.SLAE;

public class IncompleteCholeskyDecompositionTests
{
    private GlobalMatrix _globalMatrix;
    [SetUp]
    public void Setup()
    {
        _globalMatrix = new GlobalMatrix
        {
            N = 5,
            IG = new[] { 0, 0, 0, 2, 5, 6 },
            JG = new[] { 0, 1, 0, 1, 2, 3 },
            DI = new[] { 2.0, 2.0, 2.0, 2.0, 2.0 },
            GG = new[] { 1.0, 1.0, 1.0, 1.0, 1.0, 1.0 },
        };
    }

    [Test]
    public void DecompositionTest()
    {
        var actualGlobalMatrix = new GlobalMatrix
        {
            N = 5,
            IG = new[] { 0, 0, 0, 2, 5, 6 },
            JG = new[] { 0, 1, 0, 1, 2, 3 },
            DI = new[] { Math.Sqrt(2.0), Math.Sqrt(2.0), 1.0, 1.0, 1.0 },
            GG = new[] { 0.70710678118654746, 0.70710678118654746, 0.70710678118654746, 0.70710678118654746, 2.2204460492503131E-16, 1.0 },
        };
        var expectedSparseMatrix = IncompleteCholeskyDecomposition.Decomposition(_globalMatrix);
        CollectionAssert.AreEquivalent(expectedSparseMatrix.GG, actualGlobalMatrix.GG);
        CollectionAssert.AreEquivalent(expectedSparseMatrix.DI, actualGlobalMatrix.DI);
    }
}