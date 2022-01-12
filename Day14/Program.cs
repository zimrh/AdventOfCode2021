using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Day14
{
    public class Program
    {
        public static void Main(string[] args)
        {

            // Refer to the README.md for this one! Was caught out by exponential growth the first time!

            var dataFilePath = "polymer.txt";
            var testDataFilePath = "test-polymer.txt";

            var polymer = Polymer.NewPolymerFromFile(dataFilePath);
            var testPolymer = Polymer.NewPolymerFromFile(testDataFilePath);

            var grownTestPolymer = GrowPolymer(testPolymer, 10);
            var testPolymerScore = AnalyzePolymerPairings(grownTestPolymer);
            Console.WriteLine($"Test Polymer Score: {testPolymerScore}");

            var testLargePolymerPairings = GrowPolymer(testPolymer, 40);
            var testLargePolymerScore = AnalyzePolymerPairings(testLargePolymerPairings);
            Console.WriteLine($"Large Test Polymer Score: {testLargePolymerScore}");

            var grownPolymer = GrowPolymer(polymer, 10);
            var polymerScore = AnalyzePolymerPairings(grownPolymer);
            Console.WriteLine($"Polymer Score: {polymerScore}");

            var largePolymerPairings = GrowPolymer(polymer, 40);
            var largePolymerScore = AnalyzePolymerPairings(largePolymerPairings);
            Console.WriteLine($"Large Polymer Score: {largePolymerScore}");
        }

        private static ulong AnalyzePolymerPairings(Dictionary<string, ulong> pairings)
        {
            var elementCount = new Dictionary<char, ulong>();
            foreach (var (pair, value) in pairings)
            {
                foreach (var element in pair)
                {
                    if (!elementCount.ContainsKey(element))
                    {
                        elementCount.Add(element, 0);
                    }

                    elementCount[element] += value;
                }
            }

            foreach (var (key, value) in elementCount)
            {
                elementCount[key] = value / 2;
            }

            var ordered = elementCount.OrderByDescending(e => e.Value).ToList();
            return  ordered.First().Value - ordered.Last().Value;
        }

        private static Dictionary<string, ulong> GrowPolymer(Polymer polymer, int stepCount)
        {
            var pairings = new Dictionary<string, ulong>();

            for (var i = 0; i <= polymer.Template.Length - Polymer.PairingInsertionRuleLength; i++)
            {
                var pair = polymer.Template.Substring(i, Polymer.PairingInsertionRuleLength);
                if (!pairings.ContainsKey(pair))
                {
                    pairings.Add(pair, 0);
                }

                pairings[pair]++;
            }

            var step = 0;
            
            do
            {
                var nextPairings = new Dictionary<string, ulong>();

                foreach (var (key, value) in pairings)
                {
                    var newLetter = polymer.PairingInsertionRules[key];

                    var newPairs = new []
                    {
                        key[0] + newLetter,
                        newLetter + key[1]
                    };

                    foreach (var newPair in newPairs)
                    {
                        if (!nextPairings.ContainsKey(newPair))
                        {
                            nextPairings.Add(newPair, 0);
                        }

                        nextPairings[newPair] += value;
                        
                    }
                }

                pairings = nextPairings;

                step++;
            } while (step < stepCount);

            var firstAndLastElements = $"{polymer.Template.First()}{polymer.Template.Last()}";
            pairings[firstAndLastElements]++;

            return pairings;
        }

    }
}
