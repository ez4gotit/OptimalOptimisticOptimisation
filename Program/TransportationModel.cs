using System;
namespace SimplexMethod {
  public class TransportationModel {
    // private static (Matrix, Matrix, Matrix) 
    public static (double, Matrix) NortwestRule(double[] vectorS, Matrix C, double[] vectorD) {
      double [] S = new double[vectorS.Length];
      Array.Copy(vectorS, S, vectorS.Length); 
      double [] D = new double[vectorD.Length];
      Array.Copy(vectorD, D, vectorD.Length);
      int rows = C.Rows;
      int columns = C.Columns;
      Matrix solution = new(rows, columns);
      double z = 0;
      for (int i = 0; i < rows; i++) {
        for (int j = 0; j < columns; j++) {
          double currentSupply = S[i];
          double currentDemand = D[j];
          if (currentSupply == 0) {
            break;
          }
          if (currentDemand == 0) {
            continue;
          }
          double currentUnits = Math.Min(currentSupply, currentDemand);
          S[i] -= currentUnits;
          D[j] -= currentUnits;
          solution[i, j] = currentUnits;
          z += C[i, j] * currentUnits;
        }
      }
      return (z, solution);
    }
    
    public static (double, Matrix) VogelApproximation(double[] vectorS, Matrix C, double[] vectorD) {
      double [] S = new double[vectorS.Length];
      Array.Copy(vectorS, S, vectorS.Length); 
      double [] D = new double[vectorD.Length];
      Array.Copy(vectorD, D, vectorD.Length);
      // Initializing rows and columns
      int[] rows = new int[C.Rows];
      for (int i = 0; i < C.Rows; i++) {
        rows[i] = i;
      }
      int[] columns = new int[C.Columns];
      for (int i = 0; i < C.Columns; i++) {
        columns[i] = i;
      }
      Matrix solution = new(C.Rows, C.Columns);
      double z = 0;
      // Iterations until size 1 x n or n x 1
      while (rows.Length > 1 && columns.Length > 1) {
        (int rowPosition, double maxRowPenalty) = GetMaxRowPenalty(C, rows, columns);
        (int columnPosition, double maxColumnPenalty) = GetMaxColumnPenalty(C, rows, columns);
        int choosenRow, choosenColumn;
        // Choose max from row and column penalty and find minimal element in corresponding column/row
        if (maxRowPenalty >= maxColumnPenalty) {
          choosenRow = rowPosition;
          choosenColumn = GetMinIndexInRow(C, choosenRow, columns);
        } else {
          choosenColumn = columnPosition;
          choosenRow = GetMinIndexInColumn(C, choosenColumn, rows); 
        }
        // Buy current unit
        BuyUnit(ref S, ref C, ref D, ref z, ref solution, ref rows, ref columns, choosenRow, choosenColumn);
        
      }
      // Second part of iterative algorithm for remaining row/column
      if (rows.Length == 1) {
        int lastRow = rows[0];
        while (columns.Length > 0 && rows.Length > 0) {
          int minColumn = GetMinIndexInRow(C, lastRow, columns);
          
          // Buy current unit
          BuyUnit(ref S, ref C, ref D, ref z, ref solution, ref rows, ref columns, lastRow, minColumn);
        }
      } else {
        int lastColumn = columns[0];
        while (columns.Length > 0 && rows.Length > 0) {
          int minRow = GetMinIndexInColumn(C, lastColumn, rows);
          // Buy current unit
          BuyUnit(ref S, ref C, ref D, ref z, ref solution, ref rows, ref columns, minRow, lastColumn);
        }
      }
      


      return (z, solution);
    }
    public static (double, Matrix) RussellAproximation(double[] vectorS, Matrix C, double[] vectorD) {
      double [] S = new double[vectorS.Length];
      Array.Copy(vectorS, S, vectorS.Length); 
      double [] D = new double[vectorD.Length];
      Array.Copy(vectorD, D, vectorD.Length);
      // Initializing rows and columns
      int[] rows = new int[C.Rows];
      for (int i = 0; i < C.Rows; i++) {
        rows[i] = i;
      }
      int[] columns = new int[C.Columns];
      for (int i = 0; i < C.Columns; i++) {
        columns[i] = i;
      }
      Matrix solution = new(C.Rows, C.Columns);
      double z = 0;
      double[] U = new double[C.Rows];
      double[] V = new double[C.Columns];
      while (rows.Length > 0 && columns.Length > 0) {
        FillUV(C, ref U, ref V, rows, columns);
        (int minRow, int minColumn) = GetMinDeltaPosition(C, U, V, rows, columns);
        BuyUnit(ref S, ref C, ref D, ref z, ref solution, ref rows, ref columns, minRow, minColumn);
      }


      return (z, solution);
    }
    private static (int, int) GetMinDeltaPosition(Matrix C, double[] U, double[] V, int[] rows, int[] columns) {
      int minRow = rows[0];
      int minColumn = columns[0];
      double minValue = C[minRow, minColumn] - U[minRow] - V[minColumn];
      foreach (int i in rows) {
        foreach (int j in columns) {
          double currentDelta = C[i, j] - U[i] - V[j];
          if (currentDelta <= minValue) {
            minValue = currentDelta;
            minRow = i;
            minColumn = j;
          }
        }
      }
      return (minRow, minColumn);
    }
    private static void FillUV(Matrix C, ref double[] U,  ref double[] V, int[] rows, int[] columns) {
      foreach (int i in rows) {
        U[i] = GetMaxInRow(C, i, columns);
      }
      foreach (int i in columns) {
        V[i] = GetMaxInColumn(C, i, rows);
      }
    }
    
    private static double GetMaxInRow(Matrix C, int row, int[] columns) {
      int pos = columns[0];
      double maxValue = C[row, columns[0]];
      foreach (int i in columns) {
        double currentElement = C[row, i];
        if (currentElement >= maxValue) {
          maxValue = currentElement;
          pos = i;
        }
      }
      return maxValue;
    }
    private static double GetMaxInColumn(Matrix C, int column, int[] rows) {
      int pos = rows[0];
      double maxValue = C[rows[0], column];
      foreach (int i in rows) {
        double currentElement = C[i, column];
        if (currentElement >= maxValue) {
          maxValue = currentElement;
          pos = i;
        }
      }
      return maxValue;
    }
    private static void BuyUnit(ref double[] S, ref Matrix C, ref double[] D, ref double z, ref Matrix solution, 
      ref int[] rows, ref int[] columns, int row, int column) {
      double currentUnit = Math.Min(S[row], D[column]);
        S[row] -= currentUnit;
        D[column] -= currentUnit;
        if (S[row] <= 0) {
          rows = rows.Where(val => val != row).ToArray();
        }
        if (D[column] <= 0) {
          columns = columns.Where(val => val != column).ToArray();
        }
        solution[row, column] = currentUnit;
        z += C[row, column] * currentUnit;
    }
    private static int GetMinIndexInRow(Matrix C, int row, int[] columns) {
      int pos = columns[0];
      double minValue = C[row, columns[0]];
      foreach (int i in columns) {
        double currentElement = C[row, i];
        if (currentElement <= minValue) {
          minValue = currentElement;
          pos = i;
        }
      }
      return pos;
    }
    private static int GetMinIndexInColumn(Matrix C, int column, int[] rows) {
      int pos = rows[0];
      double minValue = C[rows[0], column];
      foreach (int i in rows) {
        double currentElement = C[i, column];
        if (currentElement <= minValue) {
          minValue = currentElement;
          pos = i;
        }
      }
      return pos;
    }
    private static (int, double) GetMaxRowPenalty(Matrix C, int[] rows, int[] columns) {
      double maximumRowPenalty = 0;
      int rowPosition = 0;
      // Finding max row penalty
      foreach (int i in rows) {
        double rowPenalty = GetRowPenalty(C, i, columns);
        if (rowPenalty >= maximumRowPenalty) {
          maximumRowPenalty = rowPenalty;
          rowPosition = i;
        }
      }
      return (rowPosition, maximumRowPenalty);
    }
    private static (int, double) GetMaxColumnPenalty(Matrix C, int[] rows, int[] columns) {
      double maximumColumnPenalty = 0;
      int columnPosition = 0;
      // Finding max column penalty
      foreach (int i in columns) {
        double columnPenalty = GetColumnPenalty(C, i, rows);
        if (columnPenalty >= maximumColumnPenalty) {
          maximumColumnPenalty = columnPenalty;
          columnPosition = i;
         }
      }
      return (columnPosition, maximumColumnPenalty);
    }
    
    private static double GetRowPenalty(Matrix C, int row, int[] columns) {
      double firstMin = C[row, columns[0]];
      double secondMin = C[row, columns[1]]; 
      
      foreach (int i in columns) {
        double currentElement = C[row, i];
        if (currentElement <= firstMin) {
          secondMin = firstMin;
          firstMin = currentElement;
        } else if (currentElement <= secondMin) {
          secondMin = currentElement;
        }
      }
      return secondMin - firstMin;
    }

    private static double GetColumnPenalty(Matrix C, int column, int[] rows) {
      double firstMin = C[rows[0], column];
      double secondMin = C[rows[1], column]; 
      
      foreach (int i in rows) {
        double currentElement = C[i, column];
        if (currentElement <= firstMin) {
          secondMin = firstMin;
          firstMin = currentElement;
        } else if (currentElement <= secondMin) {
          secondMin = currentElement;
        }
      }
      return secondMin - firstMin;
    }    
    
  }
}
