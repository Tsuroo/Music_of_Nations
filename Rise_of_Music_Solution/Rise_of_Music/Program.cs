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

        private static String currentUserDatFilePath = null;

        private static DateTime currentUserDatFileLastWriteTime = DateTime.Now;

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

            // Set the current date modified for the current user DAT file
            currentUserDatFileLastWriteTime = File.GetLastWriteTime(currentUserDatFilePath);

            // Set the volume for the MusicPlayer
            musicPlayer.Volume = GetCurrentUserVolumeSetting();

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

                        try
                        {
                            xmlDocument.Load(riseOfMusicXmlFilePath);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Failed to read Rise_of_Music.xml; trying again in one second.");
                            continue;
                        }

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

                    // Check to see if the user's DAT has been modified (possibly with a change to music volume).
                    if (File.GetLastWriteTime(currentUserDatFilePath) > currentUserDatFileLastWriteTime)
                    {
                        // The file has been written to; re-check the music volume node and set it in the music player
                        musicPlayer.Volume = GetCurrentUserVolumeSetting();

                        // Reset the last write time
                        currentUserDatFileLastWriteTime = File.GetLastWriteTime(currentUserDatFilePath);
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

        private static float GetCurrentUserVolumeSetting()
        {
            // Open the XML document
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(currentUserDatFilePath);

            // Find the ROOT/PLAYER_PREFS/MUSIC_VOL node
            XmlNode musicVolumeNode = xmlDocument.SelectSingleNode("/ROOT/PLAYER_PREFS/MUSIC_VOL");

            // Get the string value of "value" (ranges from 0-255)
            String musicVolumeString = musicVolumeNode.Attributes["value"].Value;

            // Cast this string to an int
            int musicVolumeInt = int.Parse(musicVolumeString);

            // Calculate the float ranging from 0-1
            // Divide by 255 because that is the max the musicVolumeInt could be.
            float musicVolumeFloat = (float)musicVolumeInt / (float)255;

            return musicVolumeFloat;
        }

        /// <summary>
        /// Reads the current_user.xml file to determine the current user in Rise of Nations.
        /// </summary>
        /// <returns></returns>
        private static String GetCurrentUsername()
        {
            String currentUserFilePath = @"C:\Users\" + Environment.UserName + @"\AppData\Roaming\microsoft games\rise of nations\playerprofile\current_user.xml";

            // Open the XML document
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(currentUserFilePath);

            // Find the CURRENT_USER node
            XmlNode currentUserNode = xmlDocument.SelectSingleNode("/ROOT/CURRENT_USER");

            // Return the name value
            return currentUserNode.Attributes["name"].Value;
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

            // Get the Rise_of_Music.xml path
            riseOfMusicXmlFilePath = @"C:\Users\" + Environment.UserName + @"\AppData\Roaming\microsoft games\rise of nations\Rise_of_Music.xml";
            Console.WriteLine("Setting Rise_of_Music.xml file path to: " + riseOfMusicXmlFilePath);

            // Get the current user in Rise of Nations
            String currentUser = GetCurrentUsername();
            Console.WriteLine("Current user in Rise of Nations: " + currentUser);

            // Check that the user's dat file exists
            String datFilePath = @"C:\Users\" + Environment.UserName + @"\AppData\Roaming\microsoft games\rise of nations\playerprofile\" + currentUser.ToLower() + ".dat";
            if (File.Exists(datFilePath))
            {
                Console.WriteLine("Current user DAT file exists (" + datFilePath + "): True");

                // Set the DAT file path
                currentUserDatFilePath = datFilePath;
            }
            else
            {
                Console.WriteLine("Current user DAT file exists (" + datFilePath + "): False");
            }

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
