using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day9
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var heightMapTestFilePath = "height-map-test.txt";
            var heightMapFilePath = "height-map.txt";
            var lowPointRiskTestScore = GetLowPointsRiskScore(heightMapTestFilePath);
            Console.WriteLine($"Low Points Risk Score: {lowPointRiskTestScore}");
            var lowPointRiskScore = GetLowPointsRiskScore(heightMapFilePath);
            Console.WriteLine($"Low Points Risk Score: {lowPointRiskScore}");
        }

        private static int GetLowPointsRiskScore(string heightMapFilePath)
        {
            var floor = new List<List<int>>();

            using var heightMapFile = File.OpenText(heightMapFilePath);
            var row = 0;

            do
            {
                var line = heightMapFile.ReadLine();
                floor.Add(new List<int>());
                foreach (var height in line ?? "")
                {
                    floor[row].Add(int.Parse(height.ToString()));
                }

                row++;
            } while (!heightMapFile.EndOfStream);

            var lowPoints = new List<int>();

            for (var x = 0; x < floor.Count(); x++)
            {
                for (var y = 0; y < floor[x].Count; y++)
                {
                    var coordinate = new Coordinates(x, y);
                    var currentHeight = floor[coordinate.X][coordinate.Y];
                    var lowest = true;

                    foreach (var direction in (Direction[])Enum.GetValues(typeof(Direction)))
                    {
                        var newCoord = coordinate.Move(direction);
                        if (newCoord.IsOutOfBounds(floor.Count, floor[x].Count))
                        {
                            continue;
                        }

                        var value = floor[newCoord.X][newCoord.Y];

                        Console.WriteLine($"{currentHeight} | {value}");

                        if (currentHeight >= value)
                        {
                            lowest = false;
                        }
                    }

                    if (lowest)
                    {
                        lowPoints.Add(currentHeight);
                    }
                    Console.WriteLine();

                }
            }

            return lowPoints.Sum() + lowPoints.Count;
        }


    }

    internal class Coordinates
    {
        public Coordinates()
        {
        }

        public Coordinates(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; set; }
        public int Y { get; set; }

        public bool IsOutOfBounds(int height, int width)
        {
            return X < 0 || X >= height || Y < 0 || Y >= width;
        }

        public Coordinates Move(Direction direction) => direction switch
        {
            Direction.Up => new Coordinates(X - 1, Y),
            Direction.Down => new Coordinates(X + 1, Y),
            Direction.Left => new Coordinates(X, Y - 1),
            Direction.Right => new Coordinates(X, Y + 1),
            _ => new Coordinates(X, Y)
        };
    }

    internal enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }


}
