using System.Diagnostics;

namespace Maths_Matrices.Tests;

public class MatrixFloat
{
    float[,] matrixFloat = new float[,] { };
    public int NbLines;
    public int NbColumns;
    public MatrixFloat(float[,] matrix)
    {
        NbLines = matrix.GetLength(0);
        NbColumns = matrix.GetLength(1);
        matrixFloat = matrix;
    }

    public float[,] ToArray2D()
    {
        return matrixFloat;
    }
    public float this[int row, int col]
    {
        get { return matrixFloat[row, col]; }
        set { matrixFloat[row, col] = value; }
    }
     public MatrixFloat InvertByRowReduction()
    {
        if (NbLines != NbColumns)
        {
            throw new InvalidOperationException("Only square matrices can be inverted.");
        }

        int n = NbLines;
        float[,] augmentedMatrix = new float[n, 2 * n];

        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                augmentedMatrix[i, j] = matrixFloat[i, j];  
                augmentedMatrix[i, j + n] = (i == j) ? 1f : 0f;  
            }
        }

        for (int i = 0; i < n; i++)
        {
            float diagElement = augmentedMatrix[i, i];
            if (Math.Abs(diagElement) < GlobalSettings.DefaultFloatingPointTolerance)
            {
                throw new InvalidOperationException("Matrix is singular and cannot be inverted.");
            }

            for (int j = 0; j < 2 * n; j++)
            {
                augmentedMatrix[i, j] /= diagElement;
            }

            for (int k = 0; k < n; k++)
            {
                if (k != i)
                {
                    float factor = augmentedMatrix[k, i];
                    for (int j = 0; j < 2 * n; j++)
                    {
                        augmentedMatrix[k, j] -= factor * augmentedMatrix[i, j];
                    }
                }
            }
        }

        float[,] invertedMatrix = new float[n, n];
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                invertedMatrix[i, j] = augmentedMatrix[i, j + n]; 
            }
        }

        return new MatrixFloat(invertedMatrix);
    }

    public static MatrixFloat InvertByRowReduction(MatrixFloat matrix)
    {
        return matrix.InvertByRowReduction();
    }
    public MatrixFloat SubMatrix(int excludeRow, int excludeColumn)
    {
        int newSize = NbLines - 1; 
        float[,] subMatrix = new float[newSize, newSize];
        int subMatrixRow = 0;

        for (int i = 0; i < NbLines; i++)
        {
            if (i == excludeRow) continue; 

            int subMatrixCol = 0;
            for (int j = 0; j < NbColumns; j++)
            {
                if (j == excludeColumn) continue; 

                subMatrix[subMatrixRow, subMatrixCol] = matrixFloat[i, j];
                subMatrixCol++;
            }
            subMatrixRow++;
        }

        return new MatrixFloat(subMatrix);
    }
    public static MatrixFloat SubMatrix(MatrixFloat matrix, int excludeRow, int excludeColumn)
    {
        int newSize = matrix.NbLines - 1; 
        float[,] subMatrix = new float[newSize, newSize];
        int subMatrixRow = 0;

        for (int i = 0; i < matrix.NbLines; i++)
        {
            if (i == excludeRow) continue; 

            int subMatrixCol = 0;
            for (int j = 0; j < matrix.NbColumns; j++)
            {
                if (j == excludeColumn) continue; 

                subMatrix[subMatrixRow, subMatrixCol] = matrix.ToArray2D()[i, j];
                subMatrixCol++;
            }
            subMatrixRow++;
        }

        return new MatrixFloat(subMatrix);
    }
    public static float Determinant(MatrixFloat matrix)
    {
        if (matrix.NbLines != matrix.NbColumns)
        {
            throw new InvalidOperationException("Determinant is only defined for square matrices.");
        }

        if (matrix.NbLines == 2)
        {
            return (matrix[0, 0] * matrix[1, 1]) - (matrix[0, 1] * matrix[1, 0]);
        }

        float determinant = 0f;

        for (int col = 0; col < matrix.NbColumns; col++)
        {
            MatrixFloat subMatrix = SubMatrix(matrix, 0, col);
            float cofactor = (col % 2 == 0 ? 1f : -1f) * matrix.ToArray2D()[0, col] * Determinant(subMatrix);
            determinant += cofactor;
        }

        return determinant;
    }
    public static MatrixFloat Identity(int size)
    {
        float[,] identityMatrix = new float[size, size];

        for (int i = 0; i < size; i++)
        {
            identityMatrix[i, i] = 1f;
        }

        return new MatrixFloat(identityMatrix);
    }
    public MatrixFloat Adjugate()
    {
        if (NbLines != NbColumns)
        {
            throw new InvalidOperationException("Adjugate is only defined for square matrices.");
        }

        float[,] adjugateMatrix = new float[NbLines, NbColumns];

        if (NbLines == 2)
        {
            adjugateMatrix[0, 0] = matrixFloat[1, 1];
            adjugateMatrix[0, 1] = -matrixFloat[0, 1];
            adjugateMatrix[1, 0] = -matrixFloat[1, 0];
            adjugateMatrix[1, 1] = matrixFloat[0, 0];
        }
        else
        {
            for (int i = 0; i < NbLines; i++)
            {
                for (int j = 0; j < NbColumns; j++)
                {
                    MatrixFloat subMatrix = SubMatrix(i, j);
                    float cofactor = subMatrix.Determinant();

                    if ((i + j) % 2 != 0)
                    {
                        cofactor = -cofactor; 
                    }

                    adjugateMatrix[j, i] = cofactor; 
                }
            }
        }

        return new MatrixFloat(adjugateMatrix);
    }
    public static MatrixFloat Adjugate(MatrixFloat matrix)
    {
        return matrix.Adjugate();
    }
    public float Determinant()
    {
        if (NbLines != NbColumns)
        {
            throw new InvalidOperationException("Determinant is only defined for square matrices.");
        }

        if (NbLines == 2)
        {
            return (matrixFloat[0, 0] * matrixFloat[1, 1]) - (matrixFloat[0, 1] * matrixFloat[1, 0]);
        }

        float determinant = 0f;

        for (int col = 0; col < NbColumns; col++)
        {
            MatrixFloat subMatrix = SubMatrix(0, col);
            float cofactor = (col % 2 == 0 ? 1f : -1f) * matrixFloat[0, col] * subMatrix.Determinant();
            determinant += cofactor;
        }

        return determinant;
    }
    public MatrixFloat InvertByDeterminant()
    {
        if (NbLines != NbColumns)
        {
            throw new InvalidOperationException("Only square matrices can be inverted.");
        }

        float determinant = Determinant(); 
        if (Math.Abs(determinant) < GlobalSettings.DefaultFloatingPointTolerance)
        {
            throw new MatrixInvertException(); 
        }

        MatrixFloat adjugate = Adjugate(); 

        
        float[,] invertedMatrix = new float[NbLines, NbColumns];

        for (int i = 0; i < NbLines; i++)
        {
            for (int j = 0; j < NbColumns; j++)
            {
                invertedMatrix[i, j] = (float)Math.Round(adjugate[i, j] / determinant, 3); 
            }
        }

        return new MatrixFloat(invertedMatrix);
    }
    public static MatrixFloat InvertByDeterminant(MatrixFloat m)
    {
        if (m.NbLines != m.NbColumns)
        {
            throw new InvalidOperationException("Only square matrices can be inverted.");
        }

        float determinant = m.Determinant(); 
        if (Math.Abs(determinant) < GlobalSettings.DefaultFloatingPointTolerance)
        {
            throw new MatrixInvertException();
        }

        MatrixFloat adjugate = m.Adjugate(); 

        float[,] invertedMatrix = new float[m.NbLines, m.NbColumns];

        for (int i = 0; i < m.NbLines; i++)
        {
            for (int j = 0; j < m.NbColumns; j++)
            {
                invertedMatrix[i, j] = (float)Math.Round(adjugate[i, j] / determinant, 3);
            }
        }

        return new MatrixFloat(invertedMatrix);
    }
    public static Vector4 operator *(MatrixFloat matrix, Vector4 vector)
    {
        float x = matrix.matrixFloat[0, 0] * vector.x +
                  matrix.matrixFloat[0, 1] * vector.y +
                  matrix.matrixFloat[0, 2] * vector.z +
                  matrix.matrixFloat[0, 3] * vector.w;

        float y = matrix.matrixFloat[1, 0] * vector.x +
                  matrix.matrixFloat[1, 1] * vector.y +
                  matrix.matrixFloat[1, 2] * vector.z +
                  matrix.matrixFloat[1, 3] * vector.w;

        float z = matrix.matrixFloat[2, 0] * vector.x +
                  matrix.matrixFloat[2, 1] * vector.y +
                  matrix.matrixFloat[2, 2] * vector.z +
                  matrix.matrixFloat[2, 3] * vector.w;

        float w = matrix.matrixFloat[3, 0] * vector.x +
                  matrix.matrixFloat[3, 1] * vector.y +
                  matrix.matrixFloat[3, 2] * vector.z +
                  matrix.matrixFloat[3, 3] * vector.w;

        return new Vector4(x, y, z, w);
    }
    public static MatrixFloat operator *(MatrixFloat a, MatrixFloat b)
    {
        if (a.NbColumns != b.NbLines)
        {
            throw new InvalidOperationException("Matrix dimensions are not compatible for multiplication.");
        }

        float[,] result = new float[a.NbLines, b.NbColumns];

        for (int i = 0; i < a.NbLines; i++)
        {
            for (int j = 0; j < b.NbColumns; j++)
            {
                result[i, j] = 0;
                for (int k = 0; k < a.NbColumns; k++)
                {
                    result[i, j] += a[i, k] * b[k, j];
                }
            }
        }

        return new MatrixFloat(result);
    }
}

public class MatrixRowReductionAlgorithm
{
    public static (MatrixFloat m3, MatrixFloat m4) Apply(MatrixFloat m1, MatrixFloat m2)
    {
        int rows = m1.NbLines;
        int cols = m1.NbColumns;
        float[,] augmentedMatrix = new float[rows, cols + 1];

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                augmentedMatrix[i, j] = m1.ToArray2D()[i, j];
            }

            augmentedMatrix[i, cols] = m2.ToArray2D()[i, 0];
        }

        for (int i = 0; i < rows; i++)
        {
            float diagElement = augmentedMatrix[i, i];
            if (Math.Abs(diagElement) > GlobalSettings.DefaultFloatingPointTolerance)
            {
                for (int j = 0; j < cols + 1; j++)
                {
                    augmentedMatrix[i, j] /= diagElement;
                }

                for (int k = 0; k < rows; k++)
                {
                    if (k != i)
                    {
                        float factor = augmentedMatrix[k, i];
                        if (Math.Abs(factor) > GlobalSettings.DefaultFloatingPointTolerance)
                        {
                            for (int j = 0; j < cols + 1; j++)
                            {
                                augmentedMatrix[k, j] -= factor * augmentedMatrix[i, j];
                            }
                        }
                    }
                }
            }
        }

        float[,] resultM1 = new float[rows, cols];
        float[,] resultM2 = new float[rows, 1];

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                resultM1[i, j] = Math.Abs(augmentedMatrix[i, j]) < GlobalSettings.DefaultFloatingPointTolerance
                    ? 0f
                    : augmentedMatrix[i, j];
            }

            resultM2[i, 0] = Math.Abs(augmentedMatrix[i, cols]) < GlobalSettings.DefaultFloatingPointTolerance
                ? 0f
                : augmentedMatrix[i, cols];
        }

        return (new MatrixFloat(resultM1), new MatrixFloat(resultM2));
    }
}

public struct Vector4
{
    public float x, y, z, w;

    public Vector4(float x, float y, float z, float w)
    {
        this.x = x;
        this.y = y;
        this.z = z;
        this.w = w;
    }
}


public class MatrixInvertException : Exception
{
    public MatrixInvertException()
    {
        Debug.WriteLine("Matrix is singular and cannot be inverted.");
    }
}