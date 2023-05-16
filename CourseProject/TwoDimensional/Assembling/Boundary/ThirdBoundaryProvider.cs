using CourseProject.Calculus;
using CourseProject.Core;
using CourseProject.Core.Base;
using CourseProject.Core.Boundary;
using CourseProject.Core.GridComponents;
using CourseProject.Core.Local;
using CourseProject.FEM.Assembling;
using CourseProject.TwoDimensional.Parameters;

namespace CourseProject.TwoDimensional.Assembling.Boundary;

public class ThirdBoundaryProvider
{
    private readonly Grid<Node2D> _grid;
    private readonly MaterialFactory _materialFactory;
    private readonly Func<Node2D, double, double> _u;
    private readonly DerivativeCalculator _derivativeCalculator;
    private readonly BaseMatrix _templateMatrixR;
    private readonly BaseMatrix _templateMatrixZ;

    public ThirdBoundaryProvider
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

    public ThirdCondition[] GetConditions(int[] elementsIndexes, Bound[] bounds, double[] betas, double time)
    {
        var conditions = new List<ThirdCondition>(elementsIndexes.Length);

        for (var i = 0; i < elementsIndexes.Length; i++)
        {
            var (indexes, h) = _grid.Elements[elementsIndexes[i]].GetBoundNodeIndexes(bounds[i]);
            var lambdas = _materialFactory.GetById(_grid.Elements[elementsIndexes[i]].MaterialId).Lambdas;

            var uS = GetUs(indexes, bounds[i], lambdas, betas[i], time);

            BaseVector vector;
            BaseMatrix matrix;
            if (bounds[i] == Bound.Left || bounds[i] == Bound.Right)
            {
                vector = GetRVector(indexes, h, uS, betas[i]);
                matrix= GetRMatrix(indexes, h, betas[i]);
            }
            else
            {
                vector = GetZVector(indexes, h, uS, betas[i]);
                matrix= GetZMatrix(indexes, h, betas[i]);
            }

            conditions.Add(new ThirdCondition(new LocalMatrix(indexes, matrix),
                new LocalVector(indexes, vector)));
        }

        return conditions.ToArray();
    }

    private BaseVector GetRVector(int[] indexes, double h, BaseVector uS, double beta)
    {
        var vector = BaseVector.Multiply
        (
            beta * h * _grid.Nodes[indexes[0]].R / 6d,
            _templateMatrixR * uS
        );

        return vector;
    }

    private BaseMatrix GetRMatrix(int[] indexes, double h, double beta)
    {
        var matrix = beta * h * _grid.Nodes[indexes[0]].R / 6d * _templateMatrixR;

        return matrix;
    }

    private BaseVector GetZVector(int[] indexes, double h, BaseVector uS, double beta)
    {
        var vector = BaseVector.Sum
        (
            BaseVector.Multiply(beta * h * _grid.Nodes[indexes[0]].R / 6d, _templateMatrixR * uS),
            BaseVector.Multiply(beta * Math.Pow(h, 2) / 12d, _templateMatrixZ * uS)
        );

        return vector;
    }

    private BaseMatrix GetZMatrix(int[] indexes, double h, double beta)
    {
        var matrix = BaseMatrix.Sum
        (
            beta * h * _grid.Nodes[indexes[0]].R / 6d * _templateMatrixR,
            beta * Math.Pow(h, 2) / 12d * _templateMatrixZ
        );

        return matrix;
    }

    private BaseVector GetUs(int[] indexes, Bound bound, double[] lambdas, double beta, double time)
    {
        var vector = new BaseVector(indexes.Length);
        switch (bound)
        {
            case Bound.Lower:
                vector[0] = (lambdas[0] *
                             -_derivativeCalculator.Calculate
                             (
                                 _u,
                                 _grid.Nodes[indexes[0]],
                                 time,
                                 'z'
                             ) +
                             beta * _u(_grid.Nodes[indexes[0]], time)
                    ) / beta;
                vector[1] = (lambdas[2] *
                             -_derivativeCalculator.Calculate
                             (
                                 _u,
                                 _grid.Nodes[indexes[1]],
                                 time,
                                 'z'
                             ) +
                             beta * _u(_grid.Nodes[indexes[1]], time)
                    ) / beta;
                break;
            case Bound.Upper:
                vector[0] = (lambdas[6] *
                             _derivativeCalculator.Calculate
                             (
                                 _u,
                                 _grid.Nodes[indexes[0]],
                                 time,
                                 'z'
                             ) +
                             beta * _u(_grid.Nodes[indexes[0]], time)
                    ) / beta;
                vector[1] = (lambdas[8] *
                             _derivativeCalculator.Calculate
                             (
                                 _u,
                                 _grid.Nodes[indexes[1]],
                                 time,
                                 'z'
                             ) +
                             beta * _u(_grid.Nodes[indexes[1]], time)
                    ) / beta;
                break;
            case Bound.Left:
                vector[0] = (lambdas[0] *
                             -_derivativeCalculator.Calculate
                             (
                                 _u,
                                 _grid.Nodes[indexes[0]],
                                 time,
                                 'r'
                             ) +
                             beta * _u(_grid.Nodes[indexes[0]], time)
                    ) / beta;
                vector[1] = (lambdas[6] *
                             -_derivativeCalculator.Calculate
                             (
                                 _u,
                                 _grid.Nodes[indexes[1]],
                                 time,
                                 'r'
                             ) +
                             beta * _u(_grid.Nodes[indexes[1]], time)
                    ) / beta;
                break;
            case Bound.Right:
                vector[0] = (lambdas[2] *
                             _derivativeCalculator.Calculate
                             (
                                 _u,
                                 _grid.Nodes[indexes[0]],
                                 time,
                                 'r'
                             ) +
                             beta * _u(_grid.Nodes[indexes[0]], time)
                    ) / beta;
                vector[1] = (lambdas[8] *
                             _derivativeCalculator.Calculate
                             (
                                 _u,
                                 _grid.Nodes[indexes[1]],
                                 time,
                                 'r'
                             ) +
                             beta * _u(_grid.Nodes[indexes[1]], time)
                    ) / beta;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(bound), bound, null);
        }

        return vector;
    }
}