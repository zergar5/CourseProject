using CourseProject.Calculus;
using CourseProject.Core.GridComponents;
using CourseProject.FEM;
using CourseProject.GridGenerator;
using CourseProject.GridGenerator.Area.Core;
using CourseProject.GridGenerator.Area.Splitting;
using CourseProject.SLAE.Preconditions.LLT;
using CourseProject.SLAE.Solvers;
using CourseProject.Time;
using CourseProject.TwoDimensional;
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
            new[] { 1d, 3d },
            new UniformSplitter(2)
        )
    )
    .SetZAxis(new AxisSplitParameter(
            new[] { 1d, 3d },
            new UniformSplitter(2)
        )
    )
    .Build();

var materialFactory = new MaterialFactory
(
    new List<double[]> { new[] { 1d, 1d, 1d, 1d, 1d, 1d, 1d, 1d, 1d } },
    new List<double> { 0d },
    new List<double> { 1d }
);

var localBasisFunctionsProvider = new LocalBasisFunctionsProvider(grid, new LinearFunctionsProvider());
var lambdaInterpolateProvider = new LambdaInterpolateProvider(localBasisFunctionsProvider, materialFactory);

var f = new RightPartParameter((p, t) => -1 / p.R - Math.Exp(t), grid);

var derivativeCalculator = new DerivativeCalculator();

var localAssembler = new LocalAssembler(grid, localBasisFunctionsProvider, materialFactory, lambdaInterpolateProvider, f,
    new DoubleIntegralCalculator(), derivativeCalculator);

var inserter = new Inserter();
var globalAssembler = new GlobalAssembler<Node2D>(new MatrixPortraitBuilder(), localAssembler, inserter);

var templateRProvider = new MatrixRTemplateProvider();
var templateZProvider = new MatrixZTemplateProvider();

var timeLayers = new UniformSplitter(120)
    .EnumerateValues(new Interval(10, 10 + 3e-9))
    .ToArray();

Func<Node2D, double, double> u = (p, t) => p.R + p.Z - Math.Exp(t);

var firstBoundaryProvider = new FirstBoundaryProvider(grid, u);
var secondBoundaryProvider = new SecondBoundaryProvider(grid, materialFactory, u, derivativeCalculator, templateRProvider, templateZProvider);
var thirdBoundaryProvider = new ThirdBoundaryProvider(grid, materialFactory, u, derivativeCalculator, templateRProvider, templateZProvider);

var lltPreconditioner = new LLTPreconditioner();
var solver = new MCG(lltPreconditioner, new LLTSparse(lltPreconditioner));

var timeDiscreditor = new TimeDisсreditor(globalAssembler, timeLayers, grid, firstBoundaryProvider,
    new GaussExcluder(), secondBoundaryProvider, thirdBoundaryProvider, inserter);

var solutions =
    timeDiscreditor
        .SetFirstInitialSolution(u)
        .SetSecondInitialSolution((p, t) => -Math.Exp(t))
        .SetThirdInitialSolution(u)
        //.SetSecondConditions
        //(
        //    new[] { 0, 0, 0 },
        //    new[] { Bound.Left, Bound.Right, Bound.Upper }
        //)
        //.SetThirdConditions
        //(
        //    new[] { 0, 0, 0 },
        //    new[] { Bound.Left, Bound.Right, Bound.Upper },
        //    new[] { 1d, 1d, 1d }
        //)
        .SetFirstConditions
        (
            new[] { 0, 0, 1, 1, 2, 2, 3, 3 },
            new[] { Bound.Lower, Bound.Left, Bound.Lower, Bound.Right, Bound.Left, Bound.Upper, Bound.Upper, Bound.Right }
        )
        //.SetFirstConditions
        //(
        //    new[] { 0 },
        //    new[] { Bound.Lower }
        //)
        .SetSolver(solver)
        .GetSolutions();

var femSolution = new FEMSolution(grid, solutions, timeLayers, localBasisFunctionsProvider);
var result = femSolution.Calculate(new Node2D(1.5d, 1.5d), 10 + 3e-9);

Console.WriteLine(Math.Abs(u(new Node2D(1.5d, 1.5d), 10 + 3e-9) - result));