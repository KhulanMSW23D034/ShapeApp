using System.Drawing;

namespace ShapeApp
{
    public class RectangleShape : Shape
    {
        public RectangleShape(Rectangle b, Color c) : base(b, c) { }

        public override string Name { get { return "Тэгш өнцөгт"; } }

        public override void Draw(Graphics g)
        {
            using (var fill = MakeFill()) g.FillRectangle(fill, Bounds);
            using (var pen = MakePen()) g.DrawRectangle(pen, Bounds);
            base.Draw(g);
        }
    }
}