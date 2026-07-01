using System.ComponentModel;

namespace ChartApp;

public enum ChartType { Bar, Pie }

public class ChartVisualizer: Panel
{
    private List<ChartDataItem> dataItems = new List<ChartDataItem>();
    private ChartType currentChartType = ChartType.Bar;
    
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public ChartType ChartType
    {
        get => currentChartType;
        set { currentChartType = value; this.Invalidate(); }
    }

    public ChartVisualizer()
    {
        this.DoubleBuffered = true;
        this.BackColor = Color.White;
        this.Resize += (s, e) => this.Invalidate();
    }

    public void UpdateData(List<ChartDataItem> newData)
    {
        dataItems = newData;
        this.Invalidate();
    }
    
    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        Graphics g = e.Graphics;
        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

        if (dataItems == null || dataItems.Count == 0)
        {
            using (Font font = new Font("Arial", 14, FontStyle.Italic))
            {
                g.DrawString("Нет данных для отображения", font, Brushes.Gray, 20, 20);
            }
            return;
        }

        if (ChartType == ChartType.Bar) DrawBarChart(g);
        else DrawPieChart(g);
    }
    
    private void DrawBarChart(Graphics g)
    {
        int padding = 50;
        int graphWidth = this.Width - padding * 2;
        int graphHeight = this.Height - padding * 2;

        double maxValue = dataItems.Max(item => item.Value);
        if (maxValue == 0) maxValue = 1;

        int barCount = dataItems.Count;
        int spacing = 15;
        int totalSpacing = spacing * (barCount + 1);
        int barWidth = (graphWidth - totalSpacing) / barCount;

        using (Pen axisPen = new Pen(Color.Black, 2f))
        using (Pen gridPen = new Pen(Color.LightGray, 1f) { DashStyle = System.Drawing.Drawing2D.DashStyle.Dash })
        using (Font font = new Font("Arial", 9))
        {
            for (int i = 0; i <= 4; i++)
            {
                int yGrid = padding + graphHeight - (graphHeight * i / 4);
                g.DrawLine(gridPen, padding, yGrid, padding + graphWidth, yGrid);
                
                double valLabel = (maxValue * i / 4);
                g.DrawString(valLabel.ToString("0.#"), font, Brushes.Black, padding - 35, yGrid - 7);
            }

            g.DrawLine(axisPen, padding, padding, padding, padding + graphHeight); // Y
            g.DrawLine(axisPen, padding, padding + graphHeight, padding + graphWidth, padding + graphHeight); // X

            for (int i = 0; i < barCount; i++)
            {
                var item = dataItems[i];
                int x = padding + spacing + i * (barWidth + spacing);
                int currentBarHeight = (int)((item.Value / maxValue) * graphHeight);
                int y = padding + graphHeight - currentBarHeight;

                if (barWidth > 0 && currentBarHeight > 0)
                {
                    using (Brush brush = new SolidBrush(item.ItemColor))
                    {
                        g.FillRectangle(brush, x, y, barWidth, currentBarHeight);
                        g.DrawRectangle(Pens.Black, x, y, barWidth, currentBarHeight);
                    }

                    string valStr = item.Value.ToString();
                    SizeF valSize = g.MeasureString(valStr, font);
                    g.DrawString(valStr, font, Brushes.Black, x + (barWidth - valSize.Width) / 2, y - 18);

                    SizeF labelSize = g.MeasureString(item.Label, font);
                    g.DrawString(item.Label, font, Brushes.Black, x + (barWidth - labelSize.Width) / 2, padding + graphHeight + 5);
                }
            }
        }
    }
    
    private void DrawPieChart(Graphics g)
    {
        double totalSum = dataItems.Sum(item => item.Value);
        if (totalSum == 0) return;

        int size = Math.Min(this.Width, this.Height) - 100;
        if (size <= 0) return;

        int cx = (this.Width - size) / 3;
        int cy = (this.Height - size) / 2;
        Rectangle rect = new Rectangle(cx, cy, size, size);

        float startAngle = 0f;
        int legendX = cx + size + 40;
        int legendY = cy + 10;

        using (Font font = new Font("Arial", 10))
        {
            for (int i = 0; i < dataItems.Count; i++)
            {
                var item = dataItems[i];
                float sweepAngle = (float)(item.Value / totalSum * 360.0);

                if (sweepAngle > 0)
                {
                    using (Brush brush = new SolidBrush(item.ItemColor))
                    {
                        g.FillPie(brush, rect, startAngle, sweepAngle);
                        g.DrawPie(Pens.Black, rect, startAngle, sweepAngle);

                        g.FillRectangle(brush, legendX, legendY + (i * 25), 15, 15);
                        g.DrawRectangle(Pens.Black, legendX, legendY + (i * 25), 15, 15);

                        double percent = (item.Value / totalSum) * 100;
                        string legendText = $"{item.Label}: {item.Value} ({percent:0.#}%)";
                        g.DrawString(legendText, font, Brushes.Black, legendX + 25, legendY + (i * 25) - 1);
                    }
                }
                startAngle += sweepAngle;
            }
        }
    }
}