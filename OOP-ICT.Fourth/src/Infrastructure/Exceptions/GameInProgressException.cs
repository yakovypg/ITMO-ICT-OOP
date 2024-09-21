namespace OOP_ICT.Fourth.Infrastructure.Exceptions
{
    public class GameInProgressException : ApplicationException
    {
        public GameInProgressException(string message = null, Exception innerException = null)
            : base(message, innerException)
        {
        }
    }
}
