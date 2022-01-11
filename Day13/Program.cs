using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Day13
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var testInputFilePath = "test-input.txt";
            var testPaperAfterAllFolds = GetPaperAfterFolding(testInputFilePath);
            Console.WriteLine($"Test paper after all folds: {Environment.NewLine}{testPaperAfterAllFolds}");

            Console.WriteLine();
            
            var actualInputFilePath = "actual-input.txt";
            var sheetAfterFirstFold = GetPaperAfterFolding(actualInputFilePath, true);
            var holeCountAfterFirstFold = sheetAfterFirstFold.Count(c => c.Equals('#'));
            Console.WriteLine($"Part 1: Hole count after first fold: {holeCountAfterFirstFold}");

            Console.WriteLine();
            
            var sheetAfterAllFolds = GetPaperAfterFolding(actualInputFilePath);
            Console.WriteLine($"Sheet after all folds: {Environment.NewLine}{sheetAfterAllFolds}");

        }

        private static string GetPaperAfterFolding(string inputFilePath, bool firstFoldCount = false)
        {
            var paper = GetPaper(inputFilePath);

            foreach (var foldInstruction in paper.FoldInstructions)
            {
                var axis = foldInstruction.Split("=").First();
                var position = int.Parse(foldInstruction.Split("=").Last());

                if (axis.Equals("x"))
                {
                    paper.FoldLeftAtX(position);
                }
                else
                {
                    paper.FoldUpAtY(position);
                }

                if (firstFoldCount)
                {
                    return paper.ToString();
                }
            }
            
            return paper.ToString();
        }

        private static Paper GetPaper(string inputFilePath)
        {
            using var inputFile = File.OpenText(inputFilePath);
            var inputPage = new bool[1500, 1500];
            var paper = new Paper();
            var pageWidth = 0;
            var pageHeight = 0;

            do
            {
                var line = inputFile.ReadLine();
                if (line.StartsWith("fold along "))
                {
                    paper.FoldInstructions.Add(line.Substring(11, line.Length - 11));
                }
                else
                {
                    var split = line.Split(',', StringSplitOptions.RemoveEmptyEntries);
                    
                    if (split.Length != 2) continue;

                    var x = int.Parse(split.First());
                    pageWidth = (x > pageWidth) ? x : pageWidth;
                    var y = int.Parse(split.Last());
                    pageHeight = (y > pageHeight) ? y : pageHeight;
                    inputPage[x, y] = true;
                }

            } while (!inputFile.EndOfStream);

            inputFile.Close();

            paper.CurrentHeight = pageHeight + 1;
            paper.CurrentWidth = pageWidth + 1;

            paper.Sheet = new bool[paper.CurrentWidth, paper.CurrentHeight];

            for (var x = 0; x < paper.CurrentWidth; x++)
            {
                for (var y = 0; y < paper.CurrentHeight; y++)
                {
                    paper.Sheet[x, y] = inputPage[x, y];
                }
            }

            return paper;
        }
    }

}
