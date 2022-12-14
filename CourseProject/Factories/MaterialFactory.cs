using CourseProject.Models.Grid;

namespace CourseProject.Factories;

public class MaterialFactory
{
    private readonly Dictionary<int, double[]> _lambdasData;
    private readonly Dictionary<int, double> _gammasData;

    public MaterialFactory(IEnumerable<double[]> lambdas, IEnumerable<double> gammas)
    {
        _lambdasData = lambdas.Select((value, index) => new KeyValuePair<int, double[]>(index, value))
            .ToDictionary(index => index.Key, value => value.Value);
        _gammasData = gammas.Select((value, index) => new KeyValuePair<int, double>(index, value)).ToDictionary(index => index.Key, value => value.Value);
    }

    public Material CreateMaterial(int index)
    {
        var material = new Material(index, _lambdasData[index], _gammasData[index]);

        return material;
    }
}