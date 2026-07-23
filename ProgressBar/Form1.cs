using System.Text;

namespace ProgressBar;

public partial class Form1 : Form
{
    private Button btnSelectFile;
    private System.Windows.Forms.ProgressBar progressBar;
    private Label lblStatus;
    private TextBox txtContent;
    
    public Form1()
    {
        this.Text = "ProgressBar";
        this.Size = new Size(600, 450);
        this.StartPosition = FormStartPosition.CenterScreen;
        
        InitializeFormComponents();
    }
    
    private void InitializeFormComponents()
    {
        btnSelectFile = new Button
        {
            Text = "Выбрать и прочитать файл",
            Top = 15,
            Left = 15,
            Width = 200,
            Height = 30,
            Font = new Font("Segoe UI", 9, FontStyle.Bold),
            BackColor = Color.FromArgb(9, 132, 227),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat
        };
        btnSelectFile.FlatAppearance.BorderSize = 0;
        btnSelectFile.Click += BtnSelectFile_Click;
        this.Controls.Add(btnSelectFile);
        
        progressBar = new System.Windows.Forms.ProgressBar
        {
            Top = 55,
            Left = 15,
            Width = 555,
            Height = 25,
            Style = ProgressBarStyle.Continuous
        };
        this.Controls.Add(progressBar);
        
        lblStatus = new Label
        {
            Text = "Файл не выбран",
            Top = 90,
            Left = 15,
            Width = 555,
            Height = 20,
            Font = new Font("Segoe UI", 9, FontStyle.Italic),
            ForeColor = Color.FromArgb(99, 110, 114)
        };
        this.Controls.Add(lblStatus);
        
        txtContent = new TextBox
        {
            Top = 120,
            Left = 15,
            Width = 555,
            Height = 270,
            Multiline = true,
            ScrollBars = ScrollBars.Vertical,
            ReadOnly = true,
            Font = new Font("Consolas", 10),
            BackColor = Color.White
        };
        this.Controls.Add(txtContent);
    }
    
    private async void BtnSelectFile_Click(object sender, EventArgs e)
    {
        using (OpenFileDialog ofd = new OpenFileDialog())
        {
            ofd.Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*";
            
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                btnSelectFile.Enabled = false;
                txtContent.Clear();
                progressBar.Value = 0;
                
                var progress = new Progress<int>(percent =>
                {
                    progressBar.Value = percent;
                    lblStatus.Text = $"Прочитано: {percent}%";
                });
                
                try
                {
                    string content = await ReadFileWithProgressAsync(ofd.FileName, progress);
                    txtContent.Text = content;
                    lblStatus.Text = "Чтение файла завершено успешно!";
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при чтении файла: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    lblStatus.Text = "Ошибка чтения файла.";
                }
                finally
                {
                    btnSelectFile.Enabled = true;
                }
            }
        }
    }
    
    private Task<string> ReadFileWithProgressAsync(string filePath, IProgress<int> progress)
    {
        return Task.Run(() =>
        {
            FileInfo fileInfo = new FileInfo(filePath);
            long totalBytes = fileInfo.Length;
            
            if (totalBytes == 0)
            {
                progress?.Report(100);
                return string.Empty;
            }
            
            StringBuilder sb = new StringBuilder();
            byte[] buffer = new byte[4096];
            long totalBytesRead = 0;

            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                int bytesRead;
                while ((bytesRead = fs.Read(buffer, 0, buffer.Length)) > 0)
                {
                    sb.Append(Encoding.UTF8.GetString(buffer, 0, bytesRead));
                        
                    totalBytesRead += bytesRead;
                    
                    int currentPercent = (int)((double)totalBytesRead / totalBytes * 100);
                    
                    progress?.Report(currentPercent);
                    
                    System.Threading.Thread.Sleep(5);
                }
            }
            
            return sb.ToString();
        });
    }
}