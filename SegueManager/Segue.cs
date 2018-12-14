using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagLib;

namespace SegueManager
{
    public class Segue
    {
        public TagLib.File file { get; set; }

        public string filename { get; set; }
        public string title { get; set; }
        public string author { get; set; }
        public string segment { get; set; }
        public DateTime date { get; set; }

        public Segue(string filename)
        {
            file = File.Create(filename);

            this.filename = filename;
            title = file.Tag.Title;
            author = file.Tag.Performers[0];
            segment = file.Tag.Album;
            date = new DateTime(int.Parse(file.Tag.Track.ToString().Substring(0, 2)) + 2000, int.Parse(file.Tag.Track.ToString().Substring(2, 2)), int.Parse(file.Tag.Track.ToString().Substring(4, 2)));
        }

        public override string ToString()
        {
            string output = "";
            output += title + "\t";
            output += author + "\t";
            output += segment + "\t";
            output += date;
            return output;
        }
    }
}
