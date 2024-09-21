using OOP_ICT.Fourth.Interfaces;

namespace OOP_ICT.Fourth.Infrastructure
{
    public class CallBetStrategy : IBetStrategy
    {
        private readonly int _bet;

        public CallBetStrategy(int bet)
        {
            _bet = bet;
        }

        public bool IsPlayerRemainsInGame => true;
        public bool IsPlayerSkipMove => false;

        public int GetBet()
        {
            return _bet;
        }
    }
}
