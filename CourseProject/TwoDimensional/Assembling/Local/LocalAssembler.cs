using CourseProject.Core;
using CourseProject.Core.Base;
using CourseProject.Core.GridComponents;
using CourseProject.Core.Local;
using CourseProject.FEM;
using CourseProject.FEM.Assembling.Local;
using CourseProject.FEM.Parameters;
using CourseProject.TwoDimensional.Parameters;

namespace CourseProject.TwoDimensional.Assembling.Local;

public class LocalAssembler : ILocalAssembler
{
    private readonly Grid<Node2D> _grid;
    private readonly LinearFunctionsProvider _linearFunctionsProvider;
    private readonly MaterialFactory _materialFactory;
    private readonly LambdaInterpolateProvider _lambdaInterpolateProvider;
    private readonly IFunctionalParameter _functionalParameter;

    public LocalAssembler
    (
        Grid<Node2D> grid,
        LinearFunctionsProvider linearFunctionsProvider,
        MaterialFactory materialFactory,
        LambdaInterpolateProvider lambdaInterpolateProvider,
        IFunctionalParameter functionalParameter
    )
    {
        _grid = grid;
        _linearFunctionsProvider = linearFunctionsProvider;
        _materialFactory = materialFactory;
        _lambdaInterpolateProvider = lambdaInterpolateProvider;
        _functionalParameter = functionalParameter;
    }

    public LocalMatrix AssembleStiffnessMatrix(Element element)
    {
        var matrix = GetComplexMatrix(element);

        return new LocalMatrix(element.NodesIndexes, matrix);
    }

    public LocalMatrix AssembleMassMatrix(Element element)
    {
        var matrix = GetComplexMatrix(element);

        return new LocalMatrix(element.NodesIndexes, matrix);
    }

    public LocalVector AssembleRightSide(Element element)
    {
        var vector = GetComplexVector(element);

        return new LocalVector(element.NodesIndexes, vector);
    }

    private BaseMatrix GetStiffnessMatrix(Element element)
    {
        var stiffness = new BaseMatrix(element.NodesIndexes.Length);

        var rUpperLimit =

        return stiffness;
    }

    private BaseMatrix GetMassMatrix(Element element)
    {
        var mass = _massTemplateProvider.GetMatrix();

        return element.Length * element.Width * element.Height / 216d * mass;
    }
}