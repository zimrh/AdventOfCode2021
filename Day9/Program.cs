#nullable enable
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
            var test1 = new Coordinates(1, 0);
            var test2 = new Coordinates(1, 0);
            
            var heightMapTestFilePath = "height-map-test.txt";
            var heightMapFilePath = "height-map.txt";

            var lowPointRiskTestScore = GetLowPointsRiskScore(heightMapTestFilePath);
            Console.WriteLine($"Test Low Points Risk Score: {lowPointRiskTestScore}");
            var lowPointRiskScore = GetLowPointsRiskScore(heightMapFilePath);
            Console.WriteLine($"Low Points Risk Score: {lowPointRiskScore}");

            var basinSizingScoreTest = GetBasinSizingScore(heightMapTestFilePath);
            Console.WriteLine($"Test Basin Size Score: {basinSizingScoreTest}");
            var basinSizingScore = GetBasinSizingScore(heightMapFilePath);
            Console.WriteLine($"Basin Size Score: {basinSizingScore}");
        }

        private static int GetBasinSizingScore(string heightMapFilePath)
        {
            var floor = ExtractFloorMap(heightMapFilePath);
            var lowPoints = GetLowPoints(floor);
            var basins = GetBasins(floor, lowPoints);
            var topThreeBasins = basins.Select(b => b.Count).OrderByDescending(c => c).Take(3).ToList();
            return topThreeBasins.Aggregate(1, (current, i1) => current * i1);
        }

        private static List<List<Coordinates>> GetBasins(List<List<int>> floor, List<Coordinates> lowPoints)
        {
            var basins = lowPoints
                .Select(point => 
                    new List<Coordinates>
                    {
                        point
                    }).ToList();

            foreach (var basin in basins)
            {
                var pointsFound = new List<string>
                {
                    basin.First().ToString()
                };
                var allPointsFound = false;

                do
                {
                    var coordinatesToAdd = new List<Coordinates>();
                    allPointsFound = true;
                    foreach (var coordinate in basin)
                    {
                        foreach (var direction in (Direction[]) Enum.GetValues(typeof(Direction)))
                        {
                            var newCoordinate = coordinate.Move(direction);
                            if (newCoordinate.IsOutOfBounds(floor.Count, floor[0].Count))
                            {
                                continue;
                            }

                            var height = floor[newCoordinate.X][newCoordinate.Y];

                            if (pointsFound.Contains(newCoordinate.ToString()) || height == 9)
                            {
                                continue;
                            }

                            allPointsFound = false;
                            pointsFound.Add(newCoordinate.ToString());
                            coordinatesToAdd.Add(newCoordinate);
                        }
                    }
                    basin.AddRange(coordinatesToAdd);
                } while (!allPointsFound);
                
            }

            return basins;
        }

        private static int GetLowPointsRiskScore(string heightMapFilePath)
        {
            var floor = ExtractFloorMap(heightMapFilePath);
            var lowPoints = GetLowPoints(floor);
            return lowPoints.Sum(p => floor[p.X][p.Y] + 1);
        }

        private static List<List<int>> ExtractFloorMap(string heightMapFilePath)
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

            return floor;
        }

        private static List<Coordinates> GetLowPoints(List<List<int>> floor)
        {
            var lowPoints = new List<Coordinates>();

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

                        if (currentHeight >= value)
                        {
                            lowest = false;
                        }
                    }

                    if (lowest)
                    {
                        lowPoints.Add(coordinate);
                    }
                }
            }
            return lowPoints;
        }

    }

    internal class Coordinates
    {
        public Coordinates(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; }
        public int Y { get; }

        public bool IsOutOfBounds(int height, int width)
        {
            return X < 0 || X >= height || Y < 0 || Y >= width;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Coordinates) obj);
        }

        protected bool Equals(Coordinates other)
        {
            return X == other.X && Y == other.Y;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }

        public override string ToString()
        {
            return $"{X}:{Y}";
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
