#region File Header

// //---------------------------------------------------------------------------------------------------------------------//
// // User Name: 
// // File Name :BouncingBall.cs
// // Date :2019 / 04 / 23 / 11:47
// // File Data: 2018 / 10 / 18
// //---------------------------------------------------------------------------------------------------------------------//

#endregion

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Media;
using System.Windows.Forms;


namespace IliumManager.Components
{
    public partial class BouncingBall : UserControl
    {
        // Some drawing parameters.
        private Rectangle[] BallLocation;
        private Point[] BallVelocity;
        private Size FormSize;
        private SoundPlayer player;

        private const int BallWidth = 20;
        private const int BallHeight = 20;

        private readonly Random RandomNumber = new Random(255);

        private Brush BallColor = Brushes.DodgerBlue;

        //----------------------------------------------------------------------------------------------------------------//
        public BouncingBall()
        {
            this.InitializeComponent();
            this.Init();
        }

        //----------------------------------------------------------------------------------------------------------------//
        /// <summary>
        ///     Init Form
        /// </summary>
        private void Init()
        {
            this.tmrBall.Enabled = false;
            this.Visible = false;
            // Pick a random start position and velocity.
            var rnd = new Random();
            const int num_balls = 5;
            BallLocation = new Rectangle[num_balls];
            BallVelocity = new Point[num_balls];
            for (int i = 0; i < num_balls; i++)
            {
                BallLocation[i] = new Rectangle(
                    rnd.Next(0, this.ClientSize.Width - BallWidth),
                    rnd.Next(0, this.ClientSize.Height - BallHeight),
                    BallWidth, BallHeight);

                int BallVx = rnd.Next(2, 10);
                int BallVy = rnd.Next(2, 10);
                if (rnd.Next(0, 2) == 0) BallVx = -BallVx;
                if (rnd.Next(0, 2) == 0) BallVy = -BallVy;
                BallVelocity[i] = new Point(BallVx, BallVy);

            }

            // Save the form's size.
            FormSize = ClientSize;

            // Use double buffering to reduce flicker.
            this.SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint |
                ControlStyles.DoubleBuffer,
                true);
            this.UpdateStyles();
        }

        //----------------------------------------------------------------------------------------------------------------//
        /// <summary>
        ///     Timer Tick Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TmrBall_Tick(object sender, EventArgs e)
        {
            this.Tick();
        }

        //----------------------------------------------------------------------------------------------------------------//
        /// <summary>
        ///     Paint event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BouncingBall_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.Clear(this.BackColor);
            for (int i = 0; i < BallLocation.Length; i++)
            {
                e.Graphics.FillEllipse(this.BallColor, BallLocation[i]);
                e.Graphics.DrawEllipse(Pens.Black, BallLocation[i]);
            }
        }

        //----------------------------------------------------------------------------------------------------------------//
        /// <summary>
        ///     To Process Ticks
        /// </summary>
        private void Tick()
        {
            for (int ball_num = 0; ball_num < BallLocation.Length; ball_num++)
            {
                // Move the ball.
                int x = BallLocation[ball_num].X +
                    BallVelocity[ball_num].X;
                int y = BallLocation[ball_num].Y +
                    BallVelocity[ball_num].Y;

                if (x < 0)
                {
                    BallVelocity[ball_num].X = -BallVelocity[ball_num].X;
                    Boing();
                }
                else if (x + BallLocation[ball_num].Width > FormSize.Width)
                {
                    BallVelocity[ball_num].X = -BallVelocity[ball_num].X;
                    Boing();
                }

                if (y < 0)
                {
                    BallVelocity[ball_num].Y = -BallVelocity[ball_num].Y;
                    Boing();
                }
                else if (y + BallLocation[ball_num].Height > FormSize.Height)
                {
                    BallVelocity[ball_num].Y = -BallVelocity[ball_num].Y;
                    Boing();
                }


                if((x >= gate.Location.X && x < (gate.Location.X + gate.Width)) && (y >= gate.Location.Y && y < (gate.Location.Y + gate.Height)))
                {
                    BallVelocity[ball_num].Y = -BallVelocity[ball_num].Y;
                    BallVelocity[ball_num].X = -BallVelocity[ball_num].X;
                    Boing();
                }
                BallLocation[ball_num] = new Rectangle( x, y,
                    BallLocation[ball_num].Width,
                    BallLocation[ball_num].Height);
            }

            this.Refresh();
        }

        //----------------------------------------------------------------------------------------------------------------//
        /// <summary>
        ///     Play the boing sound file resource.
        /// </summary>
        private void Boing()
        {
            using (player = new SoundPlayer(BounceBall.Properties.Resources.boing))
            {
                player.LoadCompleted += new AsyncCompletedEventHandler(LoadCompleted);
                // Load Asynchronously
                player.LoadAsync();
            }
        }
        private void LoadCompleted(object sender, AsyncCompletedEventArgs args)
        {
            player.Play();
            //return;
            var color = Color.FromArgb((byte)this.RandomNumber.Next(), (byte)this.RandomNumber.Next(), (byte)this.RandomNumber.Next());
            this.BallColor = new SolidBrush(color);
        }
        //----------------------------------------------------------------------------------------------------------------//
        /// <summary>
        ///     Method Sets the Ball In Motion...
        /// </summary>
        /// <param name="startStop"></param>
        public void StartStop(bool startStop)
        {
            this.tmrBall.Enabled = startStop;
            this.Visible = startStop;
        }

        //----------------------------------------------------------------------------------------------------------------//
        /// <summary>
        /// Updates the Speed of the Ball
        /// </summary>
        /// <param name="speedValueIn"></param>
        public void BounceBallSpeed(int speedValueIn)
        {
            this.tmrBall.Interval = speedValueIn;
        }
    }
}
//------------------------------------------...ooo000 END OF FILE 000ooo...-------------------------------------------------//