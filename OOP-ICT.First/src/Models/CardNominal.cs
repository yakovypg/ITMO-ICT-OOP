namespace OOP_ICT.Models;

public class CardNominal : IEquatable<CardNominal>
{
    public string Name { get; }
    public int Weight { get; internal set; }

    public CardNominal(string name, int weight)
    {
        if (name is null)
            throw new ArgumentNullException(nameof(name));

        Name = name;
        Weight = weight;
    }

    public bool Equals(CardNominal other)
    {
        return other is not null &&
               Name == other.Name &&
               Weight == other.Weight;
    }

    public override bool Equals(object obj)
    {
        return Equals(obj as CardNominal);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Name, Weight);
    }
}
