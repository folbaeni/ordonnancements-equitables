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
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            selAlgo.Items.Add("Algo 1");
            selAlgo.Items.Add("Algo 2");
            selAlgo.Items.Add("Algo 3");
            Hogdson_helper();
            gpp_helper();

        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        public void Hogdson(Job[] jobs)
        {
            List<Job> ontime = new List<Job>();
            List<Job> late = new List<Job>();
            int c = 0;

            foreach(Job job in jobs) 
            { 
                ontime.Add(job);
                c += job.Time;

                if(c > job.Deadline)
                {
                    Job biggest = ontime.OrderByDescending(j=>j.Time).FirstOrDefault();
                    ontime.Remove(biggest);
                    late.Add(biggest);
                    c -= biggest.Time;
                }
            }

            Console.WriteLine("\n On time : \n");
            foreach (Job j in ontime) Console.WriteLine(j); 
            Console.WriteLine("\n Late : \n");
            foreach(Job j in late)  Console.WriteLine(j); 
        }

        public void Hogdson_helper()
        {
            Job[] jobs =
            {
                new Job(6, 8),
                new Job(4, 9),
                new Job(7, 15),
                new Job(8, 20),
                new Job(3, 21),
                new Job(5, 22)
            };

            foreach(Job j in jobs)
            {
                Console.WriteLine(j);
            }
            Hogdson(jobs);
        }

        public int GloutonParProfits(JobP[] jobs)
        {
            int time = 0;
            int profit = 0;
            JobP tmp;
            for (int i = 0; i < jobs.Length; i++) /*boucle sur le temps*/
            {
                for (int j = i; j < jobs.Length; j++) /* parcours du tableau */
                {
                    if (jobs[j].Deadline < jobs[i].Deadline)
                    {
                        tmp = jobs[i];
                        jobs[i] = jobs[j];
                        jobs[j] = tmp;
                    }
                } /* On a ordonnancé selon le principe glouton par profits*/

                Console.WriteLine(jobs[i] + "\n");

                if (jobs[i].Deadline > time)
                {
                    profit += jobs[i].Profit;
                }
                time++;
            }
            return profit;
        }

        public void gpp_helper()
        {
            JobP[] jobs = {
                new JobP(1, 12, 4),
                new JobP(1, 10, 3),
                new JobP(1, 8, 1),
                new JobP(1, 7, 6),
                new JobP(1, 6, 1),
                new JobP(1, 5, 6),
                new JobP(1, 4, 6),
                new JobP(1, 3, 5),
            };

            foreach(JobP j in jobs)
            {
                Console.WriteLine(j);
            }

            Console.WriteLine($"\n\nProfit = {GloutonParProfits(jobs)}");
        }
    }
}
