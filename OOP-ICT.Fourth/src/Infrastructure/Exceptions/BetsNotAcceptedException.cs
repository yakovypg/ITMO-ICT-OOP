namespace OOP_ICT.Fourth.Infrastructure.Exceptions
{
    public class BetsNotAcceptedException : ApplicationException
    {
        public BetsNotAcceptedException(string message = null, Exception innerException = null)
            : base(message ?? "Bets from the players have not yet been accepted.", innerException)
        {
        }
    }
}
