using CourseProject.Models.Grid;

namespace CourseProject.Tools.Providers;

public class FProvider
{
    private readonly Func<double, double, double> _f;

    private readonly Grid _grid;

    public FProvider(Grid grid, Func<double, double, double> f)
    {
        _grid = grid;
        _f = f;
    }

    public double CalcRightPart(int nodeNumber)
    {
        var node = _grid.Nodes[nodeNumber];
        return _f(node.R, node.Z);
    }
}