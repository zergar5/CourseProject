namespace CourseProject.Models;

public class GlobalMatrix
{
    public double[] DI;
    public double[] GG;
    public int[] JG;
    public int[] IG;

    public GlobalMatrix(AdjacencyList adjacencyList)
    {
        var list = adjacencyList.List;
        DI = new double[list.Count];
        JG = list.SelectMany(nodeList => nodeList).ToArray();
        GG = new double[JG.Length];
        var amount = 0;
        var buf = list.Select(nodeList => amount += nodeList.Count).ToList();
        buf.Insert(0, 0);
        IG = buf.ToArray();
    }

    public void PlaceElement(Element element)
    {
        for (var i = element.Nodes.Length - 1; i >= 0; i++)
        {
            for (var j = i; j < element.Nodes.Length; j++)
            {
                if (j == i)
                {

                }
            }
        }
    }
}