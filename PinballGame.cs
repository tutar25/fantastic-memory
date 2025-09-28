using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace PinballGame
{
    public partial class PinballGame : Form
    {
        Timer timer = new Timer();
        float ballX = 180, ballY = 330, vx = 3, vy = -6, r = 14;
        List<RectangleF> bumpers = new List<RectangleF>();
        RectangleF paddle = new RectangleF(140, 360, 80, 16);
        int score = 0;
        bool left = false, right = false;

        public PinballGame()
        {
            InitializeComponent();
            this.Width = 400; this.Height = 440; DoubleBuffered = true;
            bumpers.Add(new RectangleF(100, 180, 24, 24));
            bumpers.Add(new RectangleF(250, 160, 24, 24));
            bumpers.Add(new RectangleF(160, 80, 24, 24));
            timer.Interval = 16;
            timer.Tick += (s,e)=>{ UpdateGame(); Invalidate(); };
            timer.Start();
            KeyDown += (s,e) => { if (e.KeyCode==Keys.Left) left=true; if(e.KeyCode==Keys.Right) right=true; };
            KeyUp += (s,e) => { if (e.KeyCode==Keys.Left) left=false; if(e.KeyCode==Keys.Right) right=false; };
            Text = "Pinball Game";
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.Clear(Color.Black);
            foreach(var b in bumpers) e.Graphics.FillEllipse(Brushes.Orange, b);
            e.Graphics.FillRectangle(Brushes.Cyan, paddle);
            e.Graphics.FillEllipse(Brushes.White, ballX-r, ballY-r, r*2, r*2);
            e.Graphics.DrawString("Score: "+score, Font, Brushes.White, 10, 10);
        }
        void UpdateGame()
        {
            if (left) paddle.X -= 7;
            if (right) paddle.X += 7;
            paddle.X = Math.Max(0, Math.Min(320, paddle.X));
            ballX += vx; ballY += vy;
            if (ballX - r < 0 || ballX + r > 400) vx *= -1;
            if (ballY - r < 0) vy *= -1;
            if (ballY + r > 400) { ballX=180; ballY=330; vx=3; vy=-6; score=0; }
            if (paddle.Contains(ballX, ballY+r)) vy = -Math.Abs(vy);
            foreach(var b in bumpers)
                if (b.Contains(ballX, ballY)) { vy *= -1; score+=10; }
        }
        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.Run(new PinballGame());
        }
    }
}