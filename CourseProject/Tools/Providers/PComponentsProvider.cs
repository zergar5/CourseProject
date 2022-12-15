using CourseProject.Models.Grid;

namespace CourseProject.Tools.Providers;

public class PComponentsProvider
{
    private readonly Func<double, double, double> _f;

    private readonly NodeFinder _nodeFinder;

    public PComponentsProvider(Func<double, double, double> f, NodeFinder nodeFinder)
    {
        _f = f;
        _nodeFinder = nodeFinder;
    }

    public double CalcRightPart(int nodeNumber)
    {
        var node = _nodeFinder.FindNode(nodeNumber);
        return _f(node.R, node.Z);
    }
}