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
        private ICanSimulate sim;
        private int Ticks = 0;

        public DrawBox()
        {
            InitializeComponent();

            sim = new DefaultSimulator();
            sim.GenerateMap(1, true);

            mapScreen.Paint += mapScreenPaintEventHandler;

            Task.Run(() => { KeepTickin(); });
        }

        private void KeepTickin(int msRate = 100)
        {
            while (true)
            {
                sim.Simulate();

                LogToConsole(sim.GetDebugEntities());

                if (InvokeRequired)
                {
                    Invoke(new MethodInvoker(mapScreen.Refresh));
                }
                else
                {
                    mapScreen.Refresh();
                }


                Ticks++;
                Thread.Sleep(msRate);
            }
        }

        private float displayFactor
        {
            get
            {
                // Compute magnification factor
                // Make screen at least 2000x2000 flattiverse miles with Ship at center
                // i.e. minimum screenPixels corresponds to 2000 flattiverse miles
                int mapScreenMinDimension = Math.Min(mapScreen.Width, mapScreen.Height);

                float largestX = sim
                    .GetDebugEntities()
                    .Select(q => q.pos.X)
                    .Max();

                float largestY = sim
                    .GetDebugEntities()
                    .Select(q => q.pos.Y)
                    .Max();

                //return mapScreenMinDimension / Math.Max(largestX, largestY);
                return mapScreenMinDimension / 500f;
            }
        }

        private Vector center
        {
            get
            {
                float centerX = mapScreen.Width / 2;
                float centerY = mapScreen.Height / 2;
                return new Vector(centerX, centerY);
            }
        }

        private void mapScreenPaintEventHandler(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            foreach (var mapitem in sim.GetDebugEntities())
            {
                if (mapitem is Sun)
                {
                    var sun = (Sun)mapitem;
                    DrawCircle(g, sun.pos, sun.radius, Pens.Black, Pens.Yellow, sun.name + "\n" + sun.Gravity.ToString("N2"));
                }
                else if (mapitem is Planet)
                {
                    var planet = (Planet)mapitem;
                    DrawCircle(g, planet.pos, planet.radius, Pens.Black, Pens.Green, planet.name + "\n" + planet.Gravity.ToString("N2"));
                }
                else if (mapitem is Moon)
                {
                    var moon = (Moon)mapitem;
                    DrawCircle(g, moon.pos, moon.radius, Pens.Black, Pens.DarkGray, moon.name);
                }
                else if (mapitem is Ship)
                {
                    var ship = (Ship)mapitem;
                    DrawCircle(g, ship.pos, ship.radius, Pens.Black, Pens.Cyan, ship.name);
                }
                else if (mapitem is Bullet)
                {
                    var bullet = (Bullet)mapitem;
                    DrawCircle(g, bullet.pos, bullet.radius, Pens.Black, Pens.Magenta, bullet.name + "|" + bullet.origin);
                }
            }

            DrawBoxOfSize(g, 500f, true);
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

        private void DrawCircle(Graphics g, Vector pos, float radius, Pen clr, Pen FillClr, string msg = "")
        {
            SolidBrush myBrush = new SolidBrush(FillClr.Color);
            //float uX = center.X + pos.X * displayFactor;
            //float uY = center.Y + pos.Y * displayFactor;
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
            //float uX = center.X + pos.X * displayFactor;
            float uY = pos.Y * displayFactor;
            //float uY = center.Y + pos.Y * displayFactor;
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

        private Rectangle GetRectWithCenterAndSize(float centerX, float centerY, float WidthAndHeight)
        {
            var lol = new Rectangle(
                (int)(centerX - (WidthAndHeight / 2)),
                (int)(centerY - (WidthAndHeight / 2)),
                (int)WidthAndHeight,
                (int)WidthAndHeight
                );

            //Console.WriteLine("Rect: {0} {1} {2} {3}", lol.X, lol.Y, lol.Width, lol.Height);

            return lol;
        }

        private void LogToConsole(List<Entity> ents, bool dynamicOnly = true)
        {
            //Console.Clear();

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
