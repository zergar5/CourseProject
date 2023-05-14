using CourseProject.Core;
using CourseProject.Core.Global;
using CourseProject.FEM.Assembling;
using CourseProject.FEM.Assembling.Local;

namespace CourseProject.TwoDimensional.Assembling.Global;

public class GlobalAssembler<TNode>
{
    private readonly IMatrixPortraitBuilder<TNode, SymmetricSparseMatrix> _matrixPortraitBuilder;
    private readonly ILocalAssembler _localAssembler;
    private readonly IInserter<SymmetricSparseMatrix> _inserter;
    private Equation<SymmetricSparseMatrix> _equation;

    public GlobalAssembler
    (
        IMatrixPortraitBuilder<TNode, SymmetricSparseMatrix> matrixPortraitBuilder,
        ILocalAssembler localAssembler,
        IInserter<SymmetricSparseMatrix> inserter
    )
    {
        _matrixPortraitBuilder = matrixPortraitBuilder;
        _localAssembler = localAssembler;
        _inserter = inserter;
    }

    public SymmetricSparseMatrix AssembleStiffnessMatrix(Grid<TNode> grid)
    {
        var globalMatrix = _matrixPortraitBuilder.Build(grid);

        foreach (var element in grid)
        {
            var localMatrix = _localAssembler.AssembleStiffnessMatrix(element);

            _inserter.InsertMatrix(globalMatrix, localMatrix);
        }

        return globalMatrix;
    }

    public SymmetricSparseMatrix AssembleSigmaMassMatrix(Grid<TNode> grid)
    {
        var globalMatrix = _matrixPortraitBuilder.Build(grid);

        foreach (var element in grid)
        {
            var localMatrix = _localAssembler.AssembleSigmaMassMatrix(element);

            _inserter.InsertMatrix(globalMatrix, localMatrix);
        }

        return globalMatrix;
    }

    public SymmetricSparseMatrix AssembleChiMassMatrix(Grid<TNode> grid)
    {
        var globalMatrix = _matrixPortraitBuilder.Build(grid);

        foreach (var element in grid)
        {
            var localMatrix = _localAssembler.AssembleChiMassMatrix(element);

            _inserter.InsertMatrix(globalMatrix, localMatrix);
        }

        return globalMatrix;
    }

    public GlobalVector AssembleRightPart(Grid<TNode> grid, double timeLayer)
    {
        var rightPart = new GlobalVector(grid.Nodes.Length);

        foreach (var element in grid)
        {
            var localRightPart = _localAssembler.AssembleRightSide(element, timeLayer);

            _inserter.InsertVector(rightPart, localRightPart);
        }

        return rightPart;
    }
}