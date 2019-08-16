using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CodeVerse.Common;
using CodeVerse.Common.Commands;
using CodeVerse.Common.data;

namespace CodeVerse.Logic.Simulation
{
    public class DefaultSimulator : Simulator
    {
        public override List<shipData> Scan(List<PlayerCommand> input = null)
        {
            if (input == null || input.Count == 0)
                return new List<shipData>();

            return new List<shipData>();
        }

        public override void Simulate(List<PlayerCommand> input = null)
        {
            if (input != null)
            {
                // handle user ticks here
                foreach (var cmd in input)
                {
                    var targetID = entities.FindIndex(q => q.ID == cmd.targetID);

                    if (targetID != -1)
                    {
                        Ship target = (Ship)entities[targetID];

                        if (cmd is MoveCommand)
                        {
                            MoveCommand parsedCmd = (MoveCommand)cmd;
                            target.Velocity += parsedCmd.Force;
                        }
                        if (cmd is ShootCommand)
                        {
                            ShootCommand parsedCmd = (ShootCommand)cmd;
                            var bullet = new Bullet();
                            bullet.name = target.name + "'s bullet";
                            bullet.originID = target.ID;
                            bullet.Velocity = target.Velocity + (parsedCmd.Direction * parsedCmd.Power);
                            bullet.pos = target.pos;
                            entities.Add(bullet);
                        }
                        if (cmd is ShieldCommand)
                        {
                            throw new NotImplementedException();
                        }
                    }
                }
            }

            var Movables = entities
                .Where(q => q is MovingEntity)
                .Select(q => q as MovingEntity)
                .ToList();

            foreach (var movable in Movables)
                ApplyWorldForces(movable);

            foreach (var movable in Movables)
            {
                if (movable.PositionHistory.Count > 250)
                    movable.PositionHistory.RemoveAt(0);

                movable.PositionHistory.Add(movable.pos);
                movable.pos += movable.Velocity;
            }

            var CollidedMovables = new List<Entity>();
            foreach (var movable in Movables)
            {
                var collisions = movable.CollidesWithMultiple(entities);

                if (collisions.Count > 0)
                {
                    CollidedMovables.Add(movable);

                    if (movable is Ship)
                        Console.WriteLine(movable.name + " died colliding with " + collisions[0].name);
                }
            }

            foreach (var item in CollidedMovables)
                entities.Remove(item);
        }

        private void ApplyWorldForces(MovingEntity unit)
        {
            // was nehmen wir alles für Gravity? alles oder alles static oder nur sonnen?

            //var Gravitationals = entities
            //    .Where(q => q != unit)
            //    .ToList();

            var Gravitationals = entities
                .Where(q => q is StaticEntity && q != unit)
                .ToList();

            //var Gravitationals = entities
            //    .Where(q => q is Sun && q != unit)
            //    .ToList();

            foreach (var grav in Gravitationals)
            {
                Vector localVec = Vector.VecFromTo(grav.pos, unit.pos);

                // now for the real shit, real physics formula
                // Force = Gravitational Constant * Mass1 * Mass2 / distance²
                // Gravitational Constant is 6.67408 × 10^(-11) 
                float GravConstant = 0.0000000000667408f;
                GravConstant *= 1000000000f;
                float appliedGravityPower = GravConstant * ((grav.mass * unit.mass) / (localVec.Length * localVec.Length));

                var GravVector = localVec * appliedGravityPower;

                unit.Velocity += GravVector / unit.mass;
            }
        }
    }
}
