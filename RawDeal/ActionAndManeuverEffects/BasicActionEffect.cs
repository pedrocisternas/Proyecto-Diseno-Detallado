using RawDealView;

namespace RawDeal;

public class BasicActionEffect : IActionAndManeuverEffect
{
    public void ApplyEffect(NormalCard selectedCard, Player playingPlayer, Player otherPlayer, 
        View view)
    {
        playingPlayer.RemoveCardFromHand(selectedCard);
        playingPlayer.AddCardToRingArea(selectedCard);
    }
}