namespace ChessApp;

public partial class Form1 : Form
{
    private const int CellSize = 60;
    private const int Offset = 20;
    
    private string[,] _board = new string[8, 8];
    
    private ContextMenuStrip _figureMenu;
    
    private int _selectedRow = -1;
    private int _selectedCol = -1;
    
    public Form1()
    {
        this.Text = "Шахматная доска";
        this.ClientSize = new Size(CellSize * 8 + Offset * 2, CellSize * 8 + Offset * 2);
        this.DoubleBuffered = true;

        InitializeBoard();

        InitializeContextMenu();

        this.Paint += Form1_Paint;
        this.MouseDown += Form1_MouseDown;
    }
    
    private void InitializeBoard()
        {
            _board[0, 0] = "♜"; _board[0, 1] = "♞"; _board[0, 2] = "♝"; _board[0, 3] = "♛";
            _board[0, 4] = "♚"; _board[0, 5] = "♝"; _board[0, 6] = "♞"; _board[0, 7] = "♜";
            for (int i = 0; i < 8; i++) _board[1, i] = "♟";

            for (int i = 0; i < 8; i++) _board[6, i] = "♙";
            _board[7, 0] = "♖"; _board[7, 1] = "♘"; _board[7, 2] = "♗"; _board[7, 3] = "♕";
            _board[7, 4] = "♔"; _board[7, 5] = "♗"; _board[7, 6] = "♘"; _board[7, 7] = "♖";
        }

        private void InitializeContextMenu()
        {
            _figureMenu = new ContextMenuStrip();
            
            ToolStripMenuItem infoItem = new ToolStripMenuItem("Информация о фигуре");
            infoItem.Click += InfoItem_Click;
            
            ToolStripMenuItem removeItem = new ToolStripMenuItem("Удалить фигуру");
            removeItem.Click += RemoveItem_Click;

            _figureMenu.Items.Add(infoItem);
            _figureMenu.Items.Add(removeItem);
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            using (Font font = new Font("Segoe UI Symbol", 32, FontStyle.Regular))
            using (StringFormat sf = new StringFormat())
            {
                sf.Alignment = StringAlignment.Center;
                sf.LineAlignment = StringAlignment.Center;

                for (int row = 0; row < 8; row++)
                {
                    for (int col = 0; col < 8; col++)
                    {
                        Brush cellBrush = (row + col) % 2 == 0 ? Brushes.NavajoWhite : Brushes.SaddleBrown;
                        
                        int x = Offset + col * CellSize;
                        int y = Offset + row * CellSize;
                        Rectangle rect = new Rectangle(x, y, CellSize, CellSize);

                        g.FillRectangle(cellBrush, rect);

                        if (!string.IsNullOrEmpty(_board[row, col]))
                        {
                            Brush textBrush = (_board[row, col][0] >= '♀' && _board[row, col][0] <= '♇') ? Brushes.White : Brushes.Black;
                            
                            g.DrawString(_board[row, col], font, Brushes.Black, rect, sf);
                        }
                    }
                }
            }
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                int col = (e.X - Offset) / CellSize;
                int row = (e.Y - Offset) / CellSize;

                if (row >= 0 && row < 8 && col >= 0 && col < 8)
                {
                    if (!string.IsNullOrEmpty(_board[row, col]))
                    {
                        _selectedRow = row;
                        _selectedCol = col;
                        
                        _figureMenu.Show(this, e.Location);
                    }
                }
            }
        }

        private void InfoItem_Click(object sender, EventArgs e)
        {
            if (_selectedRow != -1 && _selectedCol != -1)
            {
                string figure = _board[_selectedRow, _selectedCol];
                MessageBox.Show($"Выбрана фигура: {figure} на позиции {GetChessCoordinates(_selectedRow, _selectedCol)}", 
                                "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void RemoveItem_Click(object sender, EventArgs e)
        {
            if (_selectedRow != -1 && _selectedCol != -1)
            {
                _board[_selectedRow, _selectedCol] = null;
                this.Invalidate();
            }
        }
        
        private string GetChessCoordinates(int row, int col)
        {
            char letter = (char)('A' + col);
            int number = 8 - row;
            return $"{letter}{number}";
        }
}