//using CourseProject.Core.Global;

//namespace CourseProjectTests.SLAE;

//public class SLAESolverTests
//{
//    private GlobalMatrix _globalMatrix;
//    private GlobalVector _globalVector;

//    [SetUp]
//    public void Setup()
//    {
//        _globalMatrix = new GlobalMatrix()
//        {
//            N = 5,
//            IG = new[] { 0, 0, 0, 2, 5, 6 },
//            JG = new[] { 0, 1, 0, 1, 2, 3 },
//            DI = new[] { Math.Sqrt(2.0), Math.Sqrt(2.0), 1.0, 1.0, 1.0 },
//            GG = new[] { Math.Sqrt(2.0)/2, Math.Sqrt(2.0)/2, Math.Sqrt(2.0)/2, Math.Sqrt(2.0)/2, 0.0, 1.0 },
//        };
//        _globalVector = new GlobalVector(new[] { 4.0, 4.0, 5.0, 6.0, 3.0 });
//    }

//    [Test]
//    public void CalcYTest()
//    {
//        var actual = new GlobalVector(new[] { 2.8284271247461898, 2.8284271247461898, 1.0, 2.0, 1.0 });
//        var expected = SLAESolver.CalcY(_globalMatrix, _globalVector);
//        CollectionAssert.AreEquivalent(expected.VectorArray, actual.VectorArray);
//    }

//    [Test]
//    public void CalcXTest()
//    {
//        var actual = new GlobalVector(new[] { 0.99999999999999989, 0.99999999999999989, 1.0, 1.0, 1.0 });
//        var y = new GlobalVector(new[] { 2.8284271247461898, 2.8284271247461898, 1.0, 2.0, 1.0 });
//        var expected = SLAESolver.CalcX(_globalMatrix, y);
//        CollectionAssert.AreEquivalent(expected.VectorArray, actual.VectorArray);
//    }
//}