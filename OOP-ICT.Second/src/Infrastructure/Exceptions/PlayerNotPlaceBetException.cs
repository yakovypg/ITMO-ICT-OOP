using OOP_ICT.Second.Interfaces;

namespace OOP_ICT.Second.Infrastructure.Exceptions
{
    public class PlayerNotPlaceBetException : ApplicationException
    {
        public PlayerNotPlaceBetException(IPlayer player, string message = null, Exception innerException = null) :
            base(message ?? "Player did not place a bet.", innerException)
        {
            Player = player;
        }

        public IPlayer Player { get; }
    }
}
