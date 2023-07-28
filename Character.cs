using TestProject.Exceptions;

namespace AstarPractice;

using SFML.Graphics;
using SFML.System;

public class Character : Tile
{
    public bool IsActive = true;
    public readonly RectangleShape Shape;
    
    private int _destinationX;
    private int _destinationY;
    private string _icon = "A";
    private Tile _start;
    private Tile _finish;
    private Map _map;
    private List<Tile> _path;
    private List<Tuple<int, int>> _availableTilesList;

    private static int _width = 100;
    private static int _height = 100;

    public Character(Map map, int x, int y, int destinationX, int destinationY)
    {
        _map = map;
        X = x;
        Y = y;
        _destinationX = destinationX;
        _destinationY = destinationY;
        _start = map.PlaceTile(_icon,x, y );
        _finish = map.PlaceTile("B",destinationX, destinationY );
        SetDistance(destinationX, destinationY);
        _path = CalculatePath();
        Shape = SfmlWindow.CreateSquare(x, y, _width, _height, SfmlWindow.worldMultiplier, Color.Red);
    }

    private void UpdatePath()
    {
        _start.X = X;
        _start.Y = Y;
        // Console.WriteLine($"Calculating new path with start coordinates x: {start.X} y:{start.Y}");
        _path = CalculatePath();

        // Remove first is neccessary because if the first elemement is the current position of the character and it makes it move to the current place indefinetly
        _path.RemoveAt(0);
        // WritePath();
        // map.PrintPath(CalculatePath());
        if (!_path.Any())
        {
            Console.WriteLine($"Character {_icon} became inactive");
            IsActive = false;
        }
    }

    private List<Tile> CalculatePath()
    {
        try
        {
            Astar astar = new Astar(_map);
            List<Tile> path = astar.CalculatePath(_start, _finish);
            path.Reverse();
            return path;
        }
        catch (NoPathFoundException)
        {
            return _path;
        }
    }

    private void Move(int moveToX, int moveToY)
    {
        // delete itself from the old position on the map
        if (_map.GetMap()[Y][X] == Char.Parse(_icon))
        {
            _map.PlaceOnMap(" ", X, Y);
        }


        // update position
        X = moveToX;
        Y = moveToY;
        // redraw character
        _map.PlaceOnMap(_icon, X, Y);
        Shape.Position = new Vector2f(X * SfmlWindow.worldMultiplier, Y * SfmlWindow.worldMultiplier);
        // map.PrintMap();
        // WritePath();
        // Console.WriteLine($"Character {icon}, moved to X {X},Y {Y}");
    }

    public void MakeStep()
    {
        //Two IsActive checks are necessary to ensure the correct behavior of the character in the game. The first check updates the IsActive state,
        //and if both checks were combined into the same if statement, the character would react to being inactive too late, which could result in
        //a game crash due to attempted movement. The purpose of the first IsActive check is to optimize performance by avoiding unnecessary
        //calculations.
        if (IsActive)
        {
            // Before is active to delete tiles at an old place
            EraseArea();
            UpdatePath();
        }

        if (IsActive)
        {
            Move(_path[0].X, _path[0].Y);
            // After moving draw radius on new place
            DrawArea();
        }
    }

    private void DrawArea()
    {
        FindTilesAround();
        //Draw * around if tiles 
        foreach (Tuple<int, int> tuple in _availableTilesList)
        {
            _map.PlaceOnMap("*", tuple.Item1, tuple.Item2);
        }
    }

    private void EraseArea()
    {
        FindTilesAround();
        //Remove * around if they exist
        foreach (Tuple<int, int> tuple in _availableTilesList)
        {
            _map.PlaceOnMap(" ", tuple.Item1, tuple.Item2);
        }
    }

    private void FindTilesAround()
    {
        List<Tuple<int, int>> coordinatesList = CreateCoordinatesList();
        _availableTilesList = new List<Tuple<int, int>>();
        //check if ' '


        foreach (Tuple<int, int> point in coordinatesList)
        {
            if (_map.TileAvailable(point.Item1, point.Item2))
            {
                _availableTilesList.Add(new Tuple<int, int>(point.Item1, point.Item2));
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
        foreach (Tile tile in _path)
        {
            Console.WriteLine($"X: {tile.X} Y:{tile.Y}");
        }
    }
}