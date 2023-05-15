using CourseProject.Core;
using CourseProject.Core.Boundary;
using CourseProject.Core.GridComponents;

namespace CourseProject.TwoDimensional.Assembling.Boundary;

public class FirstBoundaryProvider
{
    private readonly Grid<Node2D> _grid;
    private readonly Func<Node2D, double, double> _u;

    public FirstBoundaryProvider(Grid<Node2D> grid, Func<Node2D, double, double> u)
    {
        _grid = grid;
        _u = u;
    }

    public FirstCondition[] GetConditions(int[] elementsIndexes, Bound[] bounds, double time)
    {
        var conditions = new List<FirstCondition>(elementsIndexes.Length);

        for (var i = 0; i < elementsIndexes.Length; i++)
        {
            var (indexes, _) = _grid.Elements[elementsIndexes[i]].GetBoundNodeIndexes(bounds[i]);

            var values = new double[indexes.Length];

            for (var j = 0; j < indexes.Length; j++)
            {
                values[j] = Calculate(indexes[j], time);
            }

            conditions.Add(new FirstCondition(indexes, values));
        }

        return conditions.ToArray();
    }

    public (int[], Bound[]) GetArrays(int elementsByLength, int elementsByHeight)
    {
        var elementsIndexes = new List<int>();
        var bounds = new List<Bound>();

        for (var i = 0; i < elementsByLength; i++)
        {
            elementsIndexes.Add(i);
            bounds.Add(Bound.Lower);
        }

        for (var i = 0; i < elementsByHeight; i++)
        {
            elementsIndexes.Add(i * elementsByLength);
            bounds.Add(Bound.Left);
        }

        for (var i = 0; i < elementsByHeight; i++)
        {

            elementsIndexes.Add((i + 1) * elementsByLength - 1);
            bounds.Add(Bound.Right);
        }

        for (var i = elementsByLength * (elementsByHeight - 1); i < elementsByLength * elementsByHeight; i++)
        {
            elementsIndexes.Add(i);
            bounds.Add(Bound.Upper);
        }

        return (elementsIndexes.ToArray(), bounds.ToArray());
    }

    private double Calculate(int index, double time)
    {
        return _u(_grid.Nodes[index], time);
    }
}