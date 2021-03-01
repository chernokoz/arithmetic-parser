using NUnit.Framework;

namespace idedev2021
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

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
        public void TestPriorityAddMultyply()
        {
            var dumpVisitor = new DumpVisitor();
            SimpleParser.Parse("1+2*3").Accept(dumpVisitor);
            Assert.AreEqual("Binary(Literal(1)+Binary(Literal(2)*Literal(3)))", dumpVisitor.ToString());
            
            Assert.Pass();
        }
    }
}