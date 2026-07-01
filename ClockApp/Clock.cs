using System.Drawing.Drawing2D;

namespace Clock;

public class Clock: Panel
{
    private System.Windows.Forms.Timer timer;

    public Clock()
    {
        this.DoubleBuffered = true;
        this.BackColor = Color.White;
        this.Resize += (s, e) => this.Invalidate();
        
        timer = new System.Windows.Forms.Timer();
        timer.Interval = 1000;
        timer.Tick += (s, e) => this.Invalidate();
        timer.Start();
    }
    
    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        Graphics g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        
        int centerX = this.Width / 2;
        int centerY = this.Height / 2;
        int radius = Math.Min(this.Width, this.Height) / 2 - 20;

        if (radius <= 0) return;
        
        DateTime now = DateTime.Now;

        DrawClockFace(g, centerX, centerY, radius);
        DrawHands(g, centerX, centerY, radius, now);
    }
    
    private void DrawClockFace(Graphics g, int cx, int cy, int r)
    {
        using (Pen borderPen = new Pen(Color.FromArgb(45, 52, 54), 6f))
        using (Brush faceBrush = new SolidBrush(Color.FromArgb(245, 246, 250)))
        {
            g.FillEllipse(faceBrush, cx - r, cy - r, r * 2, r * 2);
            g.DrawEllipse(borderPen, cx - r, cy - r, r * 2, r * 2);
        }
        
        using (Pen markerPen = new Pen(Color.FromArgb(45, 52, 54), 3f))
        using (Font font = new Font("Arial", (float)(r * 0.12), FontStyle.Bold))
        using (Brush textBrush = new SolidBrush(Color.FromArgb(45, 52, 54)))
        {
            for (int i = 0; i < 12; i++)
            {
                double angle = (i * 30) * Math.PI / 180;
                
                int xStart = cx + (int)(Math.Sin(angle) * (r - 10));
                int yStart = cy - (int)(Math.Cos(angle) * (r - 10));
                int xEnd = cx + (int)(Math.Sin(angle) * r);
                int yEnd = cy - (int)(Math.Cos(angle) * r);
                
                g.DrawLine(markerPen, xStart, yStart, xEnd, yEnd);
                
                int hourNumber = i == 0 ? 12 : i;
                double textAngle = (hourNumber * 30) * Math.PI / 180;
                
                int xText = cx + (int)(Math.Sin(textAngle) * (r - 35));
                int yText = cy - (int)(Math.Cos(textAngle) * (r - 35));
                
                string numStr = hourNumber.ToString();
                SizeF size = g.MeasureString(numStr, font);
                g.DrawString(numStr, font, textBrush, xText - size.Width / 2, yText - size.Height / 2);
            }
        }
    }
    
    private void DrawHands(Graphics g, int cx, int cy, int r, DateTime time)
    {
        double sAngle = time.Second * 6; 
        double mAngle = time.Minute * 6 + time.Second * 0.1;
        double hAngle = (time.Hour % 12) * 30 + time.Minute * 0.5;
        
        int hLength = (int)(r * 0.5);
        int mLength = (int)(r * 0.7);
        int sLength = (int)(r * 0.85);
        
        DrawHand(g, cx, cy, hAngle, hLength, new Pen(Color.FromArgb(45, 52, 54), 7f) { StartCap = LineCap.RoundAnchor, EndCap = LineCap.Round });
        
        DrawHand(g, cx, cy, mAngle, mLength, new Pen(Color.FromArgb(9, 132, 227), 4.5f) { StartCap = LineCap.RoundAnchor, EndCap = LineCap.Round });
        
        DrawHand(g, cx, cy, sAngle, sLength, new Pen(Color.FromArgb(214, 48, 49), 2f) { StartCap = LineCap.RoundAnchor, EndCap = LineCap.ArrowAnchor });
        
        using (Brush centerBrush = new SolidBrush(Color.FromArgb(45, 52, 54)))
        {
            g.FillEllipse(centerBrush, cx - 6, cy - 6, 12, 12);
        }
    }
    
    private void DrawHand(Graphics g, int cx, int cy, double angleDegrees, int length, Pen pen)
    {
        double angleRadians = angleDegrees * Math.PI / 180;
            
        int xEnd = cx + (int)(Math.Sin(angleRadians) * length);
        int yEnd = cy - (int)(Math.Cos(angleRadians) * length);

        g.DrawLine(pen, cx, cy, xEnd, yEnd);
        pen.Dispose();
    }
    
    protected override void Dispose(bool disposing)
    {
        if (disposing && timer != null)
        {
            timer.Stop();
            timer.Dispose();
        }
        base.Dispose(disposing);
    }
}