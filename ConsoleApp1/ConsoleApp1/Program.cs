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
                Console.WriteLine("Калькулятор выражений польской нотации.");
                Console.WriteLine("Нажмите Esc, чтобы остановить эту адскую машину");
                Console.Write("Напишите выражение прямой польской нотации: ");

                var notation = Console.ReadLine();

                Notation calc = new Notation(notation);

                if (String.IsNullOrEmpty(calc.errormessage))
                {
                    Console.WriteLine("Результат вычисления: " + calc.NotationResult);                    
                }
                else
                {
                    Console.WriteLine("В процессе вычислений возникла ошибка: " + calc.errormessage);
                }
                Console.WriteLine("Нажмите что-нибудь на клавиатуре чтобы продолжать. Esc чтобы выйти.");
                Console.ReadKey();
            }
            while (Console.ReadKey(true).Key != ConsoleKey.Escape);            
        }
    }
}
