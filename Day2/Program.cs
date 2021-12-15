using System;
using System.IO;

namespace AdventDay2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var commandsFilePath = "commands.txt";
            var distancePartOne = GetDistancePartOne(commandsFilePath);
            var distancePartTwo = GetDistancePartTwo(commandsFilePath);
            Console.WriteLine($"Distance Part One: {distancePartOne}");
            Console.WriteLine($"Distance Part Two: {distancePartTwo}");
        }

        private static int GetDistancePartOne(string commandFilePath)
        {
            var commandFile = File.OpenText(commandFilePath);
            var depth = 0;
            var distance = 0;

            do
            {
                var instruction = commandFile.ReadLine();
                var split = instruction?.Split(" ");
                var command = split?[0] ?? "none";
                var value = int.Parse(split?[1] ?? "0");
                Console.WriteLine($"{command}:{value}");
                switch (command)
                {
                    case "forward":
                        distance += value;
                        break;
                    case "up":
                        depth -= value;
                        break;
                    case "down":
                        depth += value;
                        break;
                }
                Console.WriteLine($"Depth:{depth} Distance:{distance}");
            } while (!commandFile.EndOfStream);

            return depth * distance;
        }

        private static long GetDistancePartTwo(string commandFilePath)
        {
            var commandFile = File.OpenText(commandFilePath);
            long depth = 0;
            long distance = 0;
            long aim = 0;
            do
            {
                var instruction = commandFile.ReadLine();
                var split = instruction?.Split(" ");
                var command = split?[0] ?? "none";
                var value = int.Parse(split?[1] ?? "0");
                Console.WriteLine($"{command}:{value}");
                switch (command)
                {
                    case "forward":
                        distance += value;
                        depth += aim * value;
                        break;
                    case "up":
                        aim -= value;
                        break;
                    case "down":
                        aim += value;
                        break;
                }
                Console.WriteLine($"Depth:{depth} Distance:{distance} Aim:{aim}");
            } while (!commandFile.EndOfStream);

            return depth * distance;
        }
    }
}
