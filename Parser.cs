using System.Collections.Generic;

namespace idedev2021
{
    public interface IExpression
    {
        void Accept(IExpressionVisitor visitor);
    }

    public class Literal : IExpression
    {
        public Literal(int value)
        {
            Value = value;
        }

        public readonly int Value;
        
        public void Accept(IExpressionVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    public class Variable : IExpression
    {
        public Variable(char name)
        {
            Name = name;
        }

        public readonly char Name;
        public void Accept(IExpressionVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
    
    public class BinaryExpression : IExpression
    {
        public readonly IExpression FirstOperand;
        public readonly IExpression SecondOperand;
        public readonly char Operator;

        public BinaryExpression(IExpression firstOperand, IExpression secondOperand, char @operator)
        {
            FirstOperand = firstOperand;
            SecondOperand = secondOperand;
            Operator = @operator;
        }

        public void Accept(IExpressionVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
    
    public class ParenExpression : IExpression
    {
        public ParenExpression(IExpression operand)
        {
            Operand = operand;
        }

        public readonly IExpression Operand;
        public void Accept(IExpressionVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    public static class LangConfig
    {
        public static Dictionary<char, int> Priorities = new Dictionary<char, int>()
        {
            {'+', 1},
            {'-', 1},
            {'*', 2},
            {'/', 2},
        };
    }
    
    public static class SimpleParser
    {
        public static IExpression Parse(string text)
        {
            var operandsStack = new Stack<IExpression>();
            var operationsStack = new Stack<char>();
            for (var i = 0; i < text.Length; i++)
            {
                var ch = text[i];
                if (ch == '+' || ch == '-' || ch == '*' || ch == '/')
                {
                    if (operationsStack.Count == 0)
                    {
                        operationsStack.Push(ch);
                        continue;
                    }
                    var priority = LangConfig.Priorities[ch];
                    var headOp = operationsStack.Peek();
                    var headPriority = LangConfig.Priorities[headOp];
                    
                    while (operationsStack.Count > 0 && priority <= headPriority)
                    {
                        headOp = operationsStack.Pop();
                        var secondOperand = operandsStack.Pop();
                        var firstOperand = operandsStack.Pop();
                        var res = new BinaryExpression(firstOperand, secondOperand, headOp);
                        operandsStack.Push(res);
                        if (operationsStack.Count == 0)
                        {
                            break;
                        }
                        headOp = operationsStack.Peek();
                        headPriority = LangConfig.Priorities[headOp];
                    }
                    
                    operationsStack.Push(ch);
                }
                else if (char.IsDigit(ch))
                {
                    operandsStack.Push(new Literal(ch - '0'));
                }
                else if (char.IsLetter(ch))
                {
                    operandsStack.Push(new Variable(ch));
                }
            }
            
            while (operationsStack.Count > 0)
            {
                var headOp = operationsStack.Pop();
                var secondOperand = operandsStack.Pop();
                var firstOperand = operandsStack.Pop();
                var res = new BinaryExpression(firstOperand, secondOperand, headOp);
                operandsStack.Push(res);
                if (operationsStack.Count == 0)
                {
                    break;
                }
            }
            
            return operandsStack.Pop();
        }
    }
}