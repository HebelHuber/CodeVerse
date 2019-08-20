using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeVerse.Common
{
    public static class Physics
    {
        public static bool CollidesWith(this Entity a, Entity b)
        {
            return Vector.Distance(a.pos, b.pos) < a.radius + b.radius;

            //return CircleCollision(a.pos, a.radius, b.pos, b.radius);
        }

        public static List<Entity> CollidesWithMultiple(this Entity e, List<Entity> others)
        {
            var collisions = new List<Entity>();

            foreach (var item in others.Where(q => q != e))
            {
                if (e.CollidesWith(item))
                {
                    collisions.Add(item);
                }
            }

            return collisions;
        }

        public static bool isInsideEntity(this Vector pos, Entity e)
        {
            return Vector.VecFromTo(pos, e.pos).Length < e.radius;
        }

        public static bool isOutsideEntity(this Vector pos, Entity e)
        {
            return !pos.isInsideEntity(e);
        }

        public static bool includesPosition(this Entity e, Vector pos)
        {
            return pos.isInsideEntity(e);
        }

        public static bool isInsideScanArc(this Entity e, Entity scanner, float leftEdgeAngle, float rightEdgeAngle, float ScanRange, bool Log = false)
        {
            bool isScannedHit;
            var debugstring = e.isInsideScanArc(scanner, leftEdgeAngle, rightEdgeAngle, ScanRange, out isScannedHit);
            if (Log) Console.WriteLine(debugstring);
            return isScannedHit;
        }

        private static string isInsideScanArc(this Entity e, Entity scanner, float leftEdgeAngle, float rightEdgeAngle, float ScanRange, out bool result)
        {
            // construct a Vector from scanOrigin to target
            var targetVec = Vector.VecFromTo(scanner.pos, e.pos);

            // early exit if out of range
            if (targetVec.Length > e.radius + ScanRange)
            {
                result = false;
                return "not in range";
            }

            float targetAngle = targetVec.Angle;

            // is it between the edges?
            if (leftEdgeAngle < rightEdgeAngle)
            {
                if (targetAngle >= leftEdgeAngle && targetAngle <= rightEdgeAngle)
                {
                    result = true;
                    return "between angles";
                }
            }

            if (leftEdgeAngle > rightEdgeAngle)
            {
                if (targetAngle >= leftEdgeAngle && targetAngle <= 360
                || targetAngle <= rightEdgeAngle && targetAngle >= 0)
                {
                    result = true;
                    return "between angles";
                }
            }

            Console.WriteLine(string.Format("not between angles: left:{0} right:{1} target:{2}", leftEdgeAngle, rightEdgeAngle, targetAngle));

            // catch the circles intersecting with the edges
            var leftEdgeEndpoint = scanner.pos + Vector.FromAngleLength(leftEdgeAngle, ScanRange);
            var rightEdgeEndPoint = scanner.pos + Vector.FromAngleLength(rightEdgeAngle, ScanRange);

            if (e.IntersectsLine(scanner.pos, leftEdgeEndpoint))
            {
                result = true;
                return "cuts left edge";
            }

            if (e.IntersectsLine(scanner.pos, rightEdgeEndPoint))
            {
                result = true;
                return "cuts right edge";
            }


            result = false;
            return string.Format("no condition matched.");
        }

        /*

        private static bool CircleCollision(Vector v1, float r1, Vector v2, float r2)
        {
            float distSq = (v1.X - v2.X) * (v1.X - v2.X) + (v1.Y - v2.Y) * (v1.Y - v2.Y);
            float radSumSq = (r1 + r2) * (r1 + r2);

            if (distSq == radSumSq) // touching
                return true;
            else if (distSq > radSumSq) // no touchy touchy
                return false;
            else // intersection
                return true;
        }

        private static Vector GetClosestPointOnLineBetweenTwoPoints(Vector lineStart, Vector lineEnd, Vector point)
        {
            var vectorAP = Vector.VecFromTo(lineStart, point);
            var vectorAB = Vector.VecFromTo(lineStart, lineEnd);
            var ABAPproduct = vectorAB.X * vectorAP.X + vectorAB.Y * vectorAP.Y;
            var distance = ABAPproduct / vectorAB.Length;

            if (distance < 0)
                return new Vector(lineStart.X, lineStart.Y);
            else if (distance > 1)
                return new Vector(lineEnd.X, lineEnd.Y);
            else
                return new Vector(lineStart.X + vectorAB.X, lineStart.Y + vectorAB.Y) * distance;
        }

        private static Vector ClosestIntersection(Vector CirclePos, float radius, Vector lineStart, Vector lineEnd)
        {
            Vector intersection1;
            Vector intersection2;
            int intersections = FindLineCircleIntersections(CirclePos, radius, lineStart, lineEnd, out intersection1, out intersection2);

            if (intersections == 1)
                return intersection1;//one intersection

            if (intersections == 2)
            {
                double dist1 = Vector.Distance(intersection1, lineStart);
                double dist2 = Vector.Distance(intersection2, lineStart);

                if (dist1 < dist2)
                    return intersection1;
                else
                    return intersection2;
            }

            return null;// no intersections at all
        }

        private static int FindLineCircleIntersections(Vector CirclePos, float radius, Vector point1, Vector point2, out Vector intersection1, out Vector intersection2)
        {
            float dx, dy, A, B, C, det, t;

            dx = point2.X - point1.X;
            dy = point2.Y - point1.Y;

            A = dx * dx + dy * dy;
            B = 2 * (dx * (point1.X - CirclePos.X) + dy * (point1.Y - CirclePos.Y));
            C = (point1.X - CirclePos.X) * (point1.X - CirclePos.X) + (point1.Y - CirclePos.Y) * (point1.Y - CirclePos.Y) - radius * radius;

            det = B * B - 4 * A * C;
            if ((A <= 0.0000001) || (det < 0))
            {
                // No real solutions.
                intersection1 = new Vector(float.NaN, float.NaN);
                intersection2 = new Vector(float.NaN, float.NaN);
                return 0;
            }
            else if (det == 0)
            {
                // One solution.
                t = -B / (2 * A);
                intersection1 = new Vector(point1.X + t * dx, point1.Y + t * dy);
                intersection2 = new Vector(float.NaN, float.NaN);
                return 1;
            }
            else
            {
                // Two solutions.
                t = (float)((-B + Math.Sqrt(det)) / (2 * A));
                intersection1 = new Vector(point1.X + t * dx, point1.Y + t * dy);
                t = (float)((-B - Math.Sqrt(det)) / (2 * A));
                intersection2 = new Vector(point1.X + t * dx, point1.Y + t * dy);
                return 2;
            }
        }

        */

        public static float Distance(Entity a, Entity b)
        {
            return a.pos.DistanceTo(b.pos);
        }

        private static bool IntersectsLine(this Entity Circle, Vector point1, Vector point2)
        {
            float dx, dy, A, B, C, det, t;

            dx = point2.X - point1.X;
            dy = point2.Y - point1.Y;

            A = dx * dx + dy * dy;
            B = 2 * (dx * (point1.X - Circle.pos.X) + dy * (point1.Y - Circle.pos.Y));
            C = (point1.X - Circle.pos.X) * (point1.X - Circle.pos.X) + (point1.Y - Circle.pos.Y) * (point1.Y - Circle.pos.Y) - Circle.radius * Circle.radius;

            det = B * B - 4 * A * C;

            if ((A <= 0.0000001) || (det < 0))
            {
                // No real solutions.
                //intersection1 = new Vector(float.NaN, float.NaN);
                //intersection2 = new Vector(float.NaN, float.NaN);
                return false;
            }
            else if (det == 0)
            {
                // One solution.
                //t = -B / (2 * A);
                //intersection1 = new Vector(point1.X + t * dx, point1.Y + t * dy);
                //intersection2 = new Vector(float.NaN, float.NaN);
                return true;
            }
            else
            {
                // Two solutions.
                //t = (float)((-B + Math.Sqrt(det)) / (2 * A));
                //intersection1 = new Vector(point1.X + t * dx, point1.Y + t * dy);
                //t = (float)((-B - Math.Sqrt(det)) / (2 * A));
                //intersection2 = new Vector(point1.X + t * dx, point1.Y + t * dy);
                return true;
            }
        }
    }
}