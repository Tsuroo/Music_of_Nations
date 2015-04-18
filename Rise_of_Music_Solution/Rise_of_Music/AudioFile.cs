using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rise_of_Music
{
    public class AudioFile
    {
        /// <summary>
        /// Gets the audio file path.
        /// </summary>
        public String Path { get; private set; }

        /// <summary>
        /// Gets or sets the flag that states whether or not the audio file has been played.
        /// </summary>
        public bool HasBeenPlayed { get; set; }

        public AudioFile(String path)
        {
            this.Path = path;
            this.HasBeenPlayed = false;
        }
    }
}
