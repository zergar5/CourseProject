using CourseProject.Models.LocalParts;
using CourseProject.Tools;
using CourseProject.Tools.Calculators;
using CourseProject.Tools.Providers;

namespace CourseProject.Models.Grid;

public class Element : IEquatable<Element>
{
    public int[] GlobalNodesNumbers { get; set; }
    public Material Material { get; set; }
    public LocalBasisFunction[] LocalBasisFunctions { get; set; }
    public LocalMatrix StiffnessMatrix { get; set; }
    public LocalMatrix MassMatrix { get; set; }
    public LocalMatrix LocalMatrixA { get; set; }
    public LocalVector RightPart { get; set; }

    public Element(int[] globalNodesNumbers, Material material, LocalBasisFunction[] localBasisFunctions)
    {
        GlobalNodesNumbers = globalNodesNumbers;
        Material = material;
        LocalBasisFunctions = localBasisFunctions;
    }

    public void CalcStiffnessMatrix(NodeFinder nodeFinder)
    {
        StiffnessMatrix = new LocalMatrix(GlobalNodesNumbers.Length, GlobalNodesNumbers.Length);

        var rUpperLimit = nodeFinder.FindNode(GlobalNodesNumbers[1]).R;
        var rDownLimit = nodeFinder.FindNode(GlobalNodesNumbers[0]).R;
        var zUpperLimit = nodeFinder.FindNode(GlobalNodesNumbers[2]).Z;
        var zDownLimit = nodeFinder.FindNode(GlobalNodesNumbers[0]).Z;

        for (var i = 0; i < GlobalNodesNumbers.Length; i++)
        {
            for (var j = 0; j < GlobalNodesNumbers.Length; j++)
            {
                StiffnessMatrix[i, j] =
                    IntegralCalculator.CalcDoubleIntegralForStiffnessMatrix(rUpperLimit, rDownLimit, zUpperLimit,
                        zDownLimit, LocalBasisFunctions[i],
                        LocalBasisFunctions[j], Material.Lambdas[0]);
            }
        }
    }

    public void CalcMassMatrix(NodeFinder nodeFinder)
    {
        MassMatrix = new LocalMatrix(GlobalNodesNumbers.Length, GlobalNodesNumbers.Length);

        var rUpperLimit = nodeFinder.FindNode(GlobalNodesNumbers[1]).R;
        var rDownLimit = nodeFinder.FindNode(GlobalNodesNumbers[0]).R;
        var zUpperLimit = nodeFinder.FindNode(GlobalNodesNumbers[2]).Z;
        var zDownLimit = nodeFinder.FindNode(GlobalNodesNumbers[0]).Z;

        for (var i = 0; i < GlobalNodesNumbers.Length; i++)
        {
            for (var j = 0; j < GlobalNodesNumbers.Length; j++)
            {
                MassMatrix[i, j] =
                    IntegralCalculator.CalcDoubleIntegralForMassMatrix(rUpperLimit, rDownLimit, zUpperLimit,
                        zDownLimit, LocalBasisFunctions[i],
                        LocalBasisFunctions[j]);
            }
        }
    }

    public void CalcRightPart(PComponentsProvider pComponentsProvider)
    {
        var rightPart = new LocalVector(GlobalNodesNumbers.Length);

        for (var i = 0; i < GlobalNodesNumbers.Length; i++)
        {
            rightPart[i] = pComponentsProvider.CalcRightPart(GlobalNodesNumbers[i]);
        }

        RightPart = MassMatrix * rightPart;
    }

    public void CalcAMatrix()
    {
        LocalMatrixA = StiffnessMatrix + MassMatrix * Material.Gamma;
    }

    public bool Equals(Element? other)
    {
        return GlobalNodesNumbers.SequenceEqual(other.GlobalNodesNumbers) && Material.Equals(other.Material);
    }
}