using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventDay3
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var diagnosticDataFilePath = "diagnostic-data.txt";
            var powerConsumption = GetPowerConsumption(diagnosticDataFilePath);
            Console.WriteLine($"Power Consumption: {powerConsumption}");
            var lifeSupportRating = GetLifeSupportRating(diagnosticDataFilePath);
            Console.WriteLine($"Life Support Rating: {lifeSupportRating}");
        }

        private static int GetPowerConsumption(string diagnosticDataFilePath)
        {
            var diagnosticEntriesCount = 0;
            var bitCount = new Dictionary<int, int>();
            var diagnosticDataFile = File.OpenText(diagnosticDataFilePath);
            do
            {
                var diagnosticEntry = diagnosticDataFile.ReadLine() ?? "";
                diagnosticEntriesCount++;

                for (var i = 0; i < diagnosticEntry.Length; i++)
                {
                    if (Equals(diagnosticEntry[i], '1'))
                    {
                        if (bitCount.ContainsKey(i))
                        {
                            bitCount[i]++;
                        }
                        else
                        {
                            bitCount.Add(i, 1);
                        }
                    }
                }

            } while (!diagnosticDataFile.EndOfStream);

            var gammaRate = "";
            var epsilonRate = "";

            for (var i = 0; i < bitCount.Count; i++)
            {
                var c = (double)decimal.Divide(bitCount[i], diagnosticEntriesCount);

                if (c > 0.5)
                {
                    gammaRate += "1";
                    epsilonRate += "0";
                }
                else
                {
                    gammaRate += "0";
                    epsilonRate += "1";
                }
            }

            var gammaValue = Convert.ToInt32(gammaRate, 2);
            var epsilonValue = Convert.ToInt32(epsilonRate, 2);

            return gammaValue * epsilonValue;
        }

        private static int GetLifeSupportRating(string diagnosticDataFilePath)
        {
            var diagnosticDataFile = File.OpenText(diagnosticDataFilePath);
            var diagnosticEntries = new List<string>();
            do
            {
                var diagnosticEntry = diagnosticDataFile.ReadLine() ?? "";
                diagnosticEntries.Add(diagnosticEntry);
            } while (!diagnosticDataFile.EndOfStream);

            var oxygenRating = GetOxygenRating(diagnosticEntries);
            var co2ScrubbingRate = GetCo2ScrubbingRate(diagnosticEntries);
            return oxygenRating * co2ScrubbingRate;
        }

        private static int GetOxygenRating(List<string> diagnosticEntries)
        {
            var oxygenRating = FilterByBitPosition(diagnosticEntries, 0);
            return Convert.ToInt32(oxygenRating, 2);
        }

        private static int GetCo2ScrubbingRate(List<string> diagnosticEntries)
        {
            var co2ScrubbingRate = FilterByBitPosition(diagnosticEntries, 0, false);
            return Convert.ToInt32(co2ScrubbingRate, 2);
        }

        private static string FilterByBitPosition(IReadOnlyCollection<string> diagnosticEntries, int bitPos, bool filterByMostCommonBit = true)
        {
            if (diagnosticEntries.Count == 1)
            {
                return diagnosticEntries.FirstOrDefault();
            }

            var bitStateOnCount = 0;
            var totalCount = 0;

            //get bit counts
            foreach (var diagnosticEntry in diagnosticEntries)
            {
                if (diagnosticEntry[bitPos] == '1')
                {
                    bitStateOnCount++;
                }
                totalCount++;
            }

            //filter and pass down
            var onBitRatio = (double) decimal.Divide(bitStateOnCount, totalCount);
            var commonBit = '1';

            if (filterByMostCommonBit)
            {
                commonBit = onBitRatio >= 0.5 ? '1' : '0';
            }
            else
            {
                commonBit = onBitRatio >= 0.5 ? '0' : '1';
            }


            var newEntries = diagnosticEntries.Where(diagnosticEntry => diagnosticEntry[bitPos] == commonBit).ToList();

            return FilterByBitPosition(newEntries, ++bitPos, filterByMostCommonBit);
        }
    }
}
