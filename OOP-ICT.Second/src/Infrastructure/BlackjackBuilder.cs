using OOP_ICT.Second.Interfaces;
using OOP_ICT.Second.Models;

namespace OOP_ICT.Second.Infrastructure
{
    public class BlackjackBuilder
    {
        protected readonly Dictionary<IPlayer, int> _players; // Player -> Bet

        public BlackjackBuilder()
        {
            _players = new Dictionary<IPlayer, int>();
        }

        public IReadOnlyDictionary<IPlayer, int> Players => _players;
        public BlackjackDealer BlackjackDealer { get; private set; }

        public BlackjackBuilder AddBlackjackDealer(BlackjackDealer dealer)
        {
            BlackjackDealer = dealer ?? throw new ArgumentNullException(nameof(dealer));
            return this;
        }

        public BlackjackBuilder AddPlayer(IPlayer player)
        {
            if (player is null)
                throw new ArgumentNullException(nameof(player));

            _players.TryAdd(player, 0);
            return this;
        }

        public BlackjackBuilder AddBet(IPlayer player, int bet)
        {
            if (player is null)
                throw new ArgumentNullException(nameof(player));

            if (!_players.ContainsKey(player))
                throw new KeyNotFoundException("Player not exists.");

            if (bet <= 0)
                throw new ArgumentException("Bet must be greater than zero.", nameof(bet));

            if (bet > player.Money)
                throw new ArgumentException("Player does not have enough money to bet.", nameof(bet));

            _players[player] = bet;

            return this;
        }

        public Blackjack Build()
        {
            return new Blackjack(this);
        }
    }
}
