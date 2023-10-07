namespace SimplexMethod;


public class SimplexAlgorithm {
  public static (double, Matrix) Optimize(Matrix coefficients, Matrix constraints, Matrix rhs, double accuracy) {
    int[] basisColumns = GetInitialBasisColumns(coefficients, constraints);
    int[] nonBasisColumns =  GetInitialNonBasisColumns(basisColumns, constraints.Columns);
    SquareMatrix basis = FormInitalBasis(basisColumns, constraints);
    SquareMatrix basisInverse = basis.Inverse();
    Matrix basisCoefficients = GetBasisCoefficients(basisColumns, coefficients);
    Matrix decisionVars = new Matrix(0, 0);
    double z = 0;
    bool isFeasibleSolution = true;
    while (isFeasibleSolution) {
      decisionVars = basisInverse * rhs;
      z = (basisCoefficients * decisionVars)[0,0];
      int enteringVariableIndex = GetEnteringVariable(basisInverse, basisCoefficients, constraints, coefficients, nonBasisColumns, accuracy);
      if (enteringVariableIndex == -1) {
        break;
      }
      int enteringVariable = nonBasisColumns[enteringVariableIndex];
      int exitingVariableIndex = GetExitingVariable(basisInverse, constraints, enteringVariable, decisionVars, accuracy);
      if (exitingVariableIndex == -1) {
        Console.WriteLine("Unbounded Solution");
        break;
      }
      int exitingVariable = basisColumns[exitingVariableIndex];
      basisColumns[exitingVariableIndex] = enteringVariable;
      nonBasisColumns[enteringVariableIndex] = exitingVariable;
      basis.setRegion(0, exitingVariableIndex, constraints.getRegion(0, enteringVariable, constraints.Rows, enteringVariable));
      basisInverse = basis.Inverse();
    }

    return (z, decisionVars);
  }
  private static int GetEnteringVariable(Matrix basisInverse, Matrix basisCoefficients, Matrix constraints, Matrix coefficients, int[] nonBasisColumns, double accuracy) {
    double minElem = 0;
    int minElemPos = -1;
    int numOfColumns = nonBasisColumns.Length;
    int numOfVariables = constraints.Columns;
    int numOfEquations = constraints.Rows;
    for (int i = 0; i < numOfColumns; i++) {
      int currentColumnIndex = nonBasisColumns[i];
      Matrix currentColumn = constraints.getRegion(0, currentColumnIndex, numOfEquations, currentColumnIndex);
      double currentValue = (basisCoefficients * basisInverse * currentColumn)[0,0] - coefficients[0, currentColumnIndex];
      if (currentValue + accuracy < minElem) {
        minElem = currentValue + accuracy;
        minElemPos = currentColumnIndex;
      }
    } 
    return minElemPos;
  }
  private static int GetExitingVariable(Matrix basisInverse, Matrix constraints, int enteringVariable, Matrix currentSolution, double accuracy) {
    int numOfEquations = constraints.Rows;
    double minRatio = -1;
    int minRatioPos = -1;
    double currentRatio;
    Matrix enteringColumn = constraints.getRegion(0, enteringVariable, numOfEquations, enteringVariable);
    Matrix denominatorMatrix = basisInverse * enteringColumn;
    for (int i = 0; i < numOfEquations; i++) {
      if (denominatorMatrix[0,i] <= 0 || currentSolution[0, i] <= 0) {
        continue;
      }
      currentRatio = currentSolution[0,i] / denominatorMatrix[0, i];
      if (currentRatio < minRatio || minRatioPos == -1) {
        minRatio = currentRatio;
        minRatioPos = i;
      }
    }
    return minRatioPos;

  }
  private static int[] GetInitialBasisColumns(Matrix coefficients, Matrix constraints) {
    int numOfDecisionVariables = coefficients.Columns-coefficients.Rows;
    int[] basisColumns = new int[coefficients.Rows];
    for (int i = numOfDecisionVariables; i < coefficients.Columns; i++) {
      basisColumns[i - numOfDecisionVariables] = i;
    } 
    return basisColumns;
  }
  private static int[] GetInitialNonBasisColumns(int[] basisColumns, int n) {
    int[] nonBasisColumns = new int[n - basisColumns.Length];
    int currentColumn = 0;
    int currentBasis = 0;
    for (int i = 0; i < n; i++) {
      if (i != basisColumns[currentBasis]) {
        nonBasisColumns[currentColumn] = i;
        currentColumn++;
      } else {
        currentBasis++;
      }
    }
    return nonBasisColumns;
  }
  private static SquareMatrix FormInitalBasis(int[] basisColumns, Matrix constraints) {
    int m = basisColumns.Length;
    int rows = constraints.Rows;
    double[,] basis = new double[rows, m];
    for (int j = 0; j < m; j++) {
      int constraintColumn = basisColumns[j];
      for (int i = 0; i < rows; i++) {
        basis[i, j] = constraints[i, constraintColumn];
      }
    }
    SquareMatrix basisMatrix = new SquareMatrix(basis);
    return basisMatrix;
  }
  private static Matrix GetBasisCoefficients(int[] basisColumns, Matrix coefficients) {
    int m = basisColumns.Length;
    double[,] basisCoefficients = new double[1,m];
    for (int i = 0; i < m; i++) {
      basisCoefficients[0,i] = coefficients.values[0, basisColumns[i]];
    }
    Matrix basisCoefficientMatrix = new Matrix(basisCoefficients);
    return basisCoefficientMatrix;
  }
  
}