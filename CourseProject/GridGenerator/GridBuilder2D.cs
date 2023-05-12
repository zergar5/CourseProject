using CourseProject.Core;
using CourseProject.Core.GridComponents;
using CourseProject.GridGenerator.Area.Splitting;

namespace CourseProject.GridGenerator;

public class GridBuilder2D : IGridBuilder<Node2D>
{
    private AxisSplitParameter? _xAxisSplitParameter;
    private AxisSplitParameter? _yAxisSplitParameter;
    private int[]? _materialsId;

    private int GetTotalXElements => _xAxisSplitParameter.Splitters.Sum(x => x.Steps);
    private int GetTotalYElements => _yAxisSplitParameter.Splitters.Sum(y => y.Steps);

    public GridBuilder2D SetXAxis(AxisSplitParameter splitParameter)
    {
        _xAxisSplitParameter = splitParameter;
        return this;
    }

    public GridBuilder2D SetYAxis(AxisSplitParameter splitParameter)
    {
        _yAxisSplitParameter = splitParameter;
        return this;
    }

    public GridBuilder2D SetMaterials(int[] materialsId)
    {
        _materialsId = materialsId;
        return this;
    }

    public Grid<Node2D> Build()
    {
        if (_xAxisSplitParameter == null || _yAxisSplitParameter == null)
            throw new ArgumentNullException();

        var totalXElements = GetTotalXElements;

        var totalNodes = GetTotalNodes();
        var totalElements = GetTotalElements();

        var nodes = new Node2D[totalNodes];
        var elements = new Element[totalElements];

        _materialsId ??= new int[totalElements];

        var i = 0;

        foreach (var (ySection, ySplitter) in _yAxisSplitParameter.SectionWithParameter)
        {
            var yValues = ySplitter.EnumerateValues(ySection);
            if (i > 0) yValues = yValues.Skip(1);

            foreach (var y in yValues)
            {
                var j = 0;

                foreach (var (xSection, xSplitter) in _xAxisSplitParameter.SectionWithParameter)
                {
                    var xValues = xSplitter.EnumerateValues(xSection);
                    if (j > 0) xValues = xValues.Skip(1);

                    foreach (var x in xValues)
                    {
                        var nodeIndex = j + i * (totalXElements + 1);

                        nodes[nodeIndex] = new Node2D(x, y);

                        if (i > 0 && j > 0)
                        {
                            var elementIndex = j - 1 + (i - 1) * totalXElements;
                            var nodesIndexes = GetCurrentElementIndexes(i - 1, j - 1);

                            elements[elementIndex] = new Element(
                                nodesIndexes,
                                nodes[nodesIndexes[1]].X - nodes[nodesIndexes[0]].X,
                                nodes[nodesIndexes[2]].Y - nodes[nodesIndexes[0]].Y,
                                _materialsId[elementIndex]
                                );
                        }

                        j++;
                    }
                }

                i++;
            }
        }

        return new Grid<Node2D>(nodes, elements);
    }

    private int GetTotalNodes()
    {
        return (GetTotalXElements + 1) * (GetTotalYElements + 1);
    }

    private int GetTotalElements()
    {
        return GetTotalXElements * GetTotalYElements;
    }

    private int[] GetCurrentElementIndexes(int j, int k)
    {
        var totalXElements = GetTotalXElements;

        var indexes = new[]
        {
            k + j * (totalXElements + 1),
            k + 1 + j * (totalXElements + 1),
            k + (j + 1) * (totalXElements + 1),
            k + 1 + (j + 1) * (totalXElements + 1)
        };

        return indexes;
    }
}