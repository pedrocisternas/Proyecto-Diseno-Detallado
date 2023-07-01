using RawDealView;

namespace RawDeal;

public class PlayerDiscardCardsEffect : IActionAndManeuverEffect
{
    public void ApplyEffect(NormalCard selectedCard, Player playingPlayer, Player otherPlayer, 
        View view)
    {
        if (playingPlayer.GetHandCards().Count > 0)
        {
            playingPlayer.DiscardCardsFromHand(1, view);
        }
    }
}