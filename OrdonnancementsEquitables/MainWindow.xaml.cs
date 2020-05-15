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
using System.Windows.Controls.DataVisualization;
using OrdonnancementsEquitables.Parsers;
using OrdonnancementsEquitables.Outils;

namespace OrdonnancementsEquitables
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string HOGDSON = "Hogdson";
        private readonly List<string> allAlgos = new List<string>() { "Hogdson", "Glouton Par Profit" };

        public MainWindow()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            allAlgos.ForEach(s => selAlgo.Items.Add(s));
            selAlgo.SelectedIndex = 0;

            var hog = new Hogdson();
            var jobs = hog.ExecuteDefault();
            Console.WriteLine(hog);

            var gpp = new GloutonParProfits();
            var jobs2 = gpp.ExecuteDefault();
            Console.WriteLine(gpp);
        }

        /*private void OnFileLoaded()
        {
            Type t = Parser.Parse();
            typeof(Algorithmes<t>).Assembly.GetTypes().ForEach(t => selAlgo.Items.Add(t.ToString()));
        }
        private void OnStartButtonClicked() => Execute();*/


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
                // Open document 
                string filename = dlg.FileName;
                filePath.Text = filename;
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }
    }
}
