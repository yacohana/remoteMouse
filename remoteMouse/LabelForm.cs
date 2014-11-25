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
    public partial class LabelForm : Form
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


        public LabelForm()
        {

            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();

            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            
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

        public void Text_Change(String text, Color color, int x, int y, bool recall)
        {
            Bitmap canvas = new Bitmap(this.pictureBox1.Width, this.pictureBox1.Height);
            Graphics g = Graphics.FromImage(canvas);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            var gp = new System.Drawing.Drawing2D.GraphicsPath();
            FontFamily ff = new FontFamily("MS Gothic");
            gp.AddString(text, ff, 0, 80,
                new Point(0, 0), StringFormat.GenericDefault);
            g.FillPath(new SolidBrush(color), gp);
            Pen p = new Pen(Color.FromArgb(1,1,1), 2.0f);
            g.DrawPath(p, gp);
            RectangleF bound = gp.GetBounds();

            ff.Dispose();
            gp.Dispose();
            g.Dispose();

            Invoke((MethodInvoker)(() =>
            {
                this.Left = x;
                this.Top = y;
                pictureBox1.Top = -(int)bound.Top;
                pictureBox1.Left = -(int)bound.Left;
                pictureBox1.Width = (int)bound.Width + 100;
                pictureBox1.Height = (int)bound.Height + 100;
                this.ClientSize = new Size((int)bound.Width + 2, (int)bound.Height + 2);
                pictureBox1.Image = canvas;
            }));

            if (!recall) Text_Change(text, color, x, y, true);
        }
    }
}
