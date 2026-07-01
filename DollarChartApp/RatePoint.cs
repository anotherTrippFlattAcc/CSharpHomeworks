namespace DollarChartApp;

public class RatePoint
{
    public DateTime Date { get; set; }
    public double Value { get; set; }

    public RatePoint(DateTime date, double value)
    {
        Date = date;
        Value = value;
    }
}