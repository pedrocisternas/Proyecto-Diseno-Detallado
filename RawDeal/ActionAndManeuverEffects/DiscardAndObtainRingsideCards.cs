using RawDealView;
using RawDealView.Formatters;

namespace RawDeal;

public class DiscardAndObtainRingsideCards : IActionAndManeuverEffect
{
    public void ApplyEffect(NormalCard selectedCard, Player playingPlayer, Player otherPlayer, View view)
    {
        var numberOfCardsToDiscard = AskHowManyCards(playingPlayer, view);
        playingPlayer.DiscardCardsFromHand(numberOfCardsToDiscard, view);
        PassCardsFromRingsideToHand(playingPlayer, numberOfCardsToDiscard, view);
    }

    private int AskHowManyCards(Player playingPlayer, View view)
    {
        var numberOfCardsInHand = playingPlayer.GetDeckCount();
        if (numberOfCardsInHand is > 0 and <= 2)
        {
            return view.AskHowManyCardsToDiscard(playingPlayer.GetSuperstarName(), numberOfCardsInHand);
        }

        return numberOfCardsInHand > 2 ? 
            view.AskHowManyCardsToDiscard(playingPlayer.GetSuperstarName(), 2) : 0;
    }
    
    public void PassCardsFromRingsideToHand(Player playingPlayer, int cardsToDiscard, View view)
    {
        for (int i = 0; i < cardsToDiscard; i++)
        {
            List<NormalCard> cardsThatMightBeAdded = playingPlayer.GetRingsideCards();
            List<string> cardsString = cardsThatMightBeAdded.Select(card =>
                Formatter.CardToString(card)).ToList();

            int chosenCardIndex = view.AskPlayerToSelectCardsToPutInHisHand(
                playingPlayer.GetSuperstarName(),
                cardsToDiscard - i, cardsString);
            playingPlayer.RemoveCardFromRingside(cardsThatMightBeAdded[chosenCardIndex]);
            playingPlayer.AddCardToHand(cardsThatMightBeAdded[chosenCardIndex]);
        }
    }
}