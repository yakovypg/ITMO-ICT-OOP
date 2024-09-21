using OOP_ICT.Second.Models;

namespace OOP_ICT.Second.Infrastructure
{
    public abstract class PlayerCreator
    {
        public abstract Player CreatePlayer(string name);
        public abstract Player CreatePlayer(string name, int money);
    }
}
