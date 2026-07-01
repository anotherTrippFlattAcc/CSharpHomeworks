namespace PaintApp;

public partial class SettingsForm : Form
{
    public Color SelectedColor { get; private set; }
    public float SelectedWidth { get; private set; }

    private Button btnColor;
    private NumericUpDown numWidth;
    private Button btnOK;

    public SettingsForm(Color currentColor, float currentWidth)
    {
        this.Text = "Настройки примитива";
        this.Size = new Size(250, 180);
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;

        SelectedColor = currentColor;
        SelectedWidth = currentWidth;

        Label lblColor = new Label { Text = "Цвет:", Location = new Point(20, 20), Size = new Size(50, 20) };
        btnColor = new Button { Location = new Point(80, 15), Size = new Size(120, 25), BackColor = currentColor };
        btnColor.Click += (s, e) => {
            using (ColorDialog cd = new ColorDialog())
            {
                if (cd.ShowDialog() == DialogResult.OK)
                {
                    SelectedColor = cd.Color;
                    btnColor.BackColor = cd.Color;
                }
            }
        };

        Label lblWidth = new Label { Text = "Толщина:", Location = new Point(20, 55), Size = new Size(60, 20) };
        numWidth = new NumericUpDown { Location = new Point(80, 55), Size = new Size(120, 20), Minimum = 1, Maximum = 20, Value = (decimal)currentWidth };

        btnOK = new Button { Text = "Применить", Location = new Point(80, 100), Size = new Size(120, 30), DialogResult = DialogResult.OK };
        btnOK.Click += (s, e) => { SelectedWidth = (float)numWidth.Value; };

        this.Controls.AddRange(new Control[] { lblColor, btnColor, lblWidth, numWidth, btnOK });
    }
}