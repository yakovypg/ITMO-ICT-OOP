namespace OOP_ICT.Fourth.Infrastructure.Exceptions
{
    public class IncorrectBetException : ApplicationException
    {
        public IncorrectBetException(string message = null, Exception innerException = null)
            : base(message, innerException)
        {
        }
    }
}
