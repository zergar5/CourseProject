using CourseProject;
using CourseProject.Factories;
using CourseProject.IOs;
using CourseProject.Models.BoundaryConditions;
using CourseProject.Models.GlobalParts;
using CourseProject.Models.Grid;
using CourseProject.SLAESolution;
using CourseProject.Tools;
using CourseProject.Tools.Providers;

var boundaryConditionI = new BoundaryConditionIO("../CourseProject/Input/BoundaryConditions/");
var globalVectorI = new GlobalVectorIO("../CourseProject/Input/GlobalVectors/");
var nodeI = new NodeIO();
var materialI = new MaterialIO("../CourseProject/Input/Materials/");
var gridI = new GridIO("../CourseProject/Input/Grid/");
var parametersI = new ParametersIO("../CourseProject/Input/");

materialI.ReadMaterialsParametersFromFile("MaterialsParameters.txt", out var lambdasList, out var gammasList);

var materialFactory = new MaterialFactory(lambdasList, gammasList);
var linearFunctionsProvider = new LinearFunctionsProvider();
var gridComponentsProvider = new GridComponentsProvider(materialFactory, linearFunctionsProvider);
var gridFactory = new GridFactory(gridComponentsProvider);

gridI.ReadParametersFromFile("GridParameters.txt", out var cornerNodes, out var numberByWidth, out var numberByHeight);
var grid = gridFactory.CreateGrid(cornerNodes, numberByWidth, numberByHeight);

var adjacencyList = new AdjacencyList(grid);
adjacencyList.CreateAdjacencyList();

var globalMatrix = new GlobalMatrix(adjacencyList);
var globalVector = new GlobalVector(globalMatrix.N);

var nodeFinder = new NodeFinder(grid);
var pComponentsProvider = new PComponentsProvider((r, z) => r * z, nodeFinder);

foreach (var element in grid)
{
    element.CalcStiffnessMatrix(nodeFinder);
    element.CalcMassMatrix(nodeFinder);
    element.CalcRightPart(pComponentsProvider);
    element.CalcAMatrix();
    globalMatrix.PlaceLocalMatrix(element.LocalMatrixA, element.GlobalNodesNumbers);
    globalVector.PlaceLocalVector(element.RightPart, element.GlobalNodesNumbers);
}

boundaryConditionI.ReadFirstCondition("FirstBoundaryCondition.txt", out var globalNodesNumbersList1, out var usList1);
boundaryConditionI.ReadSecondCondition("SecondBoundaryCondition.txt", out var globalNodesNumbersList2, out var thetasList);
boundaryConditionI.ReadThirdCondition("ThirdBoundaryCondition.txt", out var globalNodesNumbersList3, out var betasList, out var usList2);

var boundaryConditionsFactory = new BoundaryConditionFactory(nodeFinder);

var firstBoundaryConditionArray = new FirstBoundaryCondition[globalNodesNumbersList1.Count];
var secondBoundaryConditionArray = new SecondBoundaryCondition[globalNodesNumbersList2.Count];
var thirdBoundaryConditionArray = new ThirdBoundaryCondition[globalNodesNumbersList3.Count];

for (var i = 0; i < firstBoundaryConditionArray.Length; i++)
{
    firstBoundaryConditionArray[i] =
        boundaryConditionsFactory.CreateFirstBoundaryCondition(globalNodesNumbersList1[i], usList1[i]);
}

for (var i = 0; i < secondBoundaryConditionArray.Length; i++)
{
    secondBoundaryConditionArray[i] =
        boundaryConditionsFactory.CreateSecondBoundaryCondition(globalNodesNumbersList1[i], thetasList[i]);
}

for (var i = 0; i < thirdBoundaryConditionArray.Length; i++)
{
    thirdBoundaryConditionArray[i] =
        boundaryConditionsFactory.CreateThirdBoundaryCondition(globalNodesNumbersList1[i], betasList[i], usList2[i]);
}

var boundaryConditionsApplicator = new BoundaryConditionsApplicator();

foreach (var secondBoundaryCondition in secondBoundaryConditionArray)
{
    boundaryConditionsApplicator.ApplySecondCondition(globalVector, secondBoundaryCondition);
}

foreach (var thirdBoundaryCondition in thirdBoundaryConditionArray)
{
    boundaryConditionsApplicator.ApplyThirdCondition(globalMatrix, globalVector, thirdBoundaryCondition);
}

foreach (var firstBoundaryCondition in firstBoundaryConditionArray)
{
    boundaryConditionsApplicator.ApplyFirstCondition(globalMatrix, globalVector, firstBoundaryCondition);
}

var startVector = globalVectorI.Read("StartVector.txt");
var (eps, maxIter) = parametersI.ReadMethodParameters("MCGParameters.txt");

var choleskyMCG = new CholeskyMCG();

var qVector = choleskyMCG.Solve(globalMatrix, startVector, globalVector, eps, maxIter);

var node = nodeI.ReadNodeFromConsole();

var solutionFinder = new SolutionFinder(grid, qVector, nodeFinder);
var result = solutionFinder.FindSolution(node);

CourseHolder.WriteSolution(node, result);