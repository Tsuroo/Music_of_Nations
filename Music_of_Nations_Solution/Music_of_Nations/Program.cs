using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Music_of_Nations
{
    public class Program
    {
        private static String SoundsTracksDirPath = "sounds/tracks/";
        private static String BattleDefeatDirPath = SoundsTracksDirPath + "battle_defeat/";
        private static String BattleVictoryDirPath = SoundsTracksDirPath + "battle_victory/";
        private static String EconomicDirPath = SoundsTracksDirPath + "economic/";
        private static String LoseDirPath = SoundsTracksDirPath + "lose/";
        private static String WinDirPath = SoundsTracksDirPath + "win/";

        public static void Main(string[] args)
        {
            PrintWelcomeMessage();

            bool initSuccess = Init();

            if (!initSuccess)
            {
                Console.WriteLine("Failed to initialize");

                Console.Write("Press any key to continue...");
                Console.ReadKey();

                Environment.Exit(-1);
            }
            else
            {
                Console.WriteLine("Music of Nations initialized successfully");
            }

            Console.Write("Press any key to continue...");
            Console.ReadKey();
        }

        private static bool Init()
        {
            Console.WriteLine("Initializing");

            bool battleDefeatDirExists = Directory.Exists(BattleDefeatDirPath);
            bool battleVictoryDirExists = Directory.Exists(BattleVictoryDirPath);
            bool economicDirExists = Directory.Exists(EconomicDirPath);
            bool loseDirExists = Directory.Exists(LoseDirPath);
            bool winDirExists = Directory.Exists(WinDirPath);

            Console.WriteLine("Directory exists (" + BattleDefeatDirPath + "): " + battleDefeatDirExists);
            Console.WriteLine("Directory exists (" + BattleVictoryDirPath + "): " + battleVictoryDirExists);
            Console.WriteLine("Directory exists (" + EconomicDirPath + "): " + economicDirExists);
            Console.WriteLine("Directory exists (" + LoseDirPath + "): " + loseDirExists);
            Console.WriteLine("Directory exists (" + WinDirPath + "): " + winDirExists);

            return (battleDefeatDirExists && battleVictoryDirExists && economicDirExists && loseDirExists && winDirExists);
        }

        private static void PrintWelcomeMessage()
        {
            Console.WriteLine("==============================");
            Console.WriteLine("       Music of Nations");
            Console.WriteLine("==============================");
        }
    }
}
