using OOP_ICT.Second.Interfaces;

namespace OOP_ICT.Third.Models
{
    public class Bank
    {
        private static int _nextAccountId = 1;
        protected readonly List<BankAccount> _accounts;

        public Bank(IEnumerable<IPlayer> clients = null)
        {
            _accounts = new List<BankAccount>();

            if (clients is not null)
            {
                foreach (var client in clients)
                    AddAccount(client);
            }
        }

        public IReadOnlyList<BankAccount> Accounts => _accounts;

        public void AddAccount(IPlayer client)
        {
            if (_accounts.Any(t => t.Client.Equals(client)))
                return;

            var account = new BankAccount(_nextAccountId++, client);
            _accounts.Add(account);
        }

        public void AddMoneyToAccount(int id, int sum)
        {
            BankAccount account = FindAccount(id);
            account.Client.Money += sum;
        }

        public void WithdrawMoneyFromAccount(int id, int sum)
        {
            BankAccount account = FindAccount(id);

            if (!CheckIfClientHasEnoughFunds(account, sum))
                throw new ArgumentException("There are not enough funds in the account.", nameof(sum));

            account.Client.Money -= sum;
        }

        public bool CheckIfClientHasEnoughFunds(BankAccount account, int sum)
        {
            return account.Client.Money >= sum;
        }

        private BankAccount FindAccount(int id)
        {
            BankAccount account = _accounts.FirstOrDefault(t => t.Id == id);

            return account is null
                ? throw new KeyNotFoundException("Account with the specified id was not found.")
                : account;
        }
    }
}
