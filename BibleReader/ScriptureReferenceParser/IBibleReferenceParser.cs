namespace ScriptureReferenceParser
{
    public interface IBibleReferenceParser
    {
        (string First, string Last) ParseBookRange(string bookRange);
        (string Book, int Chapter) ParseChapter(string chapterReference);
    }
}