using System.Xml.Serialization;

namespace UserForm;

public partial class Form1 : Form
{
    private TextBox txtFirstName;
    private TextBox txtLastName;
    private TextBox txtEmail;
    private TextBox txtPhone;
    
    private Button btnAdd;
    private Button btnEdit;
    private Button btnDelete;
    
    private Button btnExportTxt;
    private Button btnImportTxt;
    private Button btnExportXml;
    private Button btnImportXml;
    
    private ListBox lstUsers;
    
    public Form1()
    {
        this.Text = "UserForm";
        this.Size = new Size(720, 520);
        this.StartPosition = FormStartPosition.CenterScreen;
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        
        InitializeComponents();
    }
    
    private void InitializeComponents()
    {
        Label lblFirstName = new Label { Text = "Имя:", Location = new Point(20, 20), AutoSize = true };
        txtFirstName = new TextBox { Location = new Point(20, 40), Width = 220 };
        
        Label lblLastName = new Label { Text = "Фамилия:", Location = new Point(20, 75), AutoSize = true };
        txtLastName = new TextBox { Location = new Point(20, 95), Width = 220 };
        
        Label lblEmail = new Label { Text = "E-mail:", Location = new Point(20, 130), AutoSize = true };
        txtEmail = new TextBox { Location = new Point(20, 150), Width = 220 };
        
        Label lblPhone = new Label { Text = "Телефон:", Location = new Point(20, 185), AutoSize = true };
        txtPhone = new TextBox { Location = new Point(20, 205), Width = 220 };
        
        btnAdd = new Button { Text = "Добавить", Location = new Point(20, 250), Width = 220, Height = 32, BackColor = Color.LightGreen, FlatStyle = FlatStyle.Flat };
        btnEdit = new Button { Text = "Сохранить изменения", Location = new Point(20, 290), Width = 220, Height = 32, BackColor = Color.LightSkyBlue, FlatStyle = FlatStyle.Flat, Enabled = false };
        btnDelete = new Button { Text = "Удалить выбранного", Location = new Point(20, 330), Width = 220, Height = 32, BackColor = Color.LightCoral, FlatStyle = FlatStyle.Flat, Enabled = false };
        
        btnAdd.Click += BtnAdd_Click;
        btnEdit.Click += BtnEdit_Click;
        btnDelete.Click += BtnDelete_Click;
        
        this.Controls.AddRange(new Control[] { 
            lblFirstName, txtFirstName, lblLastName, txtLastName, 
            lblEmail, txtEmail, lblPhone, txtPhone, 
            btnAdd, btnEdit, btnDelete 
        });
        
        Label lblList = new Label { Text = "Список зарегистрированных пользователей:", Location = new Point(260, 20), AutoSize = true };
        lstUsers = new ListBox { Location = new Point(260, 40), Width = 420, Height = 322 };
        lstUsers.SelectedIndexChanged += LstUsers_SelectedIndexChanged;
        
        this.Controls.AddRange(new Control[] { lblList, lstUsers });
        
        GroupBox grpFiles = new GroupBox { Text = "Импорт / Экспорт данных", Location = new Point(20, 380), Width = 660, Height = 75 };
        
        btnExportTxt = new Button { Text = "Экспорт в TXT", Location = new Point(20, 25), Width = 135, Height = 35 };
        btnImportTxt = new Button { Text = "Импорт из TXT", Location = new Point(165, 25), Width = 135, Height = 35 };
        btnExportXml = new Button { Text = "Экспорт в XML", Location = new Point(360, 25), Width = 135, Height = 35 };
        btnImportXml = new Button { Text = "Импорт из XML", Location = new Point(505, 25), Width = 135, Height = 35 };
        
        btnExportTxt.Click += BtnExportTxt_Click;
        btnImportTxt.Click += BtnImportTxt_Click;
        btnExportXml.Click += BtnExportXml_Click;
        btnImportXml.Click += BtnImportXml_Click;
        
        grpFiles.Controls.AddRange(new Control[] { btnExportTxt, btnImportTxt, btnExportXml, btnImportXml });
        this.Controls.Add(grpFiles);
    }
    
    private void BtnAdd_Click(object sender, EventArgs e)
    {
        if (!ValidateInputs()) return;
        
        UserProfile newUser = new UserProfile(
            txtFirstName.Text.Trim(),
            txtLastName.Text.Trim(),
            txtEmail.Text.Trim(),
            txtPhone.Text.Trim()
        );
        
        lstUsers.Items.Add(newUser);
        ClearInputs();
    }
    
    private void LstUsers_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (lstUsers.SelectedItem is UserProfile selectedUser)
        {
            txtFirstName.Text = selectedUser.FirstName;
            txtLastName.Text = selectedUser.LastName;
            txtEmail.Text = selectedUser.Email;
            txtPhone.Text = selectedUser.Phone;
            
            btnEdit.Enabled = true;
            btnDelete.Enabled = true;
        }
        else
        {
            btnEdit.Enabled = false;
            btnDelete.Enabled = false;
        }
    }
    
    private void BtnEdit_Click(object sender, EventArgs e)
    {
        if (lstUsers.SelectedIndex == -1) return;
        if (!ValidateInputs()) return;
        
        int selectedIndex = lstUsers.SelectedIndex;
        UserProfile updatedUser = new UserProfile(
            txtFirstName.Text.Trim(),
            txtLastName.Text.Trim(),
            txtEmail.Text.Trim(),
            txtPhone.Text.Trim()
        );
        
        lstUsers.Items[selectedIndex] = updatedUser;
        
        ClearInputs();
        lstUsers.ClearSelected();
    }
    
    private void BtnDelete_Click(object sender, EventArgs e)
    {
        if (lstUsers.SelectedIndex != -1)
        {
            lstUsers.Items.RemoveAt(lstUsers.SelectedIndex);
            ClearInputs();
        }
    }
    
    private void BtnExportTxt_Click(object sender, EventArgs e)
    {
        if (lstUsers.Items.Count == 0)
        {
            MessageBox.Show("Список пуст. Нечего экспортировать!", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        
        using (SaveFileDialog sfd = new SaveFileDialog { Filter = "Текстовые файлы (*.txt)|*.txt", FileName = "users.txt" })
        {
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                using (StreamWriter writer = new StreamWriter(sfd.FileName))
                {
                    foreach (UserProfile user in lstUsers.Items)
                    {
                        writer.WriteLine($"{user.FirstName};{user.LastName};{user.Email};{user.Phone}");
                    }
                }
                MessageBox.Show("Данные успешно сохранены в TXT!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
    
    private void BtnImportTxt_Click(object sender, EventArgs e)
    {
        using (OpenFileDialog ofd = new OpenFileDialog { Filter = "Текстовые файлы (*.txt)|*.txt" })
        {
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    lstUsers.Items.Clear();
                    string[] lines = File.ReadAllLines(ofd.FileName);
                    
                    foreach (string line in lines)
                    {
                        string[] parts = line.Split(';');
                        if (parts.Length == 4)
                        {
                            lstUsers.Items.Add(new UserProfile(parts[0], parts[1], parts[2], parts[3]));
                        }
                    }
                    MessageBox.Show("Данные успешно импортированы из TXT!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при чтении TXT файла: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
    
    private void BtnExportXml_Click(object sender, EventArgs e)
    {
        if (lstUsers.Items.Count == 0)
        {
            MessageBox.Show("Список пуст. Нечего экспортировать!", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        
        using (SaveFileDialog sfd = new SaveFileDialog { Filter = "XML файлы (*.xml)|*.xml", FileName = "users.xml" })
        {
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                List<UserProfile> userList = new List<UserProfile>();
                foreach (UserProfile user in lstUsers.Items)
                {
                    userList.Add(user);
                }
                
                XmlSerializer serializer = new XmlSerializer(typeof(List<UserProfile>));
                using (StreamWriter writer = new StreamWriter(sfd.FileName))
                {
                    serializer.Serialize(writer, userList);
                }
                MessageBox.Show("Данные успешно сохранены в XML!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
    
    private void BtnImportXml_Click(object sender, EventArgs e)
    {
        using (OpenFileDialog ofd = new OpenFileDialog { Filter = "XML файлы (*.xml)|*.xml" })
        {
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(List<UserProfile>));
                    using (StreamReader reader = new StreamReader(ofd.FileName))
                    {
                        List<UserProfile> userList = (List<UserProfile>)serializer.Deserialize(reader);
                        
                        lstUsers.Items.Clear();
                        foreach (UserProfile user in userList)
                        {
                            lstUsers.Items.Add(user);
                        }
                    }
                    MessageBox.Show("Данные успешно импортированы из XML!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при чтении XML файла: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
    
    private bool ValidateInputs()
    {
        if (string.IsNullOrWhiteSpace(txtFirstName.Text) || 
            string.IsNullOrWhiteSpace(txtLastName.Text) ||
            string.IsNullOrWhiteSpace(txtEmail.Text) ||
            string.IsNullOrWhiteSpace(txtPhone.Text))
        {
            MessageBox.Show("Заполните все поля анкеты!", "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }
        return true;
    }
    
    private void ClearInputs()
    {
        txtFirstName.Clear();
        txtLastName.Clear();
        txtEmail.Clear();
        txtPhone.Clear();

        btnEdit.Enabled = false;
        btnDelete.Enabled = false;
    }
}