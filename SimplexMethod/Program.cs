using System; 

namespace SimplexMethod
{
    internal class Program
    {
        static void Main(string[] args)
        {
                     Matrix C = new Matrix(new double[,] { { 1, 1, 0, 0} });
                        Matrix b = new Matrix(new double[,] { { 24 }, { 6 }, { 1 }, { 2 } });
                        Matrix A = new Matrix(new double[,] {{2, 4, 1, 0},
                                                             {1, 3, 0, -1}});
                        Matrix x = new Matrix(new double[,] {{0.5}, {3.5}, {1}, {2}}); 
                        double accuracy = 0.01;
            // double[] arrayC = InputLibrary.ReadObjectiveFunctionCoefficients();
            // int columns = arrayC.Length;
            // double[,] arrayA = InputLibrary.ReadConstraintMatrix(columns);
            // int rows = arrayA.GetLength(0);
            // double[] arrayb = InputLibrary.ReadRightHandSide(rows);
            // double accuracy = InputLibrary.ReadApproximationAccuracy();
            // double[] initialSolution = InputLibrary.ReadInitialSolution(columns);
            // Matrix C = new Matrix(arrayC);
            // Matrix A = new Matrix(arrayA);
            // Matrix b = new Matrix(arrayb).Transpose();
            // Matrix x = new Matrix(initialSolution).Transpose();
            
            // (double z, Matrix vars) = SimplexAlgorithm.Optimize(C, A, b, accuracy);
            (double z, Matrix vars) = InteriorPointAlgorithm.Optimize(C, A, b, accuracy, x);
            Console.WriteLine(vars.ToString());
            Console.WriteLine(z);       
        }
    }
}