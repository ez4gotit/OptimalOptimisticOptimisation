using System; 

namespace SimplexMethod
{
    internal class Program
    {
        static void Main(string[] args)
        {
            
            Console.WriteLine("ATTENTION! DECIMAL NUMBERS ARE IN FORMAT OF '1,0' NOT '1.0' AND MIGHT BE ONE SPACE-SEPARATED PLEASE, PAY ATTENTION, WE ARE RE-WORKING PARSER \n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n");
            Console.SetCursorPosition(0, 10);
            double [,] arr = new double[,] {{0, 0, 0, 0}, {1000000, 100000, 100000, 1000000}, {100000000000, 10000000000000000000, 100000000000, 1000000000000}};
            Matrix mx = new(arr);
            Console.Write(mx.ToString());

               
        }
    }
}