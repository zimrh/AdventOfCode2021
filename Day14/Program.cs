using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace Day14
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var testDataFilePath = "test-polymer.txt";
            var dataFilePath = "polymer.txt";

            var testPolymer = GetPolymer(testDataFilePath);
            var grownTestPolymer = GrowPolymer(testPolymer, 10);
            var testPolymerScore = AnalyzePolymer(grownTestPolymer);
            Console.WriteLine($"Test Polymer Score: {testPolymerScore}");

            
            var polymer = GetPolymer(dataFilePath);
            var grownPolymer = GrowPolymer(polymer, 10);
            var polymerScore = AnalyzePolymer(grownPolymer);
            Console.WriteLine($"Polymer Score: {polymerScore}");

        }

        private static int AnalyzePolymer(string polymer)
        {
            var elementCount = new Dictionary<char, int>();
            foreach (var element in polymer)
            {
                if (!elementCount.ContainsKey(element))
                {
                    elementCount.Add(element, 0);
                }

                elementCount[element]++;
            }

            var ordered = elementCount.OrderBy(e => e.Value);
            return ordered.Last().Value - ordered.First().Value;
        }

        private static Polymer GetPolymer(string polymerInstructionsFilePath)
        {
            using var polymerInstructionsFile = File.OpenText(polymerInstructionsFilePath);

            var polymer = new Polymer()
            {
                Template = polymerInstructionsFile.ReadLine()
            };

            polymerInstructionsFile.ReadLine();

            do
            {
                var pairing = polymerInstructionsFile.ReadLine();
                var split = pairing.Split(" -> ", StringSplitOptions.RemoveEmptyEntries);
                
                if (split.Length != 2) continue;

                polymer.PairingInsertionRules.Add(split.First(), split.Last());

            } while (!polymerInstructionsFile.EndOfStream);

            polymerInstructionsFile.Close();

            return polymer;
        }

        private static string GrowPolymer(Polymer polymer, int stepCount)
        {
            var step = 0;

            var currentTemplate = new StringBuilder(polymer.Template);

            do
            {
                var nextTemplate = new StringBuilder();
                var sw = Stopwatch.StartNew();

                for (var i = 0; i <= currentTemplate.Length - Polymer.PairingInsertionRuleLength; i++)
                {
                    var pair = currentTemplate.ToString(i, Polymer.PairingInsertionRuleLength);
                    nextTemplate.Append(pair[0]);
                    nextTemplate.Append(polymer.PairingInsertionRules[pair]);
                }

                nextTemplate.Append(currentTemplate[^1]);
                sw.Stop();
                Console.WriteLine($"Step {step + 1}: Done in {sw.Elapsed}");

                currentTemplate = nextTemplate;

                step++;
            } while (step < stepCount);

            return currentTemplate.ToString();
        }


        internal class Polymer
        {
            public const int PairingInsertionRuleLength = 2;
            public string Template { get; set; }
            public Dictionary<string, string> PairingInsertionRules { get; set; } = new Dictionary<string, string>();
        }
    }
}
