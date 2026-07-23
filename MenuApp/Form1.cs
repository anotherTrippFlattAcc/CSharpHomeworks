namespace MenuApp;

public partial class Form1 : Form
{
    private TextBox txtTopLevelMenu;
    private TextBox txtSubItem;
    private Button btnAddTopLevel;
    private Button btnAddSubItem;
    
    private MenuStrip mainMenuStrip;
    private ToolStripMenuItem selectedTopLevelMenu = null;
    
    public Form1()
    {
        this.Text = "Menu";
        this.Size = new Size(500, 350);
        this.StartPosition = FormStartPosition.CenterScreen;
        this.BackColor = Color.FromArgb(245, 246, 250);
        
        InitializeFormComponents();
    }
    
    private void InitializeFormComponents()
    {
        mainMenuStrip = new MenuStrip();
        this.MainMenuStrip = mainMenuStrip;
        this.Controls.Add(mainMenuStrip);
        
        Panel panelControls = new Panel
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(20)
        };
        this.Controls.Add(panelControls);
        
        Label lblTop = new Label { Text = "Имя пункта верхнего уровня:", Top = 30, Left = 20, Width = 200, Font = new Font("Segoe UI", 9) };
        txtTopLevelMenu = new TextBox { Name = "TopLevelMenu", Top = 55, Left = 20, Width = 220, Font = new Font("Segoe UI", 10) };
        panelControls.Controls.AddRange(new Control[] { lblTop, txtTopLevelMenu });
        
        btnAddTopLevel = new Button
        {
            Text = "Добавить пункт меню",
            Top = 53,
            Left = 260,
            Width = 180,
            Height = 30,
            Font = new Font("Segoe UI", 9, FontStyle.Bold),
            BackColor = Color.FromArgb(76, 209, 55),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat
        };
        btnAddTopLevel.FlatAppearance.BorderSize = 0;
        btnAddTopLevel.Click += BtnAddTopLevel_Click;
        panelControls.Controls.Add(btnAddTopLevel);
        
        Label lblSub = new Label { Text = "Имя подпункта меню:", Top = 120, Left = 20, Width = 200, Font = new Font("Segoe UI", 9) };
        txtSubItem = new TextBox { Name = "SubItem", Top = 145, Left = 20, Width = 220, Font = new Font("Segoe UI", 10) };
        panelControls.Controls.AddRange(new Control[] { lblSub, txtSubItem });
        
        btnAddSubItem = new Button
        {
            Text = "Добавить подменю",
            Top = 143,
            Left = 260,
            Width = 180,
            Height = 30,
            Font = new Font("Segoe UI", 9, FontStyle.Bold),
            BackColor = Color.FromArgb(9, 132, 227),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat
        };
        btnAddSubItem.FlatAppearance.BorderSize = 0;
        btnAddSubItem.Click += BtnAddSubItem_Click;
        panelControls.Controls.Add(btnAddSubItem);
        
        Label lblStatus = new Label
        {
            Text = "💡 Подсказка: Кликните по любому созданному меню сверху,\nчтобы выбрать его, а затем добавляйте туда подпункты.",
            Top = 220,
            Left = 20,
            Width = 440,
            Height = 50,
            ForeColor = Color.FromArgb(127, 143, 166),
            Font = new Font("Segoe UI", 9, FontStyle.Italic)
        };
        panelControls.Controls.Add(lblStatus);
    }
    
    private void BtnAddTopLevel_Click(object sender, EventArgs e)
    {
        string menuName = txtTopLevelMenu.Text.Trim();
        
        if (string.IsNullOrEmpty(menuName))
        {
            MessageBox.Show("Пожалуйста, введите имя пункта меню в поле TopLevelMenu!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        
        ToolStripMenuItem newMenu = new ToolStripMenuItem(menuName);
        
        newMenu.Click += (s, ev) =>
        {
            if (selectedTopLevelMenu != null)
                selectedTopLevelMenu.Font = new Font(mainMenuStrip.Font, FontStyle.Regular);
            
            selectedTopLevelMenu = (ToolStripMenuItem)s;
            selectedTopLevelMenu.Font = new Font(mainMenuStrip.Font, FontStyle.Bold);
        };
        
        mainMenuStrip.Items.Add(newMenu);
        
        if (selectedTopLevelMenu != null)
            selectedTopLevelMenu.Font = new Font(mainMenuStrip.Font, FontStyle.Regular);
        
        selectedTopLevelMenu = newMenu;
        selectedTopLevelMenu.Font = new Font(mainMenuStrip.Font, FontStyle.Bold);
        
        txtTopLevelMenu.Clear();
        txtTopLevelMenu.Focus();
    }
    
    private void BtnAddSubItem_Click(object sender, EventArgs e)
    {
        string subItemName = txtSubItem.Text.Trim();
        
        if (string.IsNullOrEmpty(subItemName))
        {
            MessageBox.Show("Пожалуйста, введите имя подпункта в поле SubItem!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        
        if (mainMenuStrip.Items.Count == 0)
        {
            MessageBox.Show("Сначала создайте хотя бы один пункт меню верхнего уровня!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }
        
        if (selectedTopLevelMenu == null)
        {
            selectedTopLevelMenu = (ToolStripMenuItem)mainMenuStrip.Items[mainMenuStrip.Items.Count - 1];
            selectedTopLevelMenu.Font = new Font(mainMenuStrip.Font, FontStyle.Bold);
        }
        
        ToolStripMenuItem newSubItem = new ToolStripMenuItem(subItemName);
        
        newSubItem.Click += (s, ev) => 
        {
            MessageBox.Show($"Вы нажали на подпункт: {((ToolStripMenuItem)s).Text}", "Событие");
        };
        
        selectedTopLevelMenu.DropDownItems.Add(newSubItem);
        
        txtSubItem.Clear();
        txtSubItem.Focus();
    }
}