namespace AstarPractice;

public class Tile
{
    public int X { get; set; }
    public int Y { get; set; }
    public float Cost { get; set; }
    public int Distance { get; set; }
    public float CostDistance => Cost + Distance;
    public Tile Parent { get; set; }

    public char value { get; set; }

    //The distance is essentially the estimated distance, ignoring walls to our target. 
    //So how many tiles left and right, up and down, ignoring walls, to get there. 
    public void SetDistance(int targetX, int targetY)
    {
        this.Distance = Math.Abs(targetX - X) + Math.Abs(targetY - Y);
    }
}