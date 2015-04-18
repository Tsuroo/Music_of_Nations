using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Rise_of_Music
{
    public class Config
    {
        public Boolean UseCustomSoundtrack { get; set; }
        public String PlayerNumber { get; set; }
        public String PlayerColor { get; set; }

        private String ConfigFileLocation = @"C:\Users\" + Environment.UserName + @"\AppData\Roaming\microsoft games\rise of nations\Rise_of_Music\config.xml";

        public Config()
        {   
            if (File.Exists(this.ConfigFileLocation))
            {
                try
                {
                    // Open the config.xml file
                    XmlDocument xmlDocument = new XmlDocument();
                    xmlDocument.Load(this.ConfigFileLocation);

                    // Find the use-original-soundtrack node
                    XmlNode useOriginalSoundtrackNode = xmlDocument.SelectSingleNode("/ROOT/USE_CUSTOM_SOUNDTRACK");
                    this.UseCustomSoundtrack = Boolean.Parse(useOriginalSoundtrackNode.InnerText);
                }
                catch (Exception e)
                {
                    throw new Exception("Failed to read config.xml");
                }
            }
            else
            {
                Console.WriteLine("config.xml does not exist; creating now with default values.");
                this.UseDefaults();
                this.Save();
            }
        }

        public void UseDefaults()
        {
            this.UseCustomSoundtrack = false;
        }

        public void Save()
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.NewLineHandling = NewLineHandling.Replace;
            settings.Indent = true;

            using (XmlWriter xmlWriter = XmlWriter.Create(this.ConfigFileLocation, settings))
            {
                xmlWriter.WriteStartElement("ROOT");
                xmlWriter.WriteElementString("USE_CUSTOM_SOUNDTRACK", this.UseCustomSoundtrack.ToString());
                xmlWriter.WriteElementString("PLAYER_NUMBER", this.PlayerNumber);
                xmlWriter.WriteElementString("PLAYER_COLOR", this.PlayerColor);
                xmlWriter.WriteElementString("VERSION", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString());
            }
        }
    }
}