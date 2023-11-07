using System;

namespace SimplexMethod
{
  public class InteriorPointAlgorithm 
  {
      public static (double, Matrix) Optimize(Matrix C, Matrix A, Matrix b, double accuracy, Matrix x, double alpha = 0.5) {
        // Calculate value for initial solution
        int numOfDecisionVars = A.Columns - A.Rows;
        Matrix decisionX = x.GetRegion(0, 0, numOfDecisionVars, 1);
        double change = 1;
        Matrix C_T = C;
        C = C.Transpose();
        // C = -1 * C;
        double z = (x * C_T)[0, 0];
        
        // Start iterative algorithm
        while (change > 0.05) {
          // Step 1: calculate matrices for the current solution
          Matrix D = GetDiagonalMatrix(x);
          Matrix newA = A * D;
          Matrix newC = D * C;
          // Step 2: calculate projection matrix and projected values
          Matrix P = GetProjectionMatrix(newA);
          Matrix projC = P * newC;
          // Step 3: calculate projected solution and new solution
          Matrix projX = GetProjectedX(projC, alpha);
          Matrix newX = D * projX;
          double newZ = (newX * C_T)[0, 0];
          // Step 4: calculate change relative to previous solution and stop if needed
          if (z != 0) {
            change = (newZ - z) / z;
            if (change < 0) {
              change = - change;
            }
          }
          x = newX;
          z = newZ;
          decisionX = x.GetRegion(0, 0, numOfDecisionVars, 1);
        }
        
        return (z, decisionX);
      }

      // Function to calculate diagonal matrix for current solution
      private static Matrix GetDiagonalMatrix(Matrix x) {
        int size = x.Rows;
        IdentityMatrix D = new IdentityMatrix(size);
        for (int i = 0; i < size; i++) {
          D[i, i] = x[i, 0];
        }
        return D;
      }

      // Function to calculate projection matrix
      private static Matrix GetProjectionMatrix(Matrix currentA) {
        int size = currentA.Columns;
        IdentityMatrix I = new IdentityMatrix(size);
        SquareMatrix AA_T = new SquareMatrix(currentA * (currentA.Transpose()));
        Matrix P = I - ((currentA.Transpose()) * (AA_T.Inverse()) * currentA);
        return P;
      }

      // Function to calculate projected solution vector
      private static Matrix GetProjectedX(Matrix projC, double alpha) {
        Matrix I = new Matrix(projC.Rows, 1);
        for (int i = 0; i < I.Rows; i++) {
          I[i,0] = 1;
        }
        // Get v as an absolte value of the negative element with the most absolute value 
        double v = 0;
        for (int i = 0; i < projC.Rows; i++) {
          double val = projC[i, 0];
          if (val < 0) {
            val = -val;
            if (val > v) {
              v = val;
            }
          }
        }
        Matrix projX = I + (alpha/v * projC);
        return projX;
      }
  }


}