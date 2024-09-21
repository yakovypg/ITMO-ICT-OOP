using OOP_ICT.Fourth.Infrastructure.Comparers;
using OOP_ICT.Fourth.Models;
using OOP_ICT.Models;

namespace OOP_ICT.Fourth.Infrastructure
{
    public class CardCombinationDetector
    {
        private readonly List<Card> _cards;

        public CardCombinationDetector(IEnumerable<Card> cards)
        {
            if (cards is null)
                throw new ArgumentNullException(nameof(cards));

            if (!cards.Any())
                throw new ArgumentException("The number of cards must be greater than zero.", nameof(cards));

            _cards = new List<Card>(cards);
        }

        private IEnumerable<Card> Hearts => _cards.Where(t => t.Suit == CardSuit.Hearts);
        private IEnumerable<Card> Spades => _cards.Where(t => t.Suit == CardSuit.Spades);
        private IEnumerable<Card> Clubs => _cards.Where(t => t.Suit == CardSuit.Clubs);
        private IEnumerable<Card> Diamonds => _cards.Where(t => t.Suit == CardSuit.Diamonds);

        public CardCombination DetectMaxCombination()
        {
            if (DetectRoyalFlush(out List<Card> cards))
                return new CardCombination(CombinationType.RoyalFlush, cards);

            if (DetectStraightFlush(out cards))
                return new CardCombination(CombinationType.StraightFlush, cards);

            if (DetectFourOfKind(out cards))
                return new CardCombination(CombinationType.FourOfKind, cards);

            if (DetectFullHouse(out cards))
                return new CardCombination(CombinationType.FullHouse, cards);

            if (DetectFlush(out cards))
                return new CardCombination(CombinationType.Flush, cards);

            if (DetectStraight(out cards))
                return new CardCombination(CombinationType.Straight, cards);

            if (DetectThreeOfKind(out cards))
                return new CardCombination(CombinationType.ThreeOfKind, cards);

            if (DetectTwoPair(out cards))
                return new CardCombination(CombinationType.TwoPair, cards);

            if (DetectPair(out cards))
                return new CardCombination(CombinationType.Pair, cards);

            if (DetectHighCard(out cards))
                return new CardCombination(CombinationType.HighCard, cards);

            throw new NotImplementedException();
        }

        public bool DetectHighCard(out List<Card> highCard)
        {
            var orderedCards = _cards.OrderByDescending(t => t.Nominal.Weight);
            highCard = new List<Card>() { orderedCards.First() };

            return true;
        }

        public bool DetectPair(out List<Card> pair)
        {
            var pairs = GetPairs(_cards)
                .OrderByDescending(t => t.First().Nominal.Weight);

            if (pairs.Any())
            {
                pair = new List<Card>(pairs.First());
                return true;
            }

            pair = new List<Card>();
            return false;
        }

        public bool DetectTwoPair(out List<Card> twoPair)
        {
            var pairs = GetPairs(_cards)
                .OrderByDescending(t => t.First().Nominal.Weight)
                .ToArray();

            if (pairs.Length >= 2)
            {
                twoPair = new List<Card>();
                twoPair.AddRange(pairs[0]);
                twoPair.AddRange(pairs[1]);

                return true;
            }

            twoPair = new List<Card>();
            return false;
        }

        public bool DetectThreeOfKind(out List<Card> threeOfKind)
        {
            var nominalWeights = _cards.Select(t => t.Nominal.Weight).Distinct();

            foreach (var nominalWeight in nominalWeights)
            {
                if (GetThreeOfKind(_cards, nominalWeight, out threeOfKind))
                    return true;
            }

            threeOfKind = new List<Card>();
            return false;
        }

        public bool DetectStraight(out List<Card> straight)
        {
            var cards = _cards
                .GroupBy(t => t.Nominal.Weight)
                .Select(t => t.First());

            return GetStraightFlush(cards, out straight);
        }

        public bool DetectFlush(out List<Card> flush)
        {
            var orderedHearts = Hearts.OrderByDescending(t => t.Nominal.Weight);
            var orderedSpades = Spades.OrderByDescending(t => t.Nominal.Weight);
            var orderedClubs = Clubs.OrderByDescending(t => t.Nominal.Weight);
            var orderedDiamonds = Diamonds.OrderByDescending(t => t.Nominal.Weight);

            List<List<Card>> candidates = new List<List<Card>>();

            if (orderedHearts.Count() >= 5)
                candidates.Add(new List<Card>(orderedHearts.Take(5)));

            if (orderedSpades.Count() >= 5)
                candidates.Add(new List<Card>(orderedSpades.Take(5)));

            if (orderedClubs.Count() >= 5)
                candidates.Add(new List<Card>(orderedClubs.Take(5)));

            if (orderedDiamonds.Count() >= 5)
                candidates.Add(new List<Card>(orderedDiamonds.Take(5)));

            if (candidates.Count == 0)
            {
                flush = new List<Card>();
                return false;
            }

            candidates.Sort(new DeckSeniorityComparer());

            flush = candidates.Last();
            return true;
        }

        public bool DetectFullHouse(out List<Card> fullHouse)
        {
            var triples = GetUniqueTriples(_cards)
                .OrderByDescending(t => t.First().Nominal.Weight);

            foreach (var triple in triples)
            {
                IEnumerable<Card> remainingCards = _cards.Except(triple);

                var pairs = GetUniquePairs(remainingCards)
                    .OrderByDescending(t => t.First().Nominal.Weight);

                if (pairs.Any())
                {
                    fullHouse = new List<Card>();
                    fullHouse.AddRange(triple);
                    fullHouse.AddRange(pairs.First());

                    return true;
                }
            }

            fullHouse = new List<Card>();
            return false;
        }

        public bool DetectFourOfKind(out List<Card> fourOfKind)
        {
            var nominalWeights = _cards.Select(t => t.Nominal.Weight).Distinct();

            foreach (var nominalWeight in nominalWeights)
            {
                if (GetFourOfKind(_cards, nominalWeight, out fourOfKind))
                    return true;
            }

            fourOfKind = new List<Card>();
            return false;
        }

        public bool DetectStraightFlush(out List<Card> straightFlush)
        {
            if (GetStraightFlush(Hearts, out List<Card> straightFlushFromHearts))
            {
                straightFlush = straightFlushFromHearts;
                return true;
            }
            if (GetStraightFlush(Spades, out List<Card> straightFlushFromSpades))
            {
                straightFlush = straightFlushFromSpades;
                return true;
            }
            if (GetStraightFlush(Clubs, out List<Card> straightFlushFromClubs))
            {
                straightFlush = straightFlushFromClubs;
                return true;
            }
            if (GetStraightFlush(Diamonds, out List<Card> straightFlushFromDiamonds))
            {
                straightFlush = straightFlushFromDiamonds;
                return true;
            }

            straightFlush = new List<Card>();
            return false;
        }

        public bool DetectRoyalFlush(out List<Card> royalFlush)
        {
            if (GetRoyalFlush(Hearts, out List<Card> royalFlushFromHearts))
            {
                royalFlush = royalFlushFromHearts;
                return true;
            }
            if (GetRoyalFlush(Spades, out List<Card> royalFlushFromSpades))
            {
                royalFlush = royalFlushFromSpades;
                return true;
            }
            if (GetRoyalFlush(Clubs, out List<Card> royalFlushFromClubs))
            {
                royalFlush = royalFlushFromClubs;
                return true;
            }
            if (GetRoyalFlush(Diamonds, out List<Card> royalFlushFromDiamonds))
            {
                royalFlush = royalFlushFromDiamonds;
                return true;
            }

            royalFlush = new List<Card>();
            return false;
        }

        private static IEnumerable<IEnumerable<Card>> GetPairs(IEnumerable<Card> cards)
        {
            var groups = cards
                .GroupBy(t => t.Nominal.Weight)
                .Where(t => t.Count() >= 2);

            var pairs = new List<List<Card>>();

            foreach (var group in groups)
            {
                pairs.Add(group.Take(2).ToList());

                if (group.Count() == 4)
                    pairs.Add(group.TakeLast(2).ToList());
            }

            return pairs;
        }

        private static IEnumerable<IEnumerable<Card>> GetUniquePairs(IEnumerable<Card> cards)
        {
            return cards
                .GroupBy(t => t.Nominal.Weight)
                .Where(t => t.Count() >= 2)
                .Select(t => t.Take(2));
        }

        private static IEnumerable<IEnumerable<Card>> GetUniqueTriples(IEnumerable<Card> cards)
        {
            return cards
                .GroupBy(t => t.Nominal.Weight)
                .Where(t => t.Count() >= 3)
                .Select(t => t.Take(3));
        }

        private static bool GetThreeOfKind(IEnumerable<Card> cards, int nominalWeight, out List<Card> threeOfKind)
        {
            return GetCardsByNominalWeight(cards, nominalWeight, 3, out threeOfKind);
        }

        private static bool GetFourOfKind(IEnumerable<Card> cards, int nominalWeight, out List<Card> fourOfKind)
        {
            return GetCardsByNominalWeight(cards, nominalWeight, 4, out fourOfKind);
        }

        private static bool GetCardsByNominalWeight(IEnumerable<Card> cards, int nominalWeight, int cardsCount, out List<Card> fourOfKind)
        {
            var suspiciousCards = cards.Where(t => t.Nominal.Weight == nominalWeight);

            if (suspiciousCards.Count() >= cardsCount)
            {
                fourOfKind = suspiciousCards.Take(cardsCount).ToList();
                return true;
            }

            fourOfKind = new List<Card>();
            return false;
        }

        private static bool GetStraightFlush(IEnumerable<Card> cards, out List<Card> straightFlush, bool isAceInversed = false)
        {
            Card[] orderedCards = cards.OrderBy(t => t.Nominal.Weight).ToArray();

            for (int i = 0; i < orderedCards.Length - 5; ++i)
            {
                bool isStraightFlushFound = true;

                for (int j = i + 1; j < i + 5; ++j)
                {
                    if (orderedCards[j].Nominal.Weight - orderedCards[j - 1].Nominal.Weight != 1)
                    {
                        isStraightFlushFound = false;
                        break;
                    }
                }

                if (isStraightFlushFound)
                {
                    straightFlush = orderedCards.Skip(i).Take(5).ToList();
                    return true;
                }
            }

            Card two = cards.FirstOrDefault(t => t.Nominal.Name == "Two");
            Card ace = cards.FirstOrDefault(t => t.Nominal.Name == "Ace");

            if (two is not null && ace is not null && !isAceInversed)
            {
                var inversedNominal = new CardNominal(ace.Nominal.Name, two.Nominal.Weight - 1);
                var inversedAce = new Card(ace.Suit, inversedNominal);
                var cardsWithInversedAce = cards.Except(new[] { ace }).Append(inversedAce);

                return GetStraightFlush(cardsWithInversedAce, out straightFlush, true);
            }

            straightFlush = new List<Card>();
            return false;
        }

        private static bool GetRoyalFlush(IEnumerable<Card> cards, out List<Card> royalFlush)
        {
            Card ten = cards.FirstOrDefault(t => t.Nominal.Name == "Ten");
            Card jack = cards.FirstOrDefault(t => t.Nominal.Name == "Jack");
            Card queen = cards.FirstOrDefault(t => t.Nominal.Name == "Queen");
            Card king = cards.FirstOrDefault(t => t.Nominal.Name == "King");
            Card ace = cards.FirstOrDefault(t => t.Nominal.Name == "Ace");

            if (ten is null || jack is null || queen is null || king is null || ace is null)
            {
                royalFlush = new List<Card>();
                return false;
            }

            royalFlush = new List<Card>()
            {
                ten, jack, queen, king, ace
            };

            return true;
        }
    }
}
