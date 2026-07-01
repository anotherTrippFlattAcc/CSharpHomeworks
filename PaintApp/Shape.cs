namespace PaintApp;

public enum ShapeType { Line, Rectangle, Ellipse }

public abstract class Shape
{
    public Point StartPoint { get; set; }
    public Point EndPoint { get; set; }
    public Color Color { get; set; } = Color.Black;
    public float PenWidth { get; set; } = 2f;

    public abstract void Draw(Graphics g);
}

public class LineShape : Shape
{
    public override void Draw(Graphics g)
    {
        using (Pen pen = new Pen(Color, PenWidth))
        {
            g.DrawLine(pen, StartPoint, EndPoint);
        }
    }
}

public class RectangleShape : Shape
{
    public override void Draw(Graphics g)
    {
        using (Pen pen = new Pen(Color, PenWidth))
        {
            int x = Math.Min(StartPoint.X, EndPoint.X);
            int y = Math.Min(StartPoint.Y, EndPoint.Y);
            int width = Math.Abs(StartPoint.X - EndPoint.X);
            int height = Math.Abs(StartPoint.Y - EndPoint.Y);
            g.DrawRectangle(pen, x, y, width, height);
        }
    }
}

public class EllipseShape : Shape
{
    public override void Draw(Graphics g)
    {
        using (Pen pen = new Pen(Color, PenWidth))
        {
            int x = Math.Min(StartPoint.X, EndPoint.X);
            int y = Math.Min(StartPoint.Y, EndPoint.Y);
            int width = Math.Abs(StartPoint.X - EndPoint.X);
            int height = Math.Abs(StartPoint.Y - EndPoint.Y);
            g.DrawEllipse(pen, x, y, width, height);
        }
    }
}