namespace Maths_Matrices.Tests;
using System.Diagnostics;

public class MatrixElementaryOperations
{
    public static void SwapLines(MatrixInt m, int i, int j)
    {
        for (int k = 0; k < m.NbColumns; k++)
        {
            int temp = m[i, k];
            m[i, k] = m[j, k];
            m[j, k] = temp;
        }
    }
    public static void SwapColumns(MatrixInt m, int i, int j)
    {
        for (int k = 0; k < m.NbLines; k++)
        {
            int temp = m[k, i];
            m[k, i] = m[k, j];
            m[k, j] = temp;
        }
    }
    public static void MultiplyLine(MatrixInt m, int i, int k)
    {
        if(i == 0 && k == 0)
        {
            throw new MatrixScalarZeroException();
        }
        for (int j = 0; j < m.NbColumns; j++)
        {
            m[i, j] *= k;
        }
    }
    public static void MultiplyColumn(MatrixInt m, int j, int k)
    {
        if (j == 0 && k == 0)
        {
            throw new MatrixScalarZeroException();
        }
        for (int i = 0; i < m.NbLines; i++)
        {
            m[i, j] *= k;
        }
    }
    public static void AddLineToAnother(MatrixInt m, int line1, int line2, int scalarFactor)
    {
        for(int i = 0; i < m.NbColumns; i++)
        {
            m[line2, i] += m[line1, i] * scalarFactor;
        }
    }
    public static void AddColumnToAnother(MatrixInt m, int column1, int column2, int scalarFactor)
    {
        for(int i = 0; i < m.NbLines; i++)
        {
            m[i, column2] += m[i, column1] * scalarFactor;
        }
    }
}
public class MatrixScalarZeroException : Exception
{
    public MatrixScalarZeroException()
    {
        Debug.WriteLine("MatrixScalarZeroException");
    }
}

