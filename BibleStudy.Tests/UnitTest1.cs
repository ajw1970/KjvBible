using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BibleStudy.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void CanGetExpectedCurrentChapter()
        {
            BibleStudyManager bibleStudyManager = new MockBibleStudyManager();
            var current = bibleStudyManager.GetCurrentChapter(string.Empty);
            Assert.AreEqual("Ruth 1", current.ToString());
        }
    }
}
