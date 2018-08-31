using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BibleModel;
using BibleStudy;
using Newtonsoft.Json;
using ScriptureReferenceParser;

namespace BibleReaderApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string json;
            using (var reader = new StreamReader(@"C:\Users\ajw19\Documents\Bible Study\Bookmarks.json"))
            {
                json = reader.ReadToEnd();
            }

            var bookMarksData = JsonConvert.DeserializeObject<BibleReaderBookMarksData>(json);

            json = JsonConvert.SerializeObject(bookMarksData);

            var parser = new BibleReferenceParser();
            IEnumerable<BookData> books;
            using (var stream = new StreamReader(@"Data\BookDataList.json"))
            {
                var booksJson = stream.ReadLine();
                books = JsonConvert.DeserializeObject<List<BookData>>(booksJson);
            }

            var processor = new BibleReaderBookMarkProcessor(parser, books);

            var currentPosition = processor.GetCurrentPosition(bookMarksData);

            Console.WriteLine(currentPosition);

            while (true)
            {
                var key = Console.ReadKey();
                Console.Write(": ");

                switch (key.Key.ToString())
                {
                    case "N":
                        bookMarksData = processor.AdvanceToNext(bookMarksData);
                        currentPosition = processor.GetCurrentPosition(bookMarksData);
                        Console.WriteLine(currentPosition);
                        break;
                }
            }
        }
    }
}
