namespace OOP_ICT.Second.Infrastructure.Exceptions
{
    public class GameNotStartedException : ApplicationException
    {
        public GameNotStartedException(string message = null, Exception innerException = null) :
            base(message ?? "Game has not started yet.", innerException)
        {
        }
    }
}
