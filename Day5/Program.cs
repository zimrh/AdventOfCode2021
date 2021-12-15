using System;
using System.IO;
using System.Text;

namespace AdventDay5
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var ventsFilePath = "vents.txt";
            var dangerPointsIgnoringDiagonalCount = GetDangerPointsCount(ventsFilePath ,true);
            Console.WriteLine($"Total Points of Danger: {dangerPointsIgnoringDiagonalCount}");

            var dangerPointsCount = GetDangerPointsCount(ventsFilePath);
            Console.WriteLine($"Total Points of Danger: {dangerPointsCount}");
        }

        private static int GetDangerPointsCount(string ventsFilePath, bool IgnoreDiagonalVents = false)
        {
            var seaFloor = new SeaFloorMap();
            using var ventsFile = File.OpenText(ventsFilePath);

            do
            {
                var seaVent = new SeaVent(ventsFile.ReadLine());
                var x = seaVent.X1;
                var y = seaVent.Y1;

                if (!(seaVent.X1 == seaVent.X2 || seaVent.Y1 == seaVent.Y2) && IgnoreDiagonalVents)
                {
                    continue;
                }

                seaFloor.Grid[x, y]++;

                do
                {

                    if (x > seaVent.X2)
                    {
                        x--;
                    }
                    if (x < seaVent.X2)
                    {
                        x++;
                    }

                    if (y > seaVent.Y2)
                    {
                        y--;
                    }

                    if (y < seaVent.Y2)
                    {
                        y++;
                    }

                    seaFloor.Grid[x, y]++;

                } while (!(x == seaVent.X2 && y == seaVent.Y2));


            } while (!ventsFile.EndOfStream);

            ventsFile.Close();

            return seaFloor.GetDangerZonesCount();
        }
    }

    internal class SeaVent
    {
        public SeaVent() { }

        public SeaVent(string ventDetails)
        {
            var coordinates = ventDetails.Split(" -> ");
            var firstPoint = coordinates[0].Split(",");
            var secondPoint = coordinates[1].Split(",");

            X1 = int.Parse(firstPoint[0]);
            Y1 = int.Parse(firstPoint[1]);

            X2 = int.Parse(secondPoint[0]);
            Y2 = int.Parse(secondPoint[1]);
        }

        public int X1 = 0;
        public int X2 = 0;
        public int Y1 = 0;
        public int Y2 = 0;
    }

    internal class SeaFloorMap
    {
        public const int MapWidth = 999;
        public const int MapHeight = 999;
        public int[,] Grid = new int[MapWidth, MapHeight];

        public int GetDangerZonesCount()
        {
            var count = 0;
            for (var y = 0; y < MapHeight; y++)
            {
                for (var x = 0; x < MapWidth; x++)
                {
                    if (Grid[x, y] > 1)
                    {
                        count++;
                    } 
                }
            }

            return count;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            for (var y = 0; y < MapHeight; y++)
            {
                for (var x = 0; x < MapWidth; x++)
                {
                    var test = (Grid[x,y] == 0) ? "." : Grid[x,y].ToString();
                    sb.Append(test);
                }

                sb.AppendLine();
            }

            return sb.ToString();
        }
    }
}
