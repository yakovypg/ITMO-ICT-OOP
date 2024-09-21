using OOP_ICT.Second.Models;

namespace OOP_ICT.Second.Infrastructure
{
    public class LowGradeBlackjackRoomFactory : BlackjackRoomFactory
    {
        public override List<Player> CreatePlayers(int count)
        {
            var creator = new PoorPlayerCreator();

            return Enumerable.Range(1, count)
                .Select(t => creator.CreatePlayer($"Player{t}", t * 10 * 1000))
                .ToList();
        }

        public override BlackjackDealer CreateDealer()
        {
            return new BlackjackDealer();
        }
    }
}
