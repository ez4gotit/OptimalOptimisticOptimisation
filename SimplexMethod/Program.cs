using System; 

namespace SimplexMethod
{
    internal class Program
    {
        static void Main(string[] args)
        {
            /*          Matrix C = new Matrix(new double[,] { { 5, 4, 0, 0, 0, 0 } });
                        Matrix b = new Matrix(new double[,] { { 24 }, { 6 }, { 1 }, { 2 } });
                        Matrix A = new Matrix(new double[,] {{6, 4, 1, 0, 0, 0},
                                                              {1, 2, 0, 1, 0, 0},
                                                              {-1, 1, 0, 0, 1, 0},
                                                              {0, 1, 0, 0, 0, 1}});
                        Matrix x = new Matrix(new double[,] {{1}, {2}, {10}, {1}, {0}, {0}}) 
            */


            double[] arrayC = InputLibrary.ReadObjectiveFunctionCoefficients();
            int columns = arrayC.Length;
            double[,] arrayA = InputLibrary.ReadConstraintMatrix(columns);
            int rows = arrayA.GetLength(0);
            double[] arrayb = InputLibrary.ReadRightHandSide(rows);
            double accuracy = InputLibrary.ReadApproximationAccuracy();
            double[] initialSolution = InputLibrary.ReadInitialSolution(columns);
            Matrix C = new Matrix(arrayC);
            Matrix A = new Matrix(arrayA);
            Matrix b = new Matrix(arrayb).Transpose();
            Matrix x = new Matrix(initialSolution).Transpose();
/*            double accuracy = 0.01;*/
            // (double z, Matrix vars) = SimplexAlgorithm.Optimize(C, A, b, accuracy);
            (double z, Matrix vars) = InteriorPointAlgorithm.Optimize(C, A, b, accuracy, x);
            Console.WriteLine(vars.ToString());
            Console.WriteLine(z);       
        }
    }
}