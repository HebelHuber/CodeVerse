using System;
using System.Collections.Generic;
using System.Text;
using CodeVerse.Common;

namespace CodeVerse.Logic.Entities
{
    public abstract class MovingEntity : Entity
    {
        public Vector Velocity;
        public List<Vector> PositionHistory = new List<Vector>();
    }
}
