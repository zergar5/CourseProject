using CourseProject.Calculus;
using CourseProject.Core;
using CourseProject.Core.Base;
using CourseProject.Core.GridComponents;
using CourseProject.Core.Local;
using CourseProject.FEM;
using CourseProject.FEM.Assembling.Local;
using CourseProject.FEM.Parameters;
using CourseProject.GridGenerator.Area.Core;
using CourseProject.TwoDimensional.Parameters;

namespace CourseProject.TwoDimensional.Assembling.Local;

public class LocalAssembler : ILocalAssembler
{
    private readonly Grid<Node2D> _grid;
    private readonly LocalBasisFunctionsProvider _localBasisFunctionsProvider;
    private readonly MaterialFactory _materialFactory;
    private readonly LambdaInterpolateProvider _lambdaInterpolateProvider;
    private readonly IFunctionalParameter _functionalParameter;
    private readonly DoubleIntegralCalculator _doubleIntegralCalculator;
    private readonly DerivativeCalculator _derivativeCalculator;

    public LocalAssembler
    (
        Grid<Node2D> grid,
        LocalBasisFunctionsProvider localBasisFunctionsProvider,
        MaterialFactory materialFactory,
        LambdaInterpolateProvider lambdaInterpolateProvider,
        IFunctionalParameter functionalParameter, 
        DoubleIntegralCalculator doubleIntegralCalculator, 
        DerivativeCalculator derivativeCalculator
    )
    {
        _grid = grid;
        _localBasisFunctionsProvider = localBasisFunctionsProvider;
        _materialFactory = materialFactory;
        _lambdaInterpolateProvider = lambdaInterpolateProvider;
        _functionalParameter = functionalParameter;
        _doubleIntegralCalculator = doubleIntegralCalculator;
        _derivativeCalculator = derivativeCalculator;
        
    }

    public LocalMatrix AssembleStiffnessMatrix(Element element)
    {
        var matrix = GetStiffnessMatrix(element);

        return new LocalMatrix(element.NodesIndexes, matrix);
    }

    public LocalMatrix AssembleSigmaMassMatrix(Element element)
    {
        var matrix = GetMassMatrix(element);
        var material = _materialFactory.GetById(element.MaterialId);

        return new LocalMatrix(element.NodesIndexes, material.Sigma * matrix);
    }

    public LocalMatrix AssembleChiMassMatrix(Element element)
    {
        var matrix = GetMassMatrix(element);
        var material = _materialFactory.GetById(element.MaterialId);

        return new LocalMatrix(element.NodesIndexes, material.Chi * matrix);
    }

    public LocalVector AssembleRightSide(Element element)
    {
        var vector = GetRightPart(element);

        return new LocalVector(element.NodesIndexes, vector);
    }

    private BaseMatrix GetStiffnessMatrix(Element element)
    {
        var stiffness = new BaseMatrix(element.NodesIndexes.Length);

        var rInterval = new Interval(_grid.Nodes[element.NodesIndexes[0]].X, _grid.Nodes[element.NodesIndexes[1]].X);
        var zInterval = new Interval(_grid.Nodes[element.NodesIndexes[0]].Y, _grid.Nodes[element.NodesIndexes[2]].Y);

        var localBasisFunctions = _localBasisFunctionsProvider.GetBilinearFunctions(element);
        var lambdaInterpolate = _lambdaInterpolateProvider.GetLambdaInterpolate(element);

        for (var i = 0; i < element.NodesIndexes.Length; i++)
        {
            for (var j = 0; j <= i; j++)
            {
                stiffness[i, j] = _doubleIntegralCalculator.Calculate
                (
                    rInterval, 
                    zInterval,
                    (r, z) =>
                    {
                        var node = new Node2D(r, z);
                        return 
                            lambdaInterpolate(node) *
                            (_derivativeCalculator.Calculate(localBasisFunctions[i], node, 'r') *
                            _derivativeCalculator.Calculate(localBasisFunctions[j], node, 'r') +
                            _derivativeCalculator.Calculate(localBasisFunctions[i], node, 'z') *
                            _derivativeCalculator.Calculate(localBasisFunctions[j], node, 'z')) *
                            r;
                    }
                );

                stiffness[j, i] = stiffness[i, j];
            }
        }

        return stiffness;
    }

    private BaseMatrix GetMassMatrix(Element element)
    {
        var mass = new BaseMatrix(element.NodesIndexes.Length);

        var rInterval = new Interval(_grid.Nodes[element.NodesIndexes[0]].X, _grid.Nodes[element.NodesIndexes[1]].X);
        var zInterval = new Interval(_grid.Nodes[element.NodesIndexes[0]].Y, _grid.Nodes[element.NodesIndexes[2]].Y);

        var localBasisFunctions = _localBasisFunctionsProvider.GetBilinearFunctions(element);

        for (var i = 0; i < element.NodesIndexes.Length; i++)
        {
            for (var j = 0; j <= i; j++)
            {
                mass[i, j] = _doubleIntegralCalculator.Calculate
                (
                    rInterval,
                    zInterval,
                    (r, z) =>
                    {
                        var node = new Node2D(r, z);
                        return
                            localBasisFunctions[i].Calculate(node) * localBasisFunctions[j].Calculate(node) * r;
                    }
                );

                mass[j, i] = mass[i, j];
            }
        }

        return mass;
    }

    private BaseVector GetRightPart(Element element, double timeLayer)
    {
        var rightPart = new BaseVector(element.NodesIndexes.Length);

        var rInterval = new Interval(_grid.Nodes[element.NodesIndexes[0]].X, _grid.Nodes[element.NodesIndexes[1]].X);
        var zInterval = new Interval(_grid.Nodes[element.NodesIndexes[0]].Y, _grid.Nodes[element.NodesIndexes[2]].Y);

        var localBasisFunctions = _localBasisFunctionsProvider.GetBilinearFunctions(element);

        for (var i = 0; i < element.NodesIndexes.Length; i++)
        {
            rightPart[i] = _doubleIntegralCalculator.Calculate
            (
                rInterval,
                zInterval,
                (r, z) =>
                {
                    var node = new Node2D(r, z);
                    return
                        _functionalParameter.Calculate(i, timeLayer) * localBasisFunctions[i].Calculate(node) * r;
                }
            );
        }

        return rightPart;
    }
}