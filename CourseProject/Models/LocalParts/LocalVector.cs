namespace CourseProject.Models.LocalParts;

public class LocalVector
{
    public double[] VectorArray { get; set; }

    public LocalVector()
    {
        VectorArray = Array.Empty<double>();
    }

    public LocalVector(int size)
    {
        VectorArray = new double[size];
    }

    public double this[int index]
    {
        get => VectorArray[index];
        set => VectorArray[index] = value;
    }

    public int Count => VectorArray.Length;

    public static LocalVector operator +(LocalVector vector1, LocalVector vector2)
    {
        var sumOfVector = new LocalVector(vector1.Count);

        if (vector1.Count != vector2.Count) throw new Exception("Can't sum vectors");

        for (var i = 0; i < vector1.Count; i++)
        {
            sumOfVector[i] = vector1[i] + vector2[i];
        }

        return sumOfVector;
    }

    public static LocalVector operator *(LocalVector localVector, double coefficient)
    {
        var vector = new LocalVector(localVector.Count);

        for (var i = 0; i < vector.Count; i++)
        {
            vector[i] = coefficient * localVector[i];
        }

        return vector;
    }

    public IEnumerator<double> GetEnumerator() => (IEnumerator<double>)VectorArray.GetEnumerator();
}