using BibleModel;
using System;
namespace BibleStudy
{
    public interface BibleStudyManager
    {
        ReadingChapter CurrentChapter { get; }
        ReadingChapter GetNextChapter();
    }
}
