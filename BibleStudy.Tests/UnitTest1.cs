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
            BibleStudyManager bibleStudyManager = MockBibleStudyManager.Instance;
            var current = bibleStudyManager.CurrentChapter;
            Assert.AreEqual("1 Corinthians 5", current.ToString());
        }
    }
}
