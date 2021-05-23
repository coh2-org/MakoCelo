using NUnit.Framework;

namespace MakoCelo.UnitTests.Scanner
{
    [TestFixture]
    public class UtilitiesTests
    {
        [Test]
        public void FindPlayerNameInLine()
        {
            MakoCelo.Scanner.Utilities.FindPlayerNameInLine("a", 0);


            // TODO: Add your test code here
            var answer = 42;
            Assert.That(answer, Is.EqualTo(42), "Some useful error message");
        }
    }
}
