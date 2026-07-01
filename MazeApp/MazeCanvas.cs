namespace MazeApp;

public class MazeCanvas: Panel
{
    private MazeEngine maze;
    private Point playerPos;
    
    public Point PlayerPosition => playerPos;
    
    public MazeCanvas()
    {
        this.DoubleBuffered = true;
        this.BackColor = Color.FromArgb(45, 52, 54);
        this.Resize += (s, e) => this.Invalidate();
    }
    
    public void SetupMaze(MazeEngine engine)
    {
        this.maze = engine;
        this.playerPos = engine.StartPoint;
        this.Invalidate();
    }
    
    public void MovePlayer(int dx, int dy)
    {
        if (maze == null) return;

        int newX = playerPos.X + dx;
        int newY = playerPos.Y + dy;

        if (maze.IsValidMove(newX, newY))
        {
            playerPos = new Point(newX, newY);
            this.Invalidate();
        }
    }
    
    protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (maze == null) return;

            Graphics g = e.Graphics;
            
            int cellSize = Math.Min(this.Width / maze.Columns, this.Height / maze.Rows);
            if (cellSize <= 0) cellSize = 1;
            
            int offsetX = (this.Width - (maze.Columns * cellSize)) / 2;
            int offsetY = (this.Height - (maze.Rows * cellSize)) / 2;

            using (Brush wallBrush = new SolidBrush(Color.FromArgb(30, 39, 46)))       // Стены (Тёмные)
            using (Brush pathBrush = new SolidBrush(Color.FromArgb(245, 246, 250)))    // Коридоры (Белые)
            using (Brush exitBrush = new SolidBrush(Color.FromArgb(46, 204, 113)))     // Выходы (Зеленые)
            using (Brush playerBrush = new SolidBrush(Color.FromArgb(235, 94, 40)))    // Игрок (Оранжевый)
            {
                for (int x = 0; x < maze.Columns; x++)
                {
                    for (int y = 0; y < maze.Rows; y++)
                    {
                        Rectangle rect = new Rectangle(offsetX + x * cellSize, offsetY + y * cellSize, cellSize, cellSize);
                        
                        if (maze.Grid[x, y])
                            g.FillRectangle(wallBrush, rect);
                        else
                            g.FillRectangle(pathBrush, rect);
                    }
                }
                
                Rectangle exRect1 = new Rectangle(offsetX + maze.Exit1.X * cellSize, offsetY + maze.Exit1.Y * cellSize, cellSize, cellSize);
                Rectangle exRect2 = new Rectangle(offsetX + maze.Exit2.X * cellSize, offsetY + maze.Exit2.Y * cellSize, cellSize, cellSize);
                g.FillRectangle(exitBrush, exRect1);
                g.FillRectangle(exitBrush, exRect2);
                
                Rectangle pRect = new Rectangle(offsetX + playerPos.X * cellSize + 2, offsetY + playerPos.Y * cellSize + 2, cellSize - 4, cellSize - 4);
                g.FillEllipse(playerBrush, pRect);
            }
        }
}