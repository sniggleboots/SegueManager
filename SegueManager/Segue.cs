using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public bool patreon { get; set; }
        public DateTime date { get; set; }

        public Segue(string filename)
        {
            file = File.Create(filename);

            this.filename = filename;

            try
            {
                title = file.Tag.Title;
            }
            catch
            {
                title = "test";
            }

            try
            {
                segment = file.Tag.Album;
            }
            catch
            {
                segment = "test";
            }

            try
            {
                if (file.Tag.FirstGenre.ToUpper() == "PATREON")
                {
                    patreon = true;
                }
                else
                {
                    patreon = false;
                }
            }
            catch
            {
                patreon = false;
            }

            try
            {
                date = new DateTime(int.Parse(file.Tag.Track.ToString().Substring(0, 2)) + 2000, int.Parse(file.Tag.Track.ToString().Substring(2, 2)), int.Parse(file.Tag.Track.ToString().Substring(4, 2)));
                //date = System.IO.File.GetCreationTime(filename);
            }
            catch
            {
                date = new DateTime(0, 0, 0);
            }

            try
            {
                author = file.Tag.FirstPerformer;
            }
            catch
            {
                author = "test";
            }
           
        }

        public override string ToString()
        {
            string output = "";
            output += title + "\t";
            output += "|" + author + "|\t";
            output += segment + "\t";
            output += date;
            return output;
        }
    }
}
