using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Rise_of_Music
{
    public class CurrentGameXml
    {
        public String PlayerNumber { get; set; }
        public String PlayerColor { get; set; }

        private String XmlFileLocation = @"C:\Users\" + Environment.UserName + @"\AppData\Roaming\microsoft games\rise of nations\Rise_of_Music\current_game\current_game.xml";

        public void Save()
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.NewLineHandling = NewLineHandling.Replace;
            settings.Indent = true;

            using (XmlWriter xmlWriter = XmlWriter.Create(this.XmlFileLocation, settings))
            {
                xmlWriter.WriteStartElement("ROOT");
                xmlWriter.WriteElementString("PLAYER_NUMBER", this.PlayerNumber);
                xmlWriter.WriteElementString("PLAYER_COLOR", this.PlayerColor);
                xmlWriter.WriteElementString("VERSION", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString());
            }
        }
    }
}
