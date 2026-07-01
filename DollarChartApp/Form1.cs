namespace DollarChartApp;

public partial class Form1 : Form
{
    private DualAxisChart rateChart;
    
    public Form1()
    {
        this.Text = "Курс доллара";
        this.Size = new Size(900, 500);
        this.MinimumSize = new Size(600, 400);

        InitializeChart();
        GenerateMockRateData();
    }
    
    private void InitializeChart()
    {
        rateChart = new DualAxisChart { Dock = DockStyle.Fill };
        this.Controls.Add(rateChart);
    }
    
    private void GenerateMockRateData()
    {
        List<RatePoint> mockRates = new List<RatePoint>();
        Random rand = new Random();
        
        DateTime startDate = DateTime.Now.AddYears(-1);
        
        double currentRate = 88.5; 
        
        for (int i = 0; i < 12; i++)
        {
            DateTime pointDate = startDate.AddMonths(i);
            
            double fluctuation = (rand.NextDouble() * 10.5) - 4.5;
            currentRate += fluctuation;
            
            if (currentRate < 82) currentRate = 82.5;
            if (currentRate > 105) currentRate = 103.5;

            mockRates.Add(new RatePoint(pointDate, currentRate));
        }
        
        rateChart.SetData(mockRates);
    }
}