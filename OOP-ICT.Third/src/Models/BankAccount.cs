using OOP_ICT.Second.Interfaces;

namespace OOP_ICT.Third.Models
{
    public class BankAccount
    {
        public BankAccount(int id, IPlayer client)
        {
            Id = id;
            Client = client;
        }

        public int Id { get; }
        public IPlayer Client { get; }
    }
}
