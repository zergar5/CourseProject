using CourseProject.Models.Grid;

namespace CourseProject.Models.GlobalParts;

public class GlobalMatrix
{
    public double[] DI { get; set; }
    public double[] GG { get; set; }
    public int[] JG { get; set; }
    public int[] IG { get; set; }

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

    public void PlaceLocalMatrix(LocalMatrix localMatrixA, int[] globalNodesNumbers)
    {
        for (var i = 0; i < globalNodesNumbers.Length; i++)
        {
            for (var j = 0; j < i; j++)
            {
                var elementIndex = Array.IndexOf(JG, globalNodesNumbers[j], IG[globalNodesNumbers[i]],
                    IG[globalNodesNumbers[i] + 1] - IG[globalNodesNumbers[i]]);
                GG[elementIndex] += localMatrixA[i, j];
            }

            DI[globalNodesNumbers[i]] += localMatrixA[i, i];
        }
    }
}