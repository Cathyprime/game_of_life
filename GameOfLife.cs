using System.Text;

namespace gock;

public class Neighborhood
{
    public bool[,] neighborhood { get; }
    public int neighborsAlive { get; }
    public bool alive { get; }

    public Neighborhood(in bool[,] arr, Point center)
    {
        neighborhood = new bool[3, 3];
        neighborsAlive = 0;
        alive = arr[center.X, center.Y];

        for (int w = -1; w <= 1; ++w)
            for (int h = -1; h <= 1; ++h)
            {
                int x = center.X + w;
                int y = center.Y + h;

                if (x < 0 || y < 0 || x >= arr.GetLength(0) || y >= arr.GetLength(1))
                    neighborhood[w + 1, h + 1] = false;
                else
                {
                    var cell = arr[x, y];
                    if (cell)
                        if (!(x == center.X && y == center.Y))
                            neighborsAlive += 1;
                    neighborhood[w + 1, h + 1] = cell;
                }
            }
    }

    public override string ToString()
    {
        var sb = new StringBuilder();
        for (int i = 0; i < 3; ++i)
        {
            for (int j = 0; j < 3; ++j)
                sb.Append(neighborhood[i, j] + " ");
            sb.AppendLine();
        }
        return sb.ToString();
    }

    public static implicit operator bool(Neighborhood n)
    {
        return n.alive;
    }
}

public class GOL
{
    public int width { get; }
    public int height { get; }
    public bool[,] area { get; set; }
    public Predicate<Neighborhood> rule { get; set; }

    public GOL()
    {
        width = GameSettings.GAME_WIDTH;
        height = GameSettings.GAME_HEIGHT;
        area = new bool[width, height];
        rule = GetDefaultRule();

        var rng = new Random();
        for (int i = 0; i < width; ++i)
            for (int j = 0; j < height; ++j)
                area[i, j] = rng.Next(0, 100) < 25;
    }

    public GOL(int width, int height)
    {
        this.height = height;
        this.width = width;
        area = new bool[width, height];
        rule = GetDefaultRule();
    }

    public Predicate<Neighborhood> GetDefaultRule()
    {
        return (Neighborhood n) =>
        {
            if (n.alive)
                return (n.neighborsAlive == 2 || n.neighborsAlive == 3);
            else
                return (n.neighborsAlive == 3);
        };
    }

    public void Draw(ScreenSurface surface, ColoredGlyph glyph)
    {
        for (int i = 0; i < width; ++i)
            for (int ii = 0; ii < height; ++ii)
                if (area[i, ii])
                    surface.SetCellAppearance(i, ii, glyph);
    }

    public void Update()
    {
        var buffer = new bool[width, height];

        for (int i = 0; i < width; ++i)
            for (int j = 0; j < height; ++j)
                buffer[i, j] = rule(new Neighborhood(area, new(i, j)));

        area = buffer;
    }
}
