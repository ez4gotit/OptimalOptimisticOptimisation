using System;

namespace SimplexMethod
{


    public class SimplexAlgorithm
    {
        public static (double, Matrix) Optimize(Matrix C, Matrix A, Matrix b, double accuracy)
        {
            // Step 0: determine initial basis and basis variables
            int[] basisVars = GetInitialBasisVars(C, A);
            int[] nonBasisVars = GetInitialNonBasisVars(basisVars, C.Columns);
            int numOfDecisionVars = A.Columns - A.Rows;
            SquareMatrix B = FormInitalBasis(basisVars, A);
            SquareMatrix B_Inv;
            Matrix Cb;
            Matrix Xb;
            Matrix decisionVars = new Matrix(0, 0);
            double z = 0;
            bool isFeasibleSolution = true;


            // Start the iterations of the algorithm 
            while (isFeasibleSolution)
            {
                // Step 1: compute B^-1 and solution for current basis and coefficients
                Cb = GetBasisCoefficients(basisVars, C);
                B_Inv = B.Inverse();
                B_Inv.RoundMatrix(accuracy);
                Xb = B_Inv * b;
                Xb.RoundMatrix(accuracy);
                z = Matrix.RoundVal((Cb * Xb)[0, 0], accuracy);
                decisionVars = GetDecisionVars(basisVars, numOfDecisionVars, Xb);

                // Step 2: determine the entering variable and stop the algorithm if the solution is optimal
                int enteringVariableIndex = GetEnteringVar(B_Inv, Cb, A, C, nonBasisVars, accuracy);
                if (enteringVariableIndex == -1)
                {
                    break;
                }
                int enteringVariable = nonBasisVars[enteringVariableIndex];


                // Step 3: determine the exiting variable and stop the algorithm if the solution is unbounded
                int exitingVariableIndex = GetExitingVar(B_Inv, A, enteringVariable, Xb, accuracy);
                if (exitingVariableIndex == -1)
                {
                    Console.WriteLine("Unbounded Solution");
                    break;
                }
                int exitingVariable = basisVars[exitingVariableIndex];


                // Step 4: Replace exiting column with entering column in basis.
                basisVars[exitingVariableIndex] = enteringVariable;
                nonBasisVars[enteringVariableIndex] = exitingVariable;
                B.SetRegion(0, exitingVariableIndex, A.GetRegion(0, enteringVariable, A.Rows, enteringVariable + 1));
            }

            // Return the result at the end of algorithm
            return (z, decisionVars);
        }

        private static Matrix GetDecisionVars(int[] basisVars, int n, Matrix Xb)
        {
            Matrix decisionVars = new Matrix(1, n);
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < basisVars.Length; j++)
                {
                    if (basisVars[j] == i)
                    {
                        decisionVars[0, i] = Xb[j, 0];
                        break;
                    }
                }
            }
            return decisionVars;
        }
        private static int GetEnteringVar(Matrix B_Inv, Matrix Cb, Matrix A, Matrix C, int[] nonBasisVars, double accuracy)
        {
            double minVarValue = 0;
            int enteringVar = -1;
            int numOfEquations = A.Rows;
            for (int i = 0; i < nonBasisVars.Length; i++)
            {
                int currentVar = nonBasisVars[i];
                Matrix currentColumn = A.GetRegion(0, currentVar, numOfEquations, currentVar + 1);
                double currentValue = Matrix.RoundVal((Cb * B_Inv * currentColumn)[0, 0] - C[0, currentVar], accuracy);
                if (currentValue < minVarValue)
                {
                    minVarValue = currentValue;
                    enteringVar = i;
                }
            }
            return enteringVar;
        }
        private static int GetExitingVar(Matrix B_Inv, Matrix A, int enteringVar, Matrix Xb, double accuracy)
        {
            int numOfEquations = A.Rows;
            double minRatio = -1;
            int exitingVar = -1;
            double currentRatio;
            Matrix enteringVarColumn = A.GetRegion(0, enteringVar, numOfEquations, enteringVar + 1);
            Matrix enteringVarCoefficients = B_Inv * enteringVarColumn;
            enteringVarCoefficients.RoundMatrix(accuracy);
            for (int i = 0; i < numOfEquations; i++)
            {
                if (enteringVarCoefficients[i, 0] <= 0 || Xb[i, 0] <= 0)
                {
                    continue;
                }
                currentRatio = Matrix.RoundVal(Xb[i, 0] / enteringVarCoefficients[i, 0], accuracy);
                if (currentRatio < minRatio || exitingVar == -1)
                {
                    minRatio = currentRatio;
                    exitingVar = i;
                }
            }
            return exitingVar;

        }
        private static int[] GetInitialBasisVars(Matrix C, Matrix A)
        {
            int basisLen = A.Columns - A.Rows;
            int[] basisVars = new int[A.Rows];
            for (int i = basisLen; i < A.Columns; i++)
            {
                basisVars[i - basisLen] = i;
            }
            return basisVars;
        }
        private static int[] GetInitialNonBasisVars(int[] basisColumns, int n)
        {
            int[] nonBasisColumns = new int[n - basisColumns.Length];
            int currentColumn = 0;
            int currentBasis = 0;
            for (int i = 0; i < n; i++)
            {
                if (i != basisColumns[currentBasis])
                {
                    nonBasisColumns[currentColumn] = i;
                    currentColumn++;
                }
                else
                {
                    currentBasis++;
                }
            }
            return nonBasisColumns;
        }
        private static SquareMatrix FormInitalBasis(int[] basisVars, Matrix A)
        {
            int m = basisVars.Length;
            int rows = A.Rows;
            double[,] basis = new double[rows, m];
            for (int j = 0; j < m; j++)
            {
                int varColumn = basisVars[j];
                for (int i = 0; i < rows; i++)
                {
                    basis[i, j] = A[i, varColumn];
                }
            }
            SquareMatrix basisMatrix = new SquareMatrix(basis);
            return basisMatrix;
        }
        private static Matrix GetBasisCoefficients(int[] basisColumns, Matrix coefficients)
        {
            int m = basisColumns.Length;
            double[,] basisCoefficients = new double[1, m];
            for (int i = 0; i < m; i++)
            {
                basisCoefficients[0, i] = coefficients.values[0, basisColumns[i]];
            }
            Matrix basisCoefficientMatrix = new Matrix(basisCoefficients);
            return basisCoefficientMatrix;
        }

    }
}