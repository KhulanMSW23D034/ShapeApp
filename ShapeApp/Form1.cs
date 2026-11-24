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

        private Shape selected;
        private bool dragging;
        private Point lastMouse;

        public Form1()
        {
            InitializeComponent();
            DoubleBuffered = true;
            Text = "ShapeApp";
            BuildMenu();

            MouseDown += Form1_MouseDown;
            MouseMove += Form1_MouseMove;
            MouseUp += (s, e) => dragging = false;

            KeyPreview = true;
            KeyDown += Form1_KeyDown;
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

            var colorItem = new ToolStripMenuItem("Өнгө солих");
            colorItem.Click += (s, e) =>
            {
                if (selected == null) return;
                using (var dlg = new ColorDialog { Color = selected.Color })
                {
                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        selected.Color = dlg.Color;
                        Invalidate();
                    }
                }
            };
            shapeMenu.DropDownItems.Add(colorItem);

            shapeMenu.DropDownItems.Add(new ToolStripSeparator());
            var clear = new ToolStripMenuItem("Цэвэрлэх");
            clear.Click += (s, e) => { shapes.Clear(); selected = null; Invalidate(); };
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

        // Хулганаар дүрс сонгох (хамгийн дээр байгааг нь)
        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            selected = null;
            for (int i = shapes.Count - 1; i >= 0; i--)
            {
                if (shapes[i].Contains(e.Location))
                {
                    selected = shapes[i];
                    break;
                }
            }

            if (selected != null)
            {
                dragging = true;
                lastMouse = e.Location;
            }
            Invalidate();
        }

        // Хулганаар чирж зөөх
        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragging && selected != null)
            {
                selected.Move(e.X - lastMouse.X, e.Y - lastMouse.Y);
                lastMouse = e.Location;
                Invalidate();
            }
        }

        // Delete товчоор устгах
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete && selected != null)
            {
                shapes.Remove(selected);
                selected = null;
                Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            foreach (var s in shapes) s.Draw(e.Graphics);

            if (selected != null)
            {
                using (var pen = new Pen(Color.Black, 1) { DashStyle = DashStyle.Dash })
                {
                    e.Graphics.DrawRectangle(pen, selected.X - 3, selected.Y - 3,
                                             selected.Width + 6, selected.Height + 6);
                }
            }
        }
    }
}