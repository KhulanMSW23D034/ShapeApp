using System.Drawing;

namespace ShapeApp
{
    public class PieShape : Shape
    {
        private readonly float _start;
        private readonly float _sweep;

        public PieShape(Rectangle b, Color c) : base(b, c)
        {
            _start = Rnd.Next(0, 360);
            _sweep = Rnd.Next(60, 300);
        }

        public override string Name { get { return "Pie"; } }

        public override void Draw(Graphics g)
        {
            using (var fill = MakeFill()) g.FillPie(fill, Bounds, _start, _sweep);
            using (var pen = MakePen()) g.DrawPie(pen, Bounds, _start, _sweep);
            base.Draw(g);
        }
    }
}