namespace OOP_ICT.Fourth.Infrastructure.Exceptions
{
    public class AnotherPlayerTurnException : ApplicationException
    {
        public AnotherPlayerTurnException(string message = null, Exception innerException = null)
            : base(message ?? "Now it is the other player's turn to place a bet.", innerException)
        {
        }
    }
}
