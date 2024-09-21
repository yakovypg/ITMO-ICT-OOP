using OOP_ICT.Second.Models;

namespace OOP_ICT.Second.Infrastructure
{
    public abstract class BlackjackRoomFactory
    {
        public abstract List<Player> CreatePlayers(int count);
        public abstract BlackjackDealer CreateDealer();
    }
}
