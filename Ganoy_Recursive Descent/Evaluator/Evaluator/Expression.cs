using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expression
{
    //mao ni ang tig convert sa mga infix input into postfix
    public class InfixToPostfixConverter
    {
        //fields
        private Queue<string> _queue;
        private Stack<string> _stack;

        public string[] Infix { get; set; }
        //constructor
        public InfixToPostfixConverter()
        {
            _queue = new Queue<string>();
            _stack = new Stack<string>();

            Infix = null;
        }
        //parameterized constructor
        public InfixToPostfixConverter(string[] infix)
        {
            _queue = new Queue<string>();
            _stack = new Stack<string>();

            Infix = infix;
        }
        //mao ni converter nako
        public Queue<string> convert()
        {
           // kung way sulod ang infix kay wla siyay value
            if (Infix == null)
            {
                return null;
            }
            //diri nako ginacheck ang "(" og ")" 
            //sa mga equations
            foreach (var x in Infix)
            {
                if (x == "(")
                {
                    _stack.Push(x);
                }
                else if (x == ")")
                {
                    while (_stack.Count > 0)
                    {
                        string y = _stack.Pop();
                        if (y == "(")
                            break;
                        _queue.Enqueue(y);
                    }

                    if (_stack.Count <= 0)
                    {
                        throw new Exception("Error: The parenthasis is missing!");
                    }
                }               
                //if dili siya ( og ) and dili pud siya number
                //basin operator siya
                else if (IsOperator(x))
                {
                    //kung ang size kay zero na kay mag Push na sa stack
                    if (_stack.Count <= 0)
                    {
                        _stack.Push(x);
                    }
                    else
                    {
                        //kung dili pa zero ang size sa infix kay tanawo niya kung operator ba ang next element
                        //sunod kay komparahon niya ang Precedence sa operator sa sulod sa stack og ang established precedence nato
                        while (_stack.Count > 0 && IsOperator(_stack.Peek()) && GetPrecedence(_stack.Peek()) >= GetPrecedence(x))
                        {
                            _queue.Enqueue(_stack.Pop());
                        }
                        _stack.Push(x);
                    }
                }
                else
                {
                    _queue.Enqueue(x);
                }
            }
            //kung dili siya operator og dili pud siya ( og ) kay isulod siya sa queue kay digit siya
            while (_stack.Count > 0)
            {
                _queue.Enqueue(_stack.Pop());
            }

            return _queue;
        }

        //method para sa operator
        private bool IsOperator(string op)
        {
            if (op.Length > 1)
            {
                return Parsing.Parser.unaryOperator(op);
            }
            return "^*+-/÷".Contains(op);
        }
        //ang precedence
        private int GetPrecedence(string op)
        {
            if (op == "^") return 4;
            if (op == "*") return 3;
            if (op == "/") return 3;
            if (op == "÷") return 3;
            if (op == "sin") return 2;
            if (op == "cos") return 2;
            if (op == "tan") return 2;
            if (op == "exp") return 2;
            if (op == "sqrt") return 2;
            if (op == "fac") return 2;
            if (op == "log") return 2;
            if (op == "ln") return 2;
            if (op == "+") return 1;
            if (op == "-") return 1;
            throw new Exception("Not a valid operator");

        }
    }
    //pagnakuha na ang postfix kay isolve para makuha ang final answer
    public class EvaluatePostfix
    {
        private Stack<double> _stack;
        private Queue<string> _queue;

        public EvaluatePostfix()
        {
            _stack = new Stack<double>();
            _queue = new Queue<string>();
        }

        public EvaluatePostfix(Queue<string> queue)
        {
            _stack = new Stack<double>();
            _queue = queue;
        }

        public double evaluate()
        {
            if(_queue.Count < 0)
            {
                throw new Exception("Error: Queue is empty!");
            }

            foreach(var x in _queue)
            {
                if (IsOperator(x))
                {
                    double b = _stack.Pop();
                    double a = _stack.Pop();
                    _stack.Push(Calculate(a,b,x));
                }
                else if (Parsing.Parser.unaryOperator(x))
                {
                    double a = _stack.Pop();
                    _stack.Push(CalculateUnary(a, x));
                }
                else
                {
                    _stack.Push(double.Parse(x));
                }
            }
            return _stack.Pop();
        }
        //method para sa mga solutoins sa binary operators
        private double Calculate(double x, double y, string op)
        {
            switch (op)
            {
                case "^":
                    return Math.Pow(x, y);
                case "*":
                    return x * y;
                case "/":
                    return x / y;
                case "÷":
                    return x / y;
                case "+":
                    return x + y;
                case "-":
                    return x - y;
                default:
                    throw new Exception("Invalid operator");
            }
        }
        //method para sa mga solutoins sa unary operators
        private double CalculateUnary(double x, string op)
        {
            
            switch (op)
            {
                case "cos":
                    return Math.Cos(Math.PI/180*x);
                case "sin":
                    return Math.Sin(Math.PI / 180 * x);
                case "tan":
                    return Math.Tan(Math.PI / 180 * x);
                case "log":
                    return Math.Log10(x);
                case "exp":
                    return Math.Exp(x);
                case "ln":
                    return Math.Log(x);
                case "sqrt":
                    return Math.Sqrt(x);
                case "fac":
                   for(int i =1; i<x;i++)
                    {
                        x = x * i;
                    }
                    return x;
                default:
                    throw new Exception("Invalid operator");
            }
        }
        //check kung operator ba ang element
        private bool IsOperator(string op)
        {
            return "^*+-/÷".Contains(op);
        }
    }
}
