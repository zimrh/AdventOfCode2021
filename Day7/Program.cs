using System;
using System.Collections.Generic;
using System.IO;

namespace Day7
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var crabsFilePath = "crabs.txt";
            var fuelCostConstantRate = GetFuelCostForOptimalPosition(crabsFilePath, true);
            Console.WriteLine($"Cheapest Fuel Cost {fuelCostConstantRate}");
            var fuelCostNonConstantRate = GetFuelCostForOptimalPosition(crabsFilePath);
            Console.WriteLine($"Cheapest Fuel Cost {fuelCostNonConstantRate}");
        }

        private static int GetFuelCostForOptimalPosition(string crabsFilePath, bool constantFuelExpense = false)
        {
            var crabDepths = new List<int>();
            var crabsFile = File.ReadAllText(crabsFilePath);
            
            var shallowestCrab = int.MaxValue;
            var deepestCrab = int.MinValue;
            var averageDepth = 0;

            // Add Crabs
            foreach (var crabDepth in crabsFile.Split(","))
            {
                var depth = int.Parse(crabDepth);
                crabDepths.Add(depth);
                shallowestCrab = (shallowestCrab < depth) ? shallowestCrab : depth;
                deepestCrab = (deepestCrab > depth) ? deepestCrab : depth;
                averageDepth += depth;
            }

            var cheapestFuelCost = int.MaxValue;

            for (var depth = shallowestCrab; depth <= deepestCrab; depth++)
            {
                var levelFuelCost = 0;
                foreach (var crabDepth in crabDepths)
                {
                    var fuelCost = 0;
                    if (constantFuelExpense)
                    {
                        fuelCost = (crabDepth > depth) ? crabDepth - depth : depth - crabDepth;
                    }
                    else
                    {
                        var diff = (crabDepth > depth) ? crabDepth - depth : depth - crabDepth;
                        fuelCost = (diff * diff  + diff) / 2;
                    }
                    
                    levelFuelCost += fuelCost;
                }

                cheapestFuelCost = (levelFuelCost < cheapestFuelCost) ? levelFuelCost : cheapestFuelCost;
            }

            return cheapestFuelCost;
        }
    }
}
