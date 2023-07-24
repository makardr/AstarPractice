public class Characters
{

    public List<Character> charactersList;
    public Characters()
    {
        this.charactersList = new List<Character>();
    }

    public void CreateCharacter(Map map, int X, int Y, int destinationX, int destinationY, string icon)
    {
        Character character = new Character(map, X, Y, destinationX, destinationY,icon);
        charactersList.Add(character);
    }
}