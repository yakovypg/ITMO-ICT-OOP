namespace OOP_ICT.Fourth.Infrastructure.Exceptions
{
    public class IncorrectCardsCountException : ApplicationException
    {
        public IncorrectCardsCountException(string message = null, Exception innerException = null)
            : base(message, innerException)
        {
        }
    }
}
