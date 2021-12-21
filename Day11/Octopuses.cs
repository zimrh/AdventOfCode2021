using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day11
{

    internal class Octopuses
    {
        public const int Height = 10;
        public const int Width = 10;
        public const int FlashLevel = 10;
        public const int ResetLevel = 0;
        
        public bool AllFlashed = false;
        public int[,] Grid = new int[Width, Height];

        public int IncrementAndGetFlashCount()
        {
            // Increment All Octopuses by 1
            for (var x = 0; x < Width; x++)
            {
                for (var y = 0; y < Height; y++)
                {
                    Grid[x, y] = Grid[x, y] + 1;
                }
            }
            
            var flashed = new bool[Width, Height];
            var foundAllThatHaveFlashed = true;
            var flashCount = 0;

            do
            {
                foundAllThatHaveFlashed = true;
                
                for (var x = 0; x < Width; x++)
                {
                    for (var y = 0; y < Height; y++)
                    {
                        if (Grid[x, y] < FlashLevel || flashed[x, y]) continue;

                        IncrementSurroundingOctopuses(x, y);
                        
                        foundAllThatHaveFlashed = false;
                        Grid[x, y] = ResetLevel;
                        flashed[x, y] = true;

                        flashCount++;
                    }
                }
            } while (!foundAllThatHaveFlashed);

            AllFlashed = flashCount == Width * Height;

            return flashCount;
        }

        private void IncrementSurroundingOctopuses(int x, int y)
        {
            var leftX = (x - 1 > 0) ? x - 1 : 0;
            var rightX = (x + 1 < Width) ? x + 1 : Width - 1;
            var topY = (y - 1 > 0) ? y - 1 : 0;
            var bottomY = (y + 1 < Height) ? y + 1 : Height - 1;

            for (var updatingX = leftX ; updatingX <= rightX; updatingX++)
            {
                for (var updatingY = topY; updatingY <= bottomY; updatingY++)
                {
                    var hasNotFlashed = Grid[updatingX, updatingY] < FlashLevel && Grid[updatingX, updatingY] != ResetLevel;
                    Grid[updatingX, updatingY] = hasNotFlashed ? Grid[updatingX, updatingY] + 1 : Grid[updatingX, updatingY];
                }
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            
            for (var y = 0; y < Height; y++)
            {
                for (var x = 0; x < Width; x++)
                {
                    sb.Append($"{Grid[x, y],2}");
                }

                sb.AppendLine();
            }
            
            return sb.ToString();
        }
    }
}
