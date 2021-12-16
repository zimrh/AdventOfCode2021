using System;
using System.IO;
using System.Linq;

namespace Day8
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var displayDataFilePath = "segments.txt";
            var easyNumbersCount = Get1478Count(displayDataFilePath);
            Console.WriteLine($"Easy Number Count: {easyNumbersCount}");
        }

        private static int Get1478Count(string displayDataFilePath)
        {
            var numbersCount = 0;

            using var displayDataFile = File.OpenText(displayDataFilePath);
            do
            {
                var line = displayDataFile.ReadLine();
                var displays = line.Split("|");
                var segments = displays[1].Split(" ");
                foreach (var segment in segments)
                {
                    var len = segment.Length;
                    if (len == 2 // 1 Segment
                        || len == 4 // 4 Segment
                        || len == 3 // 7 Segment
                        || len == 7) // 8 Segment
                    {
                        numbersCount++;
                    }
                }
            } while (!displayDataFile.EndOfStream);

            return numbersCount;
        }
    }
}
