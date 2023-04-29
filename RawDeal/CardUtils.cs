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
        var playableCardsWithType = player.GetHandCards()
            .Where(card => int.Parse(card.Fortitude) <= player.GetFortitude())
            .SelectMany(card => card.Types
                .Where(type => type == "Action" || type == "Maneuver")
                .Select(type => (card, type)))
            .ToList();

        return playableCardsWithType;
    }
}
