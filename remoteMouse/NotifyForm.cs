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
    public partial class NotifyForm : Form
    {
        public NotifyForm()
        {
            InitializeComponent();
            ShowInTaskbar = false;
            WindowState = FormWindowState.Minimized;
        }

        private void resetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (var corsor in Program.corsors)
            {
                corsor.Clear();
            }
            foreach (var lf in Program.lfs)
            {
                lf.Text_Change("",lf.color,0,0,false);
            }
            Program.corsorDict = new Dictionary<string, int>();
            Program.lfDict = new Dictionary<string, int>();
            Program.corsorItr = 0;
            Program.lfItr = 0;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            notifyIcon1.Visible = false;
            Application.Exit();
        }

        private void NotifyForm_Load(object sender, EventArgs e)
        {
            Hide();
        }
    }
}
