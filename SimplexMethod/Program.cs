namespace SimplexMethod
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Matrix A = new Matrix(new double[,] {{6, 4, 1, 0, 0, 0}, 
                                                 {1, 2, 0, 1, 0, 0},
                                                 {-1, 1, 0, 0, 1, 0},
                                                 {0, 1, 0, 0, 0, 1}});
            Matrix C = new Matrix(new double[,] {{5, 4, 0, 0, 0, 0}});
            Matrix b = new Matrix(new double[,] {{24},{6},{1},{2}});
            (double z, Matrix Xb) = SimplexAlgorithm.Optimize(C, A, b, 0);
            Console.WriteLine(Xb.ToString());
            Console.WriteLine(z);
                            
        }
    }
}