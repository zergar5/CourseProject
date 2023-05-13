using CourseProject.Core;
using CourseProject.Core.Global;
using CourseProject.Core.GridComponents;
using CourseProject.SLAE.Solvers;
using CourseProject.TwoDimensional.Assembling.Global;

namespace CourseProject.Time;

public class TimeAssembler
{
    public GlobalVector CurrentSolution { get; private set; }
    public GlobalVector PreviousSolution { get; private set; }
    public GlobalVector TwoLayersBackSolution { get; private set; }
    public GlobalVector ThreeLayersBackSolution { get; private set; }

    public double CurrentTime => _timeLayers[_currentTimeLayer];
    public double PreviousTime => _timeLayers[_currentTimeLayer - 1];
    public double TwoLayersBackTime => _timeLayers[_currentTimeLayer - 2];
    public double ThreeLayersBackTime => _timeLayers[_currentTimeLayer - 3];

    private int _currentTimeLayer = 0;

    private readonly GlobalAssembler<Node2D> _globalAssembler;
    private readonly double[] _timeLayers;
    private readonly Grid<Node2D> _grid;
    private readonly SymmetricSparseMatrix _stiffnessMatrix;
    private readonly SymmetricSparseMatrix _sigmaMassMatrix;
    private readonly SymmetricSparseMatrix _chiMassMatrix;
    private ISolver<SymmetricSparseMatrix> _solver;

    public TimeAssembler
    (
        GlobalAssembler<Node2D> globalAssembler, 
        double[] timeLayers, 
        ISolver<SymmetricSparseMatrix> solver,
        Grid<Node2D> grid
    )
    {
        _globalAssembler = globalAssembler;
        _timeLayers = timeLayers;
        _solver = solver;
        _grid = grid;
        _stiffnessMatrix = globalAssembler.AssembleStiffnessMatrix(grid);
        _sigmaMassMatrix = globalAssembler.AssembleSigmaMassMatrix(grid);
        _chiMassMatrix = globalAssembler.AssembleSigmaMassMatrix(grid);
    }

    public TimeAssembler SetFirstInitialSolution(Func<Node2D, double, double> u)
    {
        var initialSolution = new GlobalVector(_grid.Nodes.Length);
        var currentTime = CurrentTime;

        for (var i = 0; i < _grid.Nodes.Length; i++)
        {
            initialSolution[i] = u(_grid.Nodes[i], currentTime);
        }

        _currentTimeLayer++;
        CurrentSolution = initialSolution;

        return this;
    }

    public TimeAssembler SetSecondInitialSolution(Func<Node2D, double, double> u)
    {
        var initialSolution = new GlobalVector(_grid.Nodes.Length);
        var prevTime = PreviousTime;
        var currentTime = CurrentTime;

        for (var i = 0; i < _grid.Nodes.Length; i++)
        {
            initialSolution[i] = CurrentSolution[i] + u(_grid.Nodes[i], currentTime - prevTime);
        }

        _currentTimeLayer++;
        PreviousSolution = CurrentSolution;
        CurrentSolution = initialSolution;

        return this;
    }

    public TimeAssembler CalculateNextTimeSolution()
    {

    }

    public Equation<SymmetricSparseMatrix> BuildEquation()
    {
        
    }
}