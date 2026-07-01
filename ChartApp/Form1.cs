namespace ChartApp;

public partial class Form1 : Form
{
    private ChartVisualizer chartVisualizer;
    private TextBox txtLabel;
    private NumericUpDown numValue;
    private Button btnColor;
    private Color selectedColor = Color.DodgerBlue;
    
    private List<ChartDataItem> chartData = new List<ChartDataItem>();
    private Random rand = new Random();
    
    public Form1()
    {
        this.Text = "Конструктор диаграмм";
        this.Size = new Size(1000, 600);
        this.MinimumSize = new Size(800, 500);

        InitializeComponents();
        LoadMockData();
        SyncVisualizer();
    }
    
    private void InitializeComponents()
    {
        TableLayoutPanel mainLayout = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2 };
        mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 320f));
        mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
        this.Controls.Add(mainLayout);

        FlowLayoutPanel panelLeft = new FlowLayoutPanel 
        { 
            Dock = DockStyle.Fill, 
            FlowDirection = FlowDirection.TopDown,
            Padding = new Padding(10),
            BackColor = SystemColors.Control
        };
        mainLayout.Controls.Add(panelLeft, 0, 0);

        panelLeft.Controls.Add(new Label { Text = "Тип диаграммы:", Width = 290 });
        ComboBox cbChartType = new ComboBox { Width = 290, DropDownStyle = ComboBoxStyle.DropDownList };
        cbChartType.Items.AddRange(new string[] { "Столбчатая (Bar)", "Круговая (Pie)" });
        cbChartType.SelectedIndex = 0;
        cbChartType.SelectedIndexChanged += (s, e) => {
            chartVisualizer.ChartType = cbChartType.SelectedIndex == 0 ? ChartType.Bar : ChartType.Pie;
        };
        panelLeft.Controls.Add(cbChartType);

        panelLeft.Controls.Add(new Label { Text = "Добавить новый элемент:", Font = new Font("Arial", 9, FontStyle.Bold), Margin = new Padding(0, 15, 0, 5), Width = 290 });

        panelLeft.Controls.Add(new Label { Text = "Категория:", Width = 290 });
        txtLabel = new TextBox { Width = 290 };
        panelLeft.Controls.Add(txtLabel);

        panelLeft.Controls.Add(new Label { Text = "Значение:", Width = 290, Margin = new Padding(0,5,0,0) });
        numValue = new NumericUpDown { Width = 290, Minimum = 0, Maximum = 1000000, DecimalPlaces = 1 };
        panelLeft.Controls.Add(numValue);

        panelLeft.Controls.Add(new Label { Text = "Цвет элемента:", Width = 290, Margin = new Padding(0, 5, 0, 0) });
        btnColor = new Button { Width = 290, Height = 25, BackColor = selectedColor, FlatStyle = FlatStyle.Flat };
        btnColor.Click += (s, e) => {
            using (ColorDialog cd = new ColorDialog())
            {
                if (cd.ShowDialog() == DialogResult.OK)
                {
                    selectedColor = cd.Color;
                    btnColor.BackColor = selectedColor;
                }
            }
        };
        panelLeft.Controls.Add(btnColor);

        Button btnAdd = new Button { Text = "Добавить элемент", Width = 290, Height = 30, Margin = new Padding(0, 10, 0, 10) };
        btnAdd.Click += BtnAdd_Click;
        panelLeft.Controls.Add(btnAdd);

        chartVisualizer = new ChartVisualizer { Dock = DockStyle.Fill };
        mainLayout.Controls.Add(chartVisualizer, 1, 0);
    }
    
    private void LoadMockData()
    {
        chartData.Add(new ChartDataItem("Январь", 120, Color.LightCoral));
        chartData.Add(new ChartDataItem("Февраль", 180, Color.MediumSeaGreen));
        chartData.Add(new ChartDataItem("Март", 240, Color.CornflowerBlue));
        chartData.Add(new ChartDataItem("Апрель", 150, Color.Orange));
    }

    private void SyncVisualizer()
    {
        chartVisualizer.UpdateData(chartData);
    }
    
    private void BtnAdd_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtLabel.Text))
        {
            MessageBox.Show("Введите название категории!", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (numValue.Value <= 0)
        {
            MessageBox.Show("Значение должно быть больше нуля!", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        chartData.Add(new ChartDataItem(txtLabel.Text, (double)numValue.Value, selectedColor));

        txtLabel.Clear();
        numValue.Value = 0;
        selectedColor = Color.FromArgb(rand.Next(50, 220), rand.Next(50, 220), rand.Next(50, 220));
        btnColor.BackColor = selectedColor;

        SyncVisualizer();
    }
}