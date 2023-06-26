using RawDealView;

namespace RawDeal;

public class PlayerChooseOpponentDiscardCardsEffect : IActionAndManeuverEffect
{
    public void ApplyEffect(NormalCard selectedCard, Player playingPlayer, Player otherPlayer, View view)
    {
        if (otherPlayer.GetHandCards().Count > 0)
        {
            List<string> formattedCards = CardUtils.GetFormattedCards(otherPlayer.GetHandCards());
            int chosenCardId = 
                view.AskPlayerToSelectACardToDiscard(formattedCards, 
                    otherPlayer.SuperstarCard.Name, 
                    playingPlayer.SuperstarCard.Name, 1);
            NormalCard cardToDiscard = otherPlayer.GetACardFromHand(chosenCardId);
            otherPlayer.RemoveCardFromHand(cardToDiscard);
            otherPlayer.AddCardToRingside(cardToDiscard);
        }
    }
}