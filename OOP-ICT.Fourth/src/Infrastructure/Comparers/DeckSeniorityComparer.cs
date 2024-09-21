using OOP_ICT.Models;

namespace OOP_ICT.Fourth.Infrastructure.Comparers
{
    public class DeckSeniorityComparer : IComparer<List<Card>>
    {
        public int Compare(List<Card> x, List<Card> y)
        {
            x = x.OrderByDescending(t => t.Nominal.Weight).ToList();
            y = x.OrderByDescending(t => t.Nominal.Weight).ToList();

            for (int i = 0; i < Math.Min(x.Count, y.Count); ++i)
            {
                if (x[i].Nominal.Weight == y[i].Nominal.Weight)
                    continue;

                return x[i].Nominal.Weight - y[i].Nominal.Weight < 0
                    ? -1
                    : 1;
            }

            return 0;
        }
    }
}
