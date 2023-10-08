namespace SimplexMethod;
using System;

public class Matrix
{
    public int Rows { get; private set; }
    public int Columns { get; private set; }
    public double[,] values;

    public Matrix(int rows, int cols)
    {
        Rows = rows;
        Columns = cols;
        values = new double[rows, cols];
    }
    
    public Matrix(double[,] vals) {
        Rows = vals.GetLength(0);
        Columns = vals.GetLength(1);
        values = new double[Rows, Columns];
        for (int i = 0; i < Rows; i++) {
            for (int j = 0; j < Columns; j++) {
                values[i, j] = vals[i, j];
            }
        }
    }

    public Matrix(Matrix m)
    {
        Rows = m.Rows;
        Columns = m.Columns;
        values = new double[Rows, Columns];
        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Columns; j++)
            {
                values[i, j] = m.values[i, j];
            }
        }
    }

    public static Matrix operator +(Matrix a, Matrix b)
    {
        if (a.Rows != b.Rows || a.Columns != b.Columns)
        {
            throw new ArgumentException("Matrix dimensions must be the same for addition.");
        }

        Matrix result = new Matrix(a.Rows, a.Columns);
        for (int i = 0; i < a.Rows; i++)
        {
            for (int j = 0; j < a.Columns; j++)
            {
                result.values[i, j] = a.values[i, j] + b.values[i, j];
            }
        }
        return result;
    }

    public double this[int i, int j]
   {
      get => values[i, j];
      set => values[i, j] = value;
   }
    public static Matrix operator -(Matrix a, Matrix b)
    {
        if (a.Rows != b.Rows || a.Columns != b.Columns)
        {
            throw new ArgumentException("Matrix dimensions must be the same for subtraction.");
        }

        Matrix result = new Matrix(a.Rows, a.Columns);
        for (int i = 0; i < a.Rows; i++)
        {
            for (int j = 0; j < a.Columns; j++)
            {
                result.values[i, j] = a.values[i, j] - b.values[i, j];
            }
        }
        return result;
    }

    public static Matrix operator *(Matrix a, Matrix b)
    {
        if (a.Columns != b.Rows)
        {
            throw new ArgumentException("Number of columns in the first matrix must be equal to the number of rows in the second matrix.");
        }

        Matrix result = new Matrix(a.Rows, b.Columns);
        for (int i = 0; i < a.Rows; i++)
        {
            for (int j = 0; j < b.Columns; j++)
            {
                result.values[i, j] = 0;
                for (int k = 0; k < a.Columns; k++)
                {
                    result.values[i, j] += a.values[i, k] * b.values[k, j];
                }
            }
        }
        return result;
    }
    public Matrix Transpose()
    {
        Matrix result = new Matrix(Columns, Rows);
        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Columns; j++)
            {
                result.values[j, i] = values[i, j];
            }
        }
        return result;
    }
    public Matrix GetRegion(int nStart, int mStart, int nEnd, int mEnd) {
        if (nStart < 0 || mStart < 0
        || nEnd > Rows || mEnd > Columns
        || nStart > nEnd || mStart > mEnd) {
            Console.WriteLine("Wrong input data");
            return null;
        }
        int regionRows = nEnd - nStart;
        int regionColumns = mEnd - mStart;
        Matrix regionMatrix = new Matrix(regionRows, regionColumns);
        for (int i = nStart; i < nEnd; i++) {
            for (int j = mStart; j < mEnd; j++) {
                regionMatrix[i-nStart, j-mStart] = values[i, j];
            }
        } 
        return regionMatrix;
    }

    public void SetRegion(int nStart, int mStart, double[,] vals) {
        int nEnd = nStart + vals.GetLength(0);
        int mEnd = mStart + vals.GetLength(1);
        if (nStart < 0 || mStart < 0
        || nEnd > Rows || mEnd > Columns) {
            Console.WriteLine("Wrong input data");
            return;
        }
        for (int i = nStart; i < nEnd; i++) {
            for (int j = mStart; j < mEnd; j++) {
                values[i, j] = vals[i-nStart, j-mStart];
            }
        }
    }

    public void SetRegion(int nStart, int mStart, Matrix mx) {
        int nEnd = nStart + mx.Rows;
        int mEnd = mStart + mx.Columns;
        if (nStart < 0 || mStart < 0
        || nEnd > Rows || mEnd > Columns) {
            Console.WriteLine("Wrong input data");
            return;
        }
        for (int i = nStart; i < nEnd; i++) {
            for (int j = mStart; j < mEnd; j++) {
                values[i, j] = mx[i-nStart, j-mStart];
            }
        }
    }

    public override string ToString()
    {
        string output = "";
        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Columns; j++)
            {
                if (Math.Abs(values[i, j]) < 0.00005)
                {
                    output += "0.0000 ";
                }
                else
                {
                    output += values[i, j].ToString("F4") + " ";
                }
            }
            output += "\n";
        }
        return output;
    }
}

public class ColumnVector : Matrix
{
    public ColumnVector(int n) : base(n, 1)
    {
    }

    public static ColumnVector operator +(ColumnVector a, ColumnVector b)
    {
        if (a.Rows != b.Rows)
        {
            throw new ArgumentException("Vector dimensions must be the same for addition.");
        }

        ColumnVector result = new ColumnVector(a.Rows);
        for (int i = 0; i < a.Rows; i++)
        {
            result.values[i, 0] = a.values[i, 0] + b.values[i, 0];
        }
        return result;
    }

    public static ColumnVector operator -(ColumnVector a, ColumnVector b)
    {
        if (a.Rows != b.Rows)
        {
            throw new ArgumentException("Vector dimensions must be the same for subtraction.");
        }

        ColumnVector result = new ColumnVector(a.Rows);
        for (int i = 0; i < a.Rows; i++)
        {
            result.values[i, 0] = a.values[i, 0] - b.values[i, 0];
        }
        return result;
    }

    public static ColumnVector operator *(ColumnVector a, ColumnVector b)
    {
        if (a.Rows != b.Rows)
        {
            throw new ArgumentException("Vector dimensions must be the same for element-wise multiplication.");
        }

        ColumnVector result = new ColumnVector(a.Rows);
        for (int i = 0; i < a.Rows; i++)
        {
            result.values[i, 0] = a.values[i, 0] * b.values[i, 0];
        }
        return result;
    }
}

public class SquareMatrix : Matrix
{
    public SquareMatrix(int n) : base(n, n)
    {
    }
    public SquareMatrix(double[,] vals) : base(vals)
    {        
        if (vals.GetLength(0) != vals.GetLength(1)) {
            throw new ArgumentException("Invalid dimension sizes of matrix");
        }
    }

    public static SquareMatrix operator +(SquareMatrix a, SquareMatrix b)
    {
        if (a.Rows != b.Rows || a.Columns != b.Columns)
        {
            throw new ArgumentException("Matrix dimensions must be the same for addition.");
        }

        SquareMatrix result = new SquareMatrix(a.Rows);
        for (int i = 0; i < a.Rows; i++)
        {
            for (int j = 0; j < a.Columns; j++)
            {
                result.values[i, j] = a.values[i, j] + b.values[i, j];
            }
        }
        return result;
    }

    public static SquareMatrix operator -(SquareMatrix a, SquareMatrix b)
    {
        if (a.Rows != b.Rows || a.Columns != b.Columns)
        {
            throw new ArgumentException("Matrix dimensions must be the same for subtraction.");
        }

        SquareMatrix result = new SquareMatrix(a.Rows);
        for (int i = 0; i < a.Rows; i++)
        {
            for (int j = 0; j < a.Columns; j++)
            {
                result.values[i, j] = a.values[i, j] - b.values[i, j];
            }
        }
        return result;
    }

    public static SquareMatrix operator *(SquareMatrix a, SquareMatrix b)
    {
        if (a.Columns != b.Rows)
        {
            throw new ArgumentException("Number of columns in the first matrix must be equal to the number of rows in the second matrix.");
        }

        SquareMatrix result = new SquareMatrix(a.Rows);
        for (int i = 0; i < a.Rows; i++)
        {
            for (int j = 0; j < b.Columns; j++)
            {
                result.values[i, j] = 0;
                for (int k = 0; k < a.Columns; k++)
                {
                    result.values[i, j] += a.values[i, k] * b.values[k, j];
                }
            }
        }
        return result;
    }

    public new SquareMatrix Transpose()
    {
        SquareMatrix result = new SquareMatrix(Rows);
        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Columns; j++)
            {
                result.values[j, i] = values[i, j];
            }
        }
        return result;
    }
    public new void SetRegion(int nStart, int mStart, double[,] vals){
        int nEnd = nStart + vals.GetLength(0);
        int mEnd = mStart + vals.GetLength(1);
        if (nStart < 0 || mStart < 0
        || nEnd > Rows || mEnd > Columns) {
            Console.WriteLine("Wrong input data");
            return;
        }
        for (int i = nStart; i < nEnd; i++) {
            for (int j = mStart; j < mEnd; j++) {
                values[i, j] = vals[i-nStart, j-mStart];
            }
        }
    }
    public new void SetRegion(int nStart, int mStart, Matrix mx) {
        int nEnd = nStart + mx.Rows;
        int mEnd = mStart + mx.Columns;
        if (nStart < 0 || mStart < 0
        || nEnd > Rows || mEnd > Columns) {
            Console.WriteLine("Wrong input data");
            return;
        }
        for (int i = nStart; i < nEnd; i++) {
            for (int j = mStart; j < mEnd; j++) {
                values[i, j] = mx[i-nStart, j-mStart];
            }
        }
    }


    public SquareMatrix GaussianElimination()
    {
        SquareMatrix result = new SquareMatrix(Rows);
        int step = 1;
        for (int i = 0; i < Rows; i++)
        {
            int maxRow = i;
            double maxV = Math.Abs(result.values[i, i]);
            for (int j = i + 1; j < Rows; j++)
            {
                if (Math.Abs(result.values[j, i]) > maxV)
                {
                    maxV = Math.Abs(result.values[j, i]);
                    maxRow = j;
                }
            }

            if (maxRow != i)
            {
                PermutationMatrix permutationMatrix = new PermutationMatrix(Rows, i, maxRow);
                result = permutationMatrix * result;
                Console.WriteLine("step #" + step++ + ": permutation");
                Console.WriteLine(result);
            }

            for (int j = i + 1; j < Rows; j++)
            {
                if (result.values[j, i] == 0) continue;
                EliminationMatrix eliminationMatrix = new EliminationMatrix(j, i, result);
                result = eliminationMatrix * result;
                Console.WriteLine("step #" + step++ + ": elimination");
                Console.WriteLine(result);
            }
        }
        return result;
    }

    public SquareMatrix Inverse()
    {
        SquareMatrix original = new SquareMatrix(Rows);
        original.values = (double[,])values.Clone();
        SquareMatrix augmented = IdentityMatrix(Rows);

        for (int i = 0; i < Rows; i++)
        {
            int maxRow = i;
            double maxV = Math.Abs(original.values[i, i]);
            for (int j = i + 1; j < Rows; j++)
            {
                if (Math.Abs(original.values[j, i]) > maxV)
                {
                    maxV = Math.Abs(original.values[j, i]);
                    maxRow = j;
                }
            }

            if (maxRow != i)
            {
                PermutationMatrix permutationMatrix = new PermutationMatrix(Rows, i, maxRow);
                original = permutationMatrix * original;
                augmented = permutationMatrix * augmented;
            }

            for (int j = i + 1; j < Rows; j++)
            {
                if (original.values[j, i] == 0) continue;
                EliminationMatrix eliminationMatrix = new EliminationMatrix(j, i, original);
                original = eliminationMatrix * original;
                augmented = eliminationMatrix * augmented;
            }
        }

        for (int i = Rows - 1; i >= 0; i--)
        {
            for (int j = i - 1; j >= 0; j--)
            {
                if (original.values[j, i] == 0) continue;
                EliminationMatrix eliminationMatrix = new EliminationMatrix(j, i, original);
                original = eliminationMatrix * original;
                augmented = eliminationMatrix * augmented;
            }
        }

        for (int i = 0; i < Rows; i++)
        {
            double factor = original.values[i, i];
            original.values[i, i] = 1;
            for (int j = 0; j < Rows; j++)
            {
                augmented.values[i, j] /= factor;
            }
        }

        return augmented;
    }

    public double Determinant()
    {
        SquareMatrix eliminated = GaussianElimination();
        double det = 1;
        for (int i = 0; i < Rows; i++)
        {
            det *= eliminated.values[i, i];
        }
        return det;
    }

    public static SquareMatrix IdentityMatrix(int n)
    {
        SquareMatrix identity = new SquareMatrix(n);
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                identity.values[i, j] = i == j ? 1 : 0;
            }
        }
        return identity;
    }
}

public class IdentityMatrix : SquareMatrix
{
    public IdentityMatrix(int n) : base(n)
    {
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                values[i, j] = i == j ? 1 : 0;
            }
        }
    }
}

public class EliminationMatrix : SquareMatrix
{
    public EliminationMatrix(int row, int column, SquareMatrix a) : base(a.Rows)
    {
        for (int i = 0; i < a.Rows; i++)
        {
            for (int j = 0; j < a.Rows; j++)
            {
                values[i, j] = i == j ? 1 : 0;
            }
        }
        values[row, column] = -a.values[row, column] / a.values[column, column];
    }
}

public class PermutationMatrix : SquareMatrix
{
    public PermutationMatrix(int n, int row1, int row2) : base(n)
    {
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                values[i, j] = i == j ? 1 : 0;
            }
        }
        for (int i = 0; i < n; i++)
        {
            (values[row1, i], values[row2, i]) = (values[row2, i], values[row1, i]);
        }
    }
}
