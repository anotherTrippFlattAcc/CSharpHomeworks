using System.Drawing.Imaging;

namespace PaintApp;

public partial class Form1 : Form
{
    private List<Shape> _shapes = new List<Shape>();
    private Shape _currentShape = null;
    private bool _isDrawing = false;

    private ShapeType _currentType = ShapeType.Line;
    private Color _currentShapeColor = Color.Black;
    private float _currentShapeWidth = 2f;

    private ToolStrip _toolStrip;
    
    public Form1()
    {
        this.Text = "Приложение для рисования";
        this.Size = new Size(800, 600);
        this.DoubleBuffered = true;

        InitializeToolStrip();

        this.MouseDown += Form1_MouseDown;
        this.MouseMove += Form1_MouseMove;
        this.MouseUp += Form1_MouseUp;
        this.Paint += Form1_Paint;
    }
    
    private void InitializeToolStrip()
    {
        _toolStrip = new ToolStrip { Dock = DockStyle.Top };

        ToolStripButton btnLine = new ToolStripButton("Линия") { CheckOnClick = true, Checked = true };
        ToolStripButton btnRect = new ToolStripButton("Прямоугольник") { CheckOnClick = true };
        ToolStripButton btnEllipse = new ToolStripButton("Эллипс") { CheckOnClick = true };

        Action<ToolStripButton> uncheckOthers = (activeBtn) => {
            foreach (ToolStripItem item in _toolStrip.Items)
                if (item is ToolStripButton btn && btn != activeBtn && (btn.Text == "Линия" || btn.Text == "Прямоугольник" || btn.Text == "Эллипс"))
                    btn.Checked = false;
        };

        btnLine.Click += (s, e) => { _currentType = ShapeType.Line; uncheckOthers(btnLine); btnLine.Checked = true; };
        btnRect.Click += (s, e) => { _currentType = ShapeType.Rectangle; uncheckOthers(btnRect); btnRect.Checked = true; };
        btnEllipse.Click += (s, e) => { _currentType = ShapeType.Ellipse; uncheckOthers(btnEllipse); btnEllipse.Checked = true; };

        ToolStripButton btnSettings = new ToolStripButton("⚙ Настройки");
        btnSettings.Click += (s, e) => {
            using (SettingsForm sf = new SettingsForm(_currentShapeColor, _currentShapeWidth))
            {
                if (sf.ShowDialog(this) == DialogResult.OK)
                {
                    _currentShapeColor = sf.SelectedColor;
                    _currentShapeWidth = sf.SelectedWidth;
                }
            }
        };

        ToolStripButton btnSave = new ToolStripButton("💾 Сохранить");
        btnSave.Click += BtnSave_Click;

        _toolStrip.Items.AddRange(new ToolStripItem[] { btnLine, btnRect, btnEllipse, new ToolStripSeparator(), btnSettings, btnSave });
        this.Controls.Add(_toolStrip);
    }
    
    private void Form1_MouseDown(object sender, MouseEventArgs e)
    {
        if (e.Button != MouseButtons.Left) return;

        _isDrawing = true;

        switch (_currentType)
        {
            case ShapeType.Line: _currentShape = new LineShape(); break;
            case ShapeType.Rectangle: _currentShape = new RectangleShape(); break;
            case ShapeType.Ellipse: _currentShape = new EllipseShape(); break;
        }

        if (_currentShape != null)
        {
            _currentShape.StartPoint = e.Location;
            _currentShape.EndPoint = e.Location;
            _currentShape.Color = _currentShapeColor;
            _currentShape.PenWidth = _currentShapeWidth;
        }
    }
    
    private void Form1_MouseMove(object sender, MouseEventArgs e)
    {
        if (!_isDrawing || _currentShape == null) return;

        _currentShape.EndPoint = e.Location;
        this.Invalidate();
    }
    
    private void Form1_MouseUp(object sender, MouseEventArgs e)
    {
        if (!_isDrawing) return;

        _isDrawing = false;
        if (_currentShape != null)
        {
            _shapes.Add(_currentShape);
            _currentShape = null;
        }
        this.Invalidate();
    }
    
    private void Form1_Paint(object sender, PaintEventArgs e)
    {
        e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

        foreach (var shape in _shapes)
        {
            shape.Draw(e.Graphics);
        }

        if (_isDrawing && _currentShape != null)
        {
            _currentShape.Draw(e.Graphics);
        }
    }
    
    private void BtnSave_Click(object sender, EventArgs e)
    {
        using (SaveFileDialog sfd = new SaveFileDialog())
        {
            sfd.Title = "Сохранить композицию";
            sfd.Filter = "PNG Image|*.png|JPEG Image|*.jpg|Bitmap Image|*.bmp";
            sfd.DefaultExt = "png";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                // Создаем растровое изображение размером с клиентскую область (исключая ToolStrip)
                int width = this.ClientSize.Width;
                int height = this.ClientSize.Height;

                using (Bitmap bmp = new Bitmap(width, height))
                {
                    using (Graphics g = Graphics.FromImage(bmp))
                    {
                        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                        g.Clear(Color.White); // Фон по умолчанию

                        // Отрисовываем все фигуры на холст изображения
                        foreach (var shape in _shapes)
                        {
                            shape.Draw(g);
                        }
                    }

                    // Определяем формат по расширению
                    ImageFormat format = ImageFormat.Png;
                    switch (System.IO.Path.GetExtension(sfd.FileName).ToLower())
                    {
                        case ".jpg":
                        case ".jpeg": format = ImageFormat.Jpeg; break;
                        case ".bmp":  format = ImageFormat.Bmp; break;
                    }

                    bmp.Save(sfd.FileName, format);
                    MessageBox.Show("Композиция успешно сохранена!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
    }
}