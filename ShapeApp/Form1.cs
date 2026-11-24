using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace ShapeApp
{
    public partial class Form1 : Form
    {
        private readonly List<Shape> shapes = new List<Shape>();
        private readonly Random rnd = new Random();
        private readonly MenuStrip menu = new MenuStrip();
        private readonly ToolStripMenuItem shapeMenu = new ToolStripMenuItem("Дүрс");

        public Form1()
        {
            InitializeComponent();
            DoubleBuffered = true;
            Text = "ShapeApp";
            BuildMenu();
        }

        private void BuildMenu()
        {
            menu.Items.Add(shapeMenu);
            Controls.Add(menu);
            MainMenuStrip = menu;

            AddMenuItem("Шулуун", (b, c) => new LineShape(b, c));
            AddMenuItem("Тэгш өнцөгт", (b, c) => new RectangleShape(b, c));
            AddMenuItem("Дугуй", (b, c) => new CircleShape(b, c));
            AddMenuItem("Pie", (b, c) => new PieShape(b, c));
            AddMenuItem("Polygon", (b, c) => new PolygonShape(b, c));
            AddMenuItem("Curve", (b, c) => new CurveShape(b, c));

            shapeMenu.DropDownItems.Add(new ToolStripSeparator());
            var clear = new ToolStripMenuItem("Цэвэрлэх");
            clear.Click += (s, e) => { shapes.Clear(); Invalidate(); };
            shapeMenu.DropDownItems.Add(clear);
        }

        private void AddMenuItem(string text, Func<Rectangle, Color, Shape> factory)
        {
            var item = new ToolStripMenuItem(text);
            item.Click += (s, e) => AddShape(factory);
            shapeMenu.DropDownItems.Add(item);
        }

        private void AddShape(Func<Rectangle, Color, Shape> factory)
        {
            int w = rnd.Next(40, 150);
            int h = rnd.Next(40, 150);
            int top = menu.Height;
            int x = rnd.Next(0, Math.Max(1, ClientSize.Width - w));
            int y = rnd.Next(top, Math.Max(top + 1, ClientSize.Height - h));
            var color = Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));

            shapes.Add(factory(new Rectangle(x, y, w, h), color));
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            foreach (var s in shapes) s.Draw(e.Graphics);
        }
    }
}