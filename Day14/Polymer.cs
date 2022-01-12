using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day14
{
    internal class Polymer
    {
        public const int PairingInsertionRuleLength = 2;
        public string Template { get; set; }
        public Dictionary<string, string> PairingInsertionRules { get; set; } = new Dictionary<string, string>();

        public static Polymer NewPolymerFromFile(string polymerInstructionsFilePath)
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
    }
}
