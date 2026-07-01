namespace MazeApp;

public partial class Form1 : Form
{
    private MazeEngine maze;
    private MazeCanvas canvas;
    
    private const int MAZE_COLUMNS = 25;
    private const int MAZE_ROWS = 25;
    
    public Form1()
    {
        this.Text = "Лабиринт";
        this.Size = new Size(650, 700);
        this.StartPosition = FormStartPosition.CenterScreen;
        this.KeyPreview = true;

        InitializeGame();
        StartNewGame();
    }
    
    private void InitializeGame()
    {
        Label lblInfo = new Label
        {
            Text = "Управление: Стрелочки на клавиатуре. Зеленые клетки — выходы.",
            Dock = DockStyle.Top,
            Height = 30,
            TextAlign = ContentAlignment.MiddleCenter,
            Font = new Font("Segoe UI", 10, FontStyle.Regular),
            BackColor = Color.FromArgb(210, 218, 226)
        };
        this.Controls.Add(lblInfo);
        
        Button btnRestart = new Button
        {
            Text = "Сгенерировать новый лабиринт",
            Dock = DockStyle.Bottom,
            Height = 40,
            Font = new Font("Segoe UI", 10, FontStyle.Bold),
            BackColor = Color.FromArgb(9, 132, 227),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat
        };
        btnRestart.FlatAppearance.BorderSize = 0;
        btnRestart.Click += (s, e) => StartNewGame();
        this.Controls.Add(btnRestart);
        
        canvas = new MazeCanvas { Dock = DockStyle.Fill };
        this.Controls.Add(canvas);
    }
    
    private void StartNewGame()
    {
        maze = new MazeEngine(MAZE_COLUMNS, MAZE_ROWS);
        maze.Generate();
        canvas.SetupMaze(maze);
        this.Focus();
    }
    
    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
        if (keyData == Keys.Left || keyData == Keys.Right || keyData == Keys.Up || keyData == Keys.Down)
        {
            int dx = 0;
            int dy = 0;

            switch (keyData)
            {
                case Keys.Left:  dx = -1; break;
                case Keys.Right: dx = 1;  break;
                case Keys.Up:    dy = -1; break;
                case Keys.Down:  dy = 1;  break;
            }
            
            canvas.MovePlayer(dx, dy);
            
            if (canvas.PlayerPosition == maze.Exit1 || canvas.PlayerPosition == maze.Exit2)
            {
                MessageBox.Show("Поздравляем! Вы успешно нашли один из выходов и покинули лабиринт!", 
                    "Победа!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                StartNewGame();
            }
            
            return true; 
        }
        
        return base.ProcessCmdKey(ref msg, keyData);
    }
}