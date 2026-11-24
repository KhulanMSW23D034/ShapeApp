using System.Drawing;

namespace ShapeApp
{
    public class PolygonShape : Shape
    {
        private readonly Point[] _points;

        public PolygonShape(Rectangle b, Color c) : base(b, c)
        {
            _points = new Point[5];
            for (int i = 0; i < _points.Length; i++)
                _points[i] = new Point(X + Rnd.Next(Width + 1), Y + Rnd.Next(Height + 1));
        }

        public override string Name { get { return "Polygon"; } }

        public override void Move(int dx, int dy)
        {
            base.Move(dx, dy);
            for (int i = 0; i < _points.Length; i++)
                _points[i] = new Point(_points[i].X + dx, _points[i].Y + dy);
        }

        public override void Draw(Graphics g)
        {
            using (var fill = MakeFill()) g.FillPolygon(fill, _points);
            using (var pen = MakePen()) g.DrawPolygon(pen, _points);
            base.Draw(g);
        }
    }
}