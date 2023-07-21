public class Characters
{

    public List<Character> charactersList;
    public Characters()
    {
        this.charactersList = new List<Character>();
    }

    public void CreateCharacter(Map map, int X, int Y, int destinationX, int destinationY)
    {
        Character character = new Character(map, X, Y, destinationX, destinationY);
        charactersList.Add(character);
    }
}