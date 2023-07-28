namespace AstarPractice;

public class Characters
{
    private List<Character> _charactersList;

    public Characters()
    {
        _charactersList = new List<Character>();
    }

    public List<Character> GetCharactersList()
    {
        return _charactersList;
    }

    public void CreateCharacter(Map map, int x, int y, int destinationX, int destinationY)
    {
        Character character = new Character(map, x, y, destinationX, destinationY);
        _charactersList.Add(character);
    }
}