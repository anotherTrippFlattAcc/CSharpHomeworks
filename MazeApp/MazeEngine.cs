namespace MazeApp;

public class MazeEngine
{
    public int Columns { get; private set; }
    public int Rows { get; private set; }
    
    public bool[,] Grid { get; private set; }
    
    public Point StartPoint { get; private set; }
    public Point Exit1 { get; private set; }
    public Point Exit2 { get; private set; }
    
    private Random rand = new Random();
    
    public MazeEngine(int cols, int rows)
    {
        Columns = cols % 2 == 0 ? cols + 1 : cols;
        Rows = rows % 2 == 0 ? rows + 1 : rows;
        Grid = new bool[Columns, Rows];
    }
    
    public void Generate()
    {
        for (int x = 0; x < Columns; x++)
        for (int y = 0; y < Rows; y++)
            Grid[x, y] = true;
        
        Stack<Point> stack = new Stack<Point>();
        Point start = new Point(1, 1);
        Grid[start.X, start.Y] = false;
        stack.Push(start);

        while (stack.Count > 0)
        {
            Point current = stack.Peek();
            List<Point> neighbors = GetUnvisitedNeighbors(current);

            if (neighbors.Count > 0)
            {
                Point next = neighbors[rand.Next(neighbors.Count)];
                
                Grid[(current.X + next.X) / 2, (current.Y + next.Y) / 2] = false;
                Grid[next.X, next.Y] = false;
                    
                stack.Push(next);
            }
            else
            {
                stack.Pop();
            }
        }
        
        StartPoint = new Point(1, 1);
        
        Exit1 = new Point(Columns - 1, Rows - 2);
        Grid[Exit1.X, Exit1.Y] = false;
        Grid[Exit1.X - 1, Exit1.Y] = false;
        
        Exit2 = new Point(Columns - 2, Rows - 1);
        Grid[Exit2.X, Exit2.Y] = false;
        Grid[Exit2.X, Exit2.Y - 1] = false;
    }
    
    private List<Point> GetUnvisitedNeighbors(Point p)
    {
        List<Point> list = new List<Point>();
        Point[] directions = {
            new Point(0, -2), new Point(0, 2),
            new Point(-2, 0), new Point(2, 0)
        };

        foreach (var dir in directions)
        {
            int nx = p.X + dir.X;
            int ny = p.Y + dir.Y;
            
            if (nx > 0 && nx < Columns - 1 && ny > 0 && ny < Rows - 1)
            {
                if (Grid[nx, ny])
                {
                    list.Add(new Point(nx, ny));
                }
            }
        }
        return list;
    }
    
    public bool IsValidMove(int x, int y)
    {
        if (x < 0 || x >= Columns || y < 0 || y >= Rows) return false;
        return !Grid[x, y];
    }
}