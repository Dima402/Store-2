using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Checker
{
    public class Class1
    {
        public string First(string str)
        {
            bool a = true;

            if (str.Length < 1 || str.Length > 50)
            {
                a = false;
            }
            else
            {
                foreach (char d in str)
                {

                    if (!Char.IsLetter(d))
                    {
                        if (d != ' ')
                        {
                            if (d != '_')
                            {
                                a = false;
                            }
                        }

                    }

                }
            }


            if (a == true)
            {
                return "";
            }
            else
            {
                return "Неверное ФИО";
            }

        }

        public string Second(string str)
        {
            string expression = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
            if (Regex.IsMatch(str, expression))
            {
                return "";
            }
            else
            {
                return "Неверный Email";
            }
        }

        public string Third(string str)
        {
            string expression = @"^((8|\+7)[\- ]?)?(\(?\d{3}\)?[\- ]?)?[\d\- ]{7,10}$";
            if (Regex.IsMatch(str, expression))
            {
                return "";
            }
            else
            {
                return "Неверный Телефон";
            }
        }
    }
}
