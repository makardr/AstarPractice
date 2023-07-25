class Program
{
    public static void Main(string[] args)
    {
        //SilkWindow silkWindow = new SilkWindow();

        Map map = new Map();
        Characters characters = new Characters();

        characters.CreateCharacter(map, 1, 1, 10, 1, "A");
        characters.CreateCharacter(map, 9, 1, 0, 1, "J");
        characters.CreateCharacter(map, 2, 0, 5, 3, "X");

        while (true)
        {
            if (!characters.charactersList.Any(obj => obj.IsActive))
            {
                Console.WriteLine("Pathfinding finished");
                break;
            }
            foreach (Character character in characters.charactersList)
            {
                character.MakeStep();
                map.PrintMap();
                
            }
            Console.ReadLine();
            Console.WriteLine("---------------------------------------");
        }
    }
}