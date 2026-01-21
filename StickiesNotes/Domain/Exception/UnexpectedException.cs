public class UnexpectedException : Exception
{
    public UnexpectedException() : base("UnexpectedException") {}
    public UnexpectedException(string errorMessage) : base($"UnexpectedException: {errorMessage}") {}
}