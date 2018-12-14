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

namespace SegueManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Process myProcess = new Process();
        ProcessStartInfo startInfo = new ProcessStartInfo("wmplayer.exe");

        List<Segue> files = new List<Segue>();
        List<string> exts = new List<string>();

        public MainWindow()
        {
            InitializeComponent();

            exts.Add("*.mp3");
            exts.Add("*.wav");

            foreach (string ext in exts)
            {
                foreach (string filename in Directory.EnumerateFiles(@"..\..\Segues", ext))
                {
                    files.Add(new Segue(filename));
                }
            }

            lstAllSegues.ItemsSource = files;
        }
    }
}
