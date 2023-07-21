// G cost = distance from starting node
// H cost = distance from end node
// F cost = G cost + F cost
class Program
{
    public Map map;
    public static void Main(string[] args)
    {
        Map map = new Map();
        Astar astar = new Astar(map);

        Tile start = map.PlaceTile(3, 4, "A");
        Tile finish = map.PlaceTile(3, 1, "B");

        map.PrintMap();

        Console.WriteLine($"start x, {start.X}, start Y {start.Y}");
        Console.WriteLine($"finish x, {finish.X}, finish Y {finish.Y}");

        start.SetDistance(finish.X, finish.Y);

        List<Tile> path = astar.CalculatePath(start, finish);

        map.PrintPath(path);

    }
}
