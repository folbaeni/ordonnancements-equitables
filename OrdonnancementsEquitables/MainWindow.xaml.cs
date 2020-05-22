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

namespace OrdonnancementsEquitables
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;

            //new Hogdson().ExecuteDefault();
            //new GloutonParProfits().ExecuteDefault();
        }

        private void OnFileLoaded(string filename)
        {
            filePath.Text = filename;
            Job[] content = Parser.ParseFromFile(filename, out Type jobType);

            var assembly = Assembly.GetAssembly(typeof(Algorithme<>)).GetTypes().ToList();
            var children = assembly.Where(t => t.IsClass && t.Namespace == typeof(Algorithme<>).Namespace && t.IsPublic).ToList();
            var typed = children.Where(t => t.BaseType.IsGenericType && t.BaseType.GetGenericArguments().FirstOrDefault() == jobType).ToList();

            if (typed.Count > 0)
            {
                SelAlgo.Items.Clear();
                typed.ForEach(t => SelAlgo.Items.Add(t.Name.SystToAff()));
                //SelAlgo.SelectedIndex = 0;
                SelAlgo.Focus();
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
            Job[] content = Parser.ParseFromFile(filePath.Text, out Type jobType);
            string nomAlgo = SelAlgo.SelectedItem.ToString().AffToSyst();
            Type algoType = Type.GetType(typeof(Algorithme<Job>).Namespace + "." + nomAlgo);
            var algo = Activator.CreateInstance(algoType);

            switch (jobType.Name)
            {
                case "Job":
                    ((Algorithme<Job>)algo).Execute(content);
                    Console.WriteLine(((Algorithme<Job>)algo).ToString());
                    break;
                case "JobP":
                    ((Algorithme<JobP>)algo).Execute(Parser.ParseFromFile<JobP>(filePath.Text));
                    Console.WriteLine(((Algorithme<JobP>)algo).ToString());
                    break;
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }
    }
}
