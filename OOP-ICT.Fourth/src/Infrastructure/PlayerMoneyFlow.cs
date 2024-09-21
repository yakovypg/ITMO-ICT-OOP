using OOP_ICT.Fourth.Models;
using OOP_ICT.Second.Interfaces;

namespace OOP_ICT.Fourth.Infrastructure
{
    public class PlayerMoneyFlow : IPlayerMoneyFlow
    {
        private readonly PokerBank _bank;

        public PlayerMoneyFlow(PokerBank bank)
        {
            _bank = bank ?? throw new ArgumentNullException(nameof(bank));
        }

        public void DepositMoney(IPlayer player, int sum)
        {
            _bank.AddWinningsToPlayer(player, sum);
        }

        public void WithdrawMoney(IPlayer player, int sum)
        {
            _bank.WithdrawBetFromPlayer(player, sum);
        }
    }
}
