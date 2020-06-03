using OrdonnancementsEquitables.Algos;
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
using OrdonnancementsEquitables.Parsers;
using OrdonnancementsEquitables.Utils;
using OrdonnancementsEquitables.Jobs;
using System.Reflection;
using System.IO;
using OrdonnancementsEquitables.Drawing;

namespace OrdonnancementsEquitables
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Parser fileParser;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnFileLoaded(string filename)
        {
            filePath.Text = filename;
            fileParser = new Parser(filePath.Text);

            var assembly = Assembly.GetAssembly(typeof(Algorithme<>)).GetTypes().ToList();
            var children = assembly.Where(t => t.IsClass && t.Namespace == typeof(Algorithme<>).Namespace && t.IsPublic).ToList();
            var typed = children.Where(t => t.BaseType.IsGenericType && t.BaseType.GetGenericArguments().FirstOrDefault() == fileParser.JobType).ToList();

            if (typed.Count > 0)
            {
                SelAlgo.Items.Clear();
                typed.ForEach(t => SelAlgo.Items.Add(t.Name.SystToAff()));
                //SelAlgo.SelectedIndex = 0;
            }
            var x = new Job(0, 0) == new Job(1, 12);
        }


        private void SelectionFile(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog
            {
                DefaultExt = ".json",
                Filter = "JSON Files (*.json)|*json"
            };

            bool? result = dlg.ShowDialog();

            if (result == true)
            {
                OnFileLoaded(dlg.FileName);
            }
        }

        private void StartAlgo(object sender, RoutedEventArgs e)
        {
            string nomAlgo = SelAlgo.SelectedItem.ToString().AffToSyst();
            Type algoType = Type.GetType(typeof(Algorithme<>).Namespace + "." + nomAlgo);
            var algo = Activator.CreateInstance(algoType);

            Job.CountToZero();
            switch (fileParser.JobType.Name)
            {
                case "Job":
                    Algorithme<Job> algorithmeJ = (Algorithme<Job>)algo;
                    var jobs = fileParser.ParseJobsFromJSON<Job>();
                    algorithmeJ.Execute(jobs);
                    Console.WriteLine(algorithmeJ);
                    algorithmeJ.Draw(screen);
                    break;
                case "JobP":
                    Algorithme<JobP> algorithmeJP = (Algorithme<JobP>)algo;
                    var jobsP = fileParser.ParseJobsFromJSON<JobP>();
                    algorithmeJP.Execute(jobsP);
                    Console.WriteLine(algorithmeJP);
                    algorithmeJP.Draw(screen);
                    break;
                case "JobCo":
                    Algorithme<JobCo> algorithmeJC = (Algorithme<JobCo>)algo;
                    var jobsJC = fileParser.ParseJobsFromJSON<JobCo>();
                    algorithmeJC.Execute(jobsJC);
                    Console.WriteLine(algorithmeJC);
                    algorithmeJC.Draw(screen);
                    break;
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }
    }
}
