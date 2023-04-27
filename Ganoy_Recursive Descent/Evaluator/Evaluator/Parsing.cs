using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Variables;

namespace Parsing
{
    class Parser
    {
        //para mka butang ko og name sa variables and
        //key word na status
        public static string[] parse(string expression)
        {
            if (expression == String.Empty)
            {
                return null;
            }
            else if (expression.ToLower().Contains("print"))
            {
                return expression.Split(' ');
            }
            else if (expression.Contains("="))
            {
                var s = expression.Replace(" ", "");
                return s.Split('=');
            }
            else if (expression.ToLower().Contains("status"))
            {
                var x = new string[1];
                x[0] = "status";
                return x;
            }

            return null;
        }
        //para ma parse ang expression og masulod sa infix
        public static string[] parseExpression(string expression)
        {
            List<string> tokenList = new List<string>();
            string var = "";

            expression = expression.Replace(";", "");
            expression = expression.Replace("--", "+");
            expression = expression.Replace("-+", "-");
            expression = expression.Replace("+-", "-");
            expression = expression.Replace(")(", ")*(");
            for (int i=0;i<expression.Length;i++)
            {
               
                if (IsNonVariable(expression[i].ToString()))
                {
                    tokenList.Add(expression[i].ToString());
                }
                else
                {
                    while (i < expression.Length && !IsNonVariable(expression[i].ToString()))
                    {
                        var += expression[i];
                        i++;
                    }

                    tokenList.Add(var);
                    i--;
                    var = "";
                }
            } 
            return tokenList.ToArray();
        }
        //boolean method para makita ang unary operators
        public static bool unaryOperator(string op)
        {
            op = op.ToLower();
            string[] sr = new string[] { "ln", "log", "sin", "cos", "tan", "exp","sqrt","fac" };
            return sr.Contains(op);
        }
        //boolean method para makita ang binary operators
        public static bool IsNonVariable(string op)
        {
            return "^*+-/÷()".Contains(op);
        }
    }
}
