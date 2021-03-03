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
    
    enum CharType
    {
        Operand,
        Operation,
        OpeningBracket,
        ClosingBracket,
        Nothing
    }
    
    public static class SimpleParser
    {
        public static IExpression Parse(string text)
        {
            var operandsStack = new Stack<IExpression>();
            var operationsStack = new Stack<char>();
            CharType lastType = CharType.Nothing;

            foreach (var ch in text)
            {
                if (ch == '(')
                {
                    if (lastType == CharType.Operand) throw new IncorrectSyntaxException(
                        "Need operation or brackets before brackets"
                    );
                    lastType = CharType.OpeningBracket;
                    operationsStack.Push('(');
                    
                }
                else if (ch == ')')
                {
                    if (lastType == CharType.Operation || lastType == CharType.OpeningBracket) 
                        throw new IncorrectSyntaxException(
                        "Need operand or closing bracket before brackets"
                    );
                    while (operationsStack.Count > 0 && operationsStack.Peek() != '(')
                    {
                        var headOp = operationsStack.Pop();
                        var secondOperand = operandsStack.Pop();
                        var firstOperand = operandsStack.Pop();
                        var res = new BinaryExpression(firstOperand, secondOperand, headOp);
                        operandsStack.Push(res);
                        if (operationsStack.Count == 0) throw new IncorrectSyntaxException(
                            "Closed brackets without opening");
                    }
                    if (operationsStack.Count == 0) 
                        throw new IncorrectSyntaxException(
                            "There is unclosed bracket"
                        );
                    operationsStack.Pop();
                    operandsStack.Push(new ParenExpression(operandsStack.Pop()));
                    lastType = CharType.Operand;
                }
                else if (ch == '+' || ch == '-' || ch == '*' || ch == '/')
                {
                    if (lastType == CharType.Operation || lastType == CharType.OpeningBracket) 
                        throw new IncorrectSyntaxException(
                            "Unsupported operation and brackets combination"
                        );
                    if (operationsStack.Count == 0 || operationsStack.Peek() == '(')
                    {
                        operationsStack.Push(ch);
                        lastType = CharType.Operation;
                        continue;
                    }
                    var priority = LangConfig.Priorities[ch];
                    var headOp = operationsStack.Peek();
                    var headPriority = LangConfig.Priorities[headOp];
                    
                    while (operationsStack.Count > 0 && priority <= headPriority)
                    {
                        headOp = operationsStack.Pop();
                        if (operandsStack.Count < 2)
                            throw new IncorrectSyntaxException(
                                "Not enough operands for operation " + headOp
                            );
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
                    lastType = CharType.Operation;
                    operationsStack.Push(ch);
                }
                else if (char.IsDigit(ch))
                {
                    operandsStack.Push(new Literal(ch - '0'));
                    lastType = CharType.Operand;
                }
                else if (char.IsLetter(ch))
                {
                    operandsStack.Push(new Variable(ch));
                    lastType = CharType.Operand;
                }
            }
            
            while (operationsStack.Count > 0)
            {
                var headOp = operationsStack.Pop();
                if (operandsStack.Count < 2)
                    throw new IncorrectSyntaxException(
                        "Not enough operands for operation " + headOp
                    );
                var secondOperand = operandsStack.Pop();
                var firstOperand = operandsStack.Pop();
                var res = new BinaryExpression(firstOperand, secondOperand, headOp);
                operandsStack.Push(res);
            }

            if (operandsStack.Count != 1)
                throw new IncorrectSyntaxException(
                    "There are few operands without operations"
                );
            return operandsStack.Pop();
        }
    }
}