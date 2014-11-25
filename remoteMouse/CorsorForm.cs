using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace remoteMouse
{
    public partial class CorsorForm : Form
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.PictureBox pictureBox1;
        public System.Drawing.Color color { set; get; }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        public CorsorForm()
        {
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();

            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Size = new System.Drawing.Size(200, 200);

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pictureBox1);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = Color.Black;
            this.TransparencyKey = this.BackColor;
            this.color = Color.White;
            this.TopMost = true;
            this.ShowInTaskbar = false;
        }

        public void Corsor_Rewrite(int x, int y, Color color)
        {
            Bitmap canvas = new Bitmap(this.pictureBox1.Width, this.pictureBox1.Height);
            Graphics g = Graphics.FromImage(canvas);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            var gp = new System.Drawing.Drawing2D.GraphicsPath();
            gp.AddPolygon(new Point[]{
                new Point(0,0),
                new Point(0,50),
                new Point(10,40),
                new Point(20,60),
                new Point(25,57),
                new Point(15,38),
                new Point(30,38)
            });

            g.FillPath(new SolidBrush(color), gp);
            g.DrawPath(new Pen(Color.FromArgb(1, 1, 1), 2f), gp);

            RectangleF bound = gp.GetBounds();

            gp.Dispose();
            g.Dispose();

            Invoke((MethodInvoker)(() => {
                this.Left += x;
                this.Top += y;
                if (this.Left <= 0) this.Left = 0;
                if (this.Left >= Screen.PrimaryScreen.Bounds.Width) this.Left = Screen.PrimaryScreen.Bounds.Width;
                if (this.Top <= 0) this.Top = 0;
                if (this.Top >= Screen.PrimaryScreen.Bounds.Height) this.Top = Screen.PrimaryScreen.Bounds.Height;
                this.color = color;
                this.ClientSize = new Size((int)bound.Width + 4, (int)bound.Height + 4);
                pictureBox1.Image = canvas;
            }));
        }

        public void Clear()
        {
            Bitmap canvas = new Bitmap(this.pictureBox1.Width, this.pictureBox1.Height);
            Graphics g = Graphics.FromImage(canvas);
            g.Clear(this.BackColor);
            g.Dispose();
            Invoke((MethodInvoker)(() =>
            {
                pictureBox1.Image = canvas;
                this.Refresh();
            }));

        }
 
    }
}

