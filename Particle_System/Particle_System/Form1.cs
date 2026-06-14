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

        DotCounter dotCounter;

        Radar radar;

        public Form1()
        {
            InitializeComponent();
            picDisplay.Image = new Bitmap(picDisplay.Width, picDisplay.Height);

            this.KeyPreview = true;
            this.KeyDown += Form1_KeyDown;

            this.picDisplay.MouseMove += PicDisplay_MouseMove;
            picDisplay.MouseWheel += picDisplay_MouseWheel;

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
                X = picDisplay.Width / 2 - picDisplay.Width / 4,
                Y = picDisplay.Height / 2,
                Radius = 50
            };

            teleportOut = new TeleportOut
            {
                X = picDisplay.Width / 2 + picDisplay.Width / 4,
                Y = picDisplay.Height / 2,
                Radius = 50,
                ExitDirection = 0
            };

            dotCounter = new DotCounter
            {
                X = picDisplay.Width / 2,
                Y = picDisplay.Height / 4 + picDisplay.Height / 2,
                Radius = 50
            };

            radar = new Radar
            {
                X = picDisplay.Width / 2,
                Y = picDisplay.Height / 2,
                Radius = 50
            };

            teleportIn.OutputPoint = teleportOut;

            emitter.impactPoints.Add(teleportIn);
            emitter.impactPoints.Add(teleportOut);

            emitter.impactPoints.Add(dotCounter);

            emitter.impactPoints.Add(radar);


            lblDirection.Text = $"{tbDirection.Value}°";
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

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                Point cursorPos = picDisplay.PointToClient(Cursor.Position);

                if (cursorPos.X >= 0 && cursorPos.X <= picDisplay.Width &&
                    cursorPos.Y >= 0 && cursorPos.Y <= picDisplay.Height)
                {
                    DotCounter newDotCounter = new DotCounter
                    {
                        X = cursorPos.X,
                        Y = cursorPos.Y,
                        Radius = 50
                    };

                    emitter.impactPoints.Add(newDotCounter);
                }

                e.Handled = true;
            }
        }

        private void PicDisplay_MouseMove(object sender, MouseEventArgs e)
        {
            radar.X = e.X;
            radar.Y = e.Y;
        }

        private void picDisplay_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0)
            {
                radar.Radius += 5;
            }
            else if (e.Delta < 0)
            {
                radar.Radius -= 5;
            }
        }
    }
}
