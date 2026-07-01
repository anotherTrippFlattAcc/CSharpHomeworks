namespace ChartApp;

public class ChartDataItem
{
    public string Label { get; set; }
    public double Value { get; set; }
    public Color ItemColor { get; set; }

    public ChartDataItem(string label, double value, Color color)
    {
        Label = label;
        Value = value;
        ItemColor = color;
    }
}