using System;
using System.Collections.Generic;
using System.Text;

namespace CodeVerse.Common.data
{
    public class RadarBlip
    {
        public float size;
        public float mass;
        public Vector pos;
        public BlipType kind;
    }

    public enum BlipType
    {
        Sun,
        Planet,
        Moon,
        Ship,
        Bullet,
    }
}
