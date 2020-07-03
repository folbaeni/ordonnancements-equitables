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
                _ = algo.GetType().BaseType.GetGenericArguments()[0].Name switch
                {
                    "Job" => DefaultExecution(algo as Algorithm<Job>),
                    "JobP" => DefaultExecution(algo as Algorithm<JobP>),
                    "JobCo" => DefaultExecution(algo as Algorithm<JobCo>),
                    _ => false
                };
            } 
            else
            {
                _ = fileParser.JobType.Name switch 
                { 
                    "Job" => Execution(algo as Algorithm<Job>),
                    "JobP" => Execution(algo as Algorithm<JobP>),
                    "JobCo" => Execution(algo as Algorithm<JobCo>),
                    _ => false
                };
            }
        }

        private bool DefaultExecution<TJob>(Algorithm<TJob> algo) where TJob : Job
        {
            algo.ExecuteDefault();
            algo.Draw(screen);

            return true;
        }

        private bool Execution<TJob>(Algorithm<TJob> algo) where TJob : Job
        {
            var jobs = fileParser.ParseJobsFromJSON<TJob>();
            var users = fileParser.ParseUsersFromJSON<TJob>();
            if (fileParser.IsSingleUser == false && algo is IMultipleDevicesAndUsers<TJob> mduJ)
                mduJ.Execute(users, (int)DevicesSlider.Value);
            else if (fileParser.IsSingleUser == false && algo is IMultipleUsers<TJob> muJ)
                muJ.Execute(users);
            else if (algo is IMultipleDevices<TJob> mdJ)
                mdJ.Execute(jobs, (int)DevicesSlider.Value);
            else
                algo.Execute(jobs);
            algo.Draw(screen);

            return true;
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
