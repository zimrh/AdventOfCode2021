using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AdventDay4
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var bingoFilePath = "bingo.txt";
            var firstWinningCardScore = GetFirstWinningCardScore(bingoFilePath);
            Console.WriteLine($"First Winning Card Score: {firstWinningCardScore}");
            var lastWinningCardScore = GetLastWinningCardScore(bingoFilePath);
            Console.WriteLine($"Last Winning Card Score: {lastWinningCardScore}");
        }

        private static int GetLastWinningCardScore(string bingoFilePath)
        {
            using var bingoFile = File.OpenText(bingoFilePath);
            var numbers = bingoFile.ReadLine();
            bingoFile.Close();

            var cards = PopulateBingoCards(bingoFilePath);
    
            foreach (var number in numbers.Split(","))
            {
                var cardsToRemove = new List<BingoCard>();
                foreach (var card in cards)
                {
                    var isBingo = card.IsBingo(int.Parse(number));

                    if (!isBingo) { continue; }

                    cardsToRemove.Add(card);

                    if (cards.Count == 1)
                    {
                        var unmarkedScore = card.GetUnmarkedScore();
                        return int.Parse(number) * unmarkedScore;
                    }
                }

                foreach (var cardToRemove in cardsToRemove)
                {
                    cards.Remove(cardToRemove);
                }
            }

            return 0;
        }

        private static int GetFirstWinningCardScore(string bingoFilePath)
        {
            using var bingoFile = File.OpenText(bingoFilePath);
            var numbers = bingoFile.ReadLine();
            bingoFile.Close();

            var cards = PopulateBingoCards(bingoFilePath);

            foreach (var number in numbers.Split(","))
            {
                foreach (var card in cards)
                {
                    var isBingo = card.IsBingo(int.Parse(number));

                    if (!isBingo) { continue; }
                    var unmarkedScore = card.GetUnmarkedScore();
                    return int.Parse(number) * unmarkedScore;
                }
            }

            return 0;
        }

        private static IList<BingoCard> PopulateBingoCards(string bingoFilePath)
        {
            using var bingoFile = File.OpenText(bingoFilePath);
            bingoFile.ReadLine();
            var cards = new List<BingoCard>();
            do
            {
                bingoFile.ReadLine(); //Blank Line

                var card = new BingoCard();
                for (var x = 0; x < BingoCard.CardHeight; x++)
                {
                    var line = bingoFile.ReadLine();
                    var numbers = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                    for (var y = 0; y < BingoCard.CardHeight; y++)
                    {
                        card.Numbers[x, y] = int.Parse(numbers[y]);
                    }
                }
                cards.Add(card);
            } while (!bingoFile.EndOfStream);

            bingoFile.Close();

            return cards;
        }
    }

    internal class BingoCard
    {
        public const int CardHeight = 5;
        public const int CardWidth = 5;

        public int[,] Numbers = new int[CardHeight, CardWidth];
        public bool[,] Marks = new bool[CardHeight, CardWidth];

        public bool IsBingo()
        {
            for (var x = 0; x < CardHeight; x++)
            {
                for (var y = 0; y < CardWidth; y++)
                {
                    if (Marks[x, y] != true) { break; }
                    if (y == CardWidth - 1) { return true; }
                }
            }

            for (var y = 0; y < CardWidth; y++) 
            {
                for (var x = 0; x < CardHeight; x++)
                {
                    if (Marks[x, y] != true) { break; }
                    if (x == CardHeight - 1) { return true; }
                }
            }
            return false;
        }

        public int GetUnmarkedScore()
        {
            var score = 0;
            for (var x = 0; x < CardHeight; x++)
            {
                for (var y = 0; y < CardWidth; y++)
                {
                    if (!Marks[x, y])
                    {
                        score += Numbers[x, y];
                    }
                }
            }

            return score;
        }

        public bool IsBingo(int number)
        {
            for (var x = 0; x < CardHeight; x++)
            {
                for (var y = 0; y < CardWidth; y++)
                {
                    if (Numbers[x, y] != number) { continue; }
                    Marks[x, y] = true;
                    break;
                }
            }

            return IsBingo();
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            for (var x = 0; x < CardHeight; x++)
            {
                for (var y = 0; y < CardWidth; y++)
                {
                    sb.Append($"{Numbers[x,y],3}");
                }
                sb.AppendLine();
            }

            return sb.ToString();
        }
    }
}
