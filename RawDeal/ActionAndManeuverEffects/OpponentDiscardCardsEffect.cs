using RawDealView;
using RawDealView.Formatters;

namespace RawDeal;

public class OpponentDiscardCardsEffect : IActionAndManeuverEffect
{
    private readonly int _numberOfCards;
    
    public OpponentDiscardCardsEffect(int numberOfCards)
    {
        _numberOfCards = numberOfCards;
    }

    public void ApplyEffect(NormalCard selectedCard, Player playingPlayer, Player otherPlayer, View view)
    {
        for (int i = 0; i < _numberOfCards; i++)
        {
            if (otherPlayer.GetHandCards().Count > 0)
            {
                DiscardCardFromHand(otherPlayer, view, _numberOfCards - i);
            }
        }
    }

    private void DiscardCardFromHand(Player otherPlayer, View view, int remainingCardsToDiscard)
    {
        List<NormalCard> cardsThatMightBeDiscarded = otherPlayer.GetHandCards();
        List<string> cardsString = FormatCards(cardsThatMightBeDiscarded);

        int chosenCardIndex = view.AskPlayerToSelectACardToDiscard(cardsString, 
            otherPlayer.SuperstarCard.Name, 
            otherPlayer.SuperstarCard.Name, remainingCardsToDiscard);
        
        otherPlayer.DiscardCard(cardsThatMightBeDiscarded[chosenCardIndex]);
    }

    private List<string> FormatCards(List<NormalCard> cards)
    {
        return cards.Select(card =>
        {
            IViewableCardInfo cardInfo = card;
            return Formatter.CardToString(cardInfo);
        }).ToList();
    }
}