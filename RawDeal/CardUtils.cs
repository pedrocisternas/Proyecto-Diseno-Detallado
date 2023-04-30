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

    // La saqué por que tenía un train wreck. Revisar nuevamente a ver si se puede mejorar
    // public static List<(NormalCard card, string type)> GetPlayableCards(Player player)
    // {
    //     var playableCardsWithType = player.GetHandCards()
    //         .Where(card => int.Parse(card.Fortitude) <= player.GetFortitude())
    //         .SelectMany(card => card.Types
    //             .Where(type => type == "Action" || type == "Maneuver")
    //             .Select(type => (card, type)))
    //         .ToList();
    //
    //     return playableCardsWithType;
    // }
    
    public static List<(NormalCard card, string type)> GetPlayableCards(Player player)
    {
        var handCards = player.GetHandCards();
        var affordableCards = handCards.Where(card => int.Parse(card.Fortitude) <= player.GetFortitude()).ToList();

        var playableCardsWithType = new List<(NormalCard card, string type)>();

        foreach (var card in affordableCards)
        {
            var validTypes = card.Types.Where(type => type == "Action" || type == "Maneuver").ToList();

            foreach (var type in validTypes)
            {
                playableCardsWithType.Add((card, type));
            }
        }

        return playableCardsWithType;
    }
    
    public static List<NormalCard> GetReversalCards(Player player, NormalCard cardToReverse)
    {
        var handCards = player.GetHandCards();
        var affordableCards = handCards.Where(card => int.Parse(card.Fortitude) <= player.GetFortitude()).ToList();

        var applicableReversalCards = affordableCards.Where(card => card.Types.Contains("Reversal") && CanReverse(card, cardToReverse)).ToList();

        return applicableReversalCards;
    }

    private static bool CanReverse(NormalCard reversalCard, NormalCard cardToReverse)
    {
        foreach (ReversalEffect effect in reversalCard.ReversalEffects)
        {
            if (effect.CanReverse(reversalCard, cardToReverse))
            {
                return true;
            }
        }
        
        return false;
    }
}
