using CourseProject.Models.LocalParts;
using CourseProject.Tools.Calculators;
using CourseProject.Tools.Providers;

namespace CourseProject.Models.Grid;

public class Element
{
    public Node[] Nodes { get; set; }
    public int[] GlobalNodesNumbers { get; set; }
    public Material Material { get; set; }
    public LocalBasisFunction[] LocalBasisFunctions { get; set; }
    public LocalMatrix StiffnessMatrix { get; set; }
    public LocalMatrix MassMatrix { get; set; }
    public LocalMatrix LocalMatrixA { get; set; }
    public LocalVector RightPart { get; set; }

    public Element(Node[] nodes, int[] globalNodesNumbers, Material material, LocalBasisFunction[] localBasisFunctions)
    {
        Nodes = nodes;
        GlobalNodesNumbers = globalNodesNumbers;
        Material = material;
        LocalBasisFunctions = localBasisFunctions;
    }

    public void CalcStiffnessMatrix()
    {
        StiffnessMatrix = new LocalMatrix(Nodes.Length, Nodes.Length);

        var rUpperLimit = Nodes[1].R;
        var rDownLimit = Nodes[0].R;
        var zUpperLimit = Nodes[2].Z;
        var zDownLimit = Nodes[0].Z;

        for (var i = 0; i < Nodes.Length; i++)
        {
            for (var j = 0; j < Nodes.Length; j++)
            {
                StiffnessMatrix[i, j] =
                    IntegralCalculator.CalcDoubleIntegralForStiffnessMatrix(rUpperLimit, rDownLimit, zUpperLimit,
                        zDownLimit, LocalBasisFunctions[i],
                        LocalBasisFunctions[j], Material.Lambdas[0]);
            }
        }
    }

    public void CalcMassMatrix()
    {
        MassMatrix = new LocalMatrix(Nodes.Length, Nodes.Length);

        var rUpperLimit = Nodes[1].R;
        var rDownLimit = Nodes[0].R;
        var zUpperLimit = Nodes[2].Z;
        var zDownLimit = Nodes[0].Z;

        for (var i = 0; i < Nodes.Length; i++)
        {
            for (var j = 0; j < Nodes.Length; j++)
            {
                MassMatrix[i, j] =
                    IntegralCalculator.CalcDoubleIntegralForMassMatrix(rUpperLimit, rDownLimit, zUpperLimit,
                        zDownLimit, LocalBasisFunctions[i],
                        LocalBasisFunctions[j]);
            }
        }
    }

    public void CalcRightPart(FProvider rightPartProvider)
    {
        var rightPart = new LocalVector(Nodes.Length);

        for (var i = 0; i < Nodes.Length; i++)
        {
            rightPart[i] = rightPartProvider.CalcRightPart(GlobalNodesNumbers[i]);
        }

        RightPart = MassMatrix * rightPart;
    }

    public void CalcAMatrix()
    {
        LocalMatrixA = StiffnessMatrix + MassMatrix * Material.Gamma;
    }
}