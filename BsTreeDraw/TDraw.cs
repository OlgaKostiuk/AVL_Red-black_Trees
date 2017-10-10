using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TreeCollections;

namespace BsTreeDraw
{
    public partial class TDraw : UserControl
    {
        public TDraw()
        {
            InitializeComponent();
        }

        class BsTreeD : BsTree
        {
            public void Draw(PictureBox pb)
            {
                int dy = pb.Height / (Height() + 1);
                Graphics g = pb.CreateGraphics();
                DrawNode(root, g, 0, pb.Width, dy, 0, pb.Width / 2, 0);
            }
            private void DrawNode(Node p, Graphics g, int left, int right, int dy, int lvl, int xp, int yp)
            {
                if (p == null)
                    return;

                int x = (left + right) / 2;
                int y = ++lvl * dy;

                DrawNode(p.left, g, left, x, dy, lvl, x, y + 10);

                g.DrawLine(new Pen(Color.Black), x, y - 10, xp, yp);
                g.DrawEllipse(new Pen(Color.Black), x - 10, y - 10, 20, 20);
                g.DrawString("" + p.val, new Font("Arial", 10), Brushes.Black, x - 7, y - 7);

                DrawNode(p.right, g, x, right, dy, lvl, x, y + 10);
            }
        }

        BsTreeD bsTreeD = new BsTreeD();
        private void buttonDraw_Click(object sender, EventArgs e)
        {
            int[] arrTree = new int[] { 15, 7, 9, 4, 1, 5, 12, 17, 2 };
            bsTreeD.Init(arrTree);
            bsTreeD.Draw(pictureBoxTree);
        }
    }
}
