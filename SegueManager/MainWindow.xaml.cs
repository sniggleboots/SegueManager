using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using TagLib;
using System.Diagnostics;
using Path = System.IO.Path;

namespace SegueManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Process myProcess = new Process();
        ProcessStartInfo startInfo = new ProcessStartInfo("wmplayer.exe");

        List<string> exts = new List<string>();
        List<Segue> segues = new List<Segue>();
        List<Segue> filteredFiles = new List<Segue>();
        List<string> segmentList = new List<string>();
        List<string> authorList = new List<string>();

        string activeFilter = "";

        public MainWindow()
        {
            InitializeComponent();

            //adds extensions to look out for to list
            exts.Add("*.mp3");
            exts.Add("*.wav");

            //these will be filled later with info pulled from all segues
            string allsegments = "";
            string allauthors = "";

            //iterate over the entire segue folder, find all files ending with the extentions added above. Nested foreach is probably pretty inefficient, but I don't know how to make EnumerateFiles check for more than one filter
            foreach (string ext in exts)
            {
                foreach (string filename in Directory.EnumerateFiles(Path.Combine(Environment.CurrentDirectory,"Segues"), ext))
                {
                    //add all valid files to the list of segues, using the Segue class
                    segues.Add(new Segue(filename));
                }
            }

            foreach (Segue segue in segues)
            {
                //fill listbox with all segues
                lstAllSegues.Items.Add(segue);

                //if the "segment" part of a segue does not yet exist in the list, add it to the segmentList so it can be used as a filter later. Also add it to the string, so it doesn't get added again later
                if(!allsegments.ToUpper().Contains(segue.segment.ToUpper()))
                {
                    allsegments += segue.segment + " ";
                    segmentList.Add(segue.segment);
                }

                //analogous with segment
                if(!allauthors.ToUpper().Contains(segue.author.ToUpper()))
                {
                    allauthors += segue.author + " ";
                    authorList.Add(segue.author);
                }
            }
        }

        //helper method to fill listboxes
        private void FillListbox(ListBox listbox, List<Segue> seguelist)
        {
            listbox.Items.Clear();
            foreach(Segue segue in seguelist)
            {
                listbox.Items.Add(segue);
            }
        }

        //event handler for the textbox filter, if the entered text matches anything in the title, author or segment, it is retained in the listbox.
        private void TxtFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(txtFilter.Text != string.Empty)
            {
                filteredFiles.Clear();
                foreach(Segue segue in segues)
                {
                    if(segue.ToString().ToUpper().Contains(txtFilter.Text.ToUpper()))
                    {
                        filteredFiles.Add(segue);
                    }
                }
                FillListbox(lstAllSegues, filteredFiles);
            }
            else
            {
                filteredFiles.Clear();
                FillListbox(lstAllSegues, segues);
            }
        }

        //event handler for Segment filter button. Pulls all the possible segments from the list made earlier and puts it in the filter options listbox. From there, the user will be able to filter by them.
        private void BtnSegment_Click(object sender, RoutedEventArgs e)
        {
            activeFilter = "segment";
            lstFilterOptions.Items.Clear();
            foreach(string segment in segmentList)
            {
                lstFilterOptions.Items.Add(segment);
            }
        }

        //analogous Segment
        private void BtnAuthor_Click(object sender, RoutedEventArgs e)
        {
            activeFilter = "author";
            lstFilterOptions.Items.Clear();
            foreach (string author in authorList)
            {
                lstFilterOptions.Items.Add(author);
            }
        }

        //analogous
        private void BtnPatreon_Click(object sender, RoutedEventArgs e)
        {
            activeFilter = "patreon";
            lstFilterOptions.Items.Clear();
            lstFilterOptions.Items.Add("Patreon");
            lstFilterOptions.Items.Add("No patreon");
        }

        //analogous
        private void BtnDate_Click(object sender, RoutedEventArgs e)
        {
            activeFilter = "date";
            lstFilterOptions.Items.Clear();
            lstFilterOptions.Items.Add("Younger than a year");
            lstFilterOptions.Items.Add("Older than a year");
            lstFilterOptions.Items.Add("Older than two years");
            lstFilterOptions.Items.Add("Older than three years");
            lstFilterOptions.Items.Add("Older than four years");
        }

        //resets all filters
        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            activeFilter = "";
            lstFilterOptions.Items.Clear();
            txtFilter.Text = "";
        }

        private void BtnPlayAll_Click(object sender, RoutedEventArgs e)
        {
            string test = segues[0].filename;
            startInfo.Arguments = test;
            myProcess.StartInfo = startInfo;
            myProcess.Start();
        }
    }
}
