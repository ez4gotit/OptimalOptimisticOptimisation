using System; 

namespace SimplexMethod
{
    internal class Program
    {
        static void Main(string[] args)
        {
            
            Console.WriteLine("ATTENTION! DECIMAL NUMBERS ARE IN FORMAT OF '1,0' NOT '1.0' AND MIGHT BE ONE SPACE-SEPARATED PLEASE, PAY ATTENTION, WE ARE RE-WORKING PARSER \n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n");
            // Console.SetCursorPosition(0, 10);
            // double [,] costs = new double[,] {{6, 10, 15, 3}, {14, 5, 18, 20}, {6, 13, 7, 10}};
            // Matrix C = new(costs);
            // double[] S = new double[] {20, 90, 70};
            // double[] D = new double[] {30, 40, 60, 50};
            Console.WriteLine("Please, enter values of transportation costs (C matrix with dimensions 3x4):");
            double [,] costs = InputLibrary.ReadMatrixValues(3, 4);
            Matrix C = new(costs);
            Console.WriteLine("Please, enter S and V vectors with sizes of 3 and 4 respectively.");
            Console.WriteLine("The values should be balanced i.e. sum of demands equals sum of supply.");
            Console.WriteLine("Enter the values supply (S vector with legth of 3):");
            double[] S = InputLibrary.ReadDoubleArray();
            if (S.Length != 3) {
                Console.WriteLine("Invalid size.");
                return; 
            }
            Console.WriteLine("Enter the values of demands (D vector with legth of 4):");
            double[] D = InputLibrary.ReadDoubleArray();
            if (D.Length != 4) {
                Console.WriteLine("Invalid size.");
                return; 
            }
            if (D.Sum() != S.Sum()) {
                Console.WriteLine("Invalid values: the problem is not balanced");
                return;
            }

            (double z, Matrix solution) = TransportationModel.NortwestRule(S, C, D);
            Console.WriteLine("Nortwest Corner Rule");
            Console.WriteLine("Total cost:");
            Console.WriteLine(z);
            Console.WriteLine("Solution Vector:");
            Console.Write(solution.ToString());
            (z, solution) = TransportationModel.VogelApproximation(S, C, D);
            Console.WriteLine("Vogel's Approximation");
            Console.WriteLine("Total cost:");
            Console.WriteLine(z);
            Console.WriteLine("Solution Vector:");
            Console.Write(solution.ToString());
            (z, solution) = TransportationModel.RussellAproximation(S, C, D);
            Console.WriteLine("Russell's Approximation");
            Console.WriteLine("Total cost:");
            Console.WriteLine(z);
            Console.WriteLine("Solution Vector:");
            Console.Write(solution.ToString());


               
        }
    }
}