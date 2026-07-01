namespace Clock;

public partial class Form1 : Form
{
    private Clock clock;
    
    public Form1()
    {
        this.Text = "Часы";
        this.Size = new Size(500, 530);
        this.MinimumSize = new Size(300, 330);
        this.StartPosition = FormStartPosition.CenterScreen;
        this.BackColor = Color.FromArgb(220, 221, 225);

        InitializeClock();
    }
    
    private void InitializeClock()
    {
        clock = new Clock
        {
            Dock = DockStyle.Fill,
            BackColor = Color.Transparent
        };
        this.Controls.Add(clock);
    }
}