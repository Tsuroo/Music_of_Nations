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

        private static String riseOfMusicCurrentGameDirPath = null;

        private static String playerNumber = null;

        private static String currentUserDatFilePath = null;

        private static DateTime currentUserDatFileLastWriteTime = DateTime.Now;

        public static ConfigXml Config { get; set; }

        /// <summary>
        /// The entry point to Rise of Music.
        /// </summary>
        /// <param name="args">Not currently used.</param>
        public static void Main(string[] args)
        {
            Console.WriteLine("============================================");
            Console.WriteLine("          Rise of Music v. " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version);
            Console.WriteLine("============================================");

            // Initializes the application and returns success or failure
            bool initSuccess = Init();

            // If the app failed to initialize
            if (!initSuccess)
            {
                Console.WriteLine("Failed to initialize");
                Console.WriteLine();

                // Allow the user to exit once they have read any messages
                Console.Write("Press any key to exit...");
                Console.ReadKey();

                // Exit with failure status code
                Environment.Exit(-1);
            }
            else // Else, the app successfully initialized
            {
                Console.WriteLine("Rise of Music initialized successfully");
                Console.WriteLine();
            }

            // Set the player color
            SetPlayerColor();

            // Set the volume for the MusicPlayer
            musicPlayer.Volume = GetCurrentUserVolumeSetting();

            Console.WriteLine("Waiting for music mood info file...");

            // Begin looking for a music_mood.xml file to read
            new Thread(() =>
            {
                while (true)
                {
                    try
                    {
                        // If a music_mood.xml file exists
                        if (File.Exists(riseOfMusicCurrentGameDirPath +"players\\" + playerNumber + "\\music_mood.xml"))
                        {
                            // Open the XML document
                            XmlDocument xmlDocument = new XmlDocument();

                            try
                            {
                                xmlDocument.Load(riseOfMusicCurrentGameDirPath + "players\\" + playerNumber + "\\music_mood.xml");
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine("Failed to read music_mood.xml; trying again in one second.");
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

                            // Remove the music_mood.xml
                            try
                            {
                                // Delete it
                                File.Delete(riseOfMusicCurrentGameDirPath + "players\\" + playerNumber + "\\music_mood.xml");
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine("Unable to delete current \"music_mood.xml\" file.  Please delete file and restart.");
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
                    catch (Exception e)
                    {
                        Console.WriteLine("Exception: " + e.Message);
                    }
                }
            }).Start();
            
            // Run this application.  The user will 'X' out when they are done.
            System.Windows.Forms.Application.Run();

            // Dispose of the MusicPlayer object
            musicPlayer.Dispose();

            // Exit with success status code
            Environment.Exit(0);
        }

        private static void SetPlayerColor()
        {
            CurrentGameXml currentGameXml = new CurrentGameXml();

            Console.WriteLine("Color Options");
            Console.WriteLine("--------------");
            Console.WriteLine("Red        (1)");
            Console.WriteLine("Blue       (2)");
            Console.WriteLine("Purple     (3)");
            Console.WriteLine("Green      (4)");
            Console.WriteLine("Yellow     (5)");
            Console.WriteLine("Light Blue (6)");
            Console.WriteLine("White      (7)");
            Console.WriteLine("Orange     (8)");
            
            // Loop until [1-8] is typed
            while (true)
            {
                Console.Write("What color will you play as? [1-8]: ");

                // Get the player number
                String playerNumber = Console.ReadKey().KeyChar.ToString();

                // If the player number chosen is valid
                if (playerNumber == "1" || playerNumber == "2" || playerNumber == "3" || playerNumber == "4" ||
                    playerNumber == "5" || playerNumber == "6" || playerNumber == "7" || playerNumber == "8")
                {
                    Console.WriteLine();
                    Console.WriteLine("Setting player color to: " + playerNumber);

                    switch (playerNumber)
                    {
                        case "1":
                            currentGameXml.PlayerColor = "Red";
                            break;
                        case "2":
                            currentGameXml.PlayerColor = "Blue";
                            break;
                        case "3":
                            currentGameXml.PlayerColor = "Purple";
                            break;
                        case "4":
                            currentGameXml.PlayerColor = "Green";
                            break;
                        case "5":
                            currentGameXml.PlayerColor = "Yellow";
                            break;
                        case "6":
                            currentGameXml.PlayerColor = "Light Blue";
                            break;
                        case "7":
                            currentGameXml.PlayerColor = "White";
                            break;
                        case "8":
                            currentGameXml.PlayerColor = "Orange";
                            break;
                    }

                    Program.playerNumber = playerNumber;

                    // Save to config file and break
                    currentGameXml.PlayerNumber = playerNumber;
                    currentGameXml.Save();
                    break;
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("Error: Invalid choice.");
                }
            }
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

        private static void CopyMusic()
        {

        }

        /// <summary>
        /// Initializes the program and checks for existance of necessary directories.
        /// </summary>
        /// <returns>True if successfully initialized, false otherwise.</returns>
        private static bool Init()
        {
            Console.WriteLine("Initializing...");

            // Create the directory
            Directory.CreateDirectory(@"C:\Users\" + Environment.UserName + @"\AppData\Roaming\microsoft games\rise of nations\Rise_of_Music\");

            Config = new ConfigXml();

            // Create music mood directories
            Directory.CreateDirectory(@"C:\Users\" + Environment.UserName + @"\AppData\Roaming\microsoft games\rise of nations\Rise_of_Music\sounds\tracks\battle_defeat\");
            Directory.CreateDirectory(@"C:\Users\" + Environment.UserName + @"\AppData\Roaming\microsoft games\rise of nations\Rise_of_Music\sounds\tracks\battle_victory\");
            Directory.CreateDirectory(@"C:\Users\" + Environment.UserName + @"\AppData\Roaming\microsoft games\rise of nations\Rise_of_Music\sounds\tracks\economic\");
            Directory.CreateDirectory(@"C:\Users\" + Environment.UserName + @"\AppData\Roaming\microsoft games\rise of nations\Rise_of_Music\sounds\tracks\lose\");
            Directory.CreateDirectory(@"C:\Users\" + Environment.UserName + @"\AppData\Roaming\microsoft games\rise of nations\Rise_of_Music\sounds\tracks\win\");

            riseOfMusicCurrentGameDirPath = @"C:\Users\" + Environment.UserName + @"\AppData\Roaming\microsoft games\rise of nations\Rise_of_Music\current_game\";

            // If a players dir exists
            if (Directory.Exists(riseOfMusicCurrentGameDirPath))
            {
                // Delete everything
                Directory.Delete(riseOfMusicCurrentGameDirPath, true);
            }

            // Create current game directories
            Directory.CreateDirectory(riseOfMusicCurrentGameDirPath + @"players\1\");
            Directory.CreateDirectory(riseOfMusicCurrentGameDirPath + @"players\2\");
            Directory.CreateDirectory(riseOfMusicCurrentGameDirPath + @"players\3\");
            Directory.CreateDirectory(riseOfMusicCurrentGameDirPath + @"players\4\");
            Directory.CreateDirectory(riseOfMusicCurrentGameDirPath + @"players\5\");
            Directory.CreateDirectory(riseOfMusicCurrentGameDirPath + @"players\6\");
            Directory.CreateDirectory(riseOfMusicCurrentGameDirPath + @"players\7\");
            Directory.CreateDirectory(riseOfMusicCurrentGameDirPath + @"players\8\");

            // Get the current user in Rise of Nations
            String currentUser = GetCurrentUsername();

            // Check that the user's dat file exists
            String datFilePath = @"C:\Users\" + Environment.UserName + @"\AppData\Roaming\microsoft games\rise of nations\playerprofile\" + currentUser.ToLower() + ".dat";
            if (File.Exists(datFilePath))
            {
                // Set the DAT file path
                currentUserDatFilePath = datFilePath;
            }
            else
            {
                Console.WriteLine("Current user DAT file does not exists (" + datFilePath + ")");
                return false;
            }

            InterceptKeys.OnTildePressedThreeTimesFast += InterceptKeys_OnTildePressedThreeTimesFast;
            InterceptKeys.OnRightControlPressedThreeTimesFast += InterceptKeys_OnRightControlPressedThreeTimesFast;

            String battleDefeatDirPath = @"C:\Users\" + Environment.UserName + @"\AppData\Roaming\microsoft games\rise of nations\Rise_of_Music\sounds\tracks\battle_defeat\";
            String battleVictoryDirPath = @"C:\Users\" + Environment.UserName + @"\AppData\Roaming\microsoft games\rise of nations\Rise_of_Music\sounds\tracks\battle_victory\";
            String economicDirPath = @"C:\Users\" + Environment.UserName + @"\AppData\Roaming\microsoft games\rise of nations\Rise_of_Music\sounds\tracks\economic\";
            String loseDirPath = @"C:\Users\" + Environment.UserName + @"\AppData\Roaming\microsoft games\rise of nations\Rise_of_Music\sounds\tracks\lose\";
            String winDirPath = @"C:\Users\" + Environment.UserName + @"\AppData\Roaming\microsoft games\rise of nations\Rise_of_Music\sounds\tracks\win\";

            // Check the existance of these directories
            bool battleDefeatDirExists = Directory.Exists(battleDefeatDirPath);
            bool battleVictoryDirExists = Directory.Exists(battleVictoryDirPath);
            bool economicDirExists = Directory.Exists(economicDirPath);
            bool loseDirExists = Directory.Exists(loseDirPath);
            bool winDirExists = Directory.Exists(winDirPath);

            // Set the current date modified for the current user DAT file
            currentUserDatFileLastWriteTime = File.GetLastWriteTime(currentUserDatFilePath);

            // Create the MusicPlayer object
            musicPlayer = new MusicPlayer();

            // Returns true if all directories exist, false if even one does not exist
            return (battleDefeatDirExists && battleVictoryDirExists && economicDirExists && loseDirExists && winDirExists);
        }

        private static void InterceptKeys_OnTildePressedThreeTimesFast(object sender, EventArgs e)
        {
            // Set the music mood to win
            musicPlayer.Mood = "win";
        }

        static void InterceptKeys_OnRightControlPressedThreeTimesFast(object sender, EventArgs e)
        {
            // Set the music mood to lose
            musicPlayer.Mood = "lose";
        }
    }
}
