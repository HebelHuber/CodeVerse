using System;
using System.Collections.Generic;
using System.Text;

namespace CodeVerse.Common
{
    public class Vector
    {
        public float Y { get; set; }
        public float X { get; set; }

        /// <summary>
        /// Length, same as magnitude
        /// </summary>
        public float Length
        {
            get { return Convert.ToSingle(Math.Sqrt((X * X) + (Y * Y))); }
        }

        /// <summary>
        /// Magnitue, same as Length
        /// </summary>
        public float Magnitude
        {
            get { return Length; }
        }

        public Vector normalized
        {
            get
            {
                return this * (1 / Length);
            }
        }

        public void Normalize()
        {
            var tempVec = this.normalized;
            this.X = tempVec.X;
            this.Y = tempVec.Y;
        }

        public float Angle
        {
            get
            {
                return DirectionFromVector(this);
            }
        }

        public float AngleFrom(Vector v)
        {
            return this.Angle - v.Angle;
        }

        public float AngleTo(Vector v)
        {
            return v.Angle - this.Angle;
        }

        public float DistanceTo(Vector v)
        {
            return Vector.VecFromTo(this, v).Length;
        }

        public static float Distance(Vector a, Vector b)
        {
            return Vector.VecFromTo(a, b).Length;
        }

        public static Vector VecFromTo(Vector a, Vector b)
        {
            return new Vector(a.X - b.X, a.Y - b.Y);
        }

        public override string ToString() { return "[" + X.ToFixedLengthString(5) + "|" + Y.ToFixedLengthString(5) + "]"; }

        #region ctors
        public Vector()
        {
            this.X = 0f;
            this.Y = 0f;
        }
        public Vector(Vector vectorToCopy)
        {
            this.X = vectorToCopy.X;
            this.Y = vectorToCopy.Y;
        }

        public Vector(float x, float y)
        {
            this.X = x;
            this.Y = y;
        }

        public Vector(float x, float y, float length = 1)
        {
            this.X = x * length;
            this.Y = y * length;
        }

        public static Vector Zero { get { return new Vector(0, 0); } }

        public static Vector FromAngleLength(float angle, float length = 1f)
        {
            return AngleToVector(angle) * length;
        }

        #endregion

        #region Operators
        public static Vector operator +(Vector l, Vector r)
        {
            return new Vector(l.X + r.X, l.Y + r.Y);
        }

        public static Vector operator -(Vector v)
        {
            return new Vector(-v.X, -v.Y);
        }

        public static Vector operator -(Vector l, Vector r)
        {
            return new Vector(l.X - r.X, l.Y - r.Y);
        }

        public static Vector operator *(Vector vector, float factor)
        {
            return new Vector(vector.X * factor, vector.Y * factor);
        }

        public static Vector operator /(Vector vector, float divisor)
        {
            return new Vector(vector.X / divisor, vector.Y / divisor);
        }

        public static bool operator ==(Vector l, Vector r)
        {
            return l.X == r.X && l.Y == r.Y;
        }

        public override bool Equals(object obj)
        {
            // Check if the object is a Vector.
            // The initial null check is unnecessary as the cast will result in null
            // if obj is null to start with.
            var otherVec = obj as Vector;

            if (otherVec == null)
            {
                // If it is null then it is not equal to this instance.
                return false;
            }

            // Instances are considered equal if the values match.
            return this == otherVec;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator !=(Vector l, Vector r)
        {
            return !(l == r);
        }
        #endregion

        #region Helpers
        private static float DirectionFromVector(Vector v)
        {
            return DirectionFromVector(v.X, v.X);
        }
        private static float DirectionFromVector(float aX, float aY, float bX = 1, float bY = 0)
        {
            double sin = aX * bY - bX * aY;
            double cos = aX * bX + aY * bY;

            float angleDeg = RadToDeg(Math.Atan2(sin, cos));

            return angleDeg;
        }

        private static Vector AngleToVector(float angle)
        {
            float radians = DegToRad(angle);
            return new Vector(Cos(radians), Sin(radians));
        }

        private static float Cos(float inVal)
        {
            return Convert.ToSingle(Math.Cos(Convert.ToDouble(inVal)));
        }

        private static float Sin(float inVal)
        {
            return Convert.ToSingle(Math.Sin(Convert.ToDouble(inVal)));
        }

        private static float DegToRad(float angle)
        {
            return Convert.ToSingle(Math.PI * angle / 180.0f);
        }

        private static float RadToDeg(float angle)
        {
            return Convert.ToSingle(angle * (180.0f / Math.PI));
        }

        private static float DegToRad(double angle)
        {
            return DegToRad(Convert.ToSingle(angle));
        }

        private static float RadToDeg(double angle)
        {
            return RadToDeg(Convert.ToSingle(angle));
        }
        #endregion
    }
}
