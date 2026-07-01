using System.Drawing.Drawing2D;

namespace DollarChartApp;

public class DualAxisChart : Panel
{
    private List<RatePoint> rates = new List<RatePoint>();
    
    public DualAxisChart()
    {
        this.DoubleBuffered = true;
        this.BackColor = Color.FromArgb(250, 250, 250);
        this.Resize += (s, e) => this.Invalidate();
    }
    
    public void SetData(List<RatePoint> data)
    {
        rates = data.OrderBy(r => r.Date).ToList();
        this.Invalidate();
    }
    
    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        Graphics g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        
        if (rates == null || rates.Count < 2) return;
        
        int paddingLeft = 60;
        int paddingRight = 60;
        int paddingTop = 40;
        int paddingBottom = 50;
        
        int graphWidth = this.Width - paddingLeft - paddingRight;
        int graphHeight = this.Height - paddingTop - paddingBottom;
        
        if (graphWidth <= 0 || graphHeight <= 0) return;
        
        double yMinRub = Math.Floor(rates.Min(r => r.Value) - 2);
        double yMaxRub = Math.Ceiling(rates.Max(r => r.Value) + 2);
        double firstRate = rates[0].Value;
        
        Func<int, int> toScreenX = (index) => paddingLeft + (index * graphWidth / (rates.Count - 1));
        Func<double, int> toScreenY = (val) =>
            paddingTop + graphHeight - (int)((val - yMinRub) / (yMaxRub - yMinRub) * graphHeight);
        
        using (Pen gridPen = new Pen(Color.LightGray, 1f) { DashStyle = DashStyle.Dash })
        using (Font font = new Font("Segoe UI", 9))
        using (Brush textBrush = new SolidBrush(Color.FromArgb(64, 64, 64)))
        {
            int gridLines = 5;
            for (int i = 0; i <= gridLines; i++)
            {
                double rubValue = yMinRub + (yMaxRub - yMinRub) * i / gridLines;
                int y = toScreenY(rubValue);
                
                g.DrawLine(gridPen, paddingLeft, y, paddingLeft + graphWidth, y);
                
                g.DrawString($"{rubValue:F1} ₽", font, textBrush, paddingLeft - 50, y - 7);
                
                double pctValue = ((rubValue - firstRate) / firstRate) * 100;
                string pctSign = pctValue >= 0 ? "+" : "";
                g.DrawString($"{pctSign}{pctValue:F1}%", font, textBrush, paddingLeft + graphWidth + 10, y - 7);
            }
            
            int dateStep = Math.Max(1, rates.Count / 6);
            for (int i = 0; i < rates.Count; i += dateStep)
            {
                int x = toScreenX(i);
                string dateStr = rates[i].Date.ToString("MMM yy");
                SizeF size = g.MeasureString(dateStr, font);
                g.DrawString(dateStr, font, textBrush, x - size.Width / 2, paddingTop + graphHeight + 10);
            }
        }
        
        using (Pen axisPen = new Pen(Color.DarkGray, 1.5f))
        {
            g.DrawLine(axisPen, paddingLeft, paddingTop, paddingLeft, paddingTop + graphHeight);
            
            g.DrawLine(axisPen, paddingLeft + graphWidth, paddingTop, paddingLeft + graphWidth,
                paddingTop + graphHeight);
            
            g.DrawLine(axisPen, paddingLeft, paddingTop + graphHeight, paddingLeft + graphWidth,
                paddingTop + graphHeight);
        }
        
        using (Font captionFont = new Font("Segoe UI", 9, FontStyle.Bold))
        {
            g.DrawString("Курс USD (₽)", captionFont, Brushes.Crimson, paddingLeft - 50, paddingTop - 25);
            g.DrawString("Изменение (%)", captionFont, Brushes.MediumBlue, paddingLeft + graphWidth - 30,
                paddingTop - 25);
        }
        
        List<Point> graphPoints = new List<Point>();
        for (int i = 0; i < rates.Count; i++)
        {
            graphPoints.Add(new Point(toScreenX(i), toScreenY(rates[i].Value)));
        }
        
        using (LinearGradientBrush areaBrush = new LinearGradientBrush(
                   new Point(0, paddingTop), new Point(0, paddingTop + graphHeight),
                   Color.FromArgb(60, 220, 20, 60), Color.FromArgb(0, 255, 255, 255)))
        {
            GraphicsPath path = new GraphicsPath();
            path.AddLine(paddingLeft, paddingTop + graphHeight, graphPoints[0].X, graphPoints[0].Y);
            for (int i = 1; i < graphPoints.Count; i++)
                path.AddLine(graphPoints[i - 1], graphPoints[i]);
            path.AddLine(graphPoints[graphPoints.Count - 1].X, graphPoints[graphPoints.Count - 1].Y,
                paddingLeft + graphWidth, paddingTop + graphHeight);
            path.CloseFigure();
            g.FillPath(areaBrush, path);
        }
        
        using (Pen trendPen = new Pen(Color.Crimson, 2.5f) { LineJoin = LineJoin.Round })
        {
            g.DrawLines(trendPen, graphPoints.ToArray());
        }
        
        using (Brush nodeBrush = new SolidBrush(Color.White))
        using (Pen nodePen = new Pen(Color.Crimson, 2f))
        {
            foreach (var p in graphPoints)
            {
                g.FillEllipse(nodeBrush, p.X - 3, p.Y - 3, 6, 6);
                g.DrawEllipse(nodePen, p.X - 3, p.Y - 3, 6, 6);
            }
        }
    }
}