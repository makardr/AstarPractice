// G cost = distance from starting node
// H cost = distance from end node
// F cost = G cost + F cost
class Program
{

    public static void Main(string[] args)

    {
        Map map = new Map();


        Character character1 = new Character(map, 5, 0, 5, 3,"A1");
        Character character2 = new Character(map, 3, 4, 3, 1,"A2");
        while (true)
        {
            if (!character2.path.Any() & character1.path.Any())
            {
                Console.WriteLine("Path finished");
                break;
            }
            character1.MakeStep();
            character2.MakeStep();
            map.PrintMap();
        }
    }
}
