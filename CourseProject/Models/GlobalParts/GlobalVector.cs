using CourseProject.Models.LocalParts;

namespace CourseProject.Models.GlobalParts;

public class GlobalVector : LocalVector, ICloneable
{
    public GlobalVector(int size) : base(size) { }

    public GlobalVector(double[] vector)
    {
        VectorArray = vector;
    }

    public static GlobalVector operator +(GlobalVector vector1, GlobalVector vector2)
    {
        var sumOfVector = new GlobalVector(vector1.Count);

        if (vector1.Count != vector2.Count) throw new Exception("Can't sum vectors");

        for (var i = 0; i < vector1.Count; i++)
        {
            sumOfVector[i] = vector1[i] + vector2[i];
        }

        return sumOfVector;
    }

    public static GlobalVector operator -(GlobalVector vector1, GlobalVector vector2)
    {
        var subtractOfVector = new GlobalVector(vector1.Count);

        if (vector1.Count != vector2.Count) throw new Exception("Can't sub vectors");

        for (var i = 0; i < vector1.Count; i++)
        {
            subtractOfVector[i] = vector1[i] - vector2[i];
        }

        return subtractOfVector;
    }

    public static GlobalVector operator *(GlobalVector localVector, double coefficient)
    {
        var vector = new GlobalVector(localVector.Count);

        for (var i = 0; i < vector.Count; i++)
        {
            vector[i] = coefficient * localVector[i];
        }

        return vector;
    }

    public void PlaceLocalVector(LocalVector rightPart, int[] globalNodesNumbers)
    {
        for (var i = 0; i < rightPart.Count; i++)
        {
            VectorArray[globalNodesNumbers[i]] += rightPart[i];
        }
    }

    public double CalcNorm()
    {
        return Math.Sqrt(VectorArray.Sum(element => element * element));
    }

    public object Clone()
    {
        var clone = new double[Count];
        Array.Copy(VectorArray, clone, Count);

        return new GlobalVector(Count)
        {
            VectorArray = clone
        };
    }
}