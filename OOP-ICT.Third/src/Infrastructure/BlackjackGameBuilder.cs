using OOP_ICT.Second.Infrastructure;
using OOP_ICT.Second.Interfaces;
using OOP_ICT.Second.Models;
using OOP_ICT.Third.Models;

namespace OOP_ICT.Third.Infrastructure
{
    public class BlackjackGameBuilder : BlackjackBuilder
    {       
        public BlackjackCasino BlackjackCasino
        {
            get
            {
                var bankAdapter = new BankAdapter(_players.Keys);
                return new BlackjackCasino(_players, bankAdapter);
            }
        }

        public new BlackjackGameBuilder AddBlackjackDealer(BlackjackDealer dealer)
        {
            base.AddBlackjackDealer(dealer);
            return this;
        }

        public new BlackjackGameBuilder AddPlayer(IPlayer player)
        {
            base.AddPlayer(player);
            return this;
        }

        public new BlackjackGameBuilder AddBet(IPlayer player, int bet)
        {
            base.AddBet(player, bet);
            return this;
        }

        public new BlackjackGame Build()
        {
            return new BlackjackGame(this);
        }
    }
}
