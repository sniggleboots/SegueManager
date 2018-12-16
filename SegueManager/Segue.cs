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
                if(file.Tag.Title != null)
                {
                    title = file.Tag.Title;
                }
                else
                {
                    title = "test";
                }
            }
            catch
            {
                title = "test";
            }

            try
            {
                if(file.Tag.Album != null)
                {
                    segment = file.Tag.Album;
                }
                else
                {
                    segment = "test";
                }
            }
            catch
            {
                segment = "test";
            }

            try
            {
                if(file.Tag.FirstGenre != null)
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
                if(file.Tag.Track != null || file.Tag.Track != 0)
                {
                    date = new DateTime(int.Parse(file.Tag.Track.ToString().Substring(0, 2)) + 2000, int.Parse(file.Tag.Track.ToString().Substring(2, 2)), int.Parse(file.Tag.Track.ToString().Substring(4, 2)));
                    //date = System.IO.File.GetCreationTime(filename);
                }
                else
                {
                    date = new DateTime(1, 1, 1);
                }
            }
            catch
            {
                date = new DateTime(1, 1, 1);
            }

            try
            {
                if(file.Tag.FirstPerformer != null)
                {
                    author = file.Tag.FirstPerformer;
                }
                else
                {
                    author = "test";
                }
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
