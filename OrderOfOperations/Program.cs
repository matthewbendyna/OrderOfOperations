using System;
using System.Text.RegularExpressions;

namespace OrderOfOperations
{
    class Program
    {
        static Regex quitRx = new Regex(@"^(q|e|s|quit|end|stop|exit)$");
        static void Main()
        {
            Orderofoperations ooo = new Orderofoperations();
            Cutarray cutarray = new Cutarray();
            string equation = "";
            while (true)  //Main loop
            {
                Console.Write("Enter a number or equation to calculate the answer or type quit, end, stop or exit to end the program: ");
                equation = Console.ReadLine();
                if (quitRx.IsMatch(equation))
                {
                    break;
                }
                Console.WriteLine(ooo.calculateString(equation));
            }
        }

    }
}