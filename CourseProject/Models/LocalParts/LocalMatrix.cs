using System.Numerics;

namespace CourseProject.Models.LocalParts;

public class LocalMatrix
{
    public double[,] Matrix { get; set; }

    public LocalMatrix()
    {
        Matrix = new double[0, 0];
    }

    public LocalMatrix(int n, int m)
    {
        Matrix = new double[n, m];
    }

    public LocalMatrix(double[,] matrix)
    {
        Matrix = matrix;
    }

    public double this[int i, int j]
    {
        get => Matrix[i, j];
        set => Matrix[i, j] = value;
    }

    public int CountRows()
    {
        return Matrix.GetLength(0);
    }

    public int CountColumns()
    {
        return Matrix.GetLength(1);
    }

    public static LocalMatrix operator +(LocalMatrix matrix1, LocalMatrix matrix2)
    {
        var localMatrix = new LocalMatrix(matrix1.CountRows(), matrix1.CountColumns());

        if (matrix1.CountRows() != matrix2.CountRows() && matrix1.CountColumns() != matrix2.CountColumns())
        {
            throw new Exception("Can't sum matrix");
        }

        for (var i = 0; i < localMatrix.CountRows(); i++)
        {
            for (var j = 0; j < localMatrix.CountColumns(); j++)
            {
                localMatrix[i, j] = matrix1[i, j] + matrix2[i, j];
            }
        }

        return localMatrix;
    }

    public static LocalMatrix operator *(LocalMatrix matrix, double coefficient)
    {
        var localMatrix = new LocalMatrix(matrix.CountRows(), matrix.CountColumns());

        for (var i = 0; i < localMatrix.CountRows(); i++)
        {
            for (var j = 0; j < localMatrix.CountColumns(); j++)
            {
                localMatrix[i, j] = coefficient * matrix[i, j];
            }
        }

        return localMatrix;
    }

    public static LocalVector operator *(LocalMatrix matrix, LocalVector vector)
    {
        var localVector = new LocalVector(vector.Count);

        if (matrix.CountRows() != vector.Count)
        {
            throw new Exception("Can't sum matrix");
        }

        for (var i = 0; i < matrix.CountRows(); i++)
        {
            for (var j = 0; j < matrix.CountColumns(); j++)
            {
                localVector[i] += matrix[i, j] * vector[j];
            }
        }

        return localVector;
    }
}