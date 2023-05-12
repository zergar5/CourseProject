using CourseProject.Core;
using CourseProject.Core.Global;
using CourseProject.Core.GridComponents;
using CourseProject.FEM;
using CourseProject.TwoDimensional.Assembling.Local;

namespace CourseProject.TwoDimensional;

public class FEMSolution
{
    private readonly Grid<Node2D> _grid;
    private readonly GlobalVector _solution;
    private readonly LocalBasisFunctionsProvider _basisFunctionsProvider;

    public FEMSolution(Grid<Node2D> grid, GlobalVector solution, LocalBasisFunctionsProvider basisFunctionsProvider)
    {
        _grid = grid;
        _solution = solution;
        _basisFunctionsProvider = basisFunctionsProvider;
    }

    public double Calculate(Node2D point)
    {
        if (AreaHas(point))
        {
            var element = _grid.Elements.First(x => ElementHas(x, point));

            var basisFunctions = _basisFunctionsProvider.GetBilinearFunctions(element);

            var sum = element.NodesIndexes
                .Select((t, i) => _solution[t] * basisFunctions[i].Calculate(point))
                .Sum();

            CourseHolder.WriteSolution(point, sum);

            return sum;
        }

        CourseHolder.WriteAreaInfo();
        CourseHolder.WriteSolution(point, double.NaN);
        return double.NaN;
    }

    public double CalcError(Func<Node2D, double> u)
    {
        var trueSolution = new GlobalVector(_solution.Count);

        for (var i = 0; i < _solution.Count; i++)
        {
            trueSolution[i] = u(_grid.Nodes[i]);
        }

        GlobalVector.Subtract(_solution, trueSolution);

        return trueSolution.Norm;
    }

    private bool ElementHas(Element element, Node2D node)
    {
        var leftCornerNode = _grid.Nodes[element.NodesIndexes[0]];
        var rightCornerNode = _grid.Nodes[element.NodesIndexes[^1]];
        return node.X >= leftCornerNode.X && node.Y >= leftCornerNode.Y &&
               node.X <= rightCornerNode.X && node.Y <= rightCornerNode.Y;
    }

    private bool AreaHas(Node2D node)
    {
        var leftCornerNode = _grid.Nodes[0];
        var rightCornerNode = _grid.Nodes[^1];
        return node.X >= leftCornerNode.X && node.Y >= leftCornerNode.Y && 
               node.X <= rightCornerNode.X && node.Y <= rightCornerNode.Y;
    }
}