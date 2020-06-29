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

namespace OrdonnancementsEquitables.Drawing
{
    /// <summary>
    /// Class containing tools in order to draw graphs representing task's order in machines.
    /// </summary>

    public class Drawer
    {
        /// <summary>
        /// Parameter of type Canvas representing the full graph ready to be shown.
        /// </summary>
        protected Canvas Panel;
        /// <summary>
        /// The last pixel in width of each machine to add next Job.
        /// </summary>
        protected int[] maxTime;
        /// <summary>
        /// Private random for color generation per user.
        /// </summary>
        protected static readonly Random rand = new Random();

        /// <summary>
        ///  Stocks the color of every user.
        /// </summary>
        protected readonly Brush[] userColors;


        /// <summary>
        /// Variable used for converting time in second to pixels.
        /// </summary>
        protected static readonly int pixelMultiplier = 50;

        protected static readonly int hauteur = 50;


        /// <summary>
        /// This construct initialises the new <c>Drawer</c> for one machine and one user.
        /// </summary>
        /// <param name="c">The canvas present in MainWindow to be shown.</param>
        public Drawer(Canvas c)
            : this(c, 1, 1)
        { }

        /// <summary>
        /// This constructor initialises the new <c>Drawer</c> for <paramref name="nbUsers"/> machines and one user.
        /// </summary>
        /// <param name="c">The canvas present in MainWindow to be shown.</param>
        /// <param name="nbUsers">Number of machines.</param>
        public Drawer(Canvas c, int nbUsers)
            : this(c, nbUsers, 1)
        { }

        /// <summary>
        /// This constructor initialises the new <c>Drawer</c> for <paramref name="nbMachines"/> machines and <paramref name="nbUsers"/> users.
        /// </summary>
        /// <param name="can">The canvas present in MainWindow to be shown.</param>
        /// <param name="nbUsers">Number of users</param>
        /// <param name="nbMachines">Number of machines.</param>
        public Drawer(Canvas can, int nbUsers, int nbMachines)
        {
            Panel = can;
            //Panel.Height = HeightCal(nbMachines + 1) > 350 ? HeightCal(nbMachines + 1) : 350 ;
            Panel.Height = HeightCal(nbMachines + 1);
            Panel.Width = 100;
            maxTime = new int[nbMachines];
            for (int i = 0; i < maxTime.Length; i++)
            {
                maxTime[i] = 10;
            }
            Panel.Children.Clear();

            userColors = new Brush[nbUsers];
            for (int i = 0; i < userColors.Length; i++)
            {
                userColors[i] = PickBrush();
            }
        }

        /// <summary>
        /// This method adds the rectangle representing the Job <paramref name="j"/> in the graphic for the case where there is one machine ad one user.
        /// </summary>
        /// <param name="j">Parameter of the Job that will be added. </param>
        /// <param name="late">Boolean indicating if the Job is late or not.</param>
        public void AddJob(Job j, bool late) => AddJob(j, late, PickBrush(), 0);

        /// <summary>
        /// This method adds the rectangle representing the Job <paramref name="j"/> in the graphic for the case where there is one machine and many users.
        /// </summary>
        /// <param name="machine"> Integer representing which machine has the Job.</param>
        /// <param name="j">Parameter of the Job that will be added. </param>
        /// <param name="couleur"> Brush of the color of the Job, which has to be defined by the userId (UserColor). </param>
        /// <param name="late">Boolean indicating if the Job is late or not.</param>
        public void AddJob(Job j, bool late, int user) => AddJob(j, late, userColors.Length == 1 ? PickBrush() : userColors[user], 0);

        /// <summary>
        /// This method adds the rectangle representing the Job <paramref name="j"/> for <paramref name="machine"/> in the graphic. Case with one or many users and many machines.
        /// </summary>
        /// <param name="machine"> Integer representing which machine has the Job.</param>
        /// <param name="j">Parameter of the Job that will be added. </param>
        /// <param name="late">Boolean indicating if the Job is late or not.</param>
        public void AddJob(Job j, bool late, int user, int machine) => AddJob(j, late, userColors.Length == 1 ? PickBrush() : userColors[user], machine);

        /// <summary>
        /// This method adds the rectangle representing the Job <paramref name="j"/> for <paramref name="machine"/> in the graphic.
        /// </summary>
        /// <param name="j">Parameter of the Job that will be added. </param>
        /// <param name="late">Boolean indicating if the Job is late or not.</param>
        /// <param name="couleur"> Brush of the color of the Job, which has to be defined by the userId (UserColor). </param>
        /// <param name="machine"> Integer representing which machine has the Job.</param>
        private void AddJob(Job j, bool late, Brush couleur, int machine)
        {
            Rectangle rect = new Rectangle
            {
                Fill = couleur,
                Stroke = Brushes.Black,
                Height = hauteur,
                Width = j.Time * pixelMultiplier
            };
            Canvas.SetTop(rect, HeightCal(machine));
            Canvas.SetLeft(rect, WidthCal(machine));
            Canvas.SetZIndex(rect, 0);
            if (late)
            {
                rect.Fill = LatePattern(couleur); 
            }


            //Add ID
            TextBlock textBlock = new TextBlock();
            textBlock.Text = j.Id.ToString();
            Canvas.SetTop(textBlock, HeightCal(machine) + 10);
            Canvas.SetLeft(textBlock, WidthCal(machine) + 10);
            Panel.Children.Add(textBlock);
            Canvas.SetZIndex(textBlock, 1);

            maxTime[machine] += j.Time * pixelMultiplier;
            Panel.Width = maxTime.Max() + 10;
            Panel.Children.Add(rect);
        }

        /*
        private void MakeInterlap(int time, Brush colA, Brush colB, int machine)
        {
            /// Initialisation triangle A
            PointCollection pAlpha = new PointCollection
            {
                new Point(0, 0),
                new Point(1, 0),
                new Point(0, 1)
            };
            Polygon Alpha = new Polygon
            {
                Points = pAlpha,
                Fill = colA,
                Height = 50,
                Width = pixelMultiplier * time
            };

            /// Initialisation triangle B
            PointCollection pBeta = new PointCollection
            {
                new Point(1, 1),
                new Point(1, 0),
                new Point(0, 1)
            };
            Polygon Beta = new Polygon
            {
                Points = pBeta,
                Fill = colB,
                Height = 50,
                Width = pixelMultiplier * time
            };


            //Insertion in canvas
            Canvas.SetTop(Alpha, HeightCal(machine));
            Canvas.SetLeft(Alpha, WidthCal(machine));

            Canvas.SetTop(Beta, HeightCal(machine));
            Canvas.SetLeft(Beta, WidthCal(machine));

            maxtime[machine] += time * pixelMultiplier;
            Panel.Width = maxtime.Max() + 10;

            Panel.Children.Add(Alpha);
            Panel.Children.Add(Beta);
        }
        */

        /// <summary>
        /// This method is dedicated to generate the background of a rectangle which is late adding red stripes on his actual color.
        /// </summary>
        /// <param name="couleur">Color of the rectangle background (based on user).</param>
        /// <returns>It returns the DrawingBrush to fill the rectangle which is late.</returns>
        protected DrawingBrush LatePattern(Brush couleur)
        {
            // Initialising the two layers
            GeometryDrawing aDrawing = new GeometryDrawing();
            GeometryDrawing background = new GeometryDrawing(couleur, null, new RectangleGeometry(new Rect(0, 0, 201, 815)));
            RectangleGeometry aRect = new RectangleGeometry(new Rect(200, 0, 4, 800), 0, 0, new RotateTransform(10));

            aDrawing.Geometry = aRect;
            aDrawing.Brush = Brushes.Red;


            // Overlap colour background and redstripes
            DrawingGroup linesDrawingGroup = new DrawingGroup();
            linesDrawingGroup.Children.Add(background);
            linesDrawingGroup.Children.Add(aDrawing);

            // Create a DrawingBrush
            DrawingBrush myDrawingBrush = new DrawingBrush
            {
                Drawing = linesDrawingGroup,

                // Set the DrawingBrush's Viewport and TileMode
                // properties so that it generates a pattern.
                Viewport = new Rect(0, 0, 0.125, 1),
                TileMode = TileMode.Tile
            };

            return myDrawingBrush;
        }

        /// <summary>
        /// This property calculates the <c>y</c> coordinate of top corner of rectangle based on which <paramref name="machine"/>.
        /// </summary>
        /// <param name="machine"> This parameter identifies the machine.</param>
        /// <returns>Returns the <c>y</c> coordinate in pixels.</returns>
        protected int HeightCal(int machine) => machine * 60 + 10;

        /// <summary>
        /// This property calculates the <c>x</c> coordinate of left corner of rectangle based on which <paramref name="machine"/>.
        /// </summary>
        /// <param name="machine">This parameter identifies the machine.</param>
        /// <returns>Returns the <c>x</c> coordinate in pixels.</returns>
        protected int WidthCal(int machine) => maxTime[machine];

        /// <summary>
        /// This method pickes a brush randomly for drawing rectangles in case of a single user.
        /// </summary>
        /// <returns>It returns a basic brush in a random color.</returns>
        protected static Brush PickBrush()
        {
            Brush result = Brushes.Transparent;

            Type brushesType = typeof(Brushes);

            PropertyInfo[] properties = brushesType.GetProperties();

            int random = rand.Next(properties.Length);
            result = (Brush)properties[random].GetValue(null, null);

            return result;
        }

        /// <summary>
        /// This method resets the canvas empty.
        /// </summary>
        public void CleanCanvas()
        {
            Panel.Height = HeightCal(maxTime.Length);
            Panel.Width = 100;
            Panel.Children.Clear();
        }
    }
}
