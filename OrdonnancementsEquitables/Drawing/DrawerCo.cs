using OrdonnancementsEquitables.Jobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace OrdonnancementsEquitables.Drawing
{
    public class DrawerCo : Drawer
    {

        /// <summary>
        /// Adds the counter of execution max time in order to execute graphic representation.
        /// </summary>
        private int[] maxExecTime;

        /// <summary>
        /// This construct initialises the new <c>DrawerCo</c> for one machine and one user.
        /// </summary>
        /// <param name="c">The canvas present in MainWindow to be shown.</param>
        public DrawerCo(Canvas c)
            : this(c, 1, 1)
        { }

        /// <summary>
        /// This constructor initialises the new <c>DrawerCo</c> for <paramref name="nbUsers"/> machines and one user.
        /// </summary>
        /// <param name="c">The canvas present in MainWindow to be shown.</param>
        /// <param name="nbUsers">Number of machines.</param>
        public DrawerCo(Canvas c, int nbUsers)
            : this(c, nbUsers, 1)
        { }

        /// <summary>
        /// This constructor initialises the new <c>DrawerCo</c> for <paramref name="nbMachines"/> machines and <paramref name="nbUsers"/> users.
        /// </summary>
        /// <param name="can">The canvas present in MainWindow to be shown.</param>
        /// <param name="nbUsers">Number of users</param>
        /// <param name="nbMachines">Number of machines.</param>
        public DrawerCo(Canvas can, int nbUsers, int nbMachines)
            : base(can, nbUsers, nbMachines)
        {
            maxExecTime = new int[nbMachines];
            for (int i = 0; i < maxExecTime.Length; i++)
            {
                maxExecTime[i] = 10;
            }
        }

        /// <summary>
        /// This method adds the rectangle representing the JobCo <paramref name="j"/> in the graphic for the case where there is one machine ad one user.
        /// </summary>
        /// <param name="j">Parameter of the jobCo that will be added. </param>
        /// <param name="late">Boolean indicating if the job is late or not.</param>
        public void AddJob(JobCo j, bool late) => AddJob(j, late, PickBrush(), 0);

        /// <summary>
        /// This method adds the rectangle representing the JobCo <paramref name="j"/> in the graphic for the case where there is one machine and many users.
        /// </summary>
        /// <param name="machine"> Integer representing which machine has the job.</param>
        /// <param name="j">Parameter of the jobCo that will be added. </param>
        /// <param name="couleur"> Brush of the color of the job, which has to be defined by the userId (UserColor). </param>
        /// <param name="late">Boolean indicating if the job is late or not.</param>
        public void AddJob(JobCo j, bool late, int user) => AddJob(j, late, userColors.Length == 1 ? PickBrush() : userColors[user], 0);

        /// <summary>
        /// This method adds the rectangle representing the JobCo <paramref name="j"/> for <paramref name="machine"/> in the graphic. Case with one or many users and many machines.
        /// </summary>
        /// <param name="machine"> Integer representing which machine has the job.</param>
        /// <param name="j">Parameter of the jobCo that will be added. </param>
        /// <param name="late">Boolean indicating if the job is late or not.</param>
        public void AddJob(JobCo j, bool late, int user, int machine) => AddJob(j, late, userColors.Length == 1 ? PickBrush() : userColors[user], machine);

        /// <summary>
        /// This method adds the rectangle representing the JobCo <paramref name="j"/> for <paramref name="machine"/> in the graphic.
        /// </summary>
        /// <param name="j">Parameter of the jobCo that will be added. </param>
        /// <param name="late">Boolean indicating if the job is late or not.</param>
        /// <param name="couleur"> Brush of the color of the job, which has to be defined by the userId (UserColor). </param>
        /// <param name="machine"> Integer representing which machine has the job.</param>
        private void AddJob(JobCo j, bool late, Brush couleur, int machine)
        {
            int exeT = j.ExecTime;
            int t = j.Time;
            int dif = maxTime[machine] - maxExecTime[machine];
            // Dessin forme
            Rectangle Alpha = new Rectangle
            {
                Fill = couleur,
                Stroke = Brushes.Black,
                Height = hauteur / 2,
                Width = exeT * pixelMultiplier
            };

            Rectangle Beta = new Rectangle
            {
                Fill = couleur,
                Stroke = Brushes.Black,
                Height = hauteur / 2,
                Width = t * pixelMultiplier
            };


            if (late)
            {
                Alpha.Fill = LatePattern(couleur);
                Beta.Fill = LatePattern(couleur);
            }


            // Insertion in canvas
            Canvas.SetTop(Alpha, HeightCal(machine));
            Canvas.SetLeft(Alpha, WidthCalCo(machine));
            Canvas.SetZIndex(Alpha, 1);

            Canvas.SetTop(Beta, HeightCalCo(machine));
            Canvas.SetLeft(Beta, WidthCal(machine));
            Canvas.SetZIndex(Beta, 0);

            //Add ID first
            TextBlock testo = new TextBlock();
            testo.Text = j.Id.ToString();
            Canvas.SetTop(testo, HeightCal(machine) + 5);
            Canvas.SetLeft(testo, WidthCalCo(machine) + 10);
            Panel.Children.Add(testo);
            Canvas.SetZIndex(testo, 2);

            //Add ID
            TextBlock textBlock = new TextBlock();
            textBlock.Text = j.Id.ToString();
            Canvas.SetTop(textBlock, HeightCal(machine) + 30);
            Canvas.SetLeft(textBlock, WidthCal(machine) + 10);
            Panel.Children.Add(textBlock);
            Canvas.SetZIndex(textBlock, 2);

            // Set new last pixels
            maxTime[machine] += t * pixelMultiplier;
            maxExecTime[machine] += exeT * pixelMultiplier;

            Panel.Width = maxTime.Max() + 10;
            Panel.Children.Add(Alpha);
            Panel.Children.Add(Beta);
        }


        /// <summary>
        /// This property calculates the <c>x</c> coordinate of left corner of rectangle based on which <paramref name="machine"/>.
        /// This is specific for JobCo because start point of the polygon corresponds to <c>maxExecTime[machine]</c> and not <c>maxTime[machine]</c>
        /// </summary>
        /// <param name="machine">This parameter identifies the machine.</param>
        /// <returns>Returns the <c>x</c> coordinate in pixels.</returns>
        private int WidthCalCo(int machine) => maxExecTime[machine];

        /// <summary>
        /// This property calculates the <c>x</c> coordinate of left corner of rectangle based on which <paramref name="machine"/> considering it is the time rectangle.
        /// </summary>
        /// <param name="machine">This parameter identifies the machine.</param>
        /// <returns>Returns the <c>x</c> coordinate in pixels.</returns>
        private int HeightCalCo(int machine) => machine * 60 + 10 + (hauteur / 2);
    }
}
