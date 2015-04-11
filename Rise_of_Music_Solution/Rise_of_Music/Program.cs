using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace Rise_of_Music
{
    public class Program
    {
        private static MusicPlayer musicPlayer = null;

        private static String riseOfMusicXmlFilePath = null;

        /// <summary>
        /// The entry point to Rise of Music.
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
                Console.WriteLine("Rise of Music initialized successfully");
            }

            // Create the MusicPlayer object
            musicPlayer = new MusicPlayer();

            // Begin looking for a Rise_of_Music.xml file to read
            new Thread(() =>
            {
                while (true)
                {
                    // If a Rise_of_Music.xml file exists
                    if (File.Exists(riseOfMusicXmlFilePath))
                    {
                        // Open the XML document
                        XmlDocument xmlDocument = new XmlDocument();
                        xmlDocument.Load(riseOfMusicXmlFilePath);

                        // Find the MUSIC_MOOD node
                        XmlNode musicMoodNode = xmlDocument.SelectSingleNode("/ROOT/MUSIC_MOOD");

                        // Set the current music mood to the value in the XML file
                        musicPlayer.Mood = musicMoodNode.InnerText;

                        // If we haven't started the music player yet - start it
                        if (!musicPlayer.HasStartedPlaying)
                        {
                            musicPlayer.Play();
                        }

                        // Remove the Rise_of_Music.xml
                        try
                        {
                            // Delete it
                            File.Delete(riseOfMusicXmlFilePath);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Unable to delete current \"Rise_of_Music.xml\" file.  Please delete file and restart.");
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
            Console.WriteLine("=================================");
            Console.WriteLine("          Rise of Music");
            Console.WriteLine("=================================");

            Console.WriteLine("Initializing");

            // Get the username and Rise_of_Music.xml path
            riseOfMusicXmlFilePath = @"C:\Users\" + Environment.UserName + @"\AppData\Roaming\microsoft games\rise of nations\Rise_of_Music.xml";

            Console.WriteLine("Setting Rise_of_Music.xml file path to: " + riseOfMusicXmlFilePath);
            

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

            // If a Rise_of_Music.xml file exists
            
            if (File.Exists(riseOfMusicXmlFilePath))
            {
                try
                {
                    // Delete it
                    File.Delete(riseOfMusicXmlFilePath);

                    Console.WriteLine("Removed current \"Rise_of_Music.xml\" file successfully.");
                }
                catch (Exception e)
                {
                    Console.WriteLine("Unable to delete current \"Rise_of_Music.xml\" file.  Please delete file and restart.");
                    return false;
                }
            }

            // Returns true if all directories exist, false if even one does not exist
            return (battleDefeatDirExists && battleVictoryDirExists && economicDirExists && loseDirExists && winDirExists);
        }
    }
}
