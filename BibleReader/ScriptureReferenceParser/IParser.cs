namespace ScriptureReferenceParser
{
    public interface IParser
    {
        (string First, string Last) ParseBookRange(string bookRange);
        (string Book, int Chapter) ParseChapter(string chapterReference);
    }
}