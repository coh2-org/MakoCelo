using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace MakoCelo.UnitTests.Scanner
{
    [TestFixture]
    public class UtilitiesTests
    {
        [TestCaseSource(nameof(GetFindPlayerNameInLineTestCases))]
        public void FindPlayerNameInLine_CorrectLinesFromLog_PlayerNameFound(string line, string expectedResult)
        {
            //Arrange
            //Act
            var result = MakoCelo.Scanner.Utilities.FindPlayerNameInLine(line, 39);
            //Assert
            Assert.AreEqual(expectedResult, result);
        }

        public static IEnumerable<object> GetFindPlayerNameInLineTestCases()
        {
            var lines =TestCases.testcases.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in lines)
            {
                var result = line.Split(';');
                yield return new string[] { result[0], result[1] };
            }
        }

        [TestCaseSource(nameof(GetFindPlayerRelicIdInLineTestCases))]
        public void FindPlayerRelicIdInLine_CorrectLinesFromLog_RelicIdFound(string line, string expectedResult)
        {
            //Arrange
            //Act
            var result = MakoCelo.Scanner.Utilities.FindPlayerRelicIdInLine(line);
            //Assert
            Assert.AreEqual(expectedResult, result);
        }

        public static IEnumerable<object> GetFindPlayerRelicIdInLineTestCases()
        {
            var lines = TestCases.testcases.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in lines)
            {
                var result = line.Split(';');
                yield return new string[] { result[0], result[2] };
            }
        }

    }
}
