namespace OOP_ICT.Fourth.Infrastructure.Exceptions
{
    public class CardsAlreadyDealtException : ApplicationException
    {
        public CardsAlreadyDealtException(string message = null, Exception innerException = null)
            : base(message ?? "The cards have already been dealt.", innerException)
        {
        }
    }
}
