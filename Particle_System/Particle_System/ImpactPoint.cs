using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Particle_System
{
    public abstract class IImpactPoint
    {
        public float X;
        public float Y;

        public abstract void ImpactParticle(Particle particle);

        public virtual void Render(Graphics g)
        {
            g.FillEllipse(
                    new SolidBrush(Color.Red),
                    X - 5,
                    Y - 5,
                    10,
                    10
                );
        }
    }

    public class TeleportIn : IImpactPoint
    {
        public int Radius = 50;
        public TeleportOut OutputPoint;

        public override void ImpactParticle(Particle particle)
        {
            if (OutputPoint == null) return;

            float dx = X - particle.X;
            float dy = Y - particle.Y;
            double distanceToCenter = Math.Sqrt(dx * dx + dy * dy);

            if (distanceToCenter <= Radius + particle.Radius)
            {
                float dxToParticle = particle.X - X;
                float dyToParticle = particle.Y - Y;
                float angleIn = (float)Math.Atan2(dyToParticle, dxToParticle);

                float speed = (float)Math.Sqrt(particle.SpeedX * particle.SpeedX + particle.SpeedY * particle.SpeedY);

                double rad = OutputPoint.ExitDirection * Math.PI / 180.0;
                double cos = Math.Cos(rad);
                double sin = Math.Sin(rad);

                double oldX = particle.SpeedX;
                double oldY = particle.SpeedY;

                particle.SpeedX = -(float)(oldX * cos - oldY * sin);
                particle.SpeedY = -(float)(oldX * sin + oldY * cos);

                float newSpeed = (float)Math.Sqrt(particle.SpeedX * particle.SpeedX + particle.SpeedY * particle.SpeedY);
                if (newSpeed > 0)
                {
                    particle.SpeedX = particle.SpeedX / newSpeed * speed;
                    particle.SpeedY = particle.SpeedY / newSpeed * speed;
                }

                float angleOut = angleIn + OutputPoint.ExitDirection * (float)(Math.PI / 180.0);

                float radius = Radius;
                particle.X = OutputPoint.X + radius * (float)Math.Cos(angleOut);
                particle.Y = OutputPoint.Y + radius * (float)Math.Sin(angleOut);
            }
        }

        public override void Render(Graphics g)
        {
            g.DrawEllipse(
                   new Pen(Color.OrangeRed),
                   X - Radius,
                   Y - Radius,
                   Radius * 2,
                   Radius * 2
               );
        }
    }

    public class TeleportOut : IImpactPoint
    {
        public int Radius = 50;
        public int ExitDirection;

        public override void ImpactParticle(Particle particle)
        {
            return;
        }

        public override void Render(Graphics g)
        {
            g.DrawEllipse(
                   new Pen(Color.Blue),
                   X - Radius,
                   Y - Radius,
                   Radius * 2,
                   Radius * 2
               );
        }
    }
}
