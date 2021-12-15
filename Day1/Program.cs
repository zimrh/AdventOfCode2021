using System;
using System.Collections.Generic;
using System.IO;

namespace AdventDay1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var depthFile = "depths.txt";

            var depthIncreasedPartOne = GetDepthIncreaseCountPartOne(depthFile);
            var depthIncreasedPartTwo = GetDepthIncreaseCountPartTwo(depthFile);

            Console.WriteLine($"Advent Day 1 Part 1: Depth increased: {depthIncreasedPartOne} times");
            Console.WriteLine($"Advent Day 1 Part 2: Depth increased: {depthIncreasedPartTwo} times");
            
        }

        private static int GetDepthIncreaseCountPartOne(string depthFile)
        {
            var currentDepth = 0;
            var depthIncreased = 0;
            var depthsFile = File.OpenText(depthFile);
            do
            {
                var line = depthsFile.ReadLine();
                var depth = int.Parse(line);
                if (currentDepth == 0)
                {
                    currentDepth = depth;
                    continue;
                }

                //deeper
                if (depth > currentDepth)
                {
                    depthIncreased++;
                    Console.WriteLine($"{depth}: Increased");
                }

                //shallower
                if (depth < currentDepth)
                {
                    Console.WriteLine($"{depth}: Decreased");
                }

                //Same
                if (depth == currentDepth)
                {
                    Console.WriteLine($"{depth}: Same");
                }

                currentDepth = depth;



            } while (!depthsFile.EndOfStream);

            return depthIncreased;
        }

        private static int GetDepthIncreaseCountPartTwo(string depthFile)
        {
            var depthIncreased = 0;
            var depths = new List<int>();

            var depthsFile = File.OpenText(depthFile);


            do
            {
                var line = depthsFile.ReadLine(); 
                var depth = int.Parse(line ?? "0");
                depths.Add(depth);
            } while (!depthsFile.EndOfStream);

            for (var i = 1; i < depths.Count - 2; i++)
            {
                var currentDepth = depths[i - 1] + depths[i] + depths[i + 1];
                var nextDepth = depths[i] + depths[i + 1] + depths[i + 2];
                Console.WriteLine($"{currentDepth} > {nextDepth}");
                if (nextDepth > currentDepth)
                {
                    Console.WriteLine($"{currentDepth} > {nextDepth}: Increased");
                    depthIncreased++;
                }
            }

            return depthIncreased;
        }
    }
}
