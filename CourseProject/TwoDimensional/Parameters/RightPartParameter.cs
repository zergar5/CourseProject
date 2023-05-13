using CourseProject.Core;
using CourseProject.Core.GridComponents;
using CourseProject.FEM.Parameters;

namespace CourseProject.TwoDimensional.Parameters;

public class RightPartParameter : IFunctionalParameter
{
    private readonly Func<Node2D, double, double> _function;
    private readonly Grid<Node2D> _grid;

    public RightPartParameter(
        Func<Node2D, double, double> function,
        Grid<Node2D> grid
    )
    {
        _function = function;
        _grid = grid;
    }

    public double Calculate(int nodeNumber, double timeLayer)
    {
        var node = _grid.Nodes[nodeNumber];
        return _function(node, timeLayer);
    }

    public double Calculate(Node2D node, double timeLayer)
    {
        return _function(node, timeLayer);
    }
}