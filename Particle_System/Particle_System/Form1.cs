using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Particle_System
{
    public partial class Form1 : Form
    {
        List<Particle> particles = new List<Particle>();

        List<Emitter> emitters = new List<Emitter>();
        Emitter emitter;

        TeleportIn teleportIn;
        TeleportOut teleportOut;

        public Form1()
        {
            InitializeComponent();
            picDisplay.Image = new Bitmap(picDisplay.Width, picDisplay.Height);

            this.emitter = new Emitter
            {
                Direction = 180,
                Spreading = 10,
                SpeedMin = 10,
                SpeedMax = 10,
                ColorFrom = Color.Gold,
                ColorTo = Color.FromArgb(0, Color.Red),
                ParticlesPerTick = 10,
                X = picDisplay.Width / 2,
                Y = picDisplay.Height / 2 - 150,
            };

            emitters.Add(this.emitter);

            teleportIn = new TeleportIn
            {
                X = picDisplay.Width / 2 - 194,
                Y = picDisplay.Height / 2,
                Radius = 50
            };

            teleportOut = new TeleportOut
            {
                X = picDisplay.Width / 2 + 194,
                Y = picDisplay.Height / 2,
                Radius = 50,
                ExitDirection = 0
            };

            teleportIn.OutputPoint = teleportOut;

            emitter.impactPoints.Add(teleportIn);
            emitter.impactPoints.Add(teleportOut);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            emitter.UpdateState();

            using (var g = Graphics.FromImage(picDisplay.Image))
            {
                g.Clear(Color.Black);

                using (Pen redPen = new Pen(Color.Red, 2))
                {
                    g.DrawLine(redPen, teleportIn.X, teleportIn.Y, teleportOut.X, teleportOut.Y);
                }

                emitter.Render(g);
            }

            picDisplay.Invalidate();
        }

        private void tbDirection_Scroll(object sender, EventArgs e)
        {
            emitter.Direction = tbDirection.Value;
            lblDirection.Text = $"{tbDirection.Value}°";
        }

        private void tbTeleportRadius_Scroll(object sender, EventArgs e)
        {
            teleportIn.Radius = tbTeleportRadius.Value;
            teleportOut.Radius = tbTeleportRadius.Value;
        }

        private void tbExitDirection_Scroll(object sender, EventArgs e)
        {
            teleportOut.ExitDirection = tbExitDirection.Value;
        }

        private void picDisplay_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                teleportIn.X = e.X;
                teleportIn.Y = e.Y;
            }

            else if (e.Button == MouseButtons.Right)
            {
                teleportOut.X = e.X;
                teleportOut.Y = e.Y;
            }
        }
    }
}
