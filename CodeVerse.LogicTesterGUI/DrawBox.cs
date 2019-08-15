using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using CodeVerse.Common;
using CodeVerse.Logic;


namespace CodeVerse.LogicTesterGUI
{
    public partial class DrawBox : Form
    {
        private Simulator sim;
        private int Ticks = 0;
        private List<Entity> ents;

        public DrawBox()
        {
            sim = new DefaultSimulator(200, 1000f, GravityMultiplier: 1, debugmode: true);
            sim.Simulate();

            InitializeComponent();
            mapScreen.Paint += mapScreenPaintEventHandler;

            Task.Run(() => { KeepTickin(); });
        }

        private void KeepTickin(int msRate = 20)
        {
            ents = sim.GetDebugEntities();

            while (true)
            {
                sim.Simulate();
                ents = sim.GetDebugEntities();
                //LogToConsole(ents);

                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                if (InvokeRequired)
                    Invoke(new MethodInvoker(mapScreen.Refresh));
                else
                    mapScreen.Refresh();

                stopwatch.Stop();
                Console.WriteLine("Draw Time: {0}ms", stopwatch.ElapsedMilliseconds);


                Ticks++;
                Thread.Sleep(msRate);
            }
        }

        private float displayFactor
        {
            get
            {
                int mapScreenMinDimension = Math.Min(mapScreen.Width, mapScreen.Height);
                return mapScreenMinDimension / sim.mapsize;
            }
        }

        private void mapScreenPaintEventHandler(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            bool DrawHistoryLines = true;

            foreach (var mapitem in ents)
            {
                if (mapitem is Sun)
                {
                    var sun = (Sun)mapitem;
                    DrawFilledCircle(g, sun.pos, sun.radius, Pens.Black, Pens.Yellow, sun.name + "\n" + sun.mass.ToString("N0"));
                }
                else if (mapitem is Planet)
                {
                    var planet = (Planet)mapitem;
                    DrawFilledCircle(g, planet.pos, planet.radius, Pens.Black, Pens.Green, planet.name + "\n" + planet.mass.ToString("N0"));
                }
                else if (mapitem is Moon)
                {
                    var moon = (Moon)mapitem;
                    DrawFilledCircle(g, moon.pos, moon.radius, Pens.Black, Pens.DarkGray, moon.name + "\n" + moon.mass.ToString("N0"));
                }
                else if (mapitem is Ship)
                {
                    var ship = (Ship)mapitem;
                    if (DrawHistoryLines) DrawHistory(g, ship, Pens.Red);
                    DrawFilledCircle(g, ship.pos, ship.radius, Pens.Black, Pens.Cyan, ship.name);
                }
                else if (mapitem is Bullet)
                {
                    var bullet = (Bullet)mapitem;

                    if (DrawHistoryLines) DrawHistory(g, bullet, Pens.Blue);

                    DrawFilledCircle(g, bullet.pos, bullet.radius, Pens.Black, Pens.Magenta);
                }
            }

            DrawBoxOfSize(g, sim.mapsize, false);
        }

        private void DrawBoxOfSize(Graphics g, float size, bool cross)
        {
            DrawVector(g, new Vector(0, 0), new Vector(0, size), Pens.Red); // hoch
            DrawVector(g, new Vector(0, size), new Vector(size, size), Pens.Red); // oben rüber
            DrawVector(g, new Vector(size, size), new Vector(size, 0), Pens.Red); // drüben runter
            DrawVector(g, new Vector(size, 0), new Vector(0, 0), Pens.Red); // back to origin

            if (cross)
            {
                DrawVector(g, new Vector(0, 0), new Vector(size, size), Pens.Red); // cross
                DrawVector(g, new Vector(0, size), new Vector(size, 0), Pens.Red); // cross
            }
        }

        private void DrawVector(Graphics g, Vector from, Vector to, Pen clr)
        {
            from *= displayFactor;
            to *= displayFactor;
            g.DrawLine(clr, from.X, from.Y, to.X, to.Y);
        }

        private void DrawFilledCircle(Graphics g, Vector pos, float radius, Pen clr, Pen FillClr, string msg = "")
        {
            SolidBrush myBrush = new SolidBrush(FillClr.Color);
            float uX = pos.X * displayFactor;
            float uY = pos.Y * displayFactor;
            float uR = radius * displayFactor;
            g.FillEllipse(myBrush, uX - uR, uY - uR, uR * 2, uR * 2);
            myBrush.Dispose();

            DrawCircle(g, pos, radius, clr, msg);
        }

        private void DrawCircle(Graphics g, Vector pos, float radius, Pen clr, string msg = "")
        {
            float uX = pos.X * displayFactor;
            float uY = pos.Y * displayFactor;
            float uR = radius * displayFactor;
            g.DrawEllipse(clr, uX - uR, uY - uR, uR * 2, uR * 2);

            if (msg != "")
            {
                Font drawFont = new Font("Arial", 10);
                SolidBrush drawBrush = new SolidBrush(Color.Black);

                //StringFormat drawFormat = new StringFormat();
                StringFormat format = new StringFormat
                {
                    LineAlignment = StringAlignment.Center,
                    Alignment = StringAlignment.Center
                };
                //drawFormat.FormatFlags = StringFormatFlags.DirectionVertical;
                g.DrawString(msg, drawFont, drawBrush, uX, uY, format);
            }
        }

        private void DrawHistory(Graphics g, MovingEntity e, Pen clr, int length = 10)
        {
            DrawVector(g, e.pos, e.PositionHistory.Last(), clr);

            for (int i = e.PositionHistory.Count - 2; i >= 0; i--)
            {
                DrawVector(g, e.PositionHistory[i], e.PositionHistory[i + 1], clr);
            }

            //for (int i = 0; i < length; i++)
            //{
            //    if (i < e.PositionHistory.Count - 1)
            //        DrawVector(g, e.PositionHistory[i], e.PositionHistory[i + 1], clr);
            //    else break;
            //}
        }

        private void LogToConsole(List<Entity> ents, bool dynamicOnly = true)
        {
            Console.Clear();

            Console.WriteLine("tick:" + Ticks);

            foreach (var item in ents)
            {
                if (dynamicOnly && !(item is MovingEntity))
                    continue;

                if (item is Sun)
                    Console.ForegroundColor = ConsoleColor.Yellow;
                else if (item is Planet)
                    Console.ForegroundColor = ConsoleColor.Green;
                else if (item is Moon)
                    Console.ForegroundColor = ConsoleColor.Gray;
                else if (item is Ship)
                    Console.ForegroundColor = ConsoleColor.Cyan;
                else if (item is Bullet)
                    Console.ForegroundColor = ConsoleColor.Magenta;

                Console.WriteLine(item.ToStringWithProps());
                Console.ResetColor();
            }
        }
    }
}
