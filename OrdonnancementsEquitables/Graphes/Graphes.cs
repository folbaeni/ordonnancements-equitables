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
using System.Windows.Shapes;

namespace OrdonnancementsEquitables.Graphes
{
    public class Graphe
    {
        private Canvas Panel;
        private int[] Maxtime;
        private static Random Rand = new Random();
        private Brush[] UserColors;
            
       
        public Graphe(int nb_machines, Canvas c) : this(nb_machines, c, 1)
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

        public void AddJob(int machine, Job j, bool late, int user)
        {
            AjouteJob(machine, j, UserColors[user], late);
        }

        public void AddJob(int machine, Job j, bool late)
        {
            AjouteJob(machine, j, PickBrush(), late);
        }

        private void AjouteJob(int machine, Job j, Brush couleur, bool late)
        {
            Rectangle rect = new Rectangle();
            rect.Fill = couleur;
            rect.Stroke = Brushes.Black;
            rect.Height = 50;
            rect.Width = j.Time * 50;
            Canvas.SetTop(rect, HeightCal(machine));
            Canvas.SetLeft(rect, WidthCal(machine));
            Maxtime[machine] += j.Time * 50;
            Panel.Width = Maxtime.Max() + 10;
            Panel.Children.Add(rect);
            if(late)
            {
                DrawingBrush pattern = new DrawingBrush();
                GeometryDrawing backgroundSquare = new GeometryDrawing(couleur, null, new RectangleGeometry(new Rect(0, 0, 400, 400)));
                // Create a GeometryGroup that will be added to Geometry  
                GeometryGroup gGroup = new GeometryGroup();
                gGroup.Children.Add(new RectangleGeometry(new Rect(200, 0, 2, 400)));
                // Create a GeomertyDrawing  
                GeometryDrawing checkers = new GeometryDrawing(new SolidColorBrush(Colors.Red), null, gGroup);
                DrawingGroup linesDrawingGroup = new DrawingGroup();
                linesDrawingGroup.Children.Add(backgroundSquare);
                linesDrawingGroup.Children.Add(checkers);
                pattern.Drawing = linesDrawingGroup;
                // Set Viewport and TimeMode  
                pattern.Viewport = new Rect(0, 0, 0.25, 0.25);
                pattern.TileMode = TileMode.FlipXY;
                // Fill rectangle with a DrawingBrush  
                rect.Fill = pattern;
            }

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
