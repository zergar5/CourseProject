using CourseProject.Models.GlobalParts;
using CourseProject.SLAE.Solvers;

namespace CourseProjectTests.SLAE;

public class CholeskyMCGTests
{
    private MCG _choleskyMcg;
    private GlobalMatrix _globalMatrix;
    private GlobalVector _startGlobalVector;
    private GlobalVector _bGlobalVector;

    [SetUp]
    public void Setup()
    {
        _choleskyMcg = new MCG();

        _globalMatrix = new GlobalMatrix
        {
            N = 5,
            DI = new[] { 2.0, 2.0, 2.0, 2.0, 2.0 },
            GG = new[] { 1.0, 1.0, 1.0, 1.0, 1.0, 1.0 },
            IG = new[] { 0, 0, 0, 2, 4, 6 },
            JG = new[] { 0, 1, 1, 2, 0, 2 }
        };

        _startGlobalVector = new GlobalVector(new[] { 0.0, 0.0, 0.0, 0.0, 0.0 });
        _bGlobalVector = new GlobalVector(new[] { 4.0, 4.0, 6.0, 4.0, 4.0 });
    }

    [Test]
    public void SolveTest()
    {
        var actualVector = new[] { 0.99999999999999956, 0.99999999999999956, 1.0000000000000004, 0.99999999999999989, 0.99999999999999989 };
        var expectedVector = _choleskyMcg.Solve(_globalMatrix, _startGlobalVector, _bGlobalVector, 1.0e-16, 10000);
        CollectionAssert.AreEqual(expectedVector.VectorArray, actualVector);
    }
}