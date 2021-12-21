using System;
using System.IO;
using System.Text;

namespace Day11
{
    public class Program
    {
        public static void Main(string[] args)
        {
            const string octopusesGridTestFilePath = "octopuses-grid-test.txt";
            var octopusesGridFilePath = "octopuses-grid.txt";

            var testFileFlashCount = GetFlashCount(octopusesGridTestFilePath, 100);
            Console.WriteLine($"Test File Flash Count: {testFileFlashCount}");

            var flashCount = GetFlashCount(octopusesGridFilePath, 100);
            Console.WriteLine($"Flash Count: {flashCount}");

            var testFileAllFlashOnStep = GetFirstStepTheyAllFlash(octopusesGridTestFilePath);
            Console.WriteLine($"Test File They All Flash on Step: {testFileAllFlashOnStep}");

            var allFlashOnStep = GetFirstStepTheyAllFlash(octopusesGridFilePath);
            Console.WriteLine($"They All Flash on Step: {allFlashOnStep}");
        }

        private static int GetFirstStepTheyAllFlash(string octopusGridFilePath)
        {
            var octopuses = GetOctopuses(octopusGridFilePath);
            var step = 0;
            do
            {
                var flashCount = octopuses.IncrementAndGetFlashCount();
                Console.WriteLine($"FlashCount: {flashCount} AllFlashed: {octopuses.AllFlashed}");
                step++;
            } while (!octopuses.AllFlashed);
            return step;
        }

        private static int GetFlashCount(string octopusGridFilePath, int steps)
        {
            var octopuses = GetOctopuses(octopusGridFilePath);
            var flashTotal = 0;
            for (var step = 0; step < steps; step++)
            {
                var flashCount = octopuses.IncrementAndGetFlashCount();
                flashTotal += flashCount;
                Console.WriteLine($"Step: {step}");
                Console.WriteLine(octopuses);
                Console.WriteLine($"Flash Count: {flashCount} Flash Total: {flashTotal}");
                Console.WriteLine();
            }
            return flashTotal;
        }

        private static Octopuses GetOctopuses(string octopusGridFilePath)
        {
            var octopuses = new Octopuses();
            using var octopusGridFile = File.OpenText(octopusGridFilePath);
            var y = 0;

            do
            {
                var line = octopusGridFile.ReadLine();
                for (var x = 0; x < line.Length; x++)
                {
                    octopuses.Grid[x, y] = int.Parse(line[x].ToString());
                }
                y++;
            } while (!octopusGridFile.EndOfStream);

            octopusGridFile.Close();

            return octopuses;
        }
    }

}
