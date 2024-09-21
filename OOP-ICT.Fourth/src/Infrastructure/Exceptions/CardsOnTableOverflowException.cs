namespace OOP_ICT.Fourth.Infrastructure.Exceptions
{
    public class CardsOnTableOverflowException : ApplicationException
    {
        public CardsOnTableOverflowException(string message = null, Exception innerException = null)
            : base(message ?? "The maximum number of cards on the table has been reached.", innerException)
        {
        }
    }
}
