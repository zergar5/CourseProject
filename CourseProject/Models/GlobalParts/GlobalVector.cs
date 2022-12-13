using CourseProject.Models.Grid;
using CourseProject.Models.LocalParts;

namespace CourseProject.Models.GlobalParts;

public class GlobalVector : LocalVector
{
    public void PlaceLocalVector(LocalVector rightPart, int[] globalNodesNumbers)
    {
        for (var i = 0; i < rightPart.Count; i++)
        {
            VectorArray[globalNodesNumbers[i]] += rightPart[i];
        }
    }
}