using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using TripPoint.WindowsPhone.Utils;

namespace TripPoint.Test.Utils
{
    [TestClass]
    public class CollectionPaginatorTest
    {
        private IEnumerable<int> GenerateIntDataSet(int itemsCount)
        {
            var collection = new List<int>(itemsCount);

            for (int i = 1; i <= itemsCount; i++)
            {
                collection.Add(i);
            }

            return collection;
        }

        [TestMethod]
        public void TestPaginateAllPages()
        {
            var paginator = new CollectionPaginator<int>(GenerateIntDataSet(10));
            paginator.PageSize = 3;

            var result = paginator.Paginate();
            Assert.AreEqual<int>(3, result.Count(), "The result must contain 3 items");

            result = result.Concat(paginator.Paginate()).ToList();
            Assert.AreEqual<int>(6, result.Count(), "The result must contain 6 items");

            result = result.Concat(paginator.Paginate()).ToList();
            Assert.AreEqual<int>(9, result.Count(), "The result must contain 9 items");

            result = result.Concat(paginator.Paginate()).ToList();
            Assert.AreEqual<int>(10, result.Count(), "The result must contain 10 items");

            Assert.IsFalse(paginator.CanPaginate, "Paginator must stop paginating");
        }

        [TestMethod]
        public void TestPaginateSomePages()
        {
            var paginator = new CollectionPaginator<int>(GenerateIntDataSet(5));
            paginator.PageSize = 2;

            paginator.Paginate();

            Assert.IsTrue(paginator.CanPaginate, "Paginator has not paginated through the data set yet");
        }

        [TestMethod]
        public void TestPaginateItemsOrderIsUnchanged()
        {
            var dataset = new List<string>();
            dataset.Add("yesterday");
            dataset.Add("today");
            dataset.Add("tomorrow");
            dataset.Add("the day after tomorrow");
            dataset.Add("next week");

            var paginator = new CollectionPaginator<string>(dataset);
            paginator.PageSize = 3;

            var result = new List<string>(paginator.Paginate());
            result = result.Concat(paginator.Paginate()).ToList();

            bool success = true;
            if (!result[0].Equals("yesterday")) success = false;
            if (!result[2].Equals("tomorrow")) success = false;
            if (!result[4].Equals("next week")) success = false;

            Assert.IsTrue(success, "The items have been re-ordered in the paginated collection");
        }

        [TestMethod]
        public void TestPaginatorThrowsExceptionUnlessPageSizeIsSet()
        {
            var paginator = new CollectionPaginator<int>(GenerateIntDataSet(1));

            var success = false;

            try
            {
                paginator.Paginate();
            }
            catch (InvalidOperationException)
            {
                success = true;
            }

            Assert.IsTrue(success, "Paginator must throw an exception when page size is zero");
        }

        [TestMethod]
        public void TestPaginateThroughEmptyDataset()
        {
            var paginator = new CollectionPaginator<int>(GenerateIntDataSet(0));
            paginator.PageSize = 10;

            Assert.IsFalse(paginator.CanPaginate, "Paginator must not paginate through empty data set");
            Assert.AreEqual<int>(0, paginator.Paginate().Count(), "Paginator must return empty collection");
        }
    }
}
