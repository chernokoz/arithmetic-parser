using System.Collections.Generic;

namespace idedev2021
{
    public static class LangConfig
    {
        public static Dictionary<char, int> Priorities = new Dictionary<char, int>()
        {
            {'+', 1},
            {'-', 1},
            {'*', 2},
            {'/', 2}
        };
    }
    
    public static class SimpleParser
    {
        public static IExpression Parse(string text)
        {
            var operandsStack = new Stack<IExpression>();
            var operationsStack = new Stack<char>();

            foreach (var ch in text)
            {
                if (ch == '(')
                {
                    operationsStack.Push('(');
                }
                else if (ch == ')')
                {
                    while (operationsStack.Peek() != '(')
                    {
                        var headOp = operationsStack.Pop();
                        var secondOperand = operandsStack.Pop();
                        var firstOperand = operandsStack.Pop();
                        var res = new BinaryExpression(firstOperand, secondOperand, headOp);
                        operandsStack.Push(res);
                    }
                    operationsStack.Pop();
                    operandsStack.Push(new ParenExpression(operandsStack.Pop()));
                }
                else if (ch == '+' || ch == '-' || ch == '*' || ch == '/')
                {
                    if (operationsStack.Count == 0 || operationsStack.Peek() == '(')
                    {
                        operationsStack.Push(ch);
                        continue;
                    }
                    var priority = LangConfig.Priorities[ch];
                    var headOp = operationsStack.Peek();
                    var headPriority = LangConfig.Priorities[headOp];
                    
                    // if operations
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
                        if (headOp == '(') break;
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
            }
            
            return operandsStack.Pop();
        }
    }
}