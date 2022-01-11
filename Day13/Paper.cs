using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day13
{
    internal class Paper
    {
        public bool[,] Sheet { get; set; }
        
        public List<string> FoldInstructions { get; set; } = new List<string>();

        public int CurrentWidth { get; set; }
        
        public int CurrentHeight { get; set; }

        public void FoldLeftAtX(int col)
        {
            for (var y = 0; y < CurrentHeight; y++)
            {
                for (var x = 1; x < CurrentWidth - col; x++)
                {
                    Sheet[col - x, y] = Sheet[col - x, y] || Sheet[col + x, y];
                }
            }

            CurrentWidth = col;
        }
        
        public void FoldUpAtY(int row)
        {
            for (var x = 0; x < CurrentWidth; x++)
            {
                for (var y = 1; y < CurrentHeight - row; y++)
                {
                    Sheet[x, row - y] = Sheet[x, row - y] || Sheet[x, row + y];
                }
            }

            CurrentHeight = row;
        }
        
        public int GetHoleCount()
        {
            var count = 0;
            for (var y = 0; y < CurrentHeight; y++)
            {
                for (var x = 0; x < CurrentWidth; x++)
                {
                    count = count + (Sheet[x, y] ? 1 : 0);
                }
            }

            return count;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            for (var y = 0; y < CurrentHeight; y++)
            {
                for (var x = 0; x < CurrentWidth; x++)
                {
                    sb.Append(Sheet[x, y] ? "#": ".");
                }
                sb.AppendLine();
            }

            return sb.ToString();
        }
    }
}
