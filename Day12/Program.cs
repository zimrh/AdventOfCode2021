using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace Day12
{
    public class Program
    {
        public static void Main(string[] args)
        {
            const string smallCaveTestFilePath = "cave-map-small.txt";
            const string mediumCaveTestFilePath = "cave-map-medium.txt";
            const string largeCaveTestFilePath = "cave-map-large.txt";
            var caveTestFilePath = "actual-cave-map.txt";

            var partTwoSmallCavePathCount = GetPathCountsWithOneRevisit(smallCaveTestFilePath);
            Console.WriteLine($"Part Two: Small Cave Path Count: {partTwoSmallCavePathCount}");
            
            var partTwoMediumCavePathCount = GetPathCountsWithOneRevisit(mediumCaveTestFilePath);
            Console.WriteLine($"Part Two: Medium Cave Path Count: {partTwoMediumCavePathCount}");

            var partTwoLargeCavePathCount = GetPathCountsWithOneRevisit(largeCaveTestFilePath);
            Console.WriteLine($"Part Two: Large Cave Path Count: {partTwoLargeCavePathCount}");
            
            var partTwoActualCavePathCount = GetPathCountsWithOneRevisit(caveTestFilePath);
            Console.WriteLine($"Part Two: Actual Cave Path Count: {partTwoActualCavePathCount}");


            var partOneSmallCavePathCount = GetPathCounts(smallCaveTestFilePath);
            Console.WriteLine($"Part One: Small Cave Path Count: {partOneSmallCavePathCount}");
            
            var partOneMediumCavePathCount = GetPathCounts(mediumCaveTestFilePath);
            Console.WriteLine($"Part One: Medium Cave Path Count: {partOneMediumCavePathCount}");
            
            var partOneLargeCavePathCount = GetPathCounts(largeCaveTestFilePath);
            Console.WriteLine($"Part One: Large Cave Path Count: {partOneLargeCavePathCount}");

            var partOneActualCavePathCount = GetPathCounts(caveTestFilePath);
            Console.WriteLine($"Part One: Actual Cave Path Count: {partOneActualCavePathCount}");

        }

        private static int GetPathCountsWithOneRevisit(string caveMapFilePath)
        {
            var paths = new List<string>();
            var caves = PopulateCaves(caveMapFilePath);
            foreach (var smallCave in caves.Where(c => !caves[c.Key].IsBigCave).Select(c => c.Key))
            {
                var potentialPaths = GetPaths(caves, smallCave);
                foreach (var potentialPath in potentialPaths)
                {
                    var test = string.Join(",", potentialPath);
                    if (!paths.Contains(test))
                    {
                        paths.Add(test);
                    }
                }
            }
            return paths.Count;
        }

        private static int GetPathCounts(string caveMapFilePath)
        {
            var caves = PopulateCaves(caveMapFilePath);
            var paths = GetPaths(caves, string.Empty);
            return paths.Count;
        }

        private static List<List<string>> GetPaths(Dictionary<string, Cave> caves, string caveToVisitTwice)
        {
            var currentPaths = new List<List<string>>
            {
                new List<string>
                {
                    Cave.StartCaveId
                }
            };

            do
            {
                var nextSteps = new List<List<string>>();

                foreach (var currentPath in currentPaths)
                {
                    if (string.Equals(currentPath.Last(), Cave.EndCaveId))
                    {
                        var path = new List<string>();
                        path.AddRange(currentPath);
                        nextSteps.Add(path);
                        continue;
                    }

                    foreach (var connectedCave in caves[currentPath.Last()].ConnectedCaves)
                    {
                        if (!CanWeVisitThisCave(caves, currentPath, connectedCave, caveToVisitTwice))
                        {
                            continue;
                        }

                        var path = new List<string>();
                        path.AddRange(currentPath);
                        path.Add(connectedCave);
                        
                        nextSteps.Add(path);
                    }
                }

                currentPaths.Clear();
                currentPaths.AddRange(nextSteps);

            } while (!AllPathsFound(currentPaths));

            return currentPaths;
        }

        private static bool CanWeVisitThisCave(IReadOnlyDictionary<string, Cave> caves, List<string> currentPath, string connectedCave, string caveToVisitTwice)
        {
            if (string.Equals(connectedCave, Cave.StartCaveId))
            {
                return false;
            }

            if (caves[connectedCave].IsBigCave)
            {
                return true;
            }

            var caveVisitCount = new Dictionary<string, int>();

            foreach (var step in currentPath.Where(step => !caves[step].IsBigCave))
            {
                if (!caveVisitCount.ContainsKey(step))
                {
                    caveVisitCount.Add(step, 0);
                }

                caveVisitCount[step]++;

                if (caveVisitCount[step] > 1 && !step.Equals(caveToVisitTwice))
                {
                    return false;
                }

                if (step.Equals(caveToVisitTwice) && caveVisitCount[step] > 2)
                {
                    return false;
                }
            }

            return true;
        }

        private static bool AllPathsFound(List<List<string>> paths)
        {
            return paths.All(path => string.Equals(path.LastOrDefault(), Cave.EndCaveId));
        }

        private static Dictionary<string, Cave> PopulateCaves(string caveMapFilePath)
        {
            using var caveMapFile = File.OpenText(caveMapFilePath);
            var caves = new Dictionary<string, Cave>();

            do
            {
                var line = caveMapFile.ReadLine();
                var caveId = line.Split("-", StringSplitOptions.RemoveEmptyEntries).First();
                var linkedCave = line.Split("-", StringSplitOptions.RemoveEmptyEntries).Last();

                if (!caves.ContainsKey(caveId))
                {
                    caves.Add(caveId, new Cave() {CaveId = caveId});
                }

                if (!caves.ContainsKey(linkedCave))
                {
                    caves.Add(linkedCave, new Cave() {CaveId = linkedCave});
                }

                if (!caves[caveId].ConnectedCaves.Contains(linkedCave))
                {
                    caves[caveId].ConnectedCaves.Add(linkedCave);
                }

                if (!caves[linkedCave].ConnectedCaves.Contains(caveId))
                {
                    caves[linkedCave].ConnectedCaves.Add(caveId);
                }

            } while (!caveMapFile.EndOfStream);

            caveMapFile.Close();

            return caves;
        }
    }
}
