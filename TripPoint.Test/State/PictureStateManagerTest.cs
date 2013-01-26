using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using TripPoint.WindowsPhone.State;

namespace TripPoint.Test.State
{
    [TestClass]
    public class PictureStateManagerTest
    {
        [TestMethod]
        public void TestGetParentDirectoryPath()
        {
            string path = "Directory A/Directory B/Directory C";

            string parentDirectoryPath = PictureStateManager.GetParentDirectoryPath(path);

            Assert.AreEqual<string>("Directory A/Directory B", parentDirectoryPath,
                "Expected path does not match the actual one");
        }

        [TestMethod]
        public void TestGetParentDirectoryPathNoParentDirectory()
        {
            string path = "Directory A";

            Assert.IsTrue(String.IsNullOrWhiteSpace(PictureStateManager.GetParentDirectoryPath(path)),
                "Parent directory path must be empty");
        }

        [TestMethod]
        public void TestGetParentDirectoryPathNoDirectoryPath()
        {
            Assert.IsTrue(String.IsNullOrWhiteSpace(PictureStateManager.GetParentDirectoryPath("")),
                "Parent directory path must be empty for empty path");

            Assert.IsNull(PictureStateManager.GetParentDirectoryPath(null),
                "Parent directory path must be null for null path");
        }
    }
}
