using RawDealView;

namespace RawDeal;

public class CannotBeReversedEffect : IActionAndManeuverEffect
{
    public CannotBeReversedEffect(NormalCard card)
    {
        card.CanBeReversed = false;
    }
    
    public void ApplyEffect(NormalCard selectedCard, Player playingPlayer, Player otherPlayer, View view)
    {
    }
}