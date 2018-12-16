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
        //Process myProcess = new Process();
        //ProcessStartInfo startInfo = new ProcessStartInfo("wmplayer.exe");

        List<Process> processList = new List<Process>();

        List<string> exts = new List<string>();
        List<Segue> segues = new List<Segue>();
        List<Segue> filteredFiles = new List<Segue>();
        List<string> segmentList = new List<string>();
        List<string> authorList = new List<string>();

        string activeFilter = "";

        Random random = new Random(int.Parse(DateTime.Now.ToString("dmmssfff")));

        public MainWindow()
        {
            InitializeComponent();

            //adds extensions to look out for to list
            exts.Add("*.mp3");
            exts.Add("*.wav");
            exts.Add("*.wma");

            //these will be filled later with info pulled from all segues
            string allsegments = "";
            string allauthors = "";

            //iterate over the entire segue folder, find all files ending with the extentions added above. Nested foreach is probably pretty inefficient, but I don't know how to make EnumerateFiles check for more than one filter
            foreach (string ext in exts)
            {
                foreach (string filename in Directory.EnumerateFiles(Path.GetFullPath(Path.Combine(Environment.CurrentDirectory,"Segues")), ext))
                {
                    //add all valid files to the list of segues, using the Segue class
                    segues.Add(new Segue(filename));
                }
            }

            int maxlengthAuthor = 0;
            int maxlengthTitle = 0;
            int maxlengthSegment = 0;
            foreach (Segue segue in segues)
            {
                //fill listbox with all segues
                lstAllSegues.Items.Add(segue);

                //if the "segment" part of a segue does not yet exist in the list, add it to the segmentList so it can be used as a filter later. Also add it to the string, so it doesn't get added again later
                if(!allsegments.ToUpper().Contains(segue.segment.ToUpper()) && segue.segment.ToUpper() != "FX")
                {
                    allsegments += segue.segment + " ";
                    segmentList.Add(segue.segment);
                }

                //analogous with segment
                if(!allauthors.ToUpper().Contains(segue.author.ToUpper()) && segue.author.ToUpper() != "FX")
                {
                    allauthors += segue.author + " ";
                    authorList.Add(segue.author);
                }

                if(segue.author.Length > maxlengthAuthor)
                {
                    maxlengthAuthor = segue.author.Length;
                }
                if(segue.segment.ToString().Length > maxlengthSegment)
                {
                    maxlengthSegment = segue.segment.ToString().Length;
                }
                if (segue.title.ToString().Length > maxlengthTitle)
                {
                    maxlengthTitle = segue.title.ToString().Length;
                }
            }
            foreach (Segue segue in segues)
            {
                while(segue.author.Length < maxlengthAuthor && segue.author.ToUpper() != "FX")
                {
                    segue.author += " ";
                }
                while(segue.segment.Length < maxlengthSegment && segue.author.ToUpper() != "FX")
                {
                    segue.segment += " ";
                }
                while(segue.title.Length < maxlengthTitle)
                {
                    segue.title += " ";
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
            lstFilteredSegues.Items.Clear();
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
            lstFilteredSegues.Items.Clear();
            foreach (string author in authorList)
            {
                lstFilterOptions.Items.Add(author);
            }
        }

        //analogous
        private void BtnPodcast_Click(object sender, RoutedEventArgs e)
        {
            activeFilter = "FX";
            lstFilterOptions.Items.Clear();
            lstFilteredSegues.Items.Clear();
            foreach (Segue segue in segues)
            {
                if(segue.segment.ToUpper() == "FX" || segue.author.ToUpper() == "FX")
                {
                    filteredFiles.Add(segue);
                }
            }
            FillListbox(lstFilteredSegues, filteredFiles);
        }

        //analogous
        private void BtnPatreon_Click(object sender, RoutedEventArgs e)
        {
            activeFilter = "patreon";
            lstFilterOptions.Items.Clear();
            lstFilteredSegues.Items.Clear();
            lstFilterOptions.Items.Add("Patreon");
            lstFilterOptions.Items.Add("No patreon");
        }

        //analogous
        private void BtnDate_Click(object sender, RoutedEventArgs e)
        {
            activeFilter = "date";
            lstFilterOptions.Items.Clear();
            lstFilteredSegues.Items.Clear();
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
            lstFilteredSegues.Items.Clear();
            filteredFiles.Clear();
            txtFilter.Text = "";
        }

        private void BtnPlayAll_Click(object sender, RoutedEventArgs e)
        {
            /*
            string test = segues[0].filename;
            startInfo.Arguments = test;
            myProcess.StartInfo = startInfo;
            myProcess.Start();
            */

            int randomNumber;
            do
            {
                randomNumber = random.Next(segues.Count);
            } while (segues[randomNumber].author.ToUpper() == "FX" || segues[randomNumber].segment.ToUpper() == "FX");
            PlaySegue(segues[randomNumber]);
        }

        private void BtnPlayFiltered_Click(object sender, RoutedEventArgs e)
        {
            if(lstFilterOptions.SelectedIndex != -1)
            {
                try
                {
                    PlaySegue(filteredFiles[random.Next(filteredFiles.Count)]);
                }
                catch
                {
                    MessageBox.Show("You'll only see this if I really fucked up somewhere. Oopsie!");
                }
            }
            else
            {
                MessageBox.Show("No filter selected");
            }
        }

        private void LstFilterOptions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //event handler when a filter option is selected
            if(lstFilterOptions.SelectedIndex != -1)
            {
                filteredFiles.Clear();
                lstFilteredSegues.Items.Clear();
                //switches which filter is active and fills the listbox accordingly
                switch(activeFilter)
                {
                    case "segment":
                        foreach(Segue segue in segues)
                        {
                            if(segue.segment.ToUpper().Contains(lstFilterOptions.SelectedItem.ToString().ToUpper()))
                            {
                                filteredFiles.Add(segue);
                            }
                        }
                        FillListbox(lstFilteredSegues, filteredFiles);
                        break;

                    case "author":
                        foreach(Segue segue in segues)
                        {
                            if(segue.author.ToUpper().Contains(lstFilterOptions.SelectedItem.ToString().ToUpper()))
                            {
                                filteredFiles.Add(segue);
                            }
                        }
                        FillListbox(lstFilteredSegues, filteredFiles);
                        break;

                    case "date":
                        switch(lstFilterOptions.SelectedIndex)
                        {
                            case 0:
                                foreach(Segue segue in segues)
                                {
                                    TimeSpan add = new TimeSpan(365, 0, 0, 0);
                                    if(segue.date.Add(add) > DateTime.Now && segue.segment.ToUpper() != "FX")
                                    {
                                        filteredFiles.Add(segue);
                                    }
                                }
                                FillListbox(lstFilteredSegues, filteredFiles);
                                break;

                            case 1:
                                foreach(Segue segue in segues)
                                {
                                    TimeSpan add = new TimeSpan(-365, 0, 0, 0);
                                    if(segue.date <= DateTime.Now.Add(add) && segue.segment.ToUpper() != "FX")
                                    {
                                        filteredFiles.Add(segue);
                                    }
                                }
                                FillListbox(lstFilteredSegues, filteredFiles);
                                break;

                            case 2:
                                foreach(Segue segue in segues)
                                {
                                    TimeSpan add = new TimeSpan(-365 * 2, 0, 0, 0);
                                    if (segue.date <= DateTime.Now.Add(add) && segue.segment.ToUpper() != "FX")
                                    {
                                        filteredFiles.Add(segue);
                                    }
                                }
                                FillListbox(lstFilteredSegues, filteredFiles);
                                break;

                            case 3:
                                foreach (Segue segue in segues)
                                {
                                    TimeSpan add = new TimeSpan(-365 * 3, 0, 0, 0);
                                    if (segue.date <= DateTime.Now.Add(add) && segue.segment.ToUpper() != "FX")
                                    {
                                        filteredFiles.Add(segue);
                                    }
                                }
                                FillListbox(lstFilteredSegues, filteredFiles);
                                break;

                            case 4:
                                foreach (Segue segue in segues)
                                {
                                    TimeSpan add = new TimeSpan(-365 * 4, 0, 0, 0);
                                    if (segue.date <= DateTime.Now.Add(add) && segue.segment.ToUpper() != "FX")
                                    {
                                        filteredFiles.Add(segue);
                                    }
                                }
                                FillListbox(lstFilteredSegues, filteredFiles);
                                break;
                        }
                        break;

                    case "patreon":
                        foreach (Segue segue in segues)
                        {
                            if (lstFilterOptions.SelectedIndex == 0 && segue.patreon == true && segue.segment.ToUpper() != "FX")
                            {
                                filteredFiles.Add(segue);
                            }
                            else if(lstFilterOptions.SelectedIndex == 1 && segue.patreon == false && segue.segment.ToUpper() != "FX")
                            {
                                filteredFiles.Add(segue);
                            }
                        }
                        FillListbox(lstFilteredSegues, filteredFiles);
                        break;
                }
            }
            else
            {
                lstFilterOptions.Items.Clear();
                lstFilteredSegues.Items.Clear();
            }
        }

        private void PlaySegue(Segue segue)
        {
            Process myProcess = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo("wmplayer.exe");
            startInfo.Arguments = segue.filename;
            myProcess.StartInfo = startInfo;
            myProcess.Start();

            processList.Add(myProcess);
        }

        private void LstAllSegues_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(lstAllSegues.SelectedIndex != -1)
            {
                lstFilteredSegues.SelectedIndex = -1;
            }
        }

        private void LstFilteredSegues_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(lstFilteredSegues.SelectedIndex != -1)
            {
                lstAllSegues.SelectedIndex = -1;
            }
        }

        private void BtnPlaySelected_Click(object sender, RoutedEventArgs e)
        {
            if(lstAllSegues.SelectedIndex != -1)
            {
                PlaySegue((Segue)lstAllSegues.SelectedItem);
            }
            else if(lstFilteredSegues.SelectedIndex != -1)
            {
                PlaySegue(filteredFiles[lstFilteredSegues.SelectedIndex]);
            }
            else
            {
                MessageBox.Show("No segue selected");
            }
        }

        private void BtnKill_Click(object sender, RoutedEventArgs e)
        {
            for(int i = processList.Count - 1; i >= 0; i--)
            {
                if(!processList[i].HasExited)
                {
                    processList[i].Kill();
                }
                processList.RemoveAt(i);
            }
            processList.Clear();
        }
    }
}
