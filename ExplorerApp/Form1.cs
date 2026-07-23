using System.Diagnostics;

namespace ExplorerApp;

public partial class Form1 : Form
{
    private ToolStrip toolStrip;
    private ToolStripTextBox txtAddress;
    private TreeView treeDrives;
    private ListView lvContent;
    private MenuStrip menuStrip;
    private ContextMenuStrip contextMenu;
    private Splitter splitter;
    
    private ImageList imageList;
    private string currentPath = "";
    
    public Form1()
    {
        this.Text = "Explorer";
        this.Size = new Size(1000, 600);
        this.StartPosition = FormStartPosition.CenterScreen;
        
        InitializeIcons();
        InitializeComponents();
        LoadDrives();
    }
    
    private void InitializeIcons()
    {
        imageList = new ImageList { ImageSize = new Size(16, 16) };
        
        imageList.Images.Add(SystemIcons.WinLogo); 
        imageList.Images.Add(SystemIcons.Question); 
        imageList.Images.Add(SystemIcons.Application);
    }
    
    private void InitializeComponents()
    {
        menuStrip = new MenuStrip();
        ToolStripMenuItem menuFile = new ToolStripMenuItem("Файл");
        
        ToolStripMenuItem menuOpen = new ToolStripMenuItem("Открыть", null, (s, e) => OpenSelectedContent());
        menuOpen.ShortcutKeys = Keys.Control | Keys.O;
        
        ToolStripMenuItem menuRefresh = new ToolStripMenuItem("Обновить", null, (s, e) => RefreshContent(currentPath));
        menuRefresh.ShortcutKeys = Keys.F5;
        
        ToolStripMenuItem menuExit = new ToolStripMenuItem("Выход", null, (s, e) => this.Close());
        menuExit.ShortcutKeys = Keys.Alt | Keys.F4;
        
        menuFile.DropDownItems.AddRange(new ToolStripItem[] { menuOpen, menuRefresh, new ToolStripSeparator(), menuExit });
        menuStrip.Items.Add(menuFile);
        this.Controls.Add(menuStrip);
        this.MainMenuStrip = menuStrip;
        
        toolStrip = new ToolStrip { Dock = DockStyle.Top };
        
        ToolStripButton btnBack = new ToolStripButton("Назад", null, (s, e) => NavigateUp());
        ToolStripButton btnRefresh = new ToolStripButton("Обновить", null, (s, e) => RefreshContent(currentPath));
        
        txtAddress = new ToolStripTextBox { Width = 500, ReadOnly = true };
        
        toolStrip.Items.AddRange(new ToolStripItem[] { btnBack, btnRefresh, new ToolStripSeparator(), new ToolStripLabel("Адрес:"), txtAddress });
        this.Controls.Add(toolStrip);
        
        contextMenu = new ContextMenuStrip();
        contextMenu.Items.Add("Открыть", null, (s, e) => OpenSelectedContent());
        contextMenu.Items.Add("Свойства", null, (s, e) => ShowProperties());
        
        lvContent = new ListView
        {
            Dock = DockStyle.Fill,
            View = View.Details,
            FullRowSelect = true,
            SmallImageList = imageList,
            ContextMenuStrip = contextMenu
        };
        lvContent.Columns.Add("Имя", 250);
        lvContent.Columns.Add("Тип", 100);
        lvContent.Columns.Add("Размер", 100);
        lvContent.Columns.Add("Дата изменения", 150);
        lvContent.DoubleClick += (s, e) => OpenSelectedContent();
        
        this.Controls.Add(lvContent);
        
        splitter = new Splitter { Dock = DockStyle.Left, Width = 5 };
        this.Controls.Add(splitter);
        
        treeDrives = new TreeView
        {
            Dock = DockStyle.Left,
            Width = 250,
            ImageList = imageList
        };
        treeDrives.BeforeExpand += TreeDrives_BeforeExpand;
        treeDrives.AfterSelect += TreeDrives_AfterSelect;
        this.Controls.Add(treeDrives);
    }

    private void LoadDrives()
    {
        treeDrives.Nodes.Clear();
        foreach (DriveInfo drive in DriveInfo.GetDrives())
        {
            if (!drive.IsReady) continue;
            
            TreeNode driveNode = new TreeNode(drive.Name)
            {
                Tag = drive.RootDirectory.FullName,
                ImageIndex = 0,
                SelectedImageIndex = 0
            };
            
            driveNode.Nodes.Add(new TreeNode()); 
            treeDrives.Nodes.Add(driveNode);
        }
    }
    
    private void TreeDrives_BeforeExpand(object sender, TreeViewCancelEventArgs e)
    {
        TreeNode parentNode = e.Node;
        parentNode.Nodes.Clear();
        
        string path = (string)parentNode.Tag;
        
        try
        {
            DirectoryInfo dir = new DirectoryInfo(path);
            foreach (DirectoryInfo subDir in dir.GetDirectories())
            {
                TreeNode subNode = new TreeNode(subDir.Name)
                {
                    Tag = subDir.FullName,
                    ImageIndex = 1,
                    SelectedImageIndex = 1
                };
                subNode.Nodes.Add(new TreeNode());
                parentNode.Nodes.Add(subNode);
            }
        }
        catch { }
    }
    
    private void TreeDrives_AfterSelect(object sender, TreeViewEventArgs e)
    {
        string path = (string)e.Node.Tag;
        RefreshContent(path);
    }
    
    private void RefreshContent(string path)
    {
        if (string.IsNullOrEmpty(path) || !Directory.Exists(path)) return;
        
        currentPath = path;
        txtAddress.Text = path;
        lvContent.Items.Clear();
        
        try
        {
            DirectoryInfo dir = new DirectoryInfo(path);
            
            foreach (DirectoryInfo subDir in dir.GetDirectories())
            {
                ListViewItem item = new ListViewItem(subDir.Name, 1);
                item.SubItems.Add("Папка");
                item.SubItems.Add("");
                item.SubItems.Add(subDir.LastWriteTime.ToString());
                item.Tag = subDir.FullName;
                lvContent.Items.Add(item);
            }
            
            foreach (FileInfo file in dir.GetFiles())
            {
                ListViewItem item = new ListViewItem(file.Name, 2);
                item.SubItems.Add(file.Extension);
                item.SubItems.Add(FormatFileSize(file.Length));
                item.SubItems.Add(file.LastWriteTime.ToString());
                item.Tag = file.FullName;
                lvContent.Items.Add(item);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Нет доступа к папке: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
    
    private void OpenSelectedContent()
    {
        if (lvContent.SelectedItems.Count == 0) return;
        
        ListViewItem selectedItem = lvContent.SelectedItems[0];
        string fullPath = (string)selectedItem.Tag;

        if (Directory.Exists(fullPath))
        {
            RefreshContent(fullPath);
        }
        else if (File.Exists(fullPath))
        {
            try
            {
                Process.Start(new ProcessStartInfo(fullPath) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не удалось открыть файл: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
    
    private void NavigateUp()
    {
        if (string.IsNullOrEmpty(currentPath)) return;

        DirectoryInfo parentDir = Directory.GetParent(currentPath);
        if (parentDir != null)
        {
            RefreshContent(parentDir.FullName);
        }
        else
        {
            LoadDrives();
            lvContent.Items.Clear();
            txtAddress.Text = "";
            currentPath = "";
        }
    }
    
    private void ShowProperties()
    {
        if (lvContent.SelectedItems.Count == 0) return;
        string fullPath = (string)lvContent.SelectedItems[0].Tag;
        
        if (File.Exists(fullPath))
        {
            FileInfo fi = new FileInfo(fullPath);
            MessageBox.Show($"Путь: {fi.FullName}\nРазмер: {FormatFileSize(fi.Length)}\nСоздан: {fi.CreationTime}", "Свойства файла");
        }
        else if (Directory.Exists(fullPath))
        {
            DirectoryInfo di = new DirectoryInfo(fullPath);
            MessageBox.Show($"Путь: {di.FullName}\nСоздан: {di.CreationTime}", "Свойства папки");
        }
    }
    
    private string FormatFileSize(long bytes)
    {
        string[] suffix = { "Б", "КБ", "МБ", "ГБ" };
        int i = 0;
        double dblSByte = bytes;
        while (dblSByte >= 1024 && i < suffix.Length - 1)
        {
            i++;
            dblSByte /= 1024;
        }
        return $"{dblSByte:0.##} {suffix[i]}";
    }
}