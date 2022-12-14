namespace CourseProject.IOs;

public class BoundaryConditionIO
{
    private readonly string _path;

    public BoundaryConditionIO(string path)
    {
        _path=path;
    }

    public void ReadFirstCondition(string fileName, out List<int[]> globalNodesNumbersList, out List<double[]> usList)
    {
        using var streamReader = new StreamReader(_path + fileName);

        var boundaryConditionsParameters = streamReader.ReadToEnd().Split('\n');
        globalNodesNumbersList = new List<int[]>(boundaryConditionsParameters.Length);
        usList = new List<double[]>(boundaryConditionsParameters.Length);

        var i = 0;
        foreach (var boundaryConditionParameters in boundaryConditionsParameters)
        {
            var boundaryCondition = boundaryConditionParameters.Split(' ');
            globalNodesNumbersList[i] = new[] { int.Parse(boundaryCondition[0]), int.Parse(boundaryCondition[1]) };
            usList[i++] = new[] { double.Parse(boundaryCondition[3]), double.Parse(boundaryCondition[4]) };
        }
    }

    public void ReadSecondCondition(string fileName, out List<int[]> globalNodesNumbersList, out List<double[]> thetasList)
    {
        using var streamReader = new StreamReader(_path + fileName);

        var boundaryConditionsParameters = streamReader.ReadToEnd().Split('\n');
        globalNodesNumbersList = new List<int[]>(boundaryConditionsParameters.Length);
        thetasList = new List<double[]>(boundaryConditionsParameters.Length);

        var i = 0;
        foreach (var boundaryConditionParameters in boundaryConditionsParameters)
        {
            var boundaryCondition = boundaryConditionParameters.Split(' ');
            globalNodesNumbersList[i] = new[] { int.Parse(boundaryCondition[0]), int.Parse(boundaryCondition[1]) };
            thetasList[i++] = new[] { double.Parse(boundaryCondition[3]), double.Parse(boundaryCondition[4]) };
        }
    }

    public void ReadThirdCondition(string fileName, out List<int[]> globalNodesNumbersList, out List<double[]> usList, out List<double> betas)
    {
        using var streamReader = new StreamReader(_path + fileName);

        var boundaryConditionsParameters = streamReader.ReadToEnd().Split('\n');
        globalNodesNumbersList = new List<int[]>(boundaryConditionsParameters.Length);
        usList = new List<double[]>(boundaryConditionsParameters.Length);
        betas = new List<double>(boundaryConditionsParameters.Length);

        var i = 0;
        foreach (var boundaryConditionParameters in boundaryConditionsParameters)
        {
            var boundaryCondition = boundaryConditionParameters.Split(' ');
            globalNodesNumbersList[i] = new[] { int.Parse(boundaryCondition[0]), int.Parse(boundaryCondition[1]) };
            betas[i] = double.Parse(boundaryCondition[3]);
            usList[i++] = new[] { double.Parse(boundaryCondition[4]), double.Parse(boundaryCondition[5]) };
        }
    }
}