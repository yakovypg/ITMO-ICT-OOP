using OOP_ICT.Second.Models;

namespace OOP_ICT.Second.Infrastructure
{
    public class RichPlayerCreator : PlayerCreator
    {
        private const int RICH_PLAYER_MONEY = 1000 * 1000;

        public override Player CreatePlayer(string name)
        {
            return CreatePlayer(name, RICH_PLAYER_MONEY);
        }

        public override Player CreatePlayer(string name, int money)
        {
            return money < 0
                ? throw new ArgumentException("The money must be more than zero.", nameof(money))
                : new Player(name, money);
        }
    }
}
