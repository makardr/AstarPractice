public class Characters
{
    public List<Character> charactersList;

    public Characters()
    {
        this.charactersList = new List<Character>();
    }

    public void CreateCharacter(Map map, int x, int y, int destinationX, int destinationY)
    {
        Character character = new Character(map, x, y, destinationX, destinationY);
        charactersList.Add(character);
    }
}