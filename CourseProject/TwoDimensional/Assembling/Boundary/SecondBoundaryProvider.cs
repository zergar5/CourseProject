using CourseProject.Core;
using CourseProject.Core.Base;
using CourseProject.Core.Boundary;
using CourseProject.Core.Global;
using CourseProject.Core.GridComponents;
using CourseProject.Core.Local;
using CourseProject.TwoDimensional.Parameters;

namespace CourseProject.TwoDimensional.Assembling.Boundary;

public class SecondBoundaryProvider
{
    public readonly Grid<Node2D> Grid;
    public readonly MaterialFactory MaterialFactory;
    private List<SecondCondition> _conditions = new();

    public SecondBoundaryProvider(Grid<Node2D> grid, MaterialFactory materialFactory)
    {
        Grid = grid;
        MaterialFactory = materialFactory;
    }

    public SecondCondition[] GetConditions()
    {
        var conditions = _conditions.ToArray();
        _conditions.Clear();
        return conditions;
    }

    public SecondBoundaryProvider CreateConditions(int[] elementsIndexes, Bound[] bounds,
        Func<Node2D, double> uDerivative)
    {
        var conditions = new List<SecondCondition>(elementsIndexes.Length);

        for (var i = 0; i < elementsIndexes.Length; i++)
        {
            var (indexes, h) = Grid.Elements[elementsIndexes[i]].GetBoundNodeIndexes(bounds[i]);

            BaseVector vector;

            if (bounds[i] == Bound.Lower || bounds[i] == Bound.Upper)
            {
                vector = GetRVector(indexes, h, uDerivative);
            }
            else
            {
                vector = GetZVector(indexes, h, uDerivative);
            }

            var material = MaterialFactory.GetById(Grid.Elements[elementsIndexes[i]].MaterialId);

            BaseVector.Multiply(material.Lambdas, vector);

            conditions.Add(new SecondCondition(new LocalVector(indexes, vector)));
        }

        _conditions.AddRange(conditions);

        return this;
    }

    private BaseVector GetRVector(int[] indexes, double h, Func<Node2D, double> uDerivative)
    {
        var vector = new BaseVector(indexes.Length)
        {
            [0] = 2d * uDerivative(Grid.Nodes[indexes[0]]) + uDerivative(Grid.Nodes[indexes[1]]),
            [1] = uDerivative(Grid.Nodes[indexes[0]]) + 2d * uDerivative(Grid.Nodes[indexes[1]])
        };

        BaseVector.Multiply(h * Grid.Nodes[indexes[0]].X / 6.0, vector);

        return vector;
    }

    private BaseVector GetZVector(int[] indexes, double h, Func<Node2D, double> uDerivative)
    {
        var vector = new BaseVector(indexes.Length);

        return vector;
    }
}