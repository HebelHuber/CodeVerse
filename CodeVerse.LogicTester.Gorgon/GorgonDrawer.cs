using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DX = SharpDX;
using Gorgon.Core;
using Gorgon.Graphics;
using Gorgon.Graphics.Core;
using Gorgon.Graphics.Imaging.Codecs;
using Gorgon.Renderers;
using Gorgon.UI;
using Gorgon.Timing;
using CodeVerse.Common;
using System.Linq;
using Gorgon.Graphics.Fonts;
using CodeVerse.Logic.Entities;

namespace CodeVerse.LogicTester.Gorgon
{
    internal static class GorgonDrawer
    {
        private static float displayFactor;
        private static Gorgon2D _renderer;
        private static GorgonFont _font;

        internal static void DrawEntities(Gorgon2D renderer, List<Entity> ents, float mapSize, int screenmin, GorgonFont font)
        {
            GorgonDrawer._renderer = renderer;
            GorgonDrawer._font = font;
            displayFactor = screenmin / mapSize;

            bool DrawHistoryLines = true;

            foreach (var mapitem in ents)
            {
                if (mapitem is Sun)
                {
                    var sun = (Sun)mapitem;
                    DrawFilledCircle(sun.pos, sun.radius, GorgonColor.Black, GorgonColor.YellowPure, sun.name + "\n" + sun.mass.ToString("N0"));
                }
                else if (mapitem is Planet)
                {
                    var planet = (Planet)mapitem;
                    DrawFilledCircle(planet.pos, planet.radius, GorgonColor.Black, GorgonColor.GreenPure, planet.name + "\n" + planet.mass.ToString("N0"));
                }
                else if (mapitem is Moon)
                {
                    var moon = (Moon)mapitem;
                    DrawFilledCircle(moon.pos, moon.radius, GorgonColor.Black, GorgonColor.Gray50, moon.name + "\n" + moon.mass.ToString("N0"));
                }
                else if (mapitem is Ship)
                {
                    var ship = (Ship)mapitem;
                    if (DrawHistoryLines) DrawHistory(ship, GorgonColor.RedPure);
                    DrawFilledCircle(ship.pos, ship.radius, GorgonColor.Black, GorgonColor.RedPure, ship.name);
                }
                else if (mapitem is Bullet)
                {
                    var bullet = (Bullet)mapitem;
                    if (DrawHistoryLines) DrawHistory(bullet, GorgonColor.Orange);
                    DrawFilledCircle(bullet.pos, bullet.radius, GorgonColor.Black, GorgonColor.Orange);
                }
                else if (mapitem is Scan)
                {
                    var scan = (Scan)mapitem;
                    DrawScan(scan, GorgonColor.Aquamarine, GorgonColor.Black);
                }
            }
        }

        private static void DrawVector(Vector from, Vector to, GorgonColor clr)
        {
            from *= displayFactor;
            to *= displayFactor;

            _renderer.DrawLine(from.X, from.Y, to.X, to.Y, clr, thickness: 1);
        }

        private static DX.RectangleF RectFromCenterAndRadius(Vector center, float radius)
        {
            return new DX.RectangleF(
                (center.X - radius) * displayFactor,
                (center.Y - radius) * displayFactor,
                (radius * 2) * displayFactor,
                (radius * 2) * displayFactor
                );
        }

        private static void DrawFilledCircle(Vector pos, float radius, GorgonColor clr, GorgonColor FillClr, string msg = "")
        {
            _renderer.DrawFilledEllipse(RectFromCenterAndRadius(pos, radius), FillClr);
            DrawCircle(pos, radius, clr, msg);
        }

        private static void DrawCircle(Vector pos, float radius, GorgonColor clr, string msg = "")
        {
            _renderer.DrawEllipse(RectFromCenterAndRadius(pos, radius), clr, thickness: 1);

            if (msg != "")
            {
                _renderer.DrawString(
                    msg,
                    new DX.Vector2(pos.X * displayFactor, pos.Y * displayFactor),
                    _font,
                    GorgonColor.Black
                );
            }
        }

        private static void DrawScan(Scan scan, GorgonColor fillclr, GorgonColor lineclr)
        {
            // have to calulcate size fo the Arcs bounding box first and use that as rect
            var rect = RectFromCenterAndRadius(scan.pos, scan.range);
            //var rect = new DX.RectangleF(scan.pos.X * displayFactor, scan.pos.Y * displayFactor, scan.range * displayFactor, scan.range * displayFactor);

            _renderer.DrawFilledArc(rect, fillclr, scan.leftEdgeAngle + 90, scan.rightEdgeAngle + 90);
            _renderer.DrawArc(rect, lineclr, scan.leftEdgeAngle + 90, scan.rightEdgeAngle + 90);

            var screenVecLeftEnd = scan.pos + Vector.FromAngleLength(scan.leftEdgeAngle, scan.range);
            var screenVecRightEnd = scan.pos + Vector.FromAngleLength(scan.rightEdgeAngle, scan.range);
            DrawVector(scan.pos, screenVecLeftEnd, lineclr);
            DrawVector(scan.pos, screenVecRightEnd, lineclr);

        }

        private static void DrawHistory(MovingEntity e, GorgonColor clr)
        {
            if (e.PositionHistory.Count == 0)
                return;

            DrawVector(e.pos, e.PositionHistory[e.PositionHistory.Count - 1], clr);

            for (int i = e.PositionHistory.Count - 2; i >= 0; i--)
            {
                DrawVector(e.PositionHistory[i], e.PositionHistory[i + 1], clr);
            }
        }
    }
}
