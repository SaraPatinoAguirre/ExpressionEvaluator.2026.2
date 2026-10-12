using System;
using System.Collections.Generic;

namespace Backend;

public static class ExpressionEvaluator
{
    public static double Evalute(string infix)
    {
        var tokens = ObtenerTokens(infix); 
        var postfix = ToPostfix(tokens);   
        return EvalutePostfix(postfix);
    }

    private static List<string> ObtenerTokens(string infix)
    {
        var tokens = new List<string>();
        string numeroTemporal = "";

        for (int i = 0; i < infix.Length; i++)
        {
            char c = infix[i];

            if (char.IsDigit(c) || c == '.')
            {
                numeroTemporal += c;
            }
            else
            {
                if (!string.IsNullOrEmpty(numeroTemporal))
                {
                    tokens.Add(numeroTemporal);
                    numeroTemporal = "";
                }

                
                if (!char.IsWhiteSpace(c))
                {
                    tokens.Add(c.ToString());
                }
            }
        }

        if (!string.IsNullOrEmpty(numeroTemporal))
        {
            tokens.Add(numeroTemporal);
        }

        return tokens;
    }

    
    private static List<string> ToPostfix(List<string> infixTokens)
    {
        var postfix = new List<string>();
        var stack = new Stack<string>();

        foreach (var item in infixTokens)
        {
            if (IsOperator(item))
            {
                if (item == ")")
                {
                    while (stack.Count > 0 && stack.Peek() != "(")
                    {
                        postfix.Add(stack.Pop());
                    }
                    if (stack.Count > 0 && stack.Peek() == "(")
                    {
                        stack.Pop();
                    }
                }
                else
                {
                    if (stack.Count == 0 || item == "(")
                    {
                        stack.Push(item);
                    }
                    else
                    {
                        while (stack.Count > 0 && PriorityStack(stack.Peek()) >= PriorityInfix(item))
                        {
                            postfix.Add(stack.Pop());
                        }
                        stack.Push(item);
                    }
                }
            }
            else
            {
                postfix.Add(item);
            }
        }

        while (stack.Count > 0)
        {
            postfix.Add(stack.Pop());
        }

        return postfix;
    }

    private static double EvalutePostfix(List<string> postfixTokens)
    {
        var stack = new Stack<double>();

        foreach (var item in postfixTokens)
        {
            if (IsOperator(item))
            {
                var ope2 = stack.Pop();
                var ope1 = stack.Pop();
                stack.Push(Calculate(ope1, ope2, item));
            }
            else
            {
                double numero = double.Parse(item, System.Globalization.CultureInfo.InvariantCulture);
                stack.Push(numero);
            }
        }

        return stack.Pop();
    }

    private static int PriorityStack(string op) => op switch
    {
        "^" => 3,
        "*" => 2,
        "/" => 2,
        "+" => 1,
        "-" => 1,
        "(" => 0,
        _ => 0,
    };

    private static int PriorityInfix(string op) => op switch
    {
        "^" => 4,
        "*" => 2,
        "/" => 2,
        "+" => 1,
        "-" => 1,
        "(" => 5,
        _ => 0,
    };

    private static bool IsOperator(string item) => 
        item == "^" || item == "*" || item == "/" || item == "+" || item == "-" || item == "(" || item == ")";

    private static double Calculate(double ope1, double ope2, string item) => item switch
    {
        "*" => ope1 * ope2,
        "/" => ope2 == 0 ? throw new DivideByZeroException() : ope1 / ope2,
        "+" => ope1 + ope2,
        "-" => ope1 - ope2,
        "^" => Math.Pow(ope1, ope2),
        _ => throw new Exception("Invalid expression."),
    };
}