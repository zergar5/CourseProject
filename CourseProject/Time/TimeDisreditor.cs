using CourseProject.Core;
using CourseProject.Core.Boundary;
using CourseProject.Core.Global;
using CourseProject.Core.GridComponents;
using CourseProject.SLAE.Solvers;
using CourseProject.Time.Schemes.Explicit;
using CourseProject.TwoDimensional.Assembling;
using CourseProject.TwoDimensional.Assembling.Boundary;
using CourseProject.TwoDimensional.Assembling.Global;

namespace CourseProject.Time;

public class TimeDisreditor
{
    public GlobalVector PreviousSolution => TimeSolutions[_currentTimeLayer - 1];
    public GlobalVector TwoLayersBackSolution => TimeSolutions[_currentTimeLayer - 2];
    public GlobalVector ThreeLayersBackSolution => TimeSolutions[_currentTimeLayer - 3];
    public GlobalVector[] TimeSolutions { get; private set; }

    public double CurrentTime => _timeLayers[_currentTimeLayer];
    public double PreviousTime => _timeLayers[_currentTimeLayer - 1];
    public double TwoLayersBackTime => _timeLayers[_currentTimeLayer - 2];
    public double ThreeLayersBackTime => _timeLayers[_currentTimeLayer - 3];

    private int _currentTimeLayer = 0;

    private readonly GlobalAssembler<Node2D> _globalAssembler;
    private readonly double[] _timeLayers;
    private readonly Grid<Node2D> _grid;
    private readonly ThreeLayer _threeLayer;
    private readonly FourLayer _fourLayer;
    private readonly FirstBoundaryProvider _firstBoundaryProvider;
    private readonly GaussExcluder _gaussExcluder;
    private readonly SecondBoundaryProvider _secondBoundaryProvider;
    private readonly ThirdBoundaryProvider _thirdBoundaryProvider;
    private readonly Inserter _inserter;
    private int[] _firstConditionIndexes;
    private Bound[] _firstConditionBounds;
    private int[] _secondConditionIndexes;
    private Bound[] _secondConditionBounds;
    private Func<Node2D, double, double> _secondF;
    private int[] _thirdConditionIndexes;
    private Bound[] _thirdConditionBounds;
    private Func<Node2D, double, double> _thirdF;
    private double[] _betas;
    private ISolver<SymmetricSparseMatrix> _solver;


    public TimeDisreditor
    (
        GlobalAssembler<Node2D> globalAssembler,
        double[] timeLayers,
        Grid<Node2D> grid,
        ThreeLayer threeLayer,
        FourLayer fourLayer,
        FirstBoundaryProvider firstBoundaryProvider,
        GaussExcluder gaussExcluder,
        SecondBoundaryProvider secondBoundaryProvider,
        ThirdBoundaryProvider thirdBoundaryProvider,
        Inserter inserter
    )
    {
        _globalAssembler = globalAssembler;
        _timeLayers = timeLayers;
        TimeSolutions = new GlobalVector[_timeLayers.Length];
        _grid = grid;
        _threeLayer = threeLayer;
        _fourLayer = fourLayer;
        _firstBoundaryProvider = firstBoundaryProvider;
        _gaussExcluder = gaussExcluder;
        _secondBoundaryProvider = secondBoundaryProvider;
        _thirdBoundaryProvider = thirdBoundaryProvider;
        _inserter = inserter;
        
    }

    public TimeDisreditor SetFirstInitialSolution(Func<Node2D, double, double> u)
    {
        var initialSolution = new GlobalVector(_grid.Nodes.Length);
        var currentTime = CurrentTime;

        for (var i = 0; i < _grid.Nodes.Length; i++)
        {
            initialSolution[i] = u(_grid.Nodes[i], currentTime);
        }

        TimeSolutions[_currentTimeLayer] = initialSolution;
        _currentTimeLayer++;

        return this;
    }

    public TimeDisreditor SetSecondInitialSolution(Func<Node2D, double, double> u)
    {
        var initialSolution = new GlobalVector(_grid.Nodes.Length);
        var prevTime = PreviousTime;
        var currentTime = CurrentTime;

        for (var i = 0; i < _grid.Nodes.Length; i++)
        {
            initialSolution[i] = PreviousSolution[i] + u(_grid.Nodes[i], currentTime - prevTime);
        }

        TimeSolutions[_currentTimeLayer] = initialSolution;
        _currentTimeLayer++;

        return this;
    }

    public TimeDisreditor SetFirstConditions(int[] elementsIndexes, Bound[] bounds)
    {
        _firstConditionIndexes = elementsIndexes;
        _firstConditionBounds = bounds;

        return this;
    }

    public TimeDisreditor SetSecondConditions(int[] elementsIndexes, Bound[] bounds, Func<Node2D, double, double> f)
    {
        return this;
    }

    public TimeDisreditor SetThirdConditions(int[] elementsIndexes, Bound[] bounds, Func<Node2D, double, double> f,
        double[] betas)
    {
        return this;
    }

    public TimeDisreditor SetSolver(ISolver<SymmetricSparseMatrix> solver)
    {
        _solver = solver;

        return this;
    }

    public GlobalVector[] GetSolutions()
    {
        var equation = UseThreeLayerScheme();

        if (_firstConditionIndexes.Length > 0 && _firstConditionBounds.Length > 0)
        {
            var firstConditions = _firstBoundaryProvider.GetConditions(_firstConditionIndexes,
                _firstConditionBounds, CurrentTime);
            ApplyFirstConditions(equation, firstConditions);
        }
        //if (_secondConditionIndexes.Length > 0 && _secondConditionBounds.Length > 0 && _secondF != null)
        //{
        //    var secondConditions = _secondBoundaryProvider.GetConditions(_firstConditionIndexes,
        //        _firstConditionBounds, CurrentTime);
        //    ApplyFirstConditions(equation, firstConditions);
        //}
        //if (_thirdConditionIndexes.Length > 0 && _thirdConditionBounds.Length > 0 && _thirdF != null && _betas.Length > 0)
        //{
        //    var firstConditions = _firstBoundaryProvider.GetConditions(_firstConditionIndexes,
        //        _firstConditionBounds, CurrentTime);
        //    ApplyFirstConditions(equation, firstConditions);
        //}

        TimeSolutions[_currentTimeLayer] = _solver.Solve(equation);
        _currentTimeLayer++;

        do
        {
            equation = UseFourLayerScheme();

            if (_firstConditionIndexes.Length > 0 && _firstConditionBounds.Length > 0)
            {
                var firstConditions = _firstBoundaryProvider.GetConditions(_firstConditionIndexes,
                    _firstConditionBounds, CurrentTime);
                ApplyFirstConditions(equation, firstConditions);
            }
            //if (_secondConditionIndexes.Length > 0 && _secondConditionBounds.Length > 0 && _secondF != null)
            //{
            //    var secondConditions = _secondBoundaryProvider.GetConditions(_firstConditionIndexes,
            //        _firstConditionBounds, CurrentTime);
            //    ApplyFirstConditions(equation, firstConditions);
            //}
            //if (_thirdConditionIndexes.Length > 0 && _thirdConditionBounds.Length > 0 && _thirdF != null && _betas.Length > 0)
            //{
            //    var firstConditions = _firstBoundaryProvider.GetConditions(_firstConditionIndexes,
            //        _firstConditionBounds, CurrentTime);
            //    ApplyFirstConditions(equation, firstConditions);
            //}

            TimeSolutions[_currentTimeLayer] = _solver.Solve(equation);
            _currentTimeLayer++;
        } while (_currentTimeLayer < _timeLayers.Length);

        return TimeSolutions;
    }

    private Equation<SymmetricSparseMatrix> UseThreeLayerScheme()
    {
        var equation = _threeLayer
            .BuildEquation
            (
                _globalAssembler.AssembleRightPart(_grid, PreviousTime),
                PreviousSolution,
                TwoLayersBackSolution,
                CurrentTime,
                PreviousTime,
                TwoLayersBackTime
            );

        return equation;
    }

    private Equation<SymmetricSparseMatrix> UseFourLayerScheme()
    {
        var equation = _fourLayer
            .BuildEquation
            (
                _globalAssembler.AssembleRightPart(_grid, PreviousTime),
                PreviousSolution,
                TwoLayersBackSolution,
                ThreeLayersBackSolution,
                CurrentTime,
                PreviousTime,
                TwoLayersBackTime,
                ThreeLayersBackTime
            );

        return equation;
    }

    private void ApplyFirstConditions(Equation<SymmetricSparseMatrix> equation, FirstCondition[] firstConditions)
    {
        foreach (var firstCondition in firstConditions)
        {
            _gaussExcluder.Exclude(equation, firstCondition);
        }
    }

    private void ApplySecondConditions(Equation<SymmetricSparseMatrix> equation, SecondCondition[] secondConditions)
    {
        foreach (var secondCondition in secondConditions)
        {
            _inserter.InsertVector(equation.RightSide, secondCondition.Vector);
        }
    }

    private void ApplyThirdConditions(Equation<SymmetricSparseMatrix> equation, ThirdCondition[] thirdConditions)
    {
        foreach (var thirdCondition in thirdConditions)
        {
            _inserter.InsertMatrix(equation.Matrix, thirdCondition.Matrix);
            _inserter.InsertVector(equation.RightSide, thirdCondition.Vector);
        }
    }
}