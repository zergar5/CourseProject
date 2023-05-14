using CourseProject.Calculus;
using CourseProject.Core.GridComponents;
using CourseProject.FEM;
using CourseProject.GridGenerator.Area.Splitting;
using CourseProject.GridGenerator;
using CourseProject.GridGenerator.Area.Core;
using CourseProject.SLAE.Preconditions.LLT;
using CourseProject.SLAE.Solvers;
using CourseProject.Time;
using CourseProject.Time.Schemes.Explicit;
using CourseProject.TwoDimensional.Assembling;
using CourseProject.TwoDimensional.Assembling.Boundary;
using CourseProject.TwoDimensional.Assembling.Global;
using CourseProject.TwoDimensional.Assembling.Local;
using CourseProject.TwoDimensional.Parameters;

var gridBuilder3D = new GridBuilder2D();
var grid = gridBuilder3D
    .SetRAxis(new AxisSplitParameter(
            new[] { 0d, 1d },
            new UniformSplitter(1)
        )
    )
    .SetZAxis(new AxisSplitParameter(
            new[] { 0d, 1d },
            new UniformSplitter(1)
        )
    )
    .Build();

var materialFactory = new MaterialFactory
(
    new List<double[]> { new [] {1d, 1d, 1d, 1d, 1d, 1d, 1d, 1d, 1d} },
    new List<double> { 1d },
    new List<double> { 1d }
);

var linearFunctionsProvider = new LinearFunctionsProvider();
var localBasisFunctionsProvider = new LocalBasisFunctionsProvider(grid, linearFunctionsProvider, materialFactory);

var lambdaInterpolateProvider = new LambdaInterpolateProvider(localBasisFunctionsProvider, materialFactory);

var f = new RightPartParameter((p, t) => p.R, grid);

var doubleIntegralCalculator = new DoubleIntegralCalculator();
var derivativeCalculator = new DerivativeCalculator();

var localAssembler = new LocalAssembler(grid, localBasisFunctionsProvider, materialFactory, lambdaInterpolateProvider, f,
    doubleIntegralCalculator, derivativeCalculator);

var matrixPortraitBuilder = new MatrixPortraitBuilder();
var inserter = new Inserter();
var timeDeltasCalculator = new TimeDeltasCalculator();

var globalAssembler = new GlobalAssembler<Node2D>(matrixPortraitBuilder, localAssembler, inserter);
var timeLayers = new UniformSplitter(2)
    .EnumerateValues(new Interval(0, 2))
    .ToArray();
var threeLayer = new ThreeLayer(globalAssembler, grid, timeDeltasCalculator);
var fourLayer = new FourLayer(globalAssembler, grid, timeDeltasCalculator);
var firstBoundaryProvider = new FirstBoundaryProvider(grid, (p, t) => p.R * t);
var gaussExcluder = new GaussExcluder();
var secondBoundaryProvider = new SecondBoundaryProvider(grid, materialFactory);
var thirdBoundaryProvider = new ThirdBoundaryProvider(firstBoundaryProvider, secondBoundaryProvider);
var lltPreconditioner = new LLTPreconditioner();
var lltSparse = new LLTSparse(lltPreconditioner);
var solver = new MCG(lltPreconditioner, lltSparse);

var timeDisreditor = new TimeDisreditor(globalAssembler, timeLayers, grid, threeLayer, fourLayer, firstBoundaryProvider,
    gaussExcluder, secondBoundaryProvider, thirdBoundaryProvider, inserter);

var solutions = 
    timeDisreditor
    .SetFirstInitialSolution((p, t) => p.R * t)
    .SetSecondInitialSolution((p, t) => p.R)
    .SetFirstConditions(new[] { 0, 0 }, new[] { Bound.Lower, Bound.Left })
    .SetSolver(solver)
    .GetSolutions();

Console.WriteLine();