namespace GraphPlotter;

public partial class Form1 : Form
{
    private GraphPlotter plotter;
    private ComboBox cbFunctions;
    private NumericUpDown numXMin, numXMax, numYMin, numYMax;
    private Button btnColor;
    private Color selectedColor = Color.Crimson;
    
    public Form1()
    {
        this.Text = "Построитель графиков функций";
        this.Size = new Size(950, 650);
        this.MinimumSize = new Size(700, 500);

        InitializeComponents();
        UpdatePlotterSettings();
        OnFunctionChanged();
    }
    
    private void InitializeComponents()
        {
            TableLayoutPanel mainLayout = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2 };
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 250f));
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
            this.Controls.Add(mainLayout);

            FlowLayoutPanel panelSettings = new FlowLayoutPanel 
            { 
                Dock = DockStyle.Fill, 
                FlowDirection = FlowDirection.TopDown,
                Padding = new Padding(10),
                BackColor = SystemColors.Control
            };
            mainLayout.Controls.Add(panelSettings, 0, 0);

            panelSettings.Controls.Add(new Label { Text = "Выберите функцию f(x):", Width = 220, Margin = new Padding(0,10,0,3) });
            cbFunctions = new ComboBox { Width = 220, DropDownStyle = ComboBoxStyle.DropDownList };
            cbFunctions.Items.AddRange(new string[] {
                "f(x) = sin(x)",
                "f(x) = cos(x)",
                "f(x) = tan(x)",
                "f(x) = x²",
                "f(x) = x³",
                "f(x) = sqrt(x)",
                "f(x) = 1 / x"
            });
            cbFunctions.SelectedIndex = 0;
            cbFunctions.SelectedIndexChanged += (s, e) => OnFunctionChanged();
            panelSettings.Controls.Add(cbFunctions);

            panelSettings.Controls.Add(new Label { Text = "Диапазон X (Мин / Макс):", Width = 220, Margin = new Padding(0, 15, 0, 3) });
            numXMin = new NumericUpDown { Minimum = -100, Maximum = -1, Value = -10, Width = 105 };
            numXMax = new NumericUpDown { Minimum = 1, Maximum = 100, Value = 10, Width = 105 };
            
            FlowLayoutPanel xPanel = new FlowLayoutPanel { Width = 220, Height = 30 };
            xPanel.Controls.AddRange(new Control[] { numXMin, numXMax });
            panelSettings.Controls.Add(xPanel);

            panelSettings.Controls.Add(new Label { Text = "Диапазон Y (Мин / Макс):", Width = 220, Margin = new Padding(0, 10, 0, 3) });
            numYMin = new NumericUpDown { Minimum = -100, Maximum = -1, Value = -10, Width = 105 };
            numYMax = new NumericUpDown { Minimum = 1, Maximum = 100, Value = 10, Width = 105 };

            FlowLayoutPanel yPanel = new FlowLayoutPanel { Width = 220, Height = 30 };
            yPanel.Controls.AddRange(new Control[] { numYMin, numYMax });
            panelSettings.Controls.Add(yPanel);

            EventHandler rangeChanged = (s, e) => UpdatePlotterSettings();
            numXMin.ValueChanged += rangeChanged;
            numXMax.ValueChanged += rangeChanged;
            numYMin.ValueChanged += rangeChanged;
            numYMax.ValueChanged += rangeChanged;

            panelSettings.Controls.Add(new Label { Text = "Цвет линии:", Width = 220, Margin = new Padding(0, 15, 0, 3) });
            btnColor = new Button { Width = 220, Height = 30, BackColor = selectedColor, FlatStyle = FlatStyle.Flat };
            btnColor.Click += (s, e) => {
                using (ColorDialog cd = new ColorDialog())
                {
                    if (cd.ShowDialog() == DialogResult.OK)
                    {
                        selectedColor = cd.Color;
                        btnColor.BackColor = selectedColor;
                        plotter.GraphColor = selectedColor;
                        plotter.Invalidate();
                    }
                }
            };
            panelSettings.Controls.Add(btnColor);

            plotter = new GraphPlotter { Dock = DockStyle.Fill, GraphColor = selectedColor };
            mainLayout.Controls.Add(plotter, 1, 0);
        }

        private void UpdatePlotterSettings()
        {
            if (numXMin.Value >= numXMax.Value || numYMin.Value >= numYMax.Value) return;

            plotter.XMin = (double)numXMin.Value;
            plotter.XMax = (double)numXMax.Value;
            plotter.YMin = (double)numYMin.Value;
            plotter.YMax = (double)numYMax.Value;
            plotter.Invalidate();
        }

        private void OnFunctionChanged()
        {
            switch (cbFunctions.SelectedIndex)
            {
                case 0: plotter.SetFunction(x => Math.Sin(x)); break;
                case 1: plotter.SetFunction(x => Math.Cos(x)); break;
                case 2: plotter.SetFunction(x => Math.Tan(x)); break;
                case 3: plotter.SetFunction(x => x * x); break;
                case 4: plotter.SetFunction(x => x * x * x); break;
                case 5: plotter.SetFunction(x => Math.Sqrt(x)); break;
                case 6: plotter.SetFunction(x => 1.0 / x); break;
            }
        }
}