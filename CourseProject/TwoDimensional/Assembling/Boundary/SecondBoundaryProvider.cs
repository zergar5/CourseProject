using CourseProject.Calculus;
using CourseProject.Core;
using CourseProject.Core.Base;
using CourseProject.Core.Boundary;
using CourseProject.Core.GridComponents;
using CourseProject.Core.Local;
using CourseProject.FEM.Assembling;
using CourseProject.TwoDimensional.Parameters;

namespace CourseProject.TwoDimensional.Assembling.Boundary;

public class SecondBoundaryProvider
{
    private readonly Grid<Node2D> _grid;
    private readonly MaterialFactory _materialFactory;
    private readonly Func<Node2D, double, double> _u;
    private readonly DerivativeCalculator _derivativeCalculator;
    private readonly BaseMatrix _templateMatrixR;
    private readonly BaseMatrix _templateMatrixZ;

    public SecondBoundaryProvider
    (
        Grid<Node2D> grid,
        MaterialFactory materialFactory,
        Func<Node2D, double, double> u,
        DerivativeCalculator derivativeCalculator,
        ITemplateMatrixProvider templateMatrixProviderR,
        ITemplateMatrixProvider templateMatrixProviderZ
    )
    {
        _grid = grid;
        _materialFactory = materialFactory;
        _u = u;
        _derivativeCalculator = derivativeCalculator;
        _templateMatrixR = templateMatrixProviderR.GetMatrix();
        _templateMatrixZ = templateMatrixProviderZ.GetMatrix();
    }

    public SecondCondition[] GetConditions(int[] elementsIndexes, Bound[] bounds, double time)
    {
        var conditions = new List<SecondCondition>(elementsIndexes.Length);

        for (var i = 0; i < elementsIndexes.Length; i++)
        {
            var (indexes, h) = _grid.Elements[elementsIndexes[i]].GetBoundNodeIndexes(bounds[i]);

            BaseVector vector;

            var lambdas = _materialFactory.GetById(_grid.Elements[elementsIndexes[i]].MaterialId).Lambdas;

            if (bounds[i] == Bound.Left || bounds[i] == Bound.Right)
            {
                vector = GetRVector(indexes, bounds[i], h, lambdas, time);
            }
            else
            {
                vector = GetZVector(indexes, bounds[i], h, lambdas, time);
            }

            conditions.Add(new SecondCondition(new LocalVector(indexes, vector)));
        }

        return conditions.ToArray();
    }

    private BaseVector GetRVector(int[] indexes, Bound bound, double h, double[] lambdas, double time)
    {
        var vector = new BaseVector(indexes.Length)
        {
            [0] = _derivativeCalculator.Calculate(_u, _grid.Nodes[indexes[0]], time, 'r'),
            [1] = _derivativeCalculator.Calculate(_u, _grid.Nodes[indexes[1]], time, 'r')
        };

        if (bound == Bound.Left)
        {
            vector[0] *= -lambdas[0];
            vector[1] *= -lambdas[6];
        }
        else
        {
            vector[0] *= lambdas[2];
            vector[1] *= lambdas[8];
        }

        vector = BaseVector.Multiply(h * _grid.Nodes[indexes[0]].R / 6d, _templateMatrixR * vector);

        return vector;
    }

    private BaseVector GetZVector(int[] indexes, Bound bound, double h, double[] lambdas, double time)
    {
        var vector = new BaseVector(indexes.Length)
        {
            [0] = _derivativeCalculator.Calculate(_u, _grid.Nodes[indexes[0]], time, 'z'),
            [1] = _derivativeCalculator.Calculate(_u, _grid.Nodes[indexes[1]], time, 'z')
        };

        if (bound == Bound.Lower)
        {
            vector[0] *= -lambdas[0];
            vector[1] *= -lambdas[2];
        }
        else
        {
            vector[0] *= lambdas[6];
            vector[1] *= lambdas[8];
        }

        vector = BaseVector.Sum
        (
            BaseVector.Multiply(h * _grid.Nodes[indexes[0]].R / 6d, _templateMatrixR * vector),
            BaseVector.Multiply(Math.Pow(h, 2) / 12d, _templateMatrixZ * vector)
        );

        return vector;
    }
}