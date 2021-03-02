using System;
using NUnit.Framework;

namespace idedev2021
{
    public class TestWithoutBrackets
    {
        [Test]
        public void TestAdd()
        {
            var dumpVisitor = new DumpVisitor();
            SimpleParser.Parse("1+2").Accept(dumpVisitor);
            Assert.AreEqual("Binary(Literal(1)+Literal(2))", dumpVisitor.ToString());
            
            Assert.Pass();
        }
        
        [Test]
        public void TestMinus()
        {
            var dumpVisitor = new DumpVisitor();
            SimpleParser.Parse("9-3").Accept(dumpVisitor);
            Assert.AreEqual("Binary(Literal(9)-Literal(3))", dumpVisitor.ToString());
            
            Assert.Pass();
        }
        
        [Test]
        public void TestMinusDivide()
        {
            var dumpVisitor = new DumpVisitor();
            SimpleParser.Parse("5-5/5").Accept(dumpVisitor);
            Assert.AreEqual("Binary(Literal(5)-Binary(Literal(5)/Literal(5)))", dumpVisitor.ToString());
            
            Assert.Pass();
        }
        
        [Test]
        public void TestAddAdd()
        {
            var dumpVisitor = new DumpVisitor();
            SimpleParser.Parse("1+2+3").Accept(dumpVisitor);
            Assert.AreEqual("Binary(Binary(Literal(1)+Literal(2))+Literal(3))", dumpVisitor.ToString());
            
            Assert.Pass();
        }
        
        [Test]
        public void TestAddMinusAdd()
        {
            var dumpVisitor = new DumpVisitor();
            SimpleParser.Parse("1+2-3+4").Accept(dumpVisitor);
            Assert.AreEqual("Binary(Binary(Binary(Literal(1)+Literal(2))-Literal(3))+Literal(4))", dumpVisitor.ToString());
            
            Assert.Pass();
        }
        
        [Test]
        public void TestHardCombination()
        {
            var dumpVisitor = new DumpVisitor();
            SimpleParser.Parse("1*2+3*4*5").Accept(dumpVisitor);
            Assert.AreEqual("Binary(Binary(Literal(1)*Literal(2))+Binary(Binary(Literal(3)*Literal(4))*Literal(5)))", dumpVisitor.ToString());
            Assert.Pass();
        }
        
    }

    public class TestBracketsLogic
    {
        [Test]
        public void TestAdd()
        {
            var dumpVisitor = new DumpVisitor();
            SimpleParser.Parse("1+(2+3)").Accept(dumpVisitor);
            Assert.AreEqual("Binary(Literal(1)+Paren(Binary(Literal(2)+Literal(3))))", dumpVisitor.ToString());
            Assert.Pass();
        }
        
        [Test]
        public void TestAddMultPriority()
        {
            var dumpVisitor = new DumpVisitor();
            SimpleParser.Parse("1*(2+3)").Accept(dumpVisitor);
            Assert.AreEqual("Binary(Literal(1)*Paren(Binary(Literal(2)+Literal(3))))", dumpVisitor.ToString());
            Assert.Pass();
        }
        
        [Test]
        public void TestRightPriority()
        {
            var dumpVisitor = new DumpVisitor();
            SimpleParser.Parse("1+(3+(5+7))").Accept(dumpVisitor);
            Assert.AreEqual("Binary(Literal(1)+Paren(Binary(Literal(3)+Paren(Binary(Literal(5)+Literal(7))))))", dumpVisitor.ToString());
            Assert.Pass();
        }
        
        [Test]
        public void TestNestedBracketsBeforeLiteral()
        {
            var dumpVisitor = new DumpVisitor();
            SimpleParser.Parse("(1+(2+3))*2").Accept(dumpVisitor);
            Assert.AreEqual(
                "Binary(Paren(Binary(Literal(1)+Paren(Binary(Literal(2)+Literal(3)))))*Literal(2))",
                dumpVisitor.ToString());
            Assert.Pass();
        }
        
        [Test]
        public void TestNestedBracketsAfterLiteral()
        {
            var dumpVisitor = new DumpVisitor();
            SimpleParser.Parse("2*(1/2+(2+3))").Accept(dumpVisitor);
            Assert.AreEqual(
                "Binary(Literal(2)" +
                "*Paren(Binary(Binary(Literal(1)/Literal(2))+Paren(Binary(Literal(2)+Literal(3))))))",
                
                dumpVisitor.ToString());
            Assert.Pass();
        }
        
        [Test]
        public void TestHardCombination()
        {
            var dumpVisitor = new DumpVisitor();
            SimpleParser.Parse("1+((2+3)/4+7)*5").Accept(dumpVisitor);
            Console.Write(dumpVisitor.ToString());
            Assert.AreEqual(
                "Binary(Literal(1)+" +
                "Binary(Paren(Binary(Binary(Paren(Binary(Literal(2)+Literal(3)))/Literal(4))+Literal(7)))*Literal(5)))",
                dumpVisitor.ToString());
            Assert.Pass();
        }
    }

    public class IncorrectSyntax
    {
        [Test]
        public void TestTwoOperandsWithoutOperations()
        {
            Assert.Throws<IncorrectSyntaxException>(
                () => SimpleParser.Parse("12")
            );
        }
        
        [Test]
        public void TestOperationsWithoutNeededOperands()
        {
            Assert.Throws<IncorrectSyntaxException>(
                () => SimpleParser.Parse("1+")
            );
        }
        
        [Test]
        public void NonOpenedBrackets()
        {
            Assert.Throws<IncorrectSyntaxException>(
                () => SimpleParser.Parse("1+2*3)")
            );
        }
        
        [Test]
        public void TestTwoOperands()
        {
            Assert.Throws<IncorrectSyntaxException>(
                () => SimpleParser.Parse("1+*12")
            );
        }
        
        [Test]
        public void TestBracketsAfterLiteral()
        {
            Assert.Throws<IncorrectSyntaxException>(
                () => SimpleParser.Parse("1(1+5)")
            );
        }
        
        [Test]
        public void TestEmptyBrackets()
        {
            Assert.Throws<IncorrectSyntaxException>(
                () => SimpleParser.Parse("1*(1+5/())")
            );
        }
        
        [Test]
        public void TestUnclosedBrackets()
        {
            Assert.Throws<IncorrectSyntaxException>(
                () => SimpleParser.Parse("1+2/(3-4")
            );
        }
        
        [Test]
        public void TestUnopenedBracket()
        {
            Assert.Throws<IncorrectSyntaxException>(
                () => SimpleParser.Parse("(((a))))+5")
            );
        }
    }

}