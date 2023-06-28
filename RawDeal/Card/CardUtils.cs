using RawDealView;
using RawDealView.Formatters;

namespace RawDeal;

public static class CardUtils
{
    public static List<string> GetFormattedCards(List<NormalCard> cards)
    {
        return cards.Select(card => 
        {
            IViewableCardInfo cardInfo = card;
            return Formatter.CardToString(cardInfo);
        }).ToList();
    }

    public static List<(NormalCard card, string type)> GetPlayableCards(Player player)
    {
        var playableCards = new List<(NormalCard, string)>();
        foreach (var card in player.GetHandCards())
        {
            foreach (var type in card.Types)
            {
                if (TryAddModifiedPlayableCards(player, card, type, playableCards)) continue;

                TryAddNormalPlayableCard(player, card, type, playableCards);
            }
        }

        return playableCards;
    }

    private static bool TryAddModifiedPlayableCards(Player player, NormalCard card, string type, List<(NormalCard, string)> playableCards)
    {
        bool canPlayModified = false;
        foreach (var effect in card.SpecialEffects)
        {
            if (!effect.CanApply(type)) continue;

            var clonedCard = effect.Apply(card);
            if (CanPlayAsActionOrManeuver(type) && CanPlayCard(player, clonedCard))
            {
                playableCards.Add((card, type));
                canPlayModified = true;
            }
        }
        return canPlayModified;
    }

    private static void TryAddNormalPlayableCard(Player player, NormalCard card, string type, List<(NormalCard, string)> playableCards)
    {
        if (CanPlayAsActionOrManeuver(type) && CanPlayCard(player, card))
        {
            playableCards.Add((card, type));
        }
    }

    private static bool CanPlayAsActionOrManeuver(string type)
    {
        return type == "Action" || type == "Maneuver";
    }

    private static bool CanPlayCard(Player player, NormalCard card)
    {
        return int.Parse(card.Fortitude) <= player.GetFortitude() && CanPlayCard(card, player);
    }

    public static List<NormalCard> GetReversalCards(Player player, NormalCard cardToReverse, string playedAs)
    {
        int additionalFortitude = player.CalculateAdditionalFortitude(cardToReverse);
    
        var affordableCards = 
            GetAffordableCards(player, additionalFortitude);
        var applicableReversalCards = 
            GetApplicableReversalCards(affordableCards, cardToReverse, playedAs);

        return applicableReversalCards;
    }

    public static int SelectCardToPlay(List<(NormalCard card, string type)> playableCardsWithType, View view)
    {
        List<string> playableCardStrings = FormatCards(playableCardsWithType);

        int chosenCardIndex = view.AskUserToSelectAPlay(playableCardStrings);

        return chosenCardIndex;
    }
    
    public static List<string> FormatReversalCards(List<NormalCard> reversalCards)
    {
        List<(NormalCard card, string type)> reversalCardsWithType = reversalCards.Select(card => (card, "REVERSAL")).ToList();
        return FormatCards(reversalCardsWithType);
    }

    private static List<string> FormatCards(List<(NormalCard card, string type)> cardsWithType)
    {
        return cardsWithType.Select(cardWithType =>
        {
            IViewableCardInfo cardInfo = cardWithType.card;
            string playedAs = cardWithType.type.ToUpper();
            PlayInfo playInfo = new PlayInfo(cardInfo, playedAs);
            return Formatter.PlayToString(playInfo);
        }).ToList();
    }
    
    private static List<NormalCard> GetAffordableCards(Player player, int additionalFortitude)
    {
        var handCards = player.GetHandCards();

        return handCards
            .Where(card => int.Parse(card.Fortitude) + additionalFortitude <= player.GetFortitude())
            .ToList();
    }

    private static List<NormalCard> GetApplicableReversalCards(List<NormalCard> affordableCards, 
        NormalCard cardToReverse, string playedAs)
    {
        return affordableCards
            .Where(card => card.Types.Contains("Reversal") &&
                           card.CanReverse(cardToReverse, playedAs))
            .ToList();
    }
    
    private static bool CanPlayCard(NormalCard selectedCard, Player player)
    {
        if (selectedCard.PlayConditions.Count == 0)
        {
            return true;
        }

        return selectedCard.PlayConditions.All(condition => condition.CanPlayCard(player));
    }
}
