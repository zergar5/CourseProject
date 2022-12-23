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

        var boundaryConditionsParameters = streamReader.ReadToEnd().Replace('.', ',').Split("\r\n");
        globalNodesNumbersList = new List<int[]>();
        usList = new List<double[]>();

        foreach (var boundaryConditionParameters in boundaryConditionsParameters)
        {
            var boundaryCondition = boundaryConditionParameters.Split(' ');
            globalNodesNumbersList.Add(new[] { int.Parse(boundaryCondition[0]), int.Parse(boundaryCondition[1]) });
            usList.Add(new[] { double.Parse(boundaryCondition[2]), double.Parse(boundaryCondition[3]) });
        }
    }

    public void ReadSecondCondition(string fileName, out List<int[]> globalNodesNumbersList, out List<double[]> thetasList)
    {
        using var streamReader = new StreamReader(_path + fileName);

        var boundaryConditionsParameters = streamReader.ReadToEnd().Replace('.', ',').Split("\r\n");
        globalNodesNumbersList = new List<int[]>();
        thetasList = new List<double[]>();

        foreach (var boundaryConditionParameters in boundaryConditionsParameters)
        {
            var boundaryCondition = boundaryConditionParameters.Split(' ');
            globalNodesNumbersList.Add(new[] { int.Parse(boundaryCondition[0]), int.Parse(boundaryCondition[1]) });
            thetasList.Add(new[] { double.Parse(boundaryCondition[2]), double.Parse(boundaryCondition[3]) });
        }
    }

    public void ReadThirdCondition(string fileName, out List<int[]> globalNodesNumbersList, out List<double> betasList, out List<double[]> usList)
    {
        using var streamReader = new StreamReader(_path + fileName);

        var boundaryConditionsParameters = streamReader.ReadToEnd().Replace('.', ',').Split("\r\n");
        globalNodesNumbersList = new List<int[]>();
        usList = new List<double[]>();
        betasList = new List<double>();

        foreach (var boundaryConditionParameters in boundaryConditionsParameters)
        {
            var boundaryCondition = boundaryConditionParameters.Split(' ');
            globalNodesNumbersList.Add(new[] { int.Parse(boundaryCondition[0]), int.Parse(boundaryCondition[1]) });
            betasList.Add(double.Parse(boundaryCondition[2]));
            usList.Add(new[] { double.Parse(boundaryCondition[3]), double.Parse(boundaryCondition[4]) });
        }
    }
}