using OOP_ICT.Second.Interfaces;
using OOP_ICT.Second.Models;
using OOP_ICT.Third.Infrastructure;
using OOP_ICT.Third.Infrastructure.Exceptions;

namespace OOP_ICT.Third.Models
{
    public class BlackjackGame : Blackjack
    {
        private readonly BlackjackCasino _casino;

        public BlackjackGame(BlackjackGameBuilder builder) : base(builder)
        {
            _casino = builder.BlackjackCasino ?? throw new CasinoNotFoundException();
        }

        public override void StartGame()
        {
            base.StartGame();
            _casino.CheckBlackjack();
        }

        protected override void RemoveLosingPlayer(IPlayer player)
        {
            if (player is null)
                throw new ArgumentNullException(nameof(player));

            if (_players.TryGetValue(player, out int bet))
            {
                _casino.PickUpLoss(player);
                _players.Remove(player);
            }
        }

        protected override void GiveWinningsToPlayer(IPlayer player)
        {
            if (player is null)
                throw new ArgumentNullException(nameof(player));

            _casino.GiveOutWinnings(player);
        }
    }
}
