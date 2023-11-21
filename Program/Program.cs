using System; 

namespace SimplexMethod
{
    internal class Program
    {
        static void Main(string[] args)
        {
            
            Console.WriteLine("ATTENTION! DECIMAL NUMBERS ARE IN FORMAT OF '1,0' NOT '1.0' AND MIGHT BE ONE SPACE-SEPARATED PLEASE, PAY ATTENTION, WE ARE RE-WORKING PARSER \n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n");
            // Console.SetCursorPosition(0, 10);
            double [,] costs = new double[,] {{6, 10, 15, 3}, {14, 5, 18, 20}, {6, 13, 7, 10}};
            Matrix C = new(costs);
            double[] S = new double[] {20, 90, 70};
            double[] D = new double[] {30, 40, 60, 50};
            (double z, Matrix solution) = TransportationModel.RussellAproximation(S, C, D);
            Console.WriteLine(z);
            Console.Write(solution.ToString());

               
        }
    }
}