public class Character : Tile
{
    public bool IsActive = true;
    public int DestinationX;
    public int DestinationY;

    private string icon;
    private Tile start;
    private Tile finish;
    private Map map;
    private List<Tile> path;
    private List<Tuple<int, int>> availableTilesList;

    public Character(Map map, int X, int Y, int destinationX, int destinationY, string icon)
    {
        this.map = map;
        this.X = X;
        this.Y = Y;
        this.DestinationX = destinationX;
        this.DestinationY = destinationY;
        this.icon = icon;

        start = map.PlaceTile(X, Y, icon);
        finish = map.PlaceTile(destinationX, destinationY, "B");
        this.SetDistance(destinationX, destinationY);
        this.path = CalculatePath();
    }

    private void UpdatePath()
    {
        start.X = X;
        start.Y = Y;
        // Console.WriteLine($"Calculating new path with start coordinates x: {start.X} y:{start.Y}");
        path = CalculatePath();

        // Remove first is neccessary because if the first elemement is the current position of the character and it makes it move to the current place indefinetly
        path.RemoveAt(0);
        // WritePath();
        // map.PrintPath(CalculatePath());
        if (path.Count() == 0)
        {
            Console.WriteLine($"Character {icon} became inactive");
            IsActive = false;
        }
    }

    private List<Tile> CalculatePath()
    {
        try
        {
            Astar astar = new Astar(map);
            List<Tile> path = astar.CalculatePath(start, finish);
            path.Reverse();
            return path;
        }
        catch (NoPathFoundException)
        {
            return this.path;
        }
    }

    private void Move(int moveToX, int moveToY)
    {
        // delete itself from the old position on the map
        if (map.map[Y][X] == Char.Parse(icon))
        {
            map.PlaceOnMap(" ", X, Y);
        }

        // update position
        this.X = moveToX;
        this.Y = moveToY;
        // redraw character
        map.PlaceOnMap(icon, X, Y);
        // map.PrintMap();
        // WritePath();
        // Console.WriteLine($"Character {icon}, moved to X {X},Y {Y}");
    }

    public void MakeStep()
    {
        // Two IsActive checks are necessary because first updates IsActive state and if they were in the same if statement character would react at being Inactive too late
        // and would attempt to move crashing the game. First IsActive is to improve performance by removing unnecessary calculations
        if (IsActive)
        {
            // Before is active to delete tiles at an old place
            EraseArea();
            UpdatePath();
        }

        if (IsActive)
        {
            Move(path[0].X, path[0].Y);
            // After moving draw radius on new place
            DrawArea();
        }
    }

    private void DrawArea()
    {
        FindTilesAround();
        //Draw * around if tiles 
        foreach (Tuple<int, int> tuple in availableTilesList)
        {
            map.PlaceOnMap("*", tuple.Item1, tuple.Item2);
        }
    }

    private void EraseArea()
    {
        FindTilesAround();
        //Remove * around if they exist
        foreach (Tuple<int, int> tuple in availableTilesList)
        {
            map.PlaceOnMap(" ", tuple.Item1, tuple.Item2);
        }
    }

    private void FindTilesAround()
    {
        List<Tuple<int, int>> coordinatesList = CreateCoordinatesList();
        availableTilesList = new List<Tuple<int, int>>();
        //check if ' '


        foreach (Tuple<int, int> point in coordinatesList)
        {
            if (map.TileAvailable(point.Item1, point.Item2))
            {
                availableTilesList.Add(new Tuple<int, int>(point.Item1, point.Item2));
            }
        }
    }

    private List<Tuple<int, int>> CreateCoordinatesList()
    {
        List<Tuple<int, int>> coordinatesList = new List<Tuple<int, int>>();
        coordinatesList.Add(new Tuple<int, int>(X, Y - 1));
        coordinatesList.Add(new Tuple<int, int>(X, Y + 1));
        coordinatesList.Add(new Tuple<int, int>(X - 1, Y));
        coordinatesList.Add(new Tuple<int, int>(X + 1, Y));
        coordinatesList.Add(new Tuple<int, int>(X - 1, Y - 1));
        coordinatesList.Add(new Tuple<int, int>(X + 1, Y + 1));
        coordinatesList.Add(new Tuple<int, int>(X - 1, Y + 1));
        coordinatesList.Add(new Tuple<int, int>(X + 1, Y - 1));

        return coordinatesList;
    }

    private void WritePath()
    {
        foreach (Tile tile in path)
        {
            Console.WriteLine($"X: {tile.X} Y:{tile.Y}");
        }
    }
}