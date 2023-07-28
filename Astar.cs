using TestProject.Exceptions;

namespace AstarPractice;

// G cost = distance from starting node
// H cost = distance from end node
// F cost = G cost + F cost

public class Astar
{
    private Map map;

    public Astar(Map map)
    {
        this.map = map;
    }


    public List<Tile> CalculatePath(Tile start, Tile finish)
    {
        var activeTiles = new List<Tile>();
        activeTiles.Add(start);
        var visitedTiles = new List<Tile>();


        while (activeTiles.Any())
        {
            var checkTile = activeTiles.OrderBy(x => x.CostDistance).First();
            if (checkTile.X == finish.X && checkTile.Y == finish.Y)
            {
                //We found the destination and we can be sure (Because the the OrderBy above)
                //That it's the most low cost option. 
                var tile = checkTile;
                // Console.WriteLine("Retracing steps backwards...");
                var pathTiles = new List<Tile>();
                while (true)
                {
                    // Console.WriteLine($"x: {tile.X} y: {tile.Y}");
                    // Cant remove this dublicate check because it attempts to rewrite "B" space
                    pathTiles.Add(tile);

                    // Prints the entire path, optional
                    // if (map.map[tile.Y][tile.X] == ' ')
                    // {
                    //     map.PlaceOnMap("*", tile.X, tile.Y);
                    // }

                    tile = tile.Parent;
                    if (tile == null)
                    {
                        return pathTiles;
                    }
                }
            }

            visitedTiles.Add(checkTile);
            activeTiles.Remove(checkTile);

            var walkableTiles = GetWalkableTiles(checkTile, finish);

            foreach (var walkableTile in walkableTiles)
            {
                //We have already visited this tile so we don't need to do so again!
                if (visitedTiles.Any(x => x.X == walkableTile.X && x.Y == walkableTile.Y))
                    continue;
                // MapOutput(walkableTile, map);
                //It's already in the active list, but that's OK, maybe this new tile has a better value (e.g. We might zigzag earlier but this is now straighter). 
                if (activeTiles.Any(x => x.X == walkableTile.X && x.Y == walkableTile.Y))
                {
                    var existingTile = activeTiles.First(x => x.X == walkableTile.X && x.Y == walkableTile.Y);
                    if (existingTile.CostDistance > checkTile.CostDistance)
                    {
                        activeTiles.Remove(existingTile);
                        activeTiles.Add(walkableTile);
                    }
                }
                else
                {
                    //We've never seen this tile before so add it to the list. 
                    activeTiles.Add(walkableTile);
                }
            }
        }

        // Console.WriteLine("");
        throw new NoPathFoundException("No Path Found!");
    }

    private List<Tile> GetWalkableTiles(Tile currentTile, Tile targetTile)
    {
        var possibleTiles = new List<Tile>()
        {
            new Tile
            {
                X = currentTile.X, Y = currentTile.Y - 1, Parent = currentTile,
                Cost = currentTile.Cost + 1 + CalculateCost(currentTile)
            },
            new Tile
            {
                X = currentTile.X, Y = currentTile.Y + 1, Parent = currentTile,
                Cost = currentTile.Cost + 1 + CalculateCost(currentTile)
            },
            new Tile
            {
                X = currentTile.X - 1, Y = currentTile.Y, Parent = currentTile,
                Cost = currentTile.Cost + 1 + CalculateCost(currentTile)
            },
            new Tile
            {
                X = currentTile.X + 1, Y = currentTile.Y, Parent = currentTile,
                Cost = currentTile.Cost + 1 + CalculateCost(currentTile)
            },
            new Tile
            {
                X = currentTile.X - 1, Y = currentTile.Y - 1, Parent = currentTile,
                Cost = currentTile.Cost + 1.5f + CalculateCost(currentTile)
            },
            new Tile
            {
                X = currentTile.X + 1, Y = currentTile.Y + 1, Parent = currentTile,
                Cost = currentTile.Cost + 1.5f + CalculateCost(currentTile)
            },
            new Tile
            {
                X = currentTile.X - 1, Y = currentTile.Y + 1, Parent = currentTile,
                Cost = currentTile.Cost + 1.5f + CalculateCost(currentTile)
            },
            new Tile
            {
                X = currentTile.X + 1, Y = currentTile.Y - 1, Parent = currentTile,
                Cost = currentTile.Cost + 1.5f + CalculateCost(currentTile)
            },
        };

        possibleTiles.ForEach(tile => tile.SetDistance(targetTile.X, targetTile.Y));

        var maxX = map.GetMap().First().Length - 1;
        var maxY = map.GetMap().Count - 1;

        return possibleTiles
            .Where(tile => tile.X >= 0 && tile.X <= maxX)
            .Where(tile => tile.Y >= 0 && tile.Y <= maxY)
            .Where(tile => map.GetMap()[tile.Y][tile.X] == ' ' || map.GetMap()[tile.Y][tile.X] == 'B' ||
                           map.GetMap()[tile.Y][tile.X] == '*')
            .ToList();
    }

    private float CalculateCost(Tile tile)
    {
        switch (map.GetMap()[tile.Y][tile.X])
        {
            case '*':
                return 10f;
        }

        return 0f;
    }
}