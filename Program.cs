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
        characters.CreateCharacter(map, 0, 1, 17, 7);
        characters.CreateCharacter(map, 1, 1, 17, 4);
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
