using OOP_ICT.Second.Models;

namespace OOP_ICT.Second.Infrastructure
{
    public class PoorPlayerCreator : PlayerCreator
    {
        private const int POOR_PLAYER_MONEY = 10 * 1000;

        public override Player CreatePlayer(string name)
        {
            return CreatePlayer(name, POOR_PLAYER_MONEY);
        }

        public override Player CreatePlayer(string name, int money)
        {
            return money < 0
                ? throw new ArgumentException("The money must be more than zero.", nameof(money))
                : new Player(name, money);
        }
    }
}
