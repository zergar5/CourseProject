namespace CourseProjectTests.Factories;

public class MaterialFactoryTests
{
    private MaterialFactory _materialFactory;

    [SetUp]
    public void Setup()
    {
        var lambdas = new List<double[]>
        {
            new[] { 1.0, 2.0, 3.0, 4.0, 5.0, 6.0, 7.0, 8.0, 9.0 },
            new[] { 1.0, 2.0, 3.0, 4.0, 5.0, 4.0, 3.0, 2.0, 1.0 },
            new[] { 9.0, 8.0, 7.0, 6.0, 5.0, 4.0, 3.0, 2.0, 1.0 }
        };
        var gammas = new List<double>
        {
            1.0,
            2.0,
            3.0
        };
        _materialFactory = new MaterialFactory(lambdas, gammas);
    }

    [Test]
    public void MaterialFactoryConstructorTest()
    {
        var actualLambdasData = new Dictionary<int, double[]>
        {
            { 0, new[] { 1.0, 2.0, 3.0, 4.0, 5.0, 6.0, 7.0, 8.0, 9.0 } },
            { 1, new[] { 1.0, 2.0, 3.0, 4.0, 5.0, 4.0, 3.0, 2.0, 1.0 } },
            { 2, new[] { 9.0, 8.0, 7.0, 6.0, 5.0, 4.0, 3.0, 2.0, 1.0 } }
        };
        var actualGammasData = new Dictionary<int, double>
        {
            { 0, 1.0 },
            { 1, 2.0 },
            { 2, 3.0 }
        };
        CollectionAssert.AreEqual(_materialFactory.LambdasData, actualLambdasData);
        CollectionAssert.AreEqual(_materialFactory.GammasData, actualGammasData);
    }

    [Test]
    public void CreateMaterialTest()
    {
        const int id = 0;
        var lambdas = new[] { 1.0, 2.0, 3.0, 4.0, 5.0, 6.0, 7.0, 8.0, 9.0 };
        const double gamma = 1.0;
        var actualMaterial = new Material(id, lambdas, gamma);
        var expectedMaterial = _materialFactory.CreateMaterial(id);
        Assert.That(actualMaterial, Is.EqualTo(expectedMaterial));
    }
}