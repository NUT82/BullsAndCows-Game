using System;
using System.Linq;

namespace BullsAndCows
{
    class Program
    {
        static int WindowWidth = 77;
        static int WindowHeight = 30;
        static string playerSecretNumber = "";
        static string[] allOpportunities = new string[5040];
        static string[,] ComputerTries = new string[2, 50];
        static int countGuess = 0;
        static void Main(string[] args)
        {
            Console.Title = "Bulls and cows V1.1";
            Console.WindowWidth = WindowWidth;
            Console.WindowHeight = WindowHeight;
            Console.BufferWidth = WindowWidth;
            Console.BufferHeight = WindowHeight;

            string playerGuessNumber = "";
            string computerSecretNumber = "";
            string computerGuessNumber = "";

            int allOpportunitiesCount = 0;
            for (int i = 123; i < 9877; i++)
            {
                string numberToString = i.ToString();
                if (i < 1000)
                {
                    numberToString = "0" + i.ToString();
                }
                if (isValidNumber(numberToString, "computer"))
                {
                    allOpportunities[allOpportunitiesCount] = numberToString;
                    allOpportunitiesCount++;
                }
            }

            int bulls = 0;
            int cows = 0;
            int compBulls = 0;
            int compCows = 0;

            do
            {
                WriteHelp();
                countGuess = 0;
                do
                {
                    ClearLeftSide();
                    Write("Type your secret number:", 0, 0);
                    Console.WriteLine();
                    playerSecretNumber = Console.ReadLine();
                } while (!isValidNumber(playerSecretNumber, "player"));
                
                do
                {
                    Random crn = new Random();
                    computerSecretNumber = crn.Next(1234, 9877).ToString();
                } while (!isValidNumber(computerSecretNumber, "computer"));

                do
                {
                    do
                    {
                        ClearLeftSide();
                        Write("Please type your guess:", 0, 0);
                        Console.WriteLine();
                        playerGuessNumber = Console.ReadLine();
                    } while (!isValidNumber(playerGuessNumber, "player"));
                    string currGuess = BullsCowsCheck(computerSecretNumber, playerGuessNumber);
                    bulls = int.Parse(currGuess[0].ToString());
                    cows = int.Parse(currGuess[1].ToString());


                    Write("   ****-Computer number", 0, 25, ConsoleColor.DarkRed);
                    Write("Your guess  bulls  cows", 1, 25, ConsoleColor.Red);
                    Write("   " + playerGuessNumber + "       " + bulls.ToString() + "      " + cows.ToString() + " ", countGuess + 2, 25, ConsoleColor.Red);
                    Write("     " + playerSecretNumber + "  -  Your number  ", 0, 50, ConsoleColor.DarkBlue);
                    Write("Computer guess  bulls  cows", 1, 50, ConsoleColor.Blue);

                    computerGuessNumber = ComputerGuessNumber();
                    string computerCurrGuess = BullsCowsCheck(playerSecretNumber, computerGuessNumber);
                    compBulls = int.Parse(computerCurrGuess[0].ToString());
                    compCows = int.Parse(computerCurrGuess[1].ToString());
                    Write("     " + computerGuessNumber + "         " + compBulls.ToString() + "      " + compCows.ToString() + " ", countGuess + 2, 50, ConsoleColor.Blue);
                    countGuess++;

                } while (bulls != 4 && compBulls != 4);
                Console.ForegroundColor = ConsoleColor.DarkRed;
                if (bulls > compBulls)
                {
                    Write("             " + " " + "         ", WindowHeight / 2 - 1, WindowWidth / 2 - 12, ConsoleColor.Cyan);
                    Write("You WIN with " + countGuess + " tries!!!", WindowHeight / 2, WindowWidth / 2 - 12, ConsoleColor.Cyan);
                    Write("             " + " " + "         ", WindowHeight / 2 + 1, WindowWidth / 2 - 12, ConsoleColor.Cyan);
                }
                else if (compBulls > bulls)
                {
                    Write("              " + " " + "         ", WindowHeight / 2 - 1, WindowWidth / 2 - 12, ConsoleColor.Cyan);
                    Write("You LOST with " + countGuess + " tries!!!", WindowHeight / 2, WindowWidth / 2 - 12, ConsoleColor.Cyan);
                    Write("              " + " " + "         ", WindowHeight / 2 + 1, WindowWidth / 2 - 12, ConsoleColor.Cyan);
                    
                }
                else
                {
                    Write("                                  ", WindowHeight / 2 - 1, WindowWidth / 2 - 17, ConsoleColor.Cyan);
                    Write("Game is DRAW, guess in same try!!!", WindowHeight / 2, WindowWidth / 2 - 17, ConsoleColor.Cyan);
                    Write("                                  ", WindowHeight / 2 + 1, WindowWidth / 2 - 17, ConsoleColor.Cyan);
                }
                // TODO: New game? Y/N
            } while (Console.ReadKey().Key == ConsoleKey.F12);
        }

        private static void ClearLeftSide()
        {
            for (int i = 0; i < WindowHeight - 12; i++)
            {
                Write("                         ", i, 0);
            }
        }

        private static void WriteHelp()
        {
            for (int i = 0; i < WindowWidth; i++)
            {
                Write("=", WindowHeight - 10, i);
            }
            Console.WriteLine("This is Help for playing game!");
            Console.WriteLine();
            Console.WriteLine("Player and computer write 4-digit secret number.Digits must be all different.Then ,in turn try to guess the number and gives matches count for opponent.");
            Console.WriteLine("If the matching digits are in their right positions, they are \"bulls\", if in different positions, they are \"cows\".");
            Console.WriteLine("Example:");
            Console.WriteLine("Secret number:  4271");
            Console.WriteLine("Opponent's try: 1234");
            Console.WriteLine("Answer: 1 bull and 2 cows. (The bull is \"2\", the cows are \"4\" and \"1\".)");
            Console.Write("The first one to reveal the other's secret number wins the game.");
        }

        static string ComputerGuessNumber()
        {
            string result = "";
            Random randomNumber = new Random();
            int randomPosition = randomNumber.Next(0, allOpportunities.Length - 1);
            string bullscows = BullsCowsCheck(playerSecretNumber, allOpportunities[randomPosition]);
            ComputerTries[0, countGuess] = allOpportunities[randomPosition];
            ComputerTries[1, countGuess] = bullscows;


            result = allOpportunities[randomPosition];

            allOpportunities = allOpportunities.Where(w => w != allOpportunities[randomPosition]).ToArray();
            for (int i = 0; i < allOpportunities.Length; i++)
            {
                for (int j = 0; j < countGuess + 1; j++)
                {
                    string currBullsAndCows = BullsCowsCheck(allOpportunities[i], ComputerTries[0, j]);
                    if (currBullsAndCows != ComputerTries[1, j])
                    {
                        allOpportunities = allOpportunities.Where(w => w != allOpportunities[i]).ToArray();
                        i--;
                        break;
                    }
                }
            }
            return result;
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
                ClearLeftSide();
                Write($"Invalid number \"{number}\"", 0, 0);
                Write($"See Help", 1, 0);
                Write("Example: \"1234\"", 2, 0);
                Write("Press any key to continue", 3, 0);
                Write("...", 4, 0);
                Console.ReadLine();
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
        static void Write(string text, int row, int col, System.ConsoleColor backgroundColor = ConsoleColor.Black)
        {
            Console.SetCursorPosition(col, row);
            Console.BackgroundColor = backgroundColor;
            Console.Write(text);
            Console.BackgroundColor = ConsoleColor.Black;
        }
    }
}
