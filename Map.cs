public class Map
{

    public List<string> map;
    public Map()
    {
        map = GenerateMap();
    }

    public static List<string> GenerateMap()
    {
        List<string> map = new List<string>
            {
                "                  ",
                "                  ",
                "                  ",
                "                  ",
                "              ----",
                "              |   ",
                "       --------   ",
                "                  ",
            };
        return map;
    }
    public Tile PlaceTile(int x, int y, string tile)
    {
        IsInBoundaries(map, x, y);
        var start = new Tile();
        start.X = x;
        start.Y = y;
        PlaceOnMap(tile, start.X, start.Y);
        return start;
    }

    public void PlaceOnMap(string obj, int x, int y)
    {
        IsInBoundaries(map, x, y);
        string oldMapLine = map[y];
        char[] array = oldMapLine.ToCharArray();
        array[x] = obj[0];
        string newMapLine = new string(array);
        map[y] = newMapLine;


        // if (map[y][x] == ' ')
        // {
        //     string oldMapLine = map[y];
        //     char[] array = oldMapLine.ToCharArray();
        //     array[x] = obj[0];
        //     string newMapLine = new string(array);
        //     map[y] = newMapLine;
        // }
        // else
        // {
        //     Console.WriteLine($"Can not place object {obj} on the map, coordinates x:{x} y:{y} {map[y][x]} already here");
        //     PrintMap();
        //     //throw new ArgumentException($"Can not place object {obj} on the map, coordinates x:{x} y:{y} {map[y][x]} already here");
        // }
    }

    public void PlaceOnMap(string obj, int x, int y, List<string> mapCopy)
    {
        string oldMapLine = mapCopy[y];
        char[] array = oldMapLine.ToCharArray();
        array[x] = obj[0];
        string newMapLine = new string(array);
        mapCopy[y] = newMapLine;
        PrintMap();
    }
    public void PrintMap()
    {
        map.ForEach(x => Console.WriteLine(x));
    }

    public void PrintMap(List<string> mapCopy)
    {
        mapCopy.ForEach(x => Console.WriteLine(x));
    }

    public static void IsInBoundaries(List<string> map, int x, int y)
    {
        if (x < 0 | y < 0)
        {
            throw new ArgumentException($"IsInBoundaries {x} or {y} less than zero");
        }
        // Count should begin from 0
        int listLength = map.Count - 1;

        if (listLength >= y)
        {
            // Console.WriteLine($"{y} is in {listLength}");

            int lineLength = map[y].Length - 1;
            if (lineLength >= x)
            {
                // Console.WriteLine($"{x} is in {lineLength}");
            }
            else
            {
                throw new ArgumentException($"{x} is not in {lineLength}");
            }
        }
        else
        {
            throw new ArgumentException($"{y} is not in {listLength}");
        }
    }
    public void PrintPath(List<Tile> path)
    {
        path.Reverse();
        foreach (Tile tile in path)
        {
            Console.WriteLine($"x: {tile.X} y: {tile.Y}");
            List<String> mapCopy = GenerateMap();
            PlaceOnMap("*", tile.X, tile.Y, mapCopy);
            PrintMap(mapCopy);
        }
    }





    public void MapOutput(Tile tile)
    {
        Console.WriteLine($"Current tile coordinates are {tile.X} and {tile.Y}, tile cost is {tile.Cost} tile distance is {tile.Distance} and tile CostDistance is {tile.CostDistance}");
        List<String> mapCopy = new List<string>(map);
        // PlaceOnMap(mapCopy, "X", tile.X, tile.Y);
        map.ForEach(x => Console.WriteLine(x));
    }

    public void PrintMapSize()
    {
        Console.WriteLine($"X is {map[0].Length - 1} Y is {map.Count - 1}");
    }


}