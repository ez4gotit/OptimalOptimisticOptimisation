using System;

namespace SimplexMethod
{
    public class InputLibrary
    {
        public static double[] ReadObjectiveFunctionCoefficients()
        {
            while (true)
            {
                Console.WriteLine("Enter the coefficients of the objective function (C):");
                string input = Console.ReadLine();

                if (TryParseDoubleArray(input, out double[] coefficients))
                {
                    return coefficients;
                }

                Console.WriteLine("Invalid input. Please enter comma-separated numbers.");
            }
        }

        public static double[,] ReadConstraintMatrix(int numCols)
        {
            while (true)
            {
                Console.WriteLine("Enter the coefficients of the constraint matrix (A):");
                Console.WriteLine("Enter the number of rows: ");
                if (!int.TryParse(Console.ReadLine(), out int numRows) || numRows <= 0)
                {
                    Console.WriteLine("Invalid input. Number of rows must be a positive integer.");
                    continue;
                }

                double[,] matrix = new double[numRows, numCols];

                for (int i = 0; i < numRows; i++)
                {
                    Console.WriteLine($"Enter the coefficients for row {i + 1} (comma-separated):");
                    string input = Console.ReadLine();

                    if (TryParseDoubleArray(input, out double[] rowCoefficients))
                    {
                        if (rowCoefficients.Length != numCols)
                        {
                            Console.WriteLine($"Invalid number of coefficients for row {i + 1}. Expected {numCols} coefficients.");
                            i--;
                            continue;
                        }

                        for (int j = 0; j < numCols; j++)
                        {
                            matrix[i, j] = rowCoefficients[j];
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Please enter comma-separated numbers.");
                        i--;
                    }
                }

                return matrix;
            }
        }

        public static double[] ReadRightHandSide(int numRows)
        {
            while (true)
            {
                Console.WriteLine("Enter the right-hand side numbers (b):");
                string input = Console.ReadLine();

                if (TryParseDoubleArray(input, out double[] rhs))
                {
                    if (rhs.Length != numRows) 
                    {
                        Console.WriteLine($"Invalid amount of numbers. Expected {numRows} numbers.");
                        continue;
                    }
                    foreach (double value in rhs) {
                        if (value < 0) {
                            Console.WriteLine("Method is not applicable: all right handside values should nonegative. Please, enter correct data.");
                            continue;
                        }
                    }
                    return rhs;
                }

                Console.WriteLine("Invalid input. Please enter comma-separated numbers.");
            }
        }

        public static double ReadApproximationAccuracy()
        {
            while (true)
            {
                Console.WriteLine("Enter the approximation accuracy:");
                string input = Console.ReadLine();

                if (double.TryParse(input, out double accuracy) && accuracy > 0)
                {
                    return accuracy;
                }

                Console.WriteLine("Invalid input. Accuracy must be a positive number.");
            }
        }

        private static bool TryParseDoubleArray(string input, out double[] result)
        {
            string[] parts = input.Split(' ');
            result = new double[parts.Length];

            for (int i = 0; i < parts.Length; i++)
            {

                if (!double.TryParse(parts[i], out result[i]))
                {
                    return false;
                }
            }

            return true;
        }
        public static double[] ReadInitialSolution(int numVars)
         {
            while (true)
            {
                Console.WriteLine("Enter the initial solution:");
                string input = Console.ReadLine();

                if (TryParseDoubleArray(input, out double[] solution))
                {
                    if (solution.Length != numVars) 
                    {
                        Console.WriteLine($"Invalid amount of numbers. Expected {numVars} numbers.");
                        continue;
                    }
                    return solution;
                }

                Console.WriteLine("Invalid input. Please enter space-separated numbers.");
            }
        }
    }
}
