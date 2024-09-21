namespace OOP_ICT.Fourth.Interfaces
{
    public interface IBetStrategy
    {
        bool IsPlayerRemainsInGame { get; }
        bool IsPlayerSkipMove { get; }

        int GetBet();
    }
}
