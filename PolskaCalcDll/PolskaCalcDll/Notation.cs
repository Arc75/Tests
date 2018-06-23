using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace PolskaCalcDll
{
    public class Notation
    {

        private Stack<double> Stack = new Stack<double>();

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
            if (CheckCorrectString(incomestring))
            {
                //если прошли проверку, то распарсим, на случай неправильных пробелов.
                //запустим рассчеты.
                DoSomething(ParseString(incomestring));
            }

        }

        private void DoSomething(string[] operArray)
        {
            try
            {
                for (int x = operArray.Length - 1; x >= 0; x--)
                {
                    string i = operArray[x];
                    double topnumber = 0;
                    double bottomnumber = 0;

                    if (String.IsNullOrWhiteSpace(i))
                    {
                        continue;
                    }
                    else if (i == "+" || i == "-" || i == "*" || i == "/")
                    {
                        if (Stack.Count > 1)
                        {
                            double result = 0;
                            topnumber = Stack.Pop();
                            bottomnumber = Stack.Pop();

                            switch (i)
                            {
                                case "+":
                                    result = topnumber + bottomnumber;
                                    break;
                                case "-":
                                    result = topnumber - bottomnumber;
                                    break;
                                case "*":
                                    result = topnumber * bottomnumber;
                                    break;
                                case "/":
                                    result = topnumber / bottomnumber;
                                    break;
                            }

                            Stack.Push(result);
                        }
                        else
                        {
                            throw new Exception("Wrong sequence Неправильное количество символов или последовательность.");
                        }
                    }
                    else if (Regex.IsMatch(i, "-?\\d+(\\.\\d+)?"))
                    {
                        Stack.Push(Double.Parse(i));
                    }
                }

                if (Stack.Count == 1)
                    NotationResult = Convert.ToString(Stack.Pop());
                else
                    errormessage = String.Join("Whong count. {0} numbers left.", Stack.Count);

                #region old
                //foreach (char i in _notation)
                //{
                //    int topnumber = 0;
                //    int bottomnumber = 0;

                //    if (Char.IsWhiteSpace(i))
                //    {
                //        continue;
                //    }
                //    else if (i.Equals('+') || i.Equals('-') || i.Equals('*') || i.Equals('/'))
                //    {
                //        if (Stack.Count > 1)
                //        {
                //            int result = 0;
                //            topnumber = Stack.Pop();
                //            bottomnumber = Stack.Pop();

                //            switch (i)
                //            {
                //                case '+':
                //                    result = bottomnumber + topnumber;
                //                    break;
                //                case '-':
                //                    result = bottomnumber - topnumber;
                //                    break;
                //                case '*':
                //                    result = bottomnumber * topnumber;
                //                    break;
                //                case '/':
                //                    result = bottomnumber / topnumber;
                //                    break;
                //            }

                //            Stack.Push(result);
                //        }
                //        else
                //        {
                //            throw new Exception("Неправильное количество символов или последовательность.");
                //        }
                //    }
                //    else if (Char.IsNumber(i))
                //    {
                //        Stack.Push((int)Char.GetNumericValue(i));
                //    }
                //}

                //if (Stack.Count == 1)
                //    NotationResult = Convert.ToString(Stack.Pop());
                //else
                //    errormessage = String.Join("хммм...тут что-то не так. В стаке осталось {0} цифр.", Stack.Count );
                #endregion
            }
            catch (Exception e)
            {
                errormessage = e.Message + " " + e.InnerException;
            }
        }

        private string[] ParseString(string incomestring)
        {
            //получили строку. Сплитнули по пробелам
            string[] parsedstring = incomestring.Split(' ');
            //побежали
            for (int i = 0; i < parsedstring.Length; i++)
            {
                //если строка длиннее одного символа,то кроет подвохи
                if (parsedstring[i].Length > 1)
                {
                    string s = parsedstring[i];
                    //если это минусы, то
                    if (s.Contains('-'))
                    {
                        //если больше 1, то надо бы цикл
                        if (s.Where(j => j == '-').Count() > 1)
                        {
                            //побежали
                            for (int y = 0; y < s.Length; y++)
                            {
                                //Если минус, то проверяем, что после минуса. Если циферы, то перед ними пробел не нужен.                                
                                if (s[y] == '-')
                                {
                                    var x = CheckNextSymbol(s, y);
                                    if (x == 1)
                                        s.Remove(y, 1).Insert(y, " -");
                                    else if (x == 2)
                                        s.Remove(y, 1).Insert(y, " - ");
                                }
                            }
                        }
                        else
                        {
                            var x = CheckNextSymbol(s);
                            if (x == 1)
                            {
                                s.Replace("-", " -");
                            }
                            else if (x == 2)
                                s.Replace("-", " - ");
                        }

                    }
                    //если это нечто другое, то рассыпем пробелы.
                    if (s.Contains('+') || s.Contains('*') || s.Contains('/'))
                    {
                        s.Replace("*", " * ").Replace("+", " + ").Replace("/", " / ");
                    }
                    parsedstring[i] = s;
                }

            }

            //соберём результат и рассплитим ещё раз.
            var str = string.Join(" ", parsedstring);
            parsedstring = str.Split(' ');

            //реверснём, чтобы получить опн. так всё станет проще.
            //Reverser(ref parsedstring);

            return parsedstring;
        }

        //0 - не надо пробелов. 1 - пробел спереди. 2 оба пробела.
        private short CheckNextSymbol(string str)
        {
            try
            {
                //- точно присутствует. если его индекс = 0, значит он стоит в самом начале и трогать ничего не нужно.
                if (str.IndexOf("-") > 0)
                    if (Regex.IsMatch(str.Substring(str.IndexOf("-") + 1, 1), @"[0-9]"))
                        return 1;
                    else
                        return 2;
                else
                    return 0;
            }
            catch
            {
                //словили outofrange в последовательности с более чем 1 символом. значит минус не на своём месте. Отделим его.
                return 1;
            }
        }

        //0 - не надо пробелов. 1 - пробел спереди. 2 оба пробела.
        private int CheckNextSymbol(string str, int position)
        {
            try
            {
                if (str.IndexOf("-") > 0)
                    if (Regex.IsMatch(str.Substring(str[position] + 1, 1), @"[0-9]"))
                        return 1;
                    else
                        return 2;
                else
                    return 0;
            }
            catch
            {
                return 1;
            }
        }

        //private void Reverser(ref string[] str)
        //{
        //    for (int i = 0; i < str.Length / 2; i++)
        //    {
        //        string tmp = str[i];
        //        str[i] = str[str.Length - i - 1];
        //        str[str.Length - i - 1] = tmp;
        //    }
        //}

        private bool CheckCorrectString(string str)
        {
            bool ok = true;
            try
            {
                if (str.Replace(" ", string.Empty).Length < 3)
                {
                    errormessage = "To few symbols in string. Check it please";
                    ok = false;
                }
                else if (!Regex.IsMatch(str, @"[a-zA-ZА-Яа-я0-9\+\-\*\/]"))
                {
                    errormessage = "Wrong symbols in string. Check it please";
                    ok = false;
                }
                //else
                //{
                //    var numbercount = str.Where((i) => i >= '0' && i <= '9').Count();
                //    var opercount = str.Where((i) => i == '+' || (i == '-') || i == '*' || i == '/').Count();
                //    if (numbercount < opercount)
                //    {
                //        errormessage = "Строка содержит неверное количество операторов и операндов. Пожалуйста, проверьте введённую строку.";
                //        ok = false;
                //    }
                //}
                return ok;
            }
            catch (Exception ex)
            {
                errormessage = ex.Message + " " + ex.InnerException;
                return false;
            }
        }
    }
}
