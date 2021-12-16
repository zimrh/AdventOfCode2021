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
            var segmentCount = new Dictionary<char, int> {{'a',0},{'b',0},{'c',0},{'d',0},{'e',0},{'f',0},{'g',0}};

            foreach (var input in inputs)
            {
                // Put in order for readability
                var segment = string.Concat(input.OrderBy(c => c));

                foreach (var segmentLight in segment)
                {
                    segmentCount[segmentLight]++;
                }

                var len = segment.Length;

                switch (len)
                {
                    case 2: // 1 Segment
                        knownNumbers[1] = segment;
                        break;
                    case 4: // 4 Segment
                        knownNumbers[4] = segment;
                        break;
                    case 3: // 7 Segment
                        knownNumbers[7] = segment;
                        break;
                    case 7: // 8 Segment
                        knownNumbers[8] = segment;
                        break;
                }
            }

            var topSegment = knownNumbers[7];
            foreach (var lightSegment in knownNumbers[1])
            {
                topSegment = topSegment.Replace(lightSegment.ToString(), "");
            }
            TopSegment = topSegment[0];

            foreach (var (key, count) in segmentCount)
            {
                switch (count)
                {
                    case 4:
                        BottomLeftSegment = key;
                        break;

                    case 6:
                        TopLeftSegment = key;
                        break;

                    case 8:
                        if (key != TopSegment)
                        {
                            TopRightSegment = key;
                        }
                        break;

                    case 9:
                        BottomRightSegment = key;
                        break;
                }
            }

            var middleSegment = knownNumbers[4]
                .Replace(TopLeftSegment.ToString(),"")
                .Replace(TopRightSegment.ToString(), "")
                .Replace(BottomRightSegment.ToString(), "");

            MiddleSegment = middleSegment[0];

            var bottomSegment = knownNumbers[8]
                .Replace(BottomLeftSegment.ToString(), "")
                .Replace(BottomRightSegment.ToString(), "")
                .Replace(MiddleSegment.ToString(), "")
                .Replace(TopLeftSegment.ToString(), "")
                .Replace(TopRightSegment.ToString(), "")
                .Replace(TopSegment.ToString(), "");

            BottomSegment = bottomSegment[0];

            knownNumbers[0] = string.Concat(TopSegment, TopRightSegment, TopLeftSegment,
                                            BottomLeftSegment, BottomRightSegment, BottomSegment);

            knownNumbers[2] = string.Concat(TopSegment, TopRightSegment, MiddleSegment,
                                            BottomLeftSegment, BottomSegment);

            knownNumbers[3] = string.Concat(TopSegment, TopRightSegment, MiddleSegment,
                                            BottomRightSegment, BottomSegment);
            
            knownNumbers[5] = string.Concat(TopSegment, TopLeftSegment, MiddleSegment,
                                            BottomRightSegment, BottomSegment);

            knownNumbers[6] = string.Concat(TopSegment, TopLeftSegment, MiddleSegment,
                                            BottomLeftSegment, BottomRightSegment, BottomSegment);

            knownNumbers[9] = string.Concat(TopSegment, TopRightSegment, TopLeftSegment, MiddleSegment,
                                            BottomRightSegment, BottomSegment);

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
        public char BottomLeftSegment;
        public char BottomRightSegment;
        public char BottomSegment;
        public char MiddleSegment;
        public char TopLeftSegment;
        public char TopRightSegment;
        public char TopSegment;
    }
}
