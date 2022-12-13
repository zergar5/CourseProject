using CourseProject.Models;
using CourseProject.Models.Grid;
using CourseProject.Tools;
using CourseProject.Tools.Providers;

namespace CourseProject.Factories;

public class GridFactory
{
    private readonly GridComponentsProvider _gridComponentsProvider;

    public GridFactory(GridComponentsProvider gridComponentsProvider)
    {
        _gridComponentsProvider=gridComponentsProvider;
    }

    public Grid CreateGrid(Node[] cornerNodes, double width, double height, int numberByWidth, int numberByHeight)
    {
        var grid = new Grid(
            _gridComponentsProvider.CreateNodes(cornerNodes, width, height, numberByWidth, numberByHeight),
            _gridComponentsProvider.CreateElements(cornerNodes, width, height, numberByWidth, numberByHeight),
            cornerNodes,
            width,
            height,
            numberByWidth,
            numberByHeight);

        return grid;
    }
}