using OOP_ICT.Fourth.Interfaces;

namespace OOP_ICT.Fourth.Infrastructure
{
    public class CheckBetStrategy : IBetStrategy
    {
        public bool IsPlayerRemainsInGame => true;
        public bool IsPlayerSkipMove => true;

        public int GetBet()
        {
            return 0;
        }
    }
}
