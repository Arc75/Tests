using PolskaCalcDll;
using System;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            do
            {
                Console.WriteLine("Calculator of polish notation.");
                Console.Write("Write the polish notation, using any integral numbers and operators + - * /: ");

                var notation = Console.ReadLine();

                Notation calc = new Notation(notation);

                if (String.IsNullOrEmpty(calc.errormessage))
                {
                    Console.WriteLine("Calc result: " + calc.NotationResult);                    
                }
                else
                {
                    Console.WriteLine("Error appears while calculations: " + calc.errormessage);
                }
                Console.WriteLine("Press something to proceed.");
                Console.ReadKey();
            }
            while (Console.ReadKey(true).Key != ConsoleKey.Escape);            
        }
    }
}
