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
        private double _zoomValue = 1.0;

        public MainWindow()
        {
            InitializeComponent();

            var assembly = Assembly.GetAssembly(typeof(Algorithm<>)).GetTypes().ToList();
            var typed = assembly.Where(t => t.IsClass && t.Namespace == typeof(Algorithm<>).Namespace && t.IsPublic && t != typeof(Algorithm<>)).ToList();

            if (typed.Count > 0)
            {
                SelAlgo.Items.Clear();
                typed.ForEach(t => SelAlgo.Items.Add(t.Name.SystToAff()));
                SelAlgo.SelectedIndex = 0;
            }
        }

        private void OnFileLoaded(string filename)
        {
            filePath.Text = filename;
            fileParser = new Parser(filePath.Text);

            var assembly = Assembly.GetAssembly(typeof(Algorithm<>)).GetTypes().ToList();
            var children = assembly.Where(t => t.IsClass && t.Namespace == typeof(Algorithm<>).Namespace && t.IsPublic).ToList();
            var typed = children.Where(t => t.BaseType.IsGenericType && t.BaseType.GetGenericArguments().FirstOrDefault() == fileParser.JobType).ToList();

            if (typed.Count > 0)
            {
                SelAlgo.Items.Clear();
                typed.ForEach(t => SelAlgo.Items.Add(t.Name.SystToAff()));
                SelAlgo.SelectedIndex = 0;
            }
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
            Type algoType = Type.GetType(typeof(Algorithm<>).Namespace + "." + nomAlgo);
            var algo = Activator.CreateInstance(algoType);
            Job.CountToZero();

            if (filePath.Text == Properties.Resources.InitText)
            {
                switch (algo.GetType().BaseType.GetGenericArguments()[0].Name)
                {
                    case "Job":
                        Algorithm<Job> algorithmeJ = (Algorithm<Job>)algo;
                        algorithmeJ.ExecuteDefault();
                        algorithmeJ.Draw(screen);
                        break;
                    case "JobP":
                        Algorithm<JobP> algorithmeJP = (Algorithm<JobP>)algo;
                        algorithmeJP.ExecuteDefault();
                        algorithmeJP.Draw(screen);
                        break;
                    case "JobCo":
                        Algorithm<JobCo> algorithmeJC = (Algorithm<JobCo>)algo;
                        algorithmeJC.ExecuteDefault();
                        algorithmeJC.Draw(screen);
                        break;
                }
                return;
            }

            Job.CountToZero();
            switch (fileParser.JobType.Name)
            {
                case "Job":
                    Algorithm<Job> algorithmeJ = (Algorithm<Job>)algo;
                    var jobs = fileParser.ParseJobsFromJSON<Job>();
                    if (algorithmeJ is IMultipleDevices<Job> mdJ)
                        mdJ.Execute(jobs, (int)DevicesSlider.Value);
                    else
                        algorithmeJ.Execute(jobs);
                    algorithmeJ.Draw(screen);
                    break;
                case "JobP":
                    Algorithm<JobP> algorithmeJP = (Algorithm<JobP>)algo;
                    var jobsP = fileParser.ParseJobsFromJSON<JobP>();
                    if (algorithmeJP is IMultipleDevices<JobP> mdJP)
                        mdJP.Execute(jobsP, (int)DevicesSlider.Value);
                    else
                        algorithmeJP.Execute(jobsP);
                    algorithmeJP.Draw(screen);
                    break;
                case "JobCo":
                    Algorithm<JobCo> algorithmeJC = (Algorithm<JobCo>)algo;
                    var jobsJC = fileParser.ParseJobsFromJSON<JobCo>();
                    if (algorithmeJC is IMultipleDevices<JobCo> mdJC)
                        mdJC.Execute(jobsJC, (int)DevicesSlider.Value);
                    else
                        algorithmeJC.Execute(jobsJC);
                    algorithmeJC.Draw(screen);
                    break;
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void ScrollViewer_MouseWheel(object sender, MouseWheelEventArgs e)
        {

            ScaleTransform st = new ScaleTransform();
            if (e.Delta > 0)
            {
                st.ScaleX = st.ScaleY = _zoomValue = _zoomValue * 1.25;
                if (st.ScaleX > 64) st.ScaleX = st.ScaleY = _zoomValue = 64;
            }
            else
            {
                st.ScaleX = st.ScaleY = st.ScaleX = _zoomValue = _zoomValue / 1.25;
                if (st.ScaleX < 1) st.ScaleX = st.ScaleY = _zoomValue = 1;
            }
            screen.LayoutTransform = st;
            e.Handled = true;
        }
    }
}
