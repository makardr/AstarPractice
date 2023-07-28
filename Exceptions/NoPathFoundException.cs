namespace TestProject.Exceptions;

public class NoPathFoundException : Exception
{
    public NoPathFoundException(string message) : base(message)
    {
    }
}