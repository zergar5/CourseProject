using CourseProject.Models.Grid;

namespace CourseProject.IOs;

public class MaterialIO
{
    private readonly string _path;

    public MaterialIO(string path)
    {
        _path = path;
    }

    public void ReadMaterialsParametersFromFile(string fileName, out List<double[]> lambdasList, out List<double> gammasList)
    {
        using var streamReader = new StreamReader(_path + fileName);
        var materialsParameters = streamReader.ReadToEnd().Replace("\r", "").Replace('.', ',').Split('\n');
        lambdasList = new List<double[]>(materialsParameters.Length);
        gammasList = new List<double>(materialsParameters.Length);
        var i = 0;
        foreach (var materialParameters in materialsParameters)
        {
            var materialData = materialParameters.Split(' ');
            lambdasList.Add(new ReadOnlySpan<string>(materialData, 0, 9).ToArray().Select(double.Parse).ToArray());
            gammasList.Add(double.Parse(materialData[^1]));
        }
    }

    public Material[] ReadMaterialsFromFile(string fileName)
    {
        using var streamReader = new StreamReader(_path + fileName);
        var materialsParameters = streamReader.ReadToEnd().Replace("\r", "").Replace('.', ',').Split('\n');
        var materials = new Material[materialsParameters.Length];
        var i = 0;
        foreach (var materialParameters in materialsParameters)
        {
            var materialData = materialParameters.Split(' ');
            var id = int.Parse(materialData[0]);
            var lambdas = new ReadOnlySpan<string>(materialData, 1, 9).ToArray().Select(double.Parse).ToArray();
            var gamma = double.Parse(materialData[^1]);

            materials[i++] = new Material(id, lambdas, gamma);
        }

        return materials;
    }
}