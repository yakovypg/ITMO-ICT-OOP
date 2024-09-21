namespace OOP_ICT.Fourth.Infrastructure.Exceptions
{
    public class BetsAlreadyAcceptedException : ApplicationException
    {
        public BetsAlreadyAcceptedException(string message = null, Exception innerException = null)
            : base(message ?? "Bets from the players have already been accepted.", innerException)
        {
        }
    }
}
