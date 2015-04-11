using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Music_of_Nations
{
    public class Program
    {
        private static MusicPlayer musicPlayer = null;

        /// <summary>
        /// The entry point to Music of Nations.
        /// </summary>
        /// <param name="args">Not currently used.</param>
        public static void Main(string[] args)
        {
            // Initializes the application and returns success or failure
            bool initSuccess = Init();

            // If the app failed to initialize
            if (!initSuccess)
            {
                Console.WriteLine("Failed to initialize");

                // Allow the user to exit once they have read any messages
                Console.Write("Press any key to exit...");
                Console.ReadKey();

                // Exit with failure status code
                Environment.Exit(-1);
            }
            else // Else, the app successfully initialized
            {
                Console.WriteLine("Music of Nations initialized successfully");
            }

            // Create the MusicPlayer object
            musicPlayer = new MusicPlayer();

            // Begin looking for a Music_of_Nations.xml file to read
            new Thread(() =>
            {
                while (true)
                {
                    // If a Music_of_Nations.xml file exists
                    if (File.Exists("Music_of_Nations.xml"))
                    {
                        // Parse it for the "music_mood" value
                        using (StreamReader file = new StreamReader("Music_of_Nations.xml"))
                        {
                            // While it's not the end of the file
                            while (!file.EndOfStream)
                            {
                                // Get the next line
                                String line = file.ReadLine();

                                // Make sure this line has the key we're looking for
                                if (line.Contains("music_mood"))
                                {
                                    // Split the string by "music_mood="
                                    String[] splitLinePieces = line.Split(new String[] { "music_mood=" }, StringSplitOptions.RemoveEmptyEntries);
                                    
                                    // Take the second piece, and split again to get just the value we're looking for
                                    String[] splitLinePieces2 = splitLinePieces[1].Split(new char[] { '<' });

                                    // Read the value of the first element - this is our new music mood
                                    musicPlayer.Mood = splitLinePieces2[0];

                                    // If we haven't started the music player yet - start it
                                    if (!musicPlayer.HasStartedPlaying)
                                    {
                                        musicPlayer.Play();
                                    }
                                }
                            }
                        }
                    }

                    // Wait 1 second before checking again
                    Thread.Sleep(1000);
                }
            }).Start();

            // Allow the user to exit once they have read any messages
            Console.WriteLine("NOTE: Press any key (at any time) or close the window to exit.");
            Console.ReadKey();

            // Dispose of the MusicPlayer object
            musicPlayer.Dispose();

            // Exit with success status code
            Environment.Exit(0);
        }

        /// <summary>
        /// Initializes the program and checks for existance of necessary directories.
        /// </summary>
        /// <returns>True if successfully initialized, false otherwise.</returns>
        private static bool Init()
        {
            Console.WriteLine("==============================");
            Console.WriteLine("       Music of Nations");
            Console.WriteLine("==============================");

            Console.WriteLine("Initializing");

            String battleDefeatDirPath = "sounds/tracks/battle_defeat/";
            String battleVictoryDirPath = "sounds/tracks/battle_victory/";
            String economicDirPath = "sounds/tracks/economic/";
            String loseDirPath = "sounds/tracks/lose/";
            String winDirPath = "sounds/tracks/win/";

            // Check the existance of these directories
            bool battleDefeatDirExists = Directory.Exists(battleDefeatDirPath);
            bool battleVictoryDirExists = Directory.Exists(battleVictoryDirPath);
            bool economicDirExists = Directory.Exists(economicDirPath);
            bool loseDirExists = Directory.Exists(loseDirPath);
            bool winDirExists = Directory.Exists(winDirPath);

            // Print the results
            Console.WriteLine("Directory exists (" + battleDefeatDirPath + "): " + battleDefeatDirExists);
            Console.WriteLine("Directory exists (" + battleVictoryDirPath + "): " + battleVictoryDirExists);
            Console.WriteLine("Directory exists (" + economicDirPath + "): " + economicDirExists);
            Console.WriteLine("Directory exists (" + loseDirPath + "): " + loseDirExists);
            Console.WriteLine("Directory exists (" + winDirPath + "): " + winDirExists);

            // If a Music_of_Nations.xml file exists
            if (File.Exists("Music_of_Nations.xml"))
            {
                try
                {
                    // Delete it
                    File.Delete("Music_of_Nations.xml");

                    Console.WriteLine("Removed current \"Music_of_Nations.xml\" file successfully.");
                }
                catch (Exception e)
                {
                    Console.WriteLine("Unable to delete current \"Music_of_Nations.xml\" file.  Please delete file and restart.");
                    return false;
                }
            }

            // Returns true if all directories exist, false if even one does not exist
            return (battleDefeatDirExists && battleVictoryDirExists && economicDirExists && loseDirExists && winDirExists);
        }
    }
}
