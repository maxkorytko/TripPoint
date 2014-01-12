using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using TripPoint.WindowsPhone.State;

namespace TripPoint.Test.State
{
    [TestClass]
    public class PictureStoreTest
    {
        [TestMethod]
        public void TestGetParentDirectoryPath()
        {
            string path = "Directory A/Directory B/Directory C";

            string parentDirectoryPath = PictureStore.GetParentDirectoryPath(path);

            Assert.AreEqual<string>("Directory A/Directory B", parentDirectoryPath,
                "Expected path does not match the actual one");
        }

        [TestMethod]
        public void TestGetParentDirectoryPathNoParentDirectory()
        {
            string path = "Directory A";

            Assert.IsTrue(String.IsNullOrWhiteSpace(PictureStore.GetParentDirectoryPath(path)),
                "Parent directory path must be empty");
        }

        [TestMethod]
        public void TestGetParentDirectoryPathNoDirectoryPath()
        {
            Assert.IsTrue(String.IsNullOrWhiteSpace(PictureStore.GetParentDirectoryPath("")),
                "Parent directory path must be empty for empty path");

            Assert.IsNull(PictureStore.GetParentDirectoryPath(null),
                "Parent directory path must be null for null path");
        }
    }
}
