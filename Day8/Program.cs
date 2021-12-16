using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Day8
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var displayDataFilePath = "segments.txt";
            var easyNumbersCount = Get1478Count(displayDataFilePath);
            Console.WriteLine($"Easy Number Count: {easyNumbersCount}");
            var numbersTotal = GetNumbersTotal(displayDataFilePath);
            Console.WriteLine($"Numbers Total: {numbersTotal}");
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

            displayDataFile.Close();

            return numbersCount;
        }

        private static int GetNumbersTotal(string displayDataFilePath)
        {
            var numbersTotal = 0;

            using var displayDataFile = File.OpenText(displayDataFilePath);

            do
            {
                var line = displayDataFile.ReadLine();
                var inputs = line.Split("|").First().Trim();
                var decoder = new SegmentDecoder(inputs);

                var outputs = line.Split("|").Last().Trim();

                Console.WriteLine($"outputs: {outputs}");
                var number = "";
                foreach (var output in outputs.Split(" ", StringSplitOptions.RemoveEmptyEntries))
                {
                    number = string.Concat(number, decoder.GetNumbersFromCode(output));
                }
                Console.WriteLine($"Number: {number}");

                numbersTotal += int.Parse(number);

                Console.WriteLine($"Total: {numbersTotal}");

            } while (!displayDataFile.EndOfStream);

            displayDataFile.Close();

            return numbersTotal;
        }

    }

    internal class SegmentDecoder
    {
        public SegmentDecoder(string inputsRaw)
        {
            var inputs = inputsRaw.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            var knownNumbers = new string[10];
            var segmentCount = new Dictionary<string, int> { { "a", 0 }, { "b", 0 }, { "c", 0 }, { "d", 0 }, { "e", 0 }, { "f", 0 }, { "g", 0 } };

            foreach (var input in inputs)
            {
                // Put in order for readability
                var segment = string.Concat(input.OrderBy(c => c));

                foreach (var segmentLight in segment)
                {
                    segmentCount[segmentLight.ToString()]++;
                }

                var lenToKnownNumber = new Dictionary<int, int>
                {
                    {2, 1}, // 2 Len = 1 Segment
                    {4, 4}, // 4 Len = 4 Segment
                    {3, 7}, // 3 Len = 7 Segment
                    {7, 8}  // 7 Len = 8 Segment
                };

                if (lenToKnownNumber.ContainsKey(segment.Length))
                {
                    knownNumbers[lenToKnownNumber[segment.Length]] = segment;
                }
            }

            Top = knownNumbers[7];
            foreach (var lightSegment in knownNumbers[1])
            {
                Top = Top.Replace(lightSegment.ToString(), "");
            }

            foreach (var (key, count) in segmentCount)
            {
                switch (count)
                {
                    case 4: BottomLeft = key; break;
                    case 6: TopLeft = key; break;
                    case 8: 
                        if (key != Top)
                        {
                            TopRight = key;
                        }
                        break;
                    case 9: BottomRight = key; break;
                }
            }

            Middle = knownNumbers[4]
                .Replace(TopLeft, "")
                .Replace(TopRight, "")
                .Replace(BottomRight, "");

            Bottom = knownNumbers[8]
                .Replace(BottomLeft, "")
                .Replace(BottomRight, "")
                .Replace(Middle, "")
                .Replace(TopLeft, "")
                .Replace(TopRight, "")
                .Replace(Top, "");

            knownNumbers[0] = string.Concat(Top, TopRight, TopLeft,
                                            BottomLeft, BottomRight, Bottom);

            knownNumbers[2] = string.Concat(Top, TopRight, Middle,
                                            BottomLeft, Bottom);

            knownNumbers[3] = string.Concat(Top, TopRight, Middle,
                                            BottomRight, Bottom);

            knownNumbers[5] = string.Concat(Top, TopLeft, Middle,
                                            BottomRight, Bottom);

            knownNumbers[6] = string.Concat(Top, TopLeft, Middle,
                                            BottomLeft, BottomRight, Bottom);

            knownNumbers[9] = string.Concat(Top, TopRight, TopLeft, 
                                            Middle, BottomRight, Bottom);

            for (var i = 0; i < knownNumbers.Length; i++)
            {
                NumberLookupDictionary.Add(string.Concat(knownNumbers[i].OrderBy(c => c)), i);
            }

        }

        public int GetNumbersFromCode(string code)
        {
            code = string.Concat(code.OrderBy(c => c));
            return NumberLookupDictionary[code];
        }

        public Dictionary<string, int> NumberLookupDictionary = new Dictionary<string, int>();
        public string BottomLeft;
        public string BottomRight;
        public string Bottom;
        public string Middle;
        public string TopLeft;
        public string TopRight;
        public string Top;
    }
}
