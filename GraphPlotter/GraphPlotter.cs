using System.ComponentModel;

namespace GraphPlotter;

public class GraphPlotter: Panel
{
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    [DefaultValue(-10.0)]
    public double XMin { get; set; } = -10;

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    [DefaultValue(10.0)]
    public double XMax { get; set; } = 10;

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    [DefaultValue(-10.0)]
    public double YMin { get; set; } = -10;

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    [DefaultValue(10.0)]
    public double YMax { get; set; } = 10;

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public Color GraphColor { get; set; } = Color.Blue;

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    [DefaultValue(2f)]
    public float GraphThickness { get; set; } = 2f;
    
    private Func<double, double> currentFunction = null;
    
    public GraphPlotter()
    {
        this.DoubleBuffered = true;
        this.BackColor = Color.White;
        this.Resize += (s, e) => this.Invalidate();
    }

    public void SetFunction(Func<double, double> func)
    {
        currentFunction = func;
        this.Invalidate();
    }

    private int ToScreenX(double x)
    {
        return (int)((x - XMin) / (XMax - XMin) * this.Width);
    }

    private int ToScreenY(double y)
    {
        return (int)(this.Height - (y - YMin) / (YMax - YMin) * this.Height);
    }
    
    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        Graphics g = e.Graphics;
        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

        DrawGridAndAxes(g);
        DrawFunction(g);
    }
    
    private void DrawGridAndAxes(Graphics g)
    {
        using (Pen axisPen = new Pen(Color.Black, 2f))
        using (Pen gridPen = new Pen(Color.LightGray, 1f) { DashStyle = System.Drawing.Drawing2D.DashStyle.Dash })
        using (Font font = new Font("Arial", 8))
        using (Brush brush = new SolidBrush(Color.Black))
        {
            for (double x = Math.Ceiling(XMin); x <= XMax; x += 1.0)
            {
                int px = ToScreenX(x);
                g.DrawLine(gridPen, px, 0, px, this.Height);
                if (x != 0 && px > 0 && px < this.Width)
                    g.DrawString(x.ToString(), font, brush, px - 5, ToScreenY(0) + 5);
            }

            for (double y = Math.Ceiling(YMin); y <= YMax; y += 1.0)
            {
                int py = ToScreenY(y);
                g.DrawLine(gridPen, 0, py, this.Width, py);
                if (y != 0 && py > 0 && py < this.Height)
                    g.DrawString(y.ToString(), font, brush, ToScreenX(0) + 5, py - 5);
            }

            int cx = ToScreenX(0);
            int cy = ToScreenY(0);

            if (cy >= 0 && cy <= this.Height)
                g.DrawLine(axisPen, 0, cy, this.Width, cy);
            
            if (cx >= 0 && cx <= this.Width)
                g.DrawLine(axisPen, cx, 0, cx, this.Height);

            g.DrawString("0", font, brush, cx - 12, cy + 5);
        }
    }
    
    private void DrawFunction(Graphics g)
    {
        if (currentFunction == null) return;

        List<Point> points = new List<Point>();
        double step = (XMax - XMin) / this.Width;

        for (double x = XMin; x <= XMax; x += step)
        {
            try
            {
                double y = currentFunction(x);

                if (double.IsNaN(y) || double.IsInfinity(y)) continue;

                int px = ToScreenX(x);
                int py = ToScreenY(y);

                if (py >= -1000 && py <= this.Height + 1000)
                {
                    points.Add(new Point(px, py));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        if (points.Count > 1)
        {
            using (Pen graphPen = new Pen(GraphColor, GraphThickness))
            {
                for (int i = 0; i < points.Count - 1; i++)
                {
                    if (Math.Abs(points[i].Y - points[i + 1].Y) < this.Height * 2)
                    {
                        g.DrawLine(graphPen, points[i], points[i + 1]);
                    }
                }
            }
        }
    }
}