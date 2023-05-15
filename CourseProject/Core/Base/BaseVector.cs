using CourseProject.Core.Local;

namespace CourseProject.Core.Base;

public class BaseVector
{
    public double[] Vector { get; }

    public BaseVector() : this(Array.Empty<double>()) { }
    public BaseVector(double[] vector)
    {
        Vector = vector;
    }
    public BaseVector(int size) : this(new double[size]) { }

    public int Count => Vector.Length;
    public double this[int index]
    {
        get => Vector[index];
        set => Vector[index] = value;
    }

    public static BaseVector Multiply(double number, BaseVector localVector)
    {
        for (var i = 0; i < localVector.Count; i++)
        {
            localVector[i] *= number;
        }

        return localVector;
    }

    public static BaseVector Sum(BaseVector vector1, BaseVector vector2)
    {
        if (vector1.Count != vector2.Count) throw new Exception("Can't sum vectors");

        for (var i = 0; i < vector1.Count; i++)
        {
            vector1[i] += vector2[i];
        }

        return vector1;
    }

    public IEnumerator<double> GetEnumerator() => ((IEnumerable<double>)Vector).GetEnumerator();
}