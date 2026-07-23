namespace TextEditor;

public partial class Form1 : Form
{
    private RichTextBox rtbEditor;
    private MenuStrip menuStrip;
    private ToolStrip toolStrip;
    private ContextMenuStrip contextMenu;
    
    private string currentFilePath = null;
    private readonly string appName = "TextEditor";
    
    public Form1()
    {
        this.Text = $"Новый документ — {appName}";
        this.Size = new Size(800, 600);
        this.StartPosition = FormStartPosition.CenterScreen;
        
        InitializeEditorComponents();
    }
    
    private void InitializeEditorComponents()
        {
            rtbEditor = new RichTextBox
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 11, FontStyle.Regular),
                AcceptsTab = true
            };
            this.Controls.Add(rtbEditor);
            
            ColorDialog colorDialog = new ColorDialog();
            FontDialog fontDialog = new FontDialog();
            OpenFileDialog openFileDialog = new OpenFileDialog { Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*" };
            SaveFileDialog saveFileDialog = new SaveFileDialog { Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*" };
            
            contextMenu = new ContextMenuStrip();
            contextMenu.Items.Add("Копировать", null, (s, e) => rtbEditor.Copy());
            contextMenu.Items.Add("Вырезать", null, (s, e) => rtbEditor.Cut());
            contextMenu.Items.Add("Вставить", null, (s, e) => rtbEditor.Paste());
            contextMenu.Items.Add("-"); // Разделитель
            contextMenu.Items.Add("Отменить", null, (s, e) => { if (rtbEditor.CanUndo) rtbEditor.Undo(); });
            rtbEditor.ContextMenuStrip = contextMenu;
            
            Action NewDocument = () =>
            {
                rtbEditor.Clear();
                currentFilePath = null;
                this.Text = $"Новый документ — {appName}";
            };
            
            Action OpenFile = () =>
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        rtbEditor.Text = File.ReadAllText(openFileDialog.FileName);
                        currentFilePath = openFileDialog.FileName;
                        this.Text = $"{currentFilePath} — {appName}";
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при открытии файла: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            };

            Action<bool> SaveFile = (bool saveAs) =>
            {
                if (string.IsNullOrEmpty(currentFilePath) || saveAs)
                {
                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        currentFilePath = saveFileDialog.FileName;
                    }
                    else
                    {
                        return;
                    }
                }

                try
                {
                    File.WriteAllText(currentFilePath, rtbEditor.Text);
                    this.Text = $"{currentFilePath} — {appName}";
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при сохранении файла: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            menuStrip = new MenuStrip();
            
            ToolStripMenuItem fileMenu = new ToolStripMenuItem("Файл");
            fileMenu.DropDownItems.Add("Новый документ", null, (s, e) => NewDocument());
            fileMenu.DropDownItems.Add("Открыть", null, (s, e) => OpenFile());
            fileMenu.DropDownItems.Add("Сохранить", null, (s, e) => SaveFile(false));
            fileMenu.DropDownItems.Add("Сохранить как...", null, (s, e) => SaveFile(true));
            menuStrip.Items.Add(fileMenu);
            
            ToolStripMenuItem editMenu = new ToolStripMenuItem("Правка");
            editMenu.DropDownItems.Add("Копировать", null, (s, e) => rtbEditor.Copy());
            editMenu.DropDownItems.Add("Вырезать", null, (s, e) => rtbEditor.Cut());
            editMenu.DropDownItems.Add("Вставить", null, (s, e) => rtbEditor.Paste());
            editMenu.DropDownItems.Add("Выделить все", null, (s, e) => rtbEditor.SelectAll());
            editMenu.DropDownItems.Add("-");
            editMenu.DropDownItems.Add("Отменить", null, (s, e) => { if (rtbEditor.CanUndo) rtbEditor.Undo(); });
            menuStrip.Items.Add(editMenu);
            
            ToolStripMenuItem formatMenu = new ToolStripMenuItem("Формат");
            formatMenu.DropDownItems.Add("Цвет шрифта", null, (s, e) =>
            {
                if (colorDialog.ShowDialog() == DialogResult.OK)
                    rtbEditor.SelectionColor = colorDialog.Color;
            });
            formatMenu.DropDownItems.Add("Цвет фона редактора", null, (s, e) =>
            {
                if (colorDialog.ShowDialog() == DialogResult.OK)
                    rtbEditor.BackColor = colorDialog.Color;
            });
            formatMenu.DropDownItems.Add("Шрифт", null, (s, e) =>
            {
                if (fontDialog.ShowDialog() == DialogResult.OK)
                    rtbEditor.SelectionFont = fontDialog.Font;
            });
            menuStrip.Items.Add(formatMenu);

            this.MainMenuStrip = menuStrip;
            this.Controls.Add(menuStrip);
            
            toolStrip = new ToolStrip();
            
            toolStrip.Items.Add(new ToolStripButton("Новый", null, (s, e) => NewDocument()));
            toolStrip.Items.Add(new ToolStripButton("Открыть", null, (s, e) => OpenFile()));
            toolStrip.Items.Add(new ToolStripButton("Сохранить", null, (s, e) => SaveFile(false)));
            toolStrip.Items.Add(new ToolStripSeparator());
            
            toolStrip.Items.Add(new ToolStripButton("Копировать", null, (s, e) => rtbEditor.Copy()));
            toolStrip.Items.Add(new ToolStripButton("Вырезать", null, (s, e) => rtbEditor.Cut()));
            toolStrip.Items.Add(new ToolStripButton("Вставить", null, (s, e) => rtbEditor.Paste()));
            toolStrip.Items.Add(new ToolStripButton("Отменить", null, (s, e) => { if (rtbEditor.CanUndo) rtbEditor.Undo(); }));
            toolStrip.Items.Add(new ToolStripSeparator());
            
            ToolStripDropDownButton btnSettings = new ToolStripDropDownButton("Настройки редактора");
            btnSettings.DropDownItems.Add("Цвет шрифта", null, (s, e) => { if (colorDialog.ShowDialog() == DialogResult.OK) rtbEditor.SelectionColor = colorDialog.Color; });
            btnSettings.DropDownItems.Add("Цвет фона", null, (s, e) => { if (colorDialog.ShowDialog() == DialogResult.OK) rtbEditor.BackColor = colorDialog.Color; });
            btnSettings.DropDownItems.Add("Шрифт", null, (s, e) => { if (fontDialog.ShowDialog() == DialogResult.OK) rtbEditor.SelectionFont = fontDialog.Font; });
            
            toolStrip.Items.Add(btnSettings);

            this.Controls.Add(toolStrip);
        }
}