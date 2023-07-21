public class Character : Tile
{
    Map map;
    public int destinationX;
    public int destinationY;
    string characterIcon;
    Tile start;
    Tile finish;
    public bool IsActive = true;
    public List<Tile> path;

    public Character(Map map, int X, int Y, int destinationX, int destinationY, string characterIcon)
    {
        this.map = map;
        this.X = X;
        this.Y = Y;
        this.destinationX = destinationX;
        this.destinationY = destinationY;
        this.characterIcon = characterIcon;
        Spawn();
    }
    public void Spawn()
    {
        start = map.PlaceTile(X, Y, characterIcon);
        finish = map.PlaceTile(destinationX, destinationY, "B");
        this.SetDistance(destinationX, destinationY);
        this.path = CalculatePath();
    }
    public void UpdatePath()
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
            Console.WriteLine("Character became inactive");
            IsActive = false;
        }


    }
    public List<Tile> CalculatePath()
    {
        Astar astar = new Astar(map);
        List<Tile> path = astar.CalculatePath(start, finish);
        path.Reverse();
        return path;
    }
    public void Move(int moveToX, int moveToY)
    {
        // delete from the old position on the map
        map.PlaceOnMap(" ", X, Y);
        // update position
        this.X = moveToX;
        this.Y = moveToY;
        // redraw character
        map.PlaceOnMap(characterIcon, X, Y);
        // map.PrintMap();
        // WritePath();
    }
    public void MakeStep()
    {
        
        if (IsActive)
        {
            Move(path[0].X, path[0].Y);
            UpdatePath();
        }


    }
    public void WritePath()
    {
        foreach (Tile tile in path)
        {
            Console.WriteLine($"X: {tile.X} Y:{tile.Y}");
        }
    }
}