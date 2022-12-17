using CourseProject.Factories;
using CourseProject.Models.GlobalParts;
using CourseProject.Models.Grid;
using CourseProject.Models.LocalParts;
using CourseProject.SLAESolution;
using CourseProject.Tools.Providers;
using CourseProject.Tools;

namespace CourseProjectTests.SLAE;

public class CholeskyMCGTests
{
    private CholeskyMCG _choleskyMcg;
    private GlobalMatrix _globalMatrix;
    private GlobalVector _startGlobalVector;
    private GlobalVector _bGlobalVector;

    [SetUp]
    public void Setup()
    {
        _choleskyMcg = new CholeskyMCG();

        _globalMatrix = new GlobalMatrix
        {
            N = 3,
            DI = new[] { 10.0, 10.0, 10.0 },
            GG = new[] { 1.0 },
            IG = new[] { 0, 0, 0, 1 },
            JG = new[] { 0 }
        };

        _startGlobalVector = new GlobalVector(new[] { 0.0, 0.0, 0.0 });
        _bGlobalVector = new GlobalVector(new[] { 13.0, 20.0, 31.0 });
    }

    [Test]
    public void SolveTest()
    {
        var actualVector = new[] { 0.99999999999999989, 2.0, 2.9999999999999996 };
        var expectedVector = _choleskyMcg.Solve(_globalMatrix, _startGlobalVector, _bGlobalVector, 1.0e-16, 10000);
        CollectionAssert.AreEqual(expectedVector.VectorArray, actualVector);
    }
}