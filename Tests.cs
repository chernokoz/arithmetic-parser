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
    }

}