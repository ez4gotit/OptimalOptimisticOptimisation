using System;

namespace SimplexMethod
{
    public class InputLibrary
    {
        public static double[] ReadDoubleArray()
        {
            while (true)
            {
                Console.WriteLine("Enter the double array values:");
                string input = Console.ReadLine();

                if (TryParseDoubleArray(input, out double[] coefficients))
                {
                    return coefficients;
                }

                Console.WriteLine("Invalid input. Please enter space-separated numbers.");
            }
        }

        public static double[,] ReadMatrixValues(int numRows, int numColumns)
        {
            while (true)
            {
                Console.WriteLine("Enter the matrix values:");                

                double[,] matrix = new double[numRows, numColumns];

                for (int i = 0; i < numRows; i++)
                {
                    Console.WriteLine($"Enter the coefficients for row {i + 1} (comma-separated):");
                    string input = Console.ReadLine();

                    if (input != null && TryParseDoubleArray(input, out double[] rowCoefficients))
                    {
                        if (rowCoefficients.Length != numColumns)
                        {
                            Console.WriteLine($"Invalid number of coefficients for row {i + 1}. Expected {numColumns} coefficients.");
                            i--;
                            continue;
                        }

                        for (int j = 0; j < numColumns; j++)
                        {
                            matrix[i, j] = rowCoefficients[j];
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Please enter space-separated numbers.");
                        i--;
                    }
                }

                return matrix;
            }
        }

        

        public static double ReadDoubleValue()
        {
            while (true)
            {
                Console.WriteLine("Enter the double value:");
                string input = Console.ReadLine();

                if (double.TryParse(input, out double value))
                {
                    return value;
                }

                Console.WriteLine("Invalid input. Please, input double value:");
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
        
    }
}
