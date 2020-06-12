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
            var assembly = Assembly.GetAssembly(typeof(Algorithme<>)).GetTypes().ToList();
            var children = assembly.Where(t => t.IsClass && t.Namespace == typeof(Algorithme<>).Namespace && t.IsPublic).ToList();
            var typed = children.Where(t => t.BaseType.IsGenericType).ToList();

            if (typed.Count > 0)
            {
                typed.ForEach(t => SelAlgo.Items.Add(t.Name.SystToAff()));
            }
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
            //DrawerCo dr = new DrawerCo(screen, 4, 4);
            //dr.CleanCanvas();
            //Random rrr = new Random();
            //int i;
            //for (i = 0; i < 100; i++)
            //{
            //    int rand = rrr.Next(1, 15);
            //    JobCoCo tmp = new JobCoCo(rand, rrr.Next(1, rand));
            //    //JobCoCo tmp = new JobCoCo(3, 2);
            //    dr.AddJobCo(tmp, false, rrr.Next(0, 4), rrr.Next(0, 4));
            //}
            //return;


            string nomAlgo = SelAlgo.SelectedItem.ToString().AffToSyst();
            Type algoType = Type.GetType(typeof(Algorithme<>).Namespace + "." + nomAlgo);
            var algo = Activator.CreateInstance(algoType);

            JobCo.CountToZero();
            switch (fileParser.JobType.Name)
            {
                case "JobCo":
                    Algorithme<JobCo> algorithmeJ = (Algorithme<JobCo>)algo;
                    var JobCos = fileParser.ParseJobsFromJSON<JobCo>();
                    algorithmeJ.Execute(JobCos);
                    Console.WriteLine(algorithmeJ);
                    algorithmeJ.Draw(screen);
                    break;
                case "JobCoP":
                    Algorithme<JobP> algorithmeJP = (Algorithme<JobP>)algo;
                    var JobCosP = fileParser.ParseJobsFromJSON<JobP>();
                    algorithmeJP.Execute(JobCosP);
                    Console.WriteLine(algorithmeJP);
                    algorithmeJP.Draw(screen);
                    break;
                case "JobCoCo":
                    Algorithme<JobCo> algorithmeJC = (Algorithme<JobCo>)algo;
                    var JobCosJC = fileParser.ParseJobsFromJSON<JobCo>();
                    algorithmeJC.Execute(JobCosJC);
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
