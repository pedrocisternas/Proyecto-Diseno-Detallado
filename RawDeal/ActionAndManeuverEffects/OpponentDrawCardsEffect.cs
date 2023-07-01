using RawDealView;

namespace RawDeal;

public class OpponentDrawCardsEffect : IActionAndManeuverEffect
{
    public void ApplyEffect(NormalCard selectedCard, Player playingPlayer, Player otherPlayer, 
        View view)
    {
        int cardsInDeck = otherPlayer.GetDeckCount();
        int cardsToDraw = 1 > cardsInDeck ? cardsInDeck : 1;
        if (cardsToDraw > 0)
        {
            NormalCard card = otherPlayer.RemoveTopCardFromDeck();
            otherPlayer.AddCardToHand(card);
        }
        view.SayThatPlayerDrawCards(otherPlayer.GetSuperstarName(), cardsToDraw);
    }
}