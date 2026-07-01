namespace GDILogo;

public partial class Form1 : Form
{
    private PictureBox pbLogoHolder;
    
    public Form1()
    {
        this.Text = "Динамическая загрузка логотипа";
        this.Size = new Size(650, 350);
        this.StartPosition = FormStartPosition.CenterScreen;
        this.BackColor = Color.FromArgb(34, 40, 49);

        InitializeComponents();
        LoadDynamicLogo();
    }
    
    private void InitializeComponents()
    {
        pbLogoHolder = new PictureBox
        {
            Location = new Point(50, 40),
            Size = new Size(530, 150),
            SizeMode = PictureBoxSizeMode.Zoom,
            BackColor = Color.Transparent
        };

        this.Controls.AddRange(new Control[] { pbLogoHolder });
    }
    
    private void LoadDynamicLogo()
    {
        if (pbLogoHolder.Image != null)
        {
            pbLogoHolder.Image.Dispose();
        }
        pbLogoHolder.Image = LogoGenerator.CreateCompanyLogo(pbLogoHolder.Width, pbLogoHolder.Height);
    }
}