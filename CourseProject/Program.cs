using CourseProject.Factories;
using CourseProject.IO;
using CourseProject.Models;
using CourseProject.Tools;

var gridI = new GridIO();
var cornerNodes = gridI.ReadCoordinateFromConsole();
gridI.ReadSizesFromConsole(out var numberByWidth, out var numberByHeight);

var gridComponentsProvider = new GridComponentsProvider();

var gridFactory = new GridFactory(gridComponentsProvider);

var grid = gridFactory.CreateGrid(cornerNodes, numberByWidth, numberByHeight);

var adjacencyList = new AdjacencyList(grid);
adjacencyList.CreateAdjacencyList();

var globalMatrix = new GlobalMatrix(adjacencyList);

Console.WriteLine();