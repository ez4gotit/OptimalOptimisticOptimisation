using System;
namespace SimplexMethod {
  public class TransportationModel {
    public static (double, Matrix) VogelApproximation(Matrix S, Matrix C, Matrix D) {
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
      
      while (rows.Length > 1 && columns.Length > 1) {
        double maximumRowPenalty = 0;
        int rowPosition = 0;
        foreach (int i in rows) {
          double rowPenalty = getRowDifferenceOfMins(C, i, columns);
          if (rowPenalty >= maximumRowPenalty) {
            maximumRowPenalty = rowPenalty;
            rowPosition = i;
          }
        }
        double maximumColumnPenalty = 0;
        int columnPosition = 0;
        foreach (int i in columns) {
          double columnPenalty = getColumnDifferenceOfMins(C, i, rows);
          if (columnPenalty >= maximumColumnPenalty) {
            maximumColumnPenalty = columnPenalty;
            columnPosition = i;
          }
        }
        int choosenRow, choosenColumn;
        if (maximumRowPenalty >= maximumColumnPenalty) {
          int pos = columns[0];
          double minValue = C[rowPosition, columns[0]];
          foreach (int i in columns) {
            double currentElement = C[rowPosition, i];
            if (currentElement <= minValue) {
              minValue = currentElement;
              pos = i;
            }
          }
          choosenRow = rowPosition;
          choosenColumn = pos;
        } else {
          int pos = rows[0];
          double minValue = C[rows[0], columnPosition];
          foreach (int i in rows) {
            double currentElement = C[i, columnPosition];
            if (currentElement <= minValue) {
              minValue = currentElement;
              pos = i;
            }
          }
          choosenRow = pos;
          choosenColumn = columnPosition;
        }
        
        double currentUnit = Math.Min(S[choosenRow, 0], D[choosenColumn, 0]);
        S[choosenRow, 0] -= currentUnit;
        D[choosenColumn, 0] -= currentUnit;
        if (S[choosenRow, 0] <= 0) {
          rows = rows.Where(val => val != choosenRow).ToArray();
        }
        if (D[choosenColumn, 0] <= 0) {
          columns = columns.Where(val => val != choosenColumn).ToArray();
        }
        solution[choosenRow, choosenColumn] = currentUnit;
        z += C[choosenRow, choosenColumn] * currentUnit;
      }
      if (rows.Length == 1) {
        int lastRow = rows[0];
        while (columns.Length > 0 && rows.Length > 0) {
          int pos = columns[0];
          double minValue = C[lastRow, columns[0]];
          foreach (int i in columns) {
            double currentElement = C[lastRow, i];
            if (currentElement <= minValue) {
              minValue = currentElement;
              pos = i;
            }
          }
          double currentUnit = Math.Min(S[lastRow, 0], D[pos, 0]);
          S[lastRow, 0] -= currentUnit;
          D[pos, 0] -= currentUnit;
          if (S[lastRow, 0] <= 0) {
            rows = rows.Where(val => val != lastRow).ToArray();
          }
          if (D[pos, 0] <= 0) {
            columns = columns.Where(val => val != pos).ToArray();
          }
          solution[lastRow, pos] = currentUnit;
          z += C[lastRow, pos] * currentUnit;
        }
      } else {
        int lastColumn = columns[0];
        while (columns.Length > 0 && rows.Length > 0) {
          int pos = rows[0];
          double minValue = C[rows[0], lastColumn];
          foreach (int i in rows) {
            double currentElement = C[i, lastColumn];
            if (currentElement <= minValue) {
              minValue = currentElement;
              pos = i;
            }
          }
          double currentUnit = Math.Min(S[pos, 0], D[lastColumn, 0]);
          S[pos, 0] -= currentUnit;
          D[lastColumn, 0] -= currentUnit;
          if (S[pos, 0] <= 0) {
            rows = rows.Where(val => val != pos).ToArray();
          }
          if (D[lastColumn, 0] <= 0) {
            columns = columns.Where(val => val != lastColumn).ToArray();
          }
          solution[pos, lastColumn] = currentUnit;
          z += C[pos, lastColumn] * currentUnit;
        }
      }
      


      return (z, solution);
    }
    
    private static double getRowDifferenceOfMins(Matrix C, int row, int[] columns) {
      double firstMin = C[row, 0];
      double secondMin = C[row, 1]; 
      
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

    private static double getColumnDifferenceOfMins(Matrix C, int column, int[] rows) {
      double firstMin = C[0, column];
      double secondMin = C[1, column]; 
      
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
    public static (double, Matrix) NortwestRule(Matrix S, Matrix C, Matrix D) {
      int rows = C.Rows;
      int columns = C.Columns;
      Matrix solution = new(rows, columns);
      double z = 0;
      for (int i = 0; i < rows; i++) {
        for (int j = 0; j < columns; j++) {
          double currentSupply = S[i, 0];
          double currentDemand = D[j, 0];
          if (currentSupply == 0) {
            break;
          }
          if (currentDemand == 0) {
            continue;
          }
          double currentUnits = Math.Min(currentSupply, currentDemand);
          S[i, 0] -= currentUnits;
          D[j, 0] -= currentUnits;
          solution[i, j] = currentUnits;
          z += C[i, j] * currentUnits;
        }
      }
      return (z, solution);
    }
  }
}
