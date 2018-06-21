using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace PolskaCalcDll
{
    public class Notation
    {
        private string _notation;
        private string notation
        {
            get
            {
                return _notation;
            }
            set
            {
                try
                {
                    var str = value;
                    if (str.Replace(" ", string.Empty).Length < 3)
                        errormessage = "Строка содержит слишком мало символов. Пожалуйста, проверьте введённую строку.";
                    else if (!Regex.IsMatch(str, @"[a-zA-ZА-Яа-я0-9\+\-\*\/]"))
                        errormessage = "Строка содержит неверные символы. Пожалуйста, проверьте введённую строку.";
                    else
                    {
                        var numbercount = str.Where((i) => i >= '0' && i <= '9').Count();
                        var opercount = str.Where((i) => i == '+' || i == '-' || i == '*' || i == '/').Count();
                        if (numbercount <= opercount)
                            errormessage = "Строка содержит неверное количество операторов и операндов. Пожалуйста, проверьте введённую строку.";
                    }
                    _notation = value;
                }
                catch (Exception ex)
                {
                    errormessage = ex.Message + " " + ex.InnerException;
                }
            }
        }

        private Stack<int> Stack = new Stack<int>();

        /// <summary>
        /// Результат рассчетов.
        /// </summary>
        public string NotationResult;

        /// <summary>
        /// В случае ошибки тут будет ошибка. 
        /// </summary>
        public string errormessage = "";

        /// <summary>
        /// Конструктор. Принимает string прямую пн. Конвертирует её в опн(более понимабельно). Зовёт метод рассчетов.
        /// </summary>
        /// <param name="incomestring">Строка прямой!!! польской нотации</param>
        public Notation(string incomestring)
        {
            //обратной считать проще.
            notation = new string(incomestring.Reverse().ToArray());
            //если прошли проверку, запустим.
            if(errormessage=="")DoSomething();
        }

        private void DoSomething()
        {
            try
            {
                foreach (char i in _notation)
                {
                    int topnumber = 0;
                    int bottomnumber = 0;

                    if (Char.IsWhiteSpace(i))
                    {
                        continue;
                    }
                    else if (i.Equals('+') || i.Equals('-') || i.Equals('*') || i.Equals('/'))
                    {
                        if (Stack.Count > 1)
                        {
                            int result = 0;
                            topnumber = Stack.Pop();
                            bottomnumber = Stack.Pop();

                            switch (i)
                            {
                                case '+':
                                    result = bottomnumber + topnumber;
                                    break;
                                case '-':
                                    result = bottomnumber - topnumber;
                                    break;
                                case '*':
                                    result = bottomnumber * topnumber;
                                    break;
                                case '/':
                                    result = bottomnumber / topnumber;
                                    break;
                            }

                            Stack.Push(result);
                        }
                        else
                        {
                            throw new Exception("Неправильное количество символов или последовательность.");
                        }
                    }
                    else if (Char.IsNumber(i))
                    {
                        Stack.Push((int)Char.GetNumericValue(i));
                    }
                }

                if (Stack.Count == 1)
                    NotationResult = Convert.ToString(Stack.Pop());
                else
                    errormessage = String.Join("хммм...тут что-то не так. В стаке осталось {0} цифр.", Stack.Count );
            }
            catch (Exception e)
            {
                errormessage = e.Message + " " + e.InnerException;
            }
        }
    }
}
