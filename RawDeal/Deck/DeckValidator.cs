namespace RawDeal;
using System.Collections.Generic;
using System.Linq;

public class DeckValidator
{
    private const int DeckSize = 60;
    private const int UniqueSize = 1;
    private const int SeTupSize = 3;

    public bool Validate(List<NormalCard> deck)
    {
        return HasValidDeckSize(deck) &&
               !HasMixedAlignmentCards(deck) &&
               IsValidCardCount(deck);
    }

    private bool HasValidDeckSize(List<NormalCard> deck)
    {
        return deck.Count == DeckSize;
    }

    private bool HasMixedAlignmentCards(List<NormalCard> deck)
    {
        return HasCardOfType(deck, "Heel") && HasCardOfType(deck, "Face");
    }

    private bool HasCardOfType(List<NormalCard> deck, string type)
    {
        return deck.Any(card => card.Subtypes.Contains(type));
    }

    private bool IsValidCardCount(List<NormalCard> deck)
    {
        var cardCount = CountCards(deck);
        return !cardCount.Any(element =>
        {
            var card = deck.First(c => c.Title == element.Key);
            return IsUniqueCardWithInvalidCount(card, element.Value) ||
                   IsNonSetUpCardWithInvalidCount(card, element.Value);
        });
    }

    private Dictionary<string, int> CountCards(List<NormalCard> deck)
    {
        return deck.GroupBy(card => card.Title)
            .ToDictionary(group => 
                group.Key, group => group.Count());
    }

    private bool IsUniqueCardWithInvalidCount(NormalCard card, int count)
    {
        return card.Subtypes.Contains("Unique") && count > UniqueSize;
    }

    private bool IsNonSetUpCardWithInvalidCount(NormalCard card, int count)
    {
        return !card.Subtypes.Contains("SetUp") && count > SeTupSize;
    }
}