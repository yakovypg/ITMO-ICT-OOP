using OOP_ICT.Fourth.Models;
using OOP_ICT.Second.Interfaces;

namespace OOP_ICT.Fourth.Infrastructure
{
    public class PokerBuilder
    {
        protected readonly List<IPlayer> _players;

        public PokerBuilder()
        {
            _players = new List<IPlayer>();
        }

        public IReadOnlyList<IPlayer> Players => _players;
        public PlayerMoneyFlow PlayerMoneyFlow => new(new PokerBank(_players));
        public PokerDealer PokerDealer { get; private set; }

        public PokerBuilder AddPokerDealer(PokerDealer dealer)
        {
            PokerDealer = dealer ?? throw new ArgumentNullException(nameof(dealer));
            return this;
        }

        public PokerBuilder AddPlayer(IPlayer player)
        {
            if (player is null)
                throw new ArgumentNullException(nameof(player));

            _players.Add(player);
            return this;
        }

        public Poker Build()
        {
            return new Poker(this);
        }
    }
}
