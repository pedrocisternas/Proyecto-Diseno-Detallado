using RawDealView;

namespace RawDeal;

public interface IActionAndManeuverEffect
{
    void ApplyEffect(NormalCard selectedCard, Player playingPlayer, Player otherPlayer, 
        View view);
}
