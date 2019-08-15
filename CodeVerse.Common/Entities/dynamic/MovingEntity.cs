using System;
using System.Collections.Generic;
using System.Text;

namespace CodeVerse.Common
{
    public abstract class MovingEntity : Entity
    {
        public Vector Velocity;
        public List<Vector> PositionHistory = new List<Vector>();
    }
}
