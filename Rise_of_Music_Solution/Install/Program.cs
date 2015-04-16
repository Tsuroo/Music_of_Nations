using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Install
{
    class Program
    {
        private static String riseOfNationsRoot = null;

        static void Main(string[] args)
        {
            Console.WriteLine("=================================");
            Console.WriteLine("     Rise of Music Installer");
            Console.WriteLine("=================================");

            // Prepare for install
            bool success = Prepare();
            if (!success)
            {
                Console.WriteLine("Failed to prepare for install");
                Console.Write("Press any key to continue...");
                Console.ReadKey();
                Environment.Exit(-1);
            }

            // Copy files
            success = Copy();
            if (!success)
            {
                Console.WriteLine("Failed to copy files during install");
                Console.Write("Press any key to continue...");
                Console.ReadKey();
                Environment.Exit(-2);
            }

            Console.WriteLine();
            Console.WriteLine("Rise of Music installed successfully");
            Console.Write("Press any key to continue...");
            Console.ReadKey();
            Environment.Exit(0);
        }

        private static bool Copy()
        {
            try
            {
                Console.WriteLine("Copying files");

                String riseOfMusicRoot = riseOfNationsRoot + @"scenario\Scripts\Rise_of_Music\";

                // Create directories
                Directory.CreateDirectory(riseOfMusicRoot);
                Directory.CreateDirectory(riseOfMusicRoot + @"sounds\tracks\battle_defeat\");
                Directory.CreateDirectory(riseOfMusicRoot + @"sounds\tracks\battle_victory\");
                Directory.CreateDirectory(riseOfMusicRoot + @"sounds\tracks\economic\");
                Directory.CreateDirectory(riseOfMusicRoot + @"sounds\tracks\lose\");
                Directory.CreateDirectory(riseOfMusicRoot + @"sounds\tracks\win\");

                // Copy files from build
                CopyFile(@"build\CSCore.dll", riseOfMusicRoot + "CSCore.dll");
                CopyFile(@"build\CSCore.xml", riseOfMusicRoot + "CSCore.xml");
                CopyFile(@"build\LICENSE", riseOfMusicRoot + "LICENSE");
                CopyFile(@"build\README.md", riseOfMusicRoot + "README.md");
                CopyFile(@"build\Rise_of_Music.bhs", riseOfMusicRoot + "Rise_of_Music.bhs");
                CopyFile(@"build\Rise_of_Music.exe", riseOfMusicRoot + "Rise_of_Music.exe");
                CopyFile(@"build\sounds\tracks\battle_defeat\README.txt", riseOfMusicRoot + @"sounds\tracks\battle_defeat\README.txt");
                CopyFile(@"build\sounds\tracks\battle_victory\README.txt", riseOfMusicRoot + @"sounds\tracks\battle_victory\README.txt");
                CopyFile(@"build\sounds\tracks\economic\README.txt", riseOfMusicRoot + @"sounds\tracks\economic\README.txt");
                CopyFile(@"build\sounds\tracks\lose\README.txt", riseOfMusicRoot + @"sounds\tracks\lose\README.txt");
                CopyFile(@"build\sounds\tracks\win\README.txt", riseOfMusicRoot + @"sounds\tracks\win\README.txt");

                // Copy battle_defeat music
                CopyFile(riseOfNationsRoot + @"sounds\tracks\Allerton.wav", riseOfMusicRoot + @"sounds\tracks\battle_defeat\Allerton.wav");
                CopyFile(riseOfNationsRoot + @"sounds\tracks\BattleAtWitchCreek.wav", riseOfMusicRoot + @"sounds\tracks\battle_defeat\BattleAtWitchCreek.wav");
                CopyFile(riseOfNationsRoot + @"sounds\tracks\DesertWind.wav", riseOfMusicRoot + @"sounds\tracks\battle_defeat\DesertWind.wav");
                CopyFile(riseOfNationsRoot + @"sounds\tracks\Misfire.wav", riseOfMusicRoot + @"sounds\tracks\battle_defeat\Misfire.wav");
                CopyFile(riseOfNationsRoot + @"sounds\tracks\MistAtDawn.wav", riseOfMusicRoot + @"sounds\tracks\battle_defeat\MistAtDawn.wav");
                CopyFile(riseOfNationsRoot + @"sounds\tracks\Osaka.wav", riseOfMusicRoot + @"sounds\tracks\battle_defeat\Osaka.wav");
                CopyFile(riseOfNationsRoot + @"sounds\tracks\Tribes.wav", riseOfMusicRoot + @"sounds\tracks\battle_defeat\Tribes.wav");

                // Copy battle_victory music
                CopyFile(riseOfNationsRoot + @"sounds\tracks\Attack.wav", riseOfMusicRoot + @"sounds\tracks\battle_victory\Attack.wav");
                CopyFile(riseOfNationsRoot + @"sounds\tracks\Galleons.wav", riseOfMusicRoot + @"sounds\tracks\battle_victory\Galleons.wav");
                CopyFile(riseOfNationsRoot + @"sounds\tracks\HighStrung.wav", riseOfMusicRoot + @"sounds\tracks\battle_victory\HighStrung.wav");
                CopyFile(riseOfNationsRoot + @"sounds\tracks\Revolver.wav", riseOfMusicRoot + @"sounds\tracks\battle_victory\Revolver.wav");
                CopyFile(riseOfNationsRoot + @"sounds\tracks\TheRussian.wav", riseOfMusicRoot + @"sounds\tracks\battle_victory\TheRussian.wav");
                CopyFile(riseOfNationsRoot + @"sounds\tracks\WilliamWallace.wav", riseOfMusicRoot + @"sounds\tracks\battle_victory\WilliamWallace.wav");

                // Copy economic music
                CopyFile(riseOfNationsRoot + @"sounds\tracks\AcrossTheBog.wav", riseOfMusicRoot + @"sounds\tracks\economic\AcrossTheBog.wav");
                CopyFile(riseOfNationsRoot + @"sounds\tracks\Bengal.wav", riseOfMusicRoot + @"sounds\tracks\economic\Bengal.wav");
                CopyFile(riseOfNationsRoot + @"sounds\tracks\Brazil.wav", riseOfMusicRoot + @"sounds\tracks\economic\Brazil.wav");
                CopyFile(riseOfNationsRoot + @"sounds\tracks\DarkForest.wav", riseOfMusicRoot + @"sounds\tracks\economic\DarkForest.wav");
                CopyFile(riseOfNationsRoot + @"sounds\tracks\Eire.wav", riseOfMusicRoot + @"sounds\tracks\economic\Eire.wav");
                CopyFile(riseOfNationsRoot + @"sounds\tracks\Gobi.wav", riseOfMusicRoot + @"sounds\tracks\economic\Gobi.wav");
                CopyFile(riseOfNationsRoot + @"sounds\tracks\HalfMoon.wav", riseOfMusicRoot + @"sounds\tracks\economic\HalfMoon.wav");
                CopyFile(riseOfNationsRoot + @"sounds\tracks\Hearth.wav", riseOfMusicRoot + @"sounds\tracks\economic\Hearth.wav");
                CopyFile(riseOfNationsRoot + @"sounds\tracks\Indochine.wav", riseOfMusicRoot + @"sounds\tracks\economic\Indochine.wav");
                CopyFile(riseOfNationsRoot + @"sounds\tracks\Morocco.wav", riseOfMusicRoot + @"sounds\tracks\economic\Morocco.wav");
                CopyFile(riseOfNationsRoot + @"sounds\tracks\OverTheDam.wav", riseOfMusicRoot + @"sounds\tracks\economic\OverTheDam.wav");
                CopyFile(riseOfNationsRoot + @"sounds\tracks\PeacePipe.wav", riseOfMusicRoot + @"sounds\tracks\economic\PeacePipe.wav");
                CopyFile(riseOfNationsRoot + @"sounds\tracks\Rockets.wav", riseOfMusicRoot + @"sounds\tracks\economic\Rockets.wav");
                CopyFile(riseOfNationsRoot + @"sounds\tracks\SacrificeToTheSun.wav", riseOfMusicRoot + @"sounds\tracks\economic\SacrificeToTheSun.wav");
                CopyFile(riseOfNationsRoot + @"sounds\tracks\Santiago.wav", riseOfMusicRoot + @"sounds\tracks\economic\Santiago.wav");
                CopyFile(riseOfNationsRoot + @"sounds\tracks\SimpleSong.wav", riseOfMusicRoot + @"sounds\tracks\economic\SimpleSong.wav");
                CopyFile(riseOfNationsRoot + @"sounds\tracks\SriLanka.wav", riseOfMusicRoot + @"sounds\tracks\economic\SriLanka.wav");
                CopyFile(riseOfNationsRoot + @"sounds\tracks\TheHague(ruffmix2).wav", riseOfMusicRoot + @"sounds\tracks\economic\TheHague(ruffmix2).wav");
                CopyFile(riseOfNationsRoot + @"sounds\tracks\ThunderBird.wav", riseOfMusicRoot + @"sounds\tracks\economic\ThunderBird.wav");
                CopyFile(riseOfNationsRoot + @"sounds\tracks\WingAndAPrayer.wav", riseOfMusicRoot + @"sounds\tracks\economic\WingAndAPrayer.wav");

                // Copy lose music
                CopyFile(riseOfNationsRoot + @"sounds\tracks\Waterloo.wav", riseOfMusicRoot + @"sounds\tracks\lose\Waterloo.wav");

                // Copy win music
                CopyFile(riseOfNationsRoot + @"sounds\tracks\ArcDeTriomphe.wav", riseOfMusicRoot + @"sounds\tracks\win\ArcDeTriomphe.wav");

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

                return false;
            }
        }

        private static void CopyFile(String from, String to)
        {
            Console.Write("Copying \"" + from + "\" to \"" + to + "\"...");
            File.Copy(from, to);
            Console.WriteLine("[DONE]");
        }

        private static bool Prepare()
        {
            Console.WriteLine("Preparing to install");

            // If we're running in a 64-bit operating system
            if (Environment.Is64BitOperatingSystem)
            {
                // Set the root path
                riseOfNationsRoot = @"C:\Program Files (x86)\Steam\SteamApps\common\Rise of Nations\";
            }
            else
            {
                // Set the root path
                riseOfNationsRoot = @"C:\Program Files\Steam\SteamApps\common\Rise of Nations\";
            }
            

            // Check to see if the Rise of Nations directory exists to verify it's been installed
            if (!Directory.Exists(riseOfNationsRoot))
            {
                Console.WriteLine(
                    "Rise of Nations directory does not exist; the game may not be installed.  " +
                    "Please verify Rise of Nations is installed and try again.");

                return false;
            }

            // Make sure the scripts directory exists.
            // This likely wouldn't be an issue unless the user had delete these directories.
            if (!File.Exists(riseOfNationsRoot + @"scenario\Scripts\"))
            {
                Console.Write(@"scenario\Scripts\ directory does not exist.  Creating now...");

                // Create the directories
                Directory.CreateDirectory(riseOfNationsRoot + @"scenario\Scripts\");

                Console.WriteLine("[DONE]");
            }

            return true;
        }
    }
}
