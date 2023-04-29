namespace RawDeal;
using System.Collections.Generic;
using System.Linq;

public class DeckValidator
{
    private const int DeckSize = 60;

    public bool Validate(SuperstarCard superstarCard, List<NormalCard> deck)
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
        bool hasHeelCard = HasHeelCard(deck);
        bool hasFaceCard = HasFaceCard(deck);

        return hasHeelCard && hasFaceCard;
    }

    
    private bool HasHeelCard(List<NormalCard> deck)
    {
        foreach (NormalCard card in deck)
        {
            if (card.Subtypes.Contains("Heel"))
            {
                return true;
            }
        }

        return false;
    }

    private bool HasFaceCard(List<NormalCard> deck)
    {
        foreach (NormalCard card in deck)
        {
            if (card.Subtypes.Contains("Face"))
            {
                return true;
            }
        }

        return false;
    }


    private bool IsValidCardCount(List<NormalCard> deck)
    {
        Dictionary<string, int> cardCount = CountCards(deck);

        foreach (KeyValuePair<string, int> element in cardCount)
        {
            NormalCard card = deck.First(c => c.Title == element.Key);
            if (IsUniqueCardWithInvalidCount(card, element.Value) || IsNonSetUpCardWithInvalidCount(card, element.Value))
            {
                return false;
            }
        }

        return true;
    }

    private Dictionary<string, int> CountCards(List<NormalCard> deck)
    {
        Dictionary<string, int> cardCount = new Dictionary<string, int>();

        foreach (NormalCard card in deck)
        {
            if (!cardCount.ContainsKey(card.Title))
            {
                cardCount[card.Title] = 0;
            }
            cardCount[card.Title]++;
        }

        return cardCount;
    }

    private bool IsUniqueCardWithInvalidCount(NormalCard card, int count)
    {
        return card.Subtypes.Contains("Unique") && count > 1;
    }

    private bool IsNonSetUpCardWithInvalidCount(NormalCard card, int count)
    {
        return !card.Subtypes.Contains("SetUp") && count > 3;
    }
}
