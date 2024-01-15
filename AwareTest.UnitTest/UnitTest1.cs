using AwareTest.Model;
using AwareTest.Model.Model;
using AwareTest.Services;
using AwareTest.Services.IService;
using AwareTest.Services.Services;
using System.Diagnostics;
using System.Collections.Generic;

namespace AwareTest.UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        private readonly ISortStringService _sortString = new SortStringService();
        [TestMethod]
        public void TestMethod1()
        {
            // Arrange
            string str = "A,B,111,CCC,DDD";
            var expectedResult = new List<SortStringModel>
            {
                new SortStringModel { Rank = "A" },
                new SortStringModel { Rank = "B" },
                new SortStringModel { Rank = "CCC" },
                new SortStringModel { Rank = "DDD" },
                new SortStringModel { Rank = "111" }
            };

            string str2 = "AAAA,AAAA,BAAA,,111,2,CCC,DDD";
            var expectedResult2 = new List<SortStringModel>
            {
                new SortStringModel { Rank = "AAAA" },
                new SortStringModel { Rank = "BAAA" },
                new SortStringModel { Rank = "CCC" },
                new SortStringModel { Rank = "DDD" },
                new SortStringModel { Rank = "2" },
                new SortStringModel { Rank = "111" }
            };

            string str3 = "&&%%%#,,,,,";
            var expectedResult3 = new List<SortStringModel>();

            // Act
            var actualResult = _sortString.GetSortString(str);
            var actualResult2 = _sortString.GetSortString(str2);
            var actualResult3 = _sortString.GetSortString(str3);

            // Assert
            foreach (var actual in actualResult.Select((value, i) => new { i, value }))
            {
                var expect = expectedResult[actual.i];
                Assert.AreEqual(expect.Rank, actual.value.Rank);
            }
            foreach (var actual in actualResult2.Select((value, i) => new { i, value }))
            {
                var expect = expectedResult2[actual.i];
                Assert.AreEqual(expect.Rank, actual.value.Rank);
            }
            foreach (var actual in actualResult3.Select((value, i) => new { i, value }))
            {
                var expect = expectedResult3[actual.i];
                Assert.AreEqual(expect.Rank, actual.value.Rank);
            }
        }
    }
}