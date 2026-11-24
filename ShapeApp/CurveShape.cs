using System.Drawing;

namespace ShapeApp
{
    public class CurveShape : Shape
    {
        private readonly Point[] _points;

        public CurveShape(Rectangle b, Color c) : base(b, c)
        {
            _points = new Point[5];
            for (int i = 0; i < _points.Length; i++)
                _points[i] = new Point(X + Rnd.Next(Width + 1), Y + Rnd.Next(Height + 1));
        }

        public override string Name { get { return "Curve"; } }

        public override void Draw(Graphics g)
        {
            using (var pen = MakePen()) g.DrawCurve(pen, _points);
            base.Draw(g);
        }
    }
}