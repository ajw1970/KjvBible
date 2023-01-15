using System;
using System.Collections.Generic;
using System.IO;
using BibleModel;
using BibleStudy;
using Newtonsoft.Json;
using ScriptureReferenceParser;

namespace BibleReaderApp
{
    class Program
    {
        private static string dataFileName = @"C:\Users\ajw19\OneDrive - Welty Automation\Documents\Bible Study\Bookmarks.json";

        static void Main(string[] args)
        {
            var bookMarksData = GetData();

            var parser = new BibleReferenceParser();
            IEnumerable<BookData> books;
            using (var stream = new StreamReader(@"Data\BookDataList.json"))
            {
                var booksJson = stream.ReadLine();
                books = JsonConvert.DeserializeObject<List<BookData>>(booksJson);
            }

            var processor = new BibleReaderBookMarkProcessor(parser, books);

            var currentPosition = processor.GetCurrentPosition(bookMarksData);

            DisplayCurrentAndPromptForLaunch(currentPosition);

            while (true)
            {
                var key = Console.ReadKey();
                Console.Write(": ");

                switch (key.Key.ToString())
                {
                    case "N":
                        bookMarksData = processor.AdvanceToNext(bookMarksData);
                        currentPosition = processor.GetCurrentPosition(bookMarksData);
                        SaveData(bookMarksData);
                        Console.WriteLine();

                        DisplayCurrentAndPromptForLaunch(currentPosition);

                        break;
                }
            }
        }

        private static void DisplayCurrentAndPromptForLaunch(string currentPosition)
        {
            Console.WriteLine($"Currently at: {currentPosition}");
            Console.Write("Launch in browser? ");

            var response = Console.ReadKey();
            if (response.Key.ToString().Equals("Y"))
            {
                var currentPositionArg = currentPosition.Replace(' ', '+');
                var process = new System.Diagnostics.Process();
                process.StartInfo.FileName = $"https://www.biblegateway.com/passage/?search={currentPositionArg}&version=KJV";
                process.Start();
            }

            Console.WriteLine();
            Console.WriteLine();

            Console.Write("Press N to move to next bookmark: ");
        }

        private static BibleReaderBookMarksData GetData()
        {
            string json;

            using (var reader = new StreamReader(dataFileName))
            {
                json = reader.ReadToEnd();
            }

            return JsonConvert.DeserializeObject<BibleReaderBookMarksData>(json);
        }

        private  static void SaveData(BibleReaderBookMarksData data)
        {
            using (var writer = new StreamWriter(dataFileName))
            {
                var json = JsonConvert.SerializeObject(data, Formatting.Indented);
                writer.Write(json);
            }
        }
    }
}
