using RawDealView;

namespace RawDeal;

public class PlayerCanDrawCardsEffect : IActionAndManeuverEffect
{
    private readonly int _numberOfCards;
    
    public PlayerCanDrawCardsEffect(int numberOfCards)
    {
        _numberOfCards = numberOfCards;
    }
    
    public void ApplyEffect(NormalCard selectedCard, Player playingPlayer, Player otherPlayer, 
        View view)
    {
        int cardsWantedToDraw = 
            view.AskHowManyCardsToDrawBecauseOfACardEffect(playingPlayer.
                GetSuperstarName(), _numberOfCards);
        int cardsInDeck = playingPlayer.GetDeckCount();
        int cardsToDraw = Math.Min(cardsInDeck, cardsWantedToDraw);

        if (cardsToDraw > 0)
        {
            DrawCards(playingPlayer, cardsToDraw);
        }
        view.SayThatPlayerDrawCards(playingPlayer.GetSuperstarName(), cardsToDraw);
    }

    private void DrawCards(Player player, int cardsToDraw)
    {
        for (int i = 0; i < cardsToDraw; i++)
        {
            NormalCard card = player.RemoveTopCardFromDeck();
            player.AddCardToHand(card);
        }
    }
}
