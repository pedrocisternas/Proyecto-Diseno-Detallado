using RawDealView;

namespace RawDeal;

public class RecoverRingsideCardsEffect : IActionAndManeuverEffect
{
    private readonly int _numberOfRingsideCards;
    private readonly int _numberOfCardsToDraw;
    
    public RecoverRingsideCardsEffect(int numberOfRingsideCards, int numberOfCardsToDraw)
    {
        _numberOfRingsideCards = numberOfRingsideCards;
        _numberOfCardsToDraw = numberOfCardsToDraw;
    }
    
    public void ApplyEffect(NormalCard selectedCard, Player playingPlayer, Player otherPlayer, View view)
    {
        MoveCardsFromRingsideToDeck(playingPlayer, view);
        if (_numberOfCardsToDraw > 0)
        {
            playingPlayer.DrawCards(_numberOfCardsToDraw);
            view.SayThatPlayerDrawCards(playingPlayer.SuperstarCard.Name, _numberOfCardsToDraw);
        }
    }

    private void MoveCardsFromRingsideToDeck(Player playingPlayer, View view)
    {
        for (int i = 0; i < _numberOfRingsideCards; i++)
        {
            List<string> formattedCards = CardUtils.GetFormattedCards(playingPlayer.GetRingsideCards());
            int chosenCardId = 
                view.AskPlayerToSelectCardsToRecover(playingPlayer.SuperstarCard.Name, 
                    _numberOfRingsideCards - i, formattedCards);
            NormalCard card = playingPlayer.GetACardFromRingside(chosenCardId);
            playingPlayer.RemoveCardFromRingside(card);
            playingPlayer.AddCardToDeck(card);
        }
    }
}