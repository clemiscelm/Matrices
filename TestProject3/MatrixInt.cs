using System.Diagnostics;

namespace Maths_Matrices.Tests;

public class MatrixInt
{
    int[,] _matrix = new int[,] { };
    
    public int NbLines;
    public int NbColumns;


    public MatrixInt(int nbLines, int nbColumns)
    {
        NbLines = nbLines;
        NbColumns = nbColumns;
        _matrix = new int[NbLines, NbColumns];
    }

    public MatrixInt(int[,] matrix)
    {
        NbLines = matrix.GetLength(0);
        NbColumns = matrix.GetLength(1);
        _matrix = matrix;
    }

    public int[,] ToArray2D()
    {
        return _matrix;
    }

    public int this[int i, int j]
    {
        get => _matrix[i, j];
        set => _matrix[i, j] = value;
    }

    public MatrixInt(MatrixInt matrix)
    {
        NbLines = matrix.NbLines;
        NbColumns = matrix.NbColumns;
        _matrix = new int[NbLines, NbColumns];
        for (int i = 0; i < NbLines; i++)
        {
            for (int j = 0; j < NbColumns; j++)
            {
                _matrix[i, j] = matrix[i, j];
            }
        }
    }

    public static MatrixInt Identity(int i)
    {
        MatrixInt identity = new MatrixInt(i, i);
        for (int j = 0; j < i; j++)
        {
            identity[j, j] = 1;
        }

        return identity;
    }

    public bool IsIdentity()
    {
        for (int i = 0; i < NbLines; i++)
        {
            for (int j = 0; j < NbColumns; j++)
            {
                if (i == j && _matrix[i, j] != 1)
                {
                    return false;
                }

                if (i != j && _matrix[i, j] != 0)
                {
                    return false;
                }
            }
        }

        if (NbColumns != NbLines)
        {
            return false;
        }

        return true;
    }

    public void Multiply(int scalar)
    {
        for (int i = 0; i < NbLines; i++)
        {
            for (int j = 0; j < NbColumns; j++)
            {
                _matrix[i, j] *= scalar;
            }
        }
    }

    public static MatrixInt Multiply(MatrixInt matrix, int scalar)
    {
        MatrixInt m = new MatrixInt(matrix);
        m.Multiply(scalar);
        return m;
    }

    public static MatrixInt operator *(MatrixInt matrix, int scalar)
    {
        return Multiply(matrix, scalar);
    }

    public static MatrixInt operator *(int scalar, MatrixInt matrix)
    {
        return Multiply(matrix, scalar);
    }

    public static MatrixInt operator -(MatrixInt scalar)
    {
        return Multiply(scalar, -1);
    }

    public void Add(MatrixInt matrix)
    {
        if (matrix.NbLines != NbLines || matrix.NbColumns != NbColumns)
        {
            throw new MatrixSumException();
        }

        for (int i = 0; i < matrix.NbLines; i++)
        {
            for (int j = 0; j < matrix.NbColumns; j++)
            {
                _matrix[i, j] += matrix[i, j];
            }
        }
    }

    public static MatrixInt Add(MatrixInt m1, MatrixInt m2)
    {
        MatrixInt m = new MatrixInt(m1);
        m.Add(m2);
        return m;

    }

    public static MatrixInt operator +(MatrixInt m1, MatrixInt m2)
    {
        return Add(m1, m2);
    }

    public void Minus(MatrixInt matrix)
    {

        if (matrix.NbLines != NbLines || matrix.NbColumns != NbColumns)
        {
            throw new MatrixSumException();
        }

        for (int i = 0; i < matrix.NbLines; i++)
        {
            for (int j = 0; j < matrix.NbColumns; j++)
            {
                _matrix[i, j] -= matrix[i, j];
            }
        }
    }

    public static MatrixInt operator -(MatrixInt m1, MatrixInt m2)
    {
        MatrixInt m = new MatrixInt(m1);
        m.Minus(m2);
        return m;
    }

    public static MatrixInt Multiply(MatrixInt m1, MatrixInt m2)
    {
        if (m1.NbColumns != m2.NbLines)
        {
            throw new MatrixMultiplyException();
        }

        MatrixInt m = new MatrixInt(m1.NbLines, m2.NbColumns);
        for (int i = 0; i < m1.NbLines; i++)
        {
            for (int j = 0; j < m2.NbColumns; j++)
            {
                for (int k = 0; k < m1.NbColumns; k++)
                {
                    m[i, j] += m1[i, k] * m2[k, j];
                }
            }
        }

        return m;
    }

    public static MatrixInt operator *(MatrixInt m1, MatrixInt m2)
    {
        return Multiply(m1, m2);
    }

    public MatrixInt Multiply(MatrixInt m1)
    {

        return Multiply(this, m1);
    }

    public MatrixInt Transpose()
    {
        MatrixInt m = new MatrixInt(NbColumns, NbLines);
        for (int i = 0; i < NbLines; i++)
        {
            for (int j = 0; j < NbColumns; j++)
            {
                m[j, i] = _matrix[i, j];
            }
        }

        return m;
    }


    public static MatrixInt Transpose(MatrixInt m)
    {
        return m.Transpose();
    }

    public static MatrixInt GenerateAugmentedMatrix(MatrixInt m1, MatrixInt m2)
    {
        MatrixInt m = new MatrixInt(m1.NbLines, m1.NbColumns + m2.NbColumns);

        for (int i = 0; i < m1.NbLines; i++)
        {
            for (int j = 0; j < m1.NbColumns; j++)
            {
                m[i, j] = m1[i, j];
            }
            for (int j = 0; j < m2.NbColumns; j++)
            {
                m[i, m1.NbColumns + j] = m2[i, j];
            }
        }

        return m;
    }

    public (MatrixInt m1, MatrixInt m2) Split(int j)
    {
        MatrixInt m1 = new MatrixInt(NbLines, j +1);
        MatrixInt m2 = new MatrixInt(NbLines, NbColumns - (j + 1));
        for (int i = 0; i < NbLines; i++)
        {
            for (int k = 0; k <= j; k++)
            {
                m1[i, k] = _matrix[i, k];
            }
            for (int k = 0; k < NbColumns - (j + 1); k++)
            {
                m2[i, k] = _matrix[i, j + 1 + k];
            }
        }

        return (m1, m2);
    }
}

public class MatrixSumException : Exception
{
    public MatrixSumException()
    {
        Debug.WriteLine("Matrixes must have the same number of lines and columns");
    }
}
public class MatrixMultiplyException : Exception
{
    public MatrixMultiplyException()
    {
        Debug.WriteLine("Matrixes must have the same number of lines and columns");
    }
}   