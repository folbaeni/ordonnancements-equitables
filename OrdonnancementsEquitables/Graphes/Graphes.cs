using OrdonnancementsEquitables.Jobs;
using OrdonnancementsEquitables.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace OrdonnancementsEquitables.Graphes
{
    public class Graphe
    {
        private Canvas Panel;
        private int[] Maxtime;
        private static Random Rand = new Random();
        private Brush[] UserColors;
            
       
        public Graphe(int nb_machines) : this(nb_machines, new Canvas(), 1)
        {
 
        }

        public Graphe(int nb_machines, Canvas can, int users)
        {
            Panel = can;
            Panel.Height = HeightCal(nb_machines + 1);
            Panel.Width = 10;
            Maxtime = new int[nb_machines];
            for (int i = 0; i < Maxtime.Length; i++)
            {
                Maxtime[i] = 10;
            }

            UserColors = new Brush[users];
            for (int i = 0; i < UserColors.Length; i++)
            {
                UserColors[i] = PickBrush();
            }
        }

        public void AddJob(int machine, Job j, int user)
        {
            AjouteJob(machine, j, UserColors[user]);
        }

        public void AddJob(int machine, Job j)
        {
            AjouteJob(machine, j, PickBrush());
        }
        private void AjouteJob(int machine, Job j, Brush couleur)
        {
            Canvas rect = new Canvas();
            rect.Background = couleur;
            rect.Height = 50;
            rect.Width = j.Time * 50;
            Canvas.SetTop(rect, HeightCal(machine));
            Canvas.SetLeft(rect, WidthCal(machine));
            Maxtime[machine] += j.Time * 50;
            Panel.Width = Maxtime.Max() + 10;
            Panel.Children.Add(rect);

        }

        private int HeightCal(int machine)
        {
            return machine * 60 + 10;
        }

        private int WidthCal(int machine)
        {
            return Maxtime[machine];
        }

        public static Brush PickBrush()
        {
            Brush result = Brushes.Transparent;

            Type brushesType = typeof(Brushes);

            PropertyInfo[] properties = brushesType.GetProperties();

            int random = Rand.Next(properties.Length);
            result = (Brush)properties[random].GetValue(null, null);

            return result;
        }
    }
}
