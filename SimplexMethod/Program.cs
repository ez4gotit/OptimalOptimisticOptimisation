using System; 

namespace SimplexMethod
{
    internal class Program
    {
        static void Main(string[] args)
        {
            /*            Matrix C = new Matrix(new double[,] { { 5, 4, 0, 0, 0, 0 } });
                        Matrix b = new Matrix(new double[,] { { 24 }, { 6 }, { 1 }, { 2 } });
                        Matrix A = new Matrix(new double[,] {{6, 4, 1, 0, 0, 0},
                                                              {1, 2, 0, 1, 0, 0},
                                                              {-1, 1, 0, 0, 1, 0},
                                                              {0, 1, 0, 0, 0, 1}});*/


            double[] arrayC = InputLibrary.ReadObjectiveFunctionCoefficients();
            int rows = arrayC.Length;
            double[,] arrayA = InputLibrary.ReadConstraintMatrix(rows);
            rows = arrayA.GetLength(0);
            double[] arrayb = InputLibrary.ReadRightHandSide(rows);
            double accuracy = InputLibrary.ReadApproximationAccuracy();
            Matrix C = new Matrix(arrayC);
            Matrix A = new Matrix(arrayA);
            Matrix b = new Matrix(arrayb).Transpose();
/*            double accuracy = 0.01;*/
            (double z, Matrix vars) = SimplexAlgorithm.Optimize(C, A, b, accuracy);
            Console.WriteLine(vars.ToString());
            Console.WriteLine(z);       
        }
    }
}