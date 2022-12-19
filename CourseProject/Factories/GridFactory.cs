using CourseProject.Models.Grid;
using CourseProject.Tools.Providers;

namespace CourseProject.Factories;

public class GridFactory
{
    private readonly GridComponentsProvider _gridComponentsProvider;

    public GridFactory(GridComponentsProvider gridComponentsProvider)
    {
        _gridComponentsProvider=gridComponentsProvider;
    }

    public Grid CreateGrid(Node[] cornerNodes, int numberByWidth, int numberByHeight)
    {
        var grid = new Grid(
            _gridComponentsProvider.CreateNodes(cornerNodes, numberByWidth, numberByHeight),
            _gridComponentsProvider.CreateElements(cornerNodes, numberByWidth, numberByHeight),
            cornerNodes,
            numberByWidth,
            numberByHeight);

        return grid;
    }

    public Grid CreateGrid(Node[] cornerNodes, int numberByWidth, int numberByHeight, Material[] materials)
    {
        var grid = new Grid(
            _gridComponentsProvider.CreateNodes(cornerNodes, numberByWidth, numberByHeight),
            _gridComponentsProvider.CreateElements(cornerNodes, numberByWidth, numberByHeight, materials),
            cornerNodes,
            numberByWidth,
            numberByHeight);

        return grid;
    }
}