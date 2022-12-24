using CourseProject.Models.GlobalParts;
using CourseProject.Models.Grid;
using CourseProject.Tools;

namespace CourseProject;

public class SolutionFinder
{
    private readonly Grid _grid;
    private readonly GlobalVector _globalVector;
    private readonly NodeFinder _nodeFinder;

    public SolutionFinder(Grid grid, GlobalVector globalVector, NodeFinder nodeFinder)
    {
        _grid = grid;
        _globalVector = globalVector;
        _nodeFinder = nodeFinder;
    }

    public double FindSolution(Node node)
    {
        var result = 0.0;
        foreach (var element in _grid)
        {
            if (!CheckInside(element, node)) continue;
            var i = 0;
            foreach (var globalNodeNumber in element)
            {
                if (_nodeFinder.FindNode(globalNodeNumber).Equals(node))
                {
                    result = _globalVector[globalNodeNumber];
                    return result;
                }
                result += element.LocalBasisFunctions[i++].CalcFunction(node.R, node.Z) *
                          _globalVector[globalNodeNumber];
            }
            return result;
        }
        return result;
    }

    private bool CheckInside(Element element, Node node)
    {
        var leftCornerNode = _nodeFinder.FindNode(element.GlobalNodesNumbers[0]);
        var rightCornerNode = _nodeFinder.FindNode(element.GlobalNodesNumbers[^1]);
        return leftCornerNode.R <= node.R && node.R <= rightCornerNode.R && leftCornerNode.Z <= node.Z &&
               node.Z <= rightCornerNode.Z;
    }

    public bool CheckArea(Node node)
    {
        var leftCornerNode = _grid.CornerNodes[0];
        var rightCornerNode = _grid.CornerNodes[1];
        return node.R >= leftCornerNode.R && node.Z >= leftCornerNode.Z &&
               node.R <= rightCornerNode.R && node.Z <= rightCornerNode.Z;
    }
}