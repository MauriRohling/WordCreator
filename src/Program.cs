using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace WordCreator
{
    public class Program
    {
        private static HashSet<string> _dictionary;
        private static List<string> _validWords;
        private static int _colorCounter = 0;
        private static readonly ConsoleColor[] _consoleColors = {ConsoleColor.Red, ConsoleColor.Green, ConsoleColor.Yellow, ConsoleColor.Blue, ConsoleColor.Magenta, ConsoleColor.Cyan};

        public static void Main(string[] args)
        {
            _dictionary = JsonSerializer.Deserialize<HashSet<string>>(File.ReadAllText("dictionary.json"));

            while (true)
            {
                Console.ForegroundColor = ConsoleColor.White;
                _validWords = [];

                Console.Write("\nType in the letters: ");

                List<string> letters = Console.ReadLine()
                    .Where(l => char.IsLetter(l))
                    .Select(l => l.ToString())
                    .ToList();

                Console.WriteLine("Valid Words:");

                //use all lengths possible
                for (int i = letters.Count; i > 2; i--)
                {
                    string[] combinations = new string[i];
                    Combine(combinations, letters, i);
                }

            }
        }

        /// <summary>
        /// Arrange every possible combination (w/o repeating elements) and prints the valid English words
        /// </summary>
        /// <param name="combinations">An empty array of length equal to lenght of each combination</param>
        /// <param name="letters">Letters to use in the combinations</param>
        /// <param name="combinationLength">Length of each resulting combination</param>
        private static void Combine(string[] combinations, List<string> letters, int combinationLength)
        {
            if (combinationLength > letters.Count)
                throw new ArgumentException("length has to be <= element count");

            if (combinationLength == 0)
            {
                string word = string.Join("", combinations);
                if (_dictionary.Contains(word) && !_validWords.Contains(word))
                {
                    _validWords.Add(word);
                    ColorfulPrint(word);
                }
                return;
            }

            foreach (var letter in letters)
            {
                combinations[combinations.Length - combinationLength] = letter;

                var tempList = new List<string>();
                tempList.AddRange(letters);
                tempList.Remove(letter);

                Combine(combinations, tempList, combinationLength - 1);
            }
        }


        private static void ColorfulPrint(string word)
        {
            Console.ForegroundColor = _consoleColors[_colorCounter];
            Console.WriteLine(word);
            _colorCounter = (_colorCounter + 1) % _consoleColors.Length;
        }
    }
}
