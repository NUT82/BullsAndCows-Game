using System;
using System.Security.Cryptography;

namespace TestGame1
{
    class Program
    {
        static void Main(string[] args)
        {
            string playerSecretNumber = "";
            string playerGuessNumber = "";
            string computerSecretNumber = "";
            int bulls = 0;
            int cows = 0;
            int countGuess = 0;
            do
            {
                countGuess = 0;
                do
                {
                    Console.WriteLine("Please type your secret number:");
                    playerSecretNumber = Console.ReadLine();
                } while (!isValidNumber(playerSecretNumber, "player"));
                Console.WriteLine($"Your secret number is: {playerSecretNumber}");
                do
                {
                    Random crn = new Random();
                    computerSecretNumber = crn.Next(1234, 9876).ToString();
                } while (!isValidNumber(computerSecretNumber, "computer"));
                Console.WriteLine($"Computer secret number is: ****");
                do
                {
                    do
                    {
                        Console.WriteLine("Please type your guess:");
                        playerGuessNumber = Console.ReadLine();
                    } while (!isValidNumber(playerGuessNumber, "player"));
                    string currGuess = BullsCowsCheck(computerSecretNumber, playerGuessNumber);
                    bulls = int.Parse(currGuess[0].ToString());
                    cows = int.Parse(currGuess[1].ToString());
                    countGuess++;
                    Console.WriteLine($"You have {bulls} bulls and {cows} cows!");
                } while (bulls != 4);
                Console.WriteLine("You WIN!!! (with " + countGuess + " tries)");
                Console.WriteLine("Press key for new game, or ESC to end!");
                //Console.ReadKey();
            } while (Console.ReadKey().Key != ConsoleKey.Escape);
        }

        static bool isValidNumber(string number, string playerType)
        {
            bool result = true;
            int n;
            bool isNumeric = int.TryParse(number, out n);
            if (number.Length != 4 || !isNumeric)
            {
                result = false;
            }
            else
            {
                for (int i = 0; i < 4; i++)
                {
                    for (int j = i + 1; j < 4; j++)
                    {
                        if (number[i] == number[j])
                        {
                            result = false;
                        }
                    }
                }
            }
            if (!result && playerType == "player")
            {
                Console.WriteLine($"Invalid number \"{number}\"");
                Console.WriteLine("The number must have four not repeated numbers!");
                Console.WriteLine("Example: \"1234\"");
            }
            return result;
        }
        static string BullsCowsCheck(string number, string guessNumber)
        {
            string result = "00";
            int bulls = 0;
            int cows = 0;
            
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (i != j)
                    {
                        if (number[i] == guessNumber[j])
                        {
                            cows++;
                        }
                    }
                    else
                    {
                        if (number[i] == guessNumber[j])
                        {
                            bulls++;
                        }
                    }
                }
            }
            result = bulls.ToString() + cows.ToString();
            return result;
        }
    }
}
