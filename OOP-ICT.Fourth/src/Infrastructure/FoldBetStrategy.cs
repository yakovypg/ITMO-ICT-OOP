using OOP_ICT.Fourth.Interfaces;

namespace OOP_ICT.Fourth.Infrastructure
{
    public class FoldBetStrategy : IBetStrategy
    {
        public bool IsPlayerRemainsInGame => false;
        public bool IsPlayerSkipMove => false;

        public int GetBet()
        {
            return 0;
        }
    }
}
