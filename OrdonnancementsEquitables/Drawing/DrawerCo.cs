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
            : this(1, c, 1)
        { }

        /// <summary>
        /// This constructor initialises the new <c>DrawerCo</c> for <paramref name="nb_machines"/> machines and one user.
        /// </summary>
        /// <param name="nb_machines">Number of machines.</param>
        /// <param name="c">The canvas present in MainWindow to be shown.</param>
        public DrawerCo(int nb_machines, Canvas c)
            : this(nb_machines, c, 1)
        { }

        /// <summary>
        /// This constructor initialises the new <c>DrawerCo</c> for <paramref name="nb_machines"/> machines and <paramref name="users"/> users.
        /// </summary>
        /// <param name="nb_machines">Number of machines.</param>
        /// <param name="can">The canvas present in MainWindow to be shown.</param>
        /// <param name="users">Number of users</param>
        public DrawerCo(int nb_machines, Canvas can, int users)
            : base(nb_machines, can, users)
        {
            maxExecTime = new int[nb_machines];
            for (int i = 0; i < maxExecTime.Length; i++)
            {
                maxExecTime[i] = 0;
            }
        }

        /// <summary>
        /// This method adds the rectangle representing the JobCo <paramref name="j"/> in the graphic for the case where there is one machine ad one user.
        /// </summary>
        /// <param name="j">Parameter of the jobCo that will be added. </param>
        /// <param name="late">Boolean indicating if the job is late or not.</param>
        public void AddJob(JobCo j, bool late) => AddJob(j, late, PickBrush(), 1);

        /// <summary>
        /// This method adds the rectangle representing the JobCo <paramref name="j"/> in the graphic for the case where there is one machine and many users.
        /// </summary>
        /// <param name="machine"> Integer representing which machine has the job.</param>
        /// <param name="j">Parameter of the jobCo that will be added. </param>
        /// <param name="couleur"> Brush of the color of the job, which has to be defined by the userId (UserColor). </param>
        /// <param name="late">Boolean indicating if the job is late or not.</param>
        public void AddJob(JobCo j, bool late, int user) => AddJob(j, late, userColors.Length == 1 ? PickBrush() : userColors[user], 1);

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
            PointCollection pAlpha = new PointCollection
            {
                new Point(0, 0),
                new Point(dif, hauteur),
                new Point(dif + (t * pixelMultiplier), hauteur),
                new Point(exeT * pixelMultiplier, 0)
            };

            Polygon Alpha = new Polygon
            {
                Points = pAlpha,
                Fill = couleur,
                Height = hauteur,
                Stroke = Brushes.Black,
                Width = dif + pixelMultiplier * t
            };

            if (late) { Alpha.Fill = LatePattern(couleur); }

            // Set new last pixels
            maxTime[machine] += t * pixelMultiplier;
            maxExecTime[machine] += exeT * pixelMultiplier;

            // Insertion in canvas
            Canvas.SetTop(Alpha, HeightCal(machine));
            Canvas.SetLeft(Alpha, WidthCalCo(machine));
            Panel.Width = maxTime.Max() + 10;

            Panel.Children.Add(Alpha);
        }

        /// <summary>
        /// This property calculates the <c>x</c> coordinate of left corner of rectangle based on which <paramref name="machine"/>.
        /// This is specific for JobCo because start point of the polygon corresponds to <c>maxExecTime[machine]</c> and not <c>maxTime[machine]</c>
        /// </summary>
        /// <param name="machine">This parameter identifies the machine.</param>
        /// <returns>Returns the <c>x</c> coordinate in pixels.</returns>
        private int WidthCalCo(int machine) => maxExecTime[machine];
    }
}
