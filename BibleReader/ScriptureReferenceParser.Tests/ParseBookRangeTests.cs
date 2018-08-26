using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace ScriptureReferenceParser.Tests
{
    public class ParseChapterTests
    {
        [Fact]
        public void CanParseChapter()
        {
            var parser = new BibleReferenceParser();
            parser.ParseChapter("Ex 38")
                .Should().Be(("Ex", 38));
        }

        [Fact]
        public void CanParseChapterWithMultipleSpaces()
        {
            var parser = new BibleReferenceParser();
            parser.ParseChapter("2 Peter 2")
                .Should().Be(("2 Peter", 2));
        }

        [Fact]
        public void CanParseChapterWithTrailingSpace()
        {
            var parser = new BibleReferenceParser();
            parser.ParseChapter("1 John 2 ")
                .Should().Be(("1 John", 2));
        }

        [Fact]
        public void NoSpaceShouldThrowException()
        {
            var parser = new BibleReferenceParser();
            parser.Invoking(p => p.ParseChapter("Psalm50"))
                .ShouldThrow<ArgumentException>()
                .WithMessage("Expecting a space between book and chapterReference\r\nParameter name: chapterReference");
        }

        [Fact]
        public void NoChapterNumberShouldThrowException()
        {
            var parser = new BibleReferenceParser();
            parser.Invoking(p => p.ParseChapter("Psalm fifty"))
                .ShouldThrow<ArgumentException>()
                .WithMessage("Expecting a whole number after book name\r\nParameter name: chapterReference");
        }
    }

    public class ParseBookRangeTests
    {
        [Fact]
        public void CanParseStringWithHyphenatedSetOfBooks()
        {
            var parser = new BibleReferenceParser();
            parser.ParseBookRange("Gen-Deut")
                .Should().Be(("Gen","Deut"));
        }

        [Fact]
        public void CanParseStringWithSingleBook()
        {
            var parser = new BibleReferenceParser();
            parser.ParseBookRange("Psalm")
                .Should().Be(("Psalm", ""));
        }

        [Fact]
        public void MoreThanTwoThrowsException()
        {
            var parser = new BibleReferenceParser();
            parser.Invoking(p => p.ParseBookRange("One-Two-Three"))
                .ShouldThrow<ArgumentException>()
                .WithMessage("Expecting hyphenated set of books: \"first-last\"\r\nParameter name: bookRange");
        }
    }
}
