using CSCore.SoundOut;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Music_of_Nations
{
    public class MusicPlayer : IDisposable
    {
        public bool HasStartedPlaying { private set; get; }

        /// <summary>
        /// Gets or sets the current music mood.
        /// </summary>
        public String Mood
        {
            get
            {
                return _Mood;
            }

            set
            {
                // If the mood is "age_up"
                if (value == "age_up")
                {
                    // Don't change the value, but fade out the current song and fade in a new one
                    this.FadeOutIn();
                }
                else // Else, set the mood
                {
                    Console.WriteLine("Setting music mood to: " + value);
                    this._Mood = value;
                }
            }
        }
        private String _Mood = "economic";

        /// <summary>
        /// The device to send audio to.
        /// </summary>
        private ISoundOut SoundOut { get; set; }

        /// <summary>
        /// Holds every audio file in each of the music mood directories.
        /// </summary>
        private Dictionary<String, List<AudioFile>> MoodToAudioFileDictionary = new Dictionary<String, List<AudioFile>>();

        /// <summary>
        /// The random number generator that will be used to select new audio files.
        /// </summary>
        private Random r = new Random();

        /// <summary>
        /// The audio file currently being played.
        /// </summary>
        private AudioFile currentAudioFile = null;

        public MusicPlayer()
        {
            // The MusicPlayer has not started playing yet
            this.HasStartedPlaying = false;

            // Set the SoundOut object
            this.SoundOut = GetSoundOut();

            // Set the event handler for when audio playback has stopped from the SoundOut
            this.SoundOut.Stopped += AudioStopped;

            // Init the dictionary for each mood
            this.InitAudioFilesForMood("battle_defeat");
            this.InitAudioFilesForMood("battle_victory");
            this.InitAudioFilesForMood("economic");
            this.InitAudioFilesForMood("lose");
            this.InitAudioFilesForMood("win");
        }

        /// <summary>
        /// Inits the MoodToAudioFile and MoodToAudioFileDictionary with the files from each mood directory.
        /// </summary>
        /// <param name="mood"></param>
        private void InitAudioFilesForMood(String mood)
        {
            // Create the list of AudioFile objects
            List<AudioFile> audioFiles = new List<AudioFile>();

            foreach (String filePath in Directory.GetFiles("sounds/tracks/" + mood))
            {
                // Make sure we're only using WAV or MP3 files (ignoring case)
                if (filePath.EndsWith("wav", true, System.Globalization.CultureInfo.CurrentCulture) ||
                    filePath.EndsWith("mp3", true, System.Globalization.CultureInfo.CurrentCulture))
                {
                    // Create a new AudioFile
                    AudioFile audioFile = new AudioFile(filePath, mood);

                    // Add this file path to the list of AudioFile objects
                    audioFiles.Add(audioFile);
                }
            }

            // Add the list of AudioFile objects in the dictionary for the given mood
            this.MoodToAudioFileDictionary.Add(mood, audioFiles);
        }

        /// <summary>
        /// Gets a device to send audio to.
        /// </summary>
        /// <returns>If WasapiOut is supported, a WasapiOut object is returned, else a DirectSoundOut object is returned.</returns>
        private ISoundOut GetSoundOut()
        {
            if (WasapiOut.IsSupportedOnCurrentPlatform)
                return new WasapiOut();
            else
                return new DirectSoundOut();
        }

        /// <summary>
        /// Plays a random song in the current music mood directory.
        /// </summary>
        public void Play()
        {
            // Set the fact that we're now playing audio
            this.HasStartedPlaying = true;

            // Get the audio file path to play
            String audioFilePath = this.GetUnplayedFileForCurrentMood();

            // If the audio file path is not null, then play the file
            if (audioFilePath != null)
            {
                // Initialize the SoundOut with an audio file
                this.SoundOut.Initialize(CSCore.Codecs.CodecFactory.Instance.GetCodec(audioFilePath));

                // Play the audio file
                this.SoundOut.Play();
            }
        }

        /// <summary>
        /// Fades out the current audio file and fades in a new one
        /// </summary>
        private void FadeOutIn()
        {
            new Thread(() =>
            {
                // 10 iterations of the volume decreasing
                for (int i = 9; i > 0; --i)
                {
                    Console.WriteLine("Waiting...");
                    // Wait inbetween each volume change to make it smooth
                    Thread.Sleep(150);

                    switch (i)
                    {
                        case 9:
                            this.SoundOut.Volume = .9f;
                            break;
                        case 8:
                            this.SoundOut.Volume = .8f;
                            break;
                        case 7:
                            this.SoundOut.Volume = .7f;
                            break;
                        case 6:
                            this.SoundOut.Volume = .6f;
                            break;
                        case 5:
                            this.SoundOut.Volume = .5f;
                            break;
                        case 4:
                            this.SoundOut.Volume = .4f;
                            break;
                        case 3:
                            this.SoundOut.Volume = .3f;
                            break;
                        case 2:
                            this.SoundOut.Volume = .2f;
                            break;
                        case 1:
                            this.SoundOut.Volume = .1f;
                            break;
                        case 0:
                            this.SoundOut.Volume = .0f;
                            break;
                    }
                }

                // Stop the song (this will start a new song as well)
                this.SoundOut.Stop();
            }).Start();
        }

        /// <summary>
        /// Returns a file from the current mood directory.
        /// This function will only repeat a file when every other file has been played at least once.
        /// </summary>
        /// <returns></returns>
        private String GetUnplayedFileForCurrentMood()
        {
            // Get the audio file list for the current music mood
            List<AudioFile> audioFiles = this.MoodToAudioFileDictionary[this.Mood];

            // If the list is empty, return null
            if (audioFiles.Count == 0)
                return null;

            // First, let's check to make sure that at least one audio file that hasn't been played
            bool atLeastOneHasNotBeenPlayed = false;
            foreach (AudioFile audioFile in audioFiles)
            {
                if (!audioFile.HasBeenPlayed)
                {
                    atLeastOneHasNotBeenPlayed = true;
                    break;
                }
            }

            // If all of the audio files have been played in this mood, reset the HasBeenPlayed property
            if (!atLeastOneHasNotBeenPlayed)
            {
                foreach(AudioFile audioFile in audioFiles)
                {
                    // Make sure to not change the current AudioFile, because we don't want to play that song twice in a row.
                    // It'll be reset on the next refresh like this.
                    if (audioFile != this.currentAudioFile)
                    {
                        audioFile.HasBeenPlayed = false;
                    }
                }
            }

            // Select a random index
            int index = this.r.Next(0, audioFiles.Count);

            // Loop until we find a file to play and return that file
            while (true)
            {
                // If the selected audio file hasn't been played
                if (!audioFiles[index].HasBeenPlayed)
                {
                    // Set it's HasBeenPlayed value
                    audioFiles[index].HasBeenPlayed = true;

                    // Set this audio file as the current audio file
                    this.currentAudioFile = audioFiles[index];

                    // Return this audio file to be played
                    return audioFiles[index].Path;
                }
                else // The file at this index has already been played; try the next index
                {
                    // Try the next index
                    ++index;

                    // If the index is equal to the total number of audio files in the list, set it to 0
                    if (index >= audioFiles.Count)
                    {
                        // Reset the index to zero
                        index = 0;
                    }
                }
            }
        }

        /// <summary>
        /// Called whenever the SoundOut has finished playing the current audio file.  Calls the Play() function to start another song.
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e"></param>
        private void AudioStopped(object o, EventArgs e)
        {
            new Thread(() => {
                this.Play();
            }).Start();
        }

        /// <summary>
        /// Cleans up the SoundOut object.
        /// </summary>
        public void Dispose()
        {
            this.SoundOut.Dispose();
        }
    }
}
