// G cost = distance from starting node
// H cost = distance from end node
// F cost = G cost + F cost
class Program
{

    public static void Main(string[] args)

    {
        Map map = new Map();
        map.PrintMapSize();
        Characters characters = new Characters();
        characters.CreateCharacter(map, 5, 0, 5, 3);
        characters.CreateCharacter(map, 3, 4, 3, 1);
        characters.CreateCharacter(map, 17, 5, 0, 0);

        while (true)
        {
            if (!characters.charactersList.Any(obj => obj.IsActive))
            {
                Console.WriteLine("Pathfinding finished");
                break;
            }
            foreach(Character character in characters.charactersList){
                character.MakeStep();
            }
            
            map.PrintMap();
            Console.WriteLine("---------------------------------------");
        }
    }
}
