using OOP_ICT.Second.Infrastructure;
using OOP_ICT.Second.Interfaces;
using OOP_ICT.Second.Models;

namespace OOP_ICT.Third.Models
{
    public class BlackjackCasino
    {
        private readonly IPlayerMoneyFlow _moneyFlow;
        private readonly Dictionary<IPlayer, int> _players; // Player -> Bet

        public BlackjackCasino(Dictionary<IPlayer, int> players, IPlayerMoneyFlow moneyFlow)
        {
            _players = players ?? throw new ArgumentNullException(nameof(players));
            _moneyFlow = moneyFlow ?? throw new ArgumentNullException(nameof(moneyFlow));
        }

        public IReadOnlyDictionary<IPlayer, int> Players => _players;

        public void GiveOutWinnings(IPlayer player)
        {
            if (_players.TryGetValue(player, out int bet))
                _moneyFlow.DepositMoney(player, bet * Blackjack.WINNING_RATIO);
        }

        public void PickUpLoss(IPlayer player)
        {
            if (_players.TryGetValue(player, out int bet))
                _moneyFlow.WithdrawMoney(player, bet);
        }

        public void CheckBlackjack()
        {
            foreach (var player in _players.Keys)
            {
                int playerPoints = CardPointsCalculator.CalculatePoints(player.Cards);

                if (playerPoints == Blackjack.WINNING_PLAYER_POINTS)
                {
                    GiveOutWinnings(player);
                    _players.Remove(player);
                }
            }
        }
    }
}
