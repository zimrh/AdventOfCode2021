using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventDay6
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var fishFilePath = "fish.txt";
            var lanternFishAfter80DaysCount = GetLanternFishCount(80, fishFilePath);
            Console.WriteLine(lanternFishAfter80DaysCount);
            var lanternFishAfter256DaysCount = GetLanternFishCount(256, fishFilePath);
            Console.WriteLine(lanternFishAfter256DaysCount);
        }

        private static double GetLanternFishCount(int days, string fishFilePath)
        {
            const int newFishInitialValue = 8;
            const int existingFishRefreshValue = 6;

            var fishes = new double[newFishInitialValue + 1]; 
            var fishInput = File.ReadAllText(fishFilePath);

            foreach (var seedFish in fishInput.Split(","))
            {
                var fishValue = int.Parse(seedFish);
                fishes[fishValue]++;
            }

            for (var day = 0; day < days; day++)
            {
                Console.WriteLine($"DAY: {day} - Starting");
                var fishToAdd = fishes[0];
                var fishToRefresh = fishes[0];
                for (var fishLevel = 0; fishLevel < newFishInitialValue; fishLevel++)
                {
                    fishes[fishLevel] = fishes[fishLevel + 1];
                }

                fishes[newFishInitialValue] = fishToAdd;
                fishes[existingFishRefreshValue] += fishToRefresh;

                var totalFish = fishes.Sum();
                Console.WriteLine($"DAY: {day} TOTAL: {totalFish}");
            }

            return fishes.Sum();
        }
    }
}
