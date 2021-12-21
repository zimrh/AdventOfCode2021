using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day10
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var syntaxInputTestFilePath = "syntax-input-test.txt";
            var syntaxInputFilePath = "syntax-input.txt";

            var syntaxErrorScoreTestFile = GetSyntaxErrorScore(syntaxInputTestFilePath);
            Console.WriteLine($"Syntax Error Score on Test File: {syntaxErrorScoreTestFile}");
            var syntaxErrorScoreFile = GetSyntaxErrorScore(syntaxInputFilePath);
            Console.WriteLine($"Syntax Error Score on File: {syntaxErrorScoreFile}");

            var syntaxRepairScoreTestFile = GetRepairScore(syntaxInputTestFilePath);
            Console.WriteLine($"Syntax Repair Score on Test File: {syntaxRepairScoreTestFile}");
            var syntaxRepairScoreFile = GetRepairScore(syntaxInputFilePath);
            Console.WriteLine($"Syntax Repair Score on File: {syntaxRepairScoreFile}");
        }

        private static double GetRepairScore(string syntaxInputFilePath)
        {
            using var syntaxInputFile = File.OpenText(syntaxInputFilePath);
            var repairPoints = new Dictionary<char, int>
            {
                {')', 1},
                {']', 2},
                {'}', 3},
                {'>', 4}
            };

            var scores = new List<double>();

            do
            {
                var line = syntaxInputFile.ReadLine();
                var repairCharacters = ValidateLine(line, true);
                if (Equals(repairCharacters, "."))
                {
                    //Invalid Line Skip
                    continue;
                }
                Console.WriteLine($"Characters to repair: {repairCharacters}");

                scores.Add(
                    repairCharacters.Aggregate(new double(), 
                        (current, character) 
                            => (5 * current) + repairPoints[character]));
            } while (!syntaxInputFile.EndOfStream);

            syntaxInputFile.Close();

            var mid = scores.Count / 2;

            return scores.OrderBy(s => s).Skip(mid).Take(1).LastOrDefault();
        }
        
        private static int GetSyntaxErrorScore(string syntaxInputFilePath)
        {
            using var syntaxInputFile = File.OpenText(syntaxInputFilePath);

            var errorPoints = new Dictionary<string, int>
            {
                {")", 3},
                {"]", 57},
                {"}", 1197},
                {">", 25137}
            };
            var invalidBracketCount = new Dictionary<string, int>
            {
                {")", 0},
                {"]", 0},
                {"}", 0},
                {">", 0}
            };

            do
            {
                var line = syntaxInputFile.ReadLine();
                var firstInvalidBracket = ValidateLine(line);
                if (invalidBracketCount.ContainsKey(firstInvalidBracket))
                {
                    invalidBracketCount[firstInvalidBracket]++;
                }
            } while (!syntaxInputFile.EndOfStream);
            
            syntaxInputFile.Close();

            return invalidBracketCount.Sum(
                bracketCount => bracketCount.Value * errorPoints[bracketCount.Key]);
        }

        private static string ValidateLine(string line, bool attemptRepair = false)
        {
            var characterMap = new Dictionary<char, char>
            {
                {'(',')'},
                {'[',']'},
                {'{','}'},
                {'<','>'}
            };

            var expectedClosingCharacters = new List<char>();

            for (var i = 0; i < line?.Length; i++)
            {
                if (characterMap.ContainsKey(line[i]))
                {
                    expectedClosingCharacters.Add(characterMap[line[i]]);
                }
                else
                {
                    if (Equals(line[i], expectedClosingCharacters.Last()))
                    {
                        expectedClosingCharacters.RemoveAt(expectedClosingCharacters.Count - 1);
                    }
                    else
                    {
                        if (!attemptRepair)
                        {
                            Console.WriteLine($"Expected {expectedClosingCharacters.Last()}, but found {line[i]} instead");
                            return line[i].ToString();
                        }
                        else
                        {
                            return i < line.Length - 1 
                                ? "." 
                                : expectedClosingCharacters.Aggregate("", (list, next) => $"{next}{list}" );
                        }
                    }
                }
            }

            return expectedClosingCharacters.Aggregate("", (list, next) => $"{next}{list}" );
        }
    }
}

