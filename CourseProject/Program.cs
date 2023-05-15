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
using CourseProject.TwoDimensional.Assembling.MatrixTemplates;
using CourseProject.TwoDimensional.Parameters;
using System.Globalization;

Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

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

var localBasisFunctionsProvider = new LocalBasisFunctionsProvider(grid, new LinearFunctionsProvider(), materialFactory);
var lambdaInterpolateProvider = new LambdaInterpolateProvider(localBasisFunctionsProvider, materialFactory);

var f = new RightPartParameter((p, t) => p.R, grid);

var derivativeCalculator = new DerivativeCalculator();

var localAssembler = new LocalAssembler(grid, localBasisFunctionsProvider, materialFactory, lambdaInterpolateProvider, f,
    new DoubleIntegralCalculator(), derivativeCalculator);

var inserter = new Inserter();
var globalAssembler = new GlobalAssembler<Node2D>(new MatrixPortraitBuilder(), localAssembler, inserter);

var templateRProvider = new MatrixRTemplateProvider();
var templateZProvider = new MatrixZTemplateProvider(); 

var timeLayers = new UniformSplitter(3)
    .EnumerateValues(new Interval(0, 3))
    .ToArray();

var timeDeltasCalculator = new TimeDeltasCalculator();
var threeLayer = new ThreeLayer(globalAssembler, grid, timeDeltasCalculator);
var fourLayer = new FourLayer(globalAssembler, grid, timeDeltasCalculator);
var firstBoundaryProvider = new FirstBoundaryProvider(grid, (p, t) => p.R * t);
var secondBoundaryProvider = new SecondBoundaryProvider(grid, materialFactory, (p, t) => p.R * t, derivativeCalculator, templateRProvider, templateZProvider);
var thirdBoundaryProvider = new ThirdBoundaryProvider(grid, materialFactory, (p, t) => p.R * t, derivativeCalculator, templateRProvider, templateZProvider);

var lltPreconditioner = new LLTPreconditioner();
var solver = new MCG(lltPreconditioner, new LLTSparse(lltPreconditioner));

var timeDiscreditor = new TimeDisсreditor(globalAssembler, timeLayers, grid, threeLayer, fourLayer, firstBoundaryProvider,
    new GaussExcluder(), secondBoundaryProvider, thirdBoundaryProvider, inserter);

var solutions =
    timeDiscreditor
        .SetFirstInitialSolution((p, t) => p.R * t)
        .SetSecondInitialSolution((p, t) => p.R)
        .SetSecondConditions
        (
            new[] { 0, 0, 0 },
            new[] { Bound.Left, Bound.Right, Bound.Upper }
        )
        //.SetThirdConditions
        //(
        //    new[] { 0, 0, 0 },
        //    new[] { Bound.Left, Bound.Right, Bound.Upper },
        //    new[] { 1d, 1d, 1d }
        //)
        .SetFirstConditions
        (
            new[] { 0 },
            new[] { Bound.Lower }
        )
        .SetSolver(solver)
        .GetSolutions();

Console.WriteLine();