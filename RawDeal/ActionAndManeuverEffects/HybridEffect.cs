using RawDealView;

namespace RawDeal;

public class HybridEffect : IActionAndManeuverEffect
{
    public void ApplyEffect(NormalCard selectedCard, Player playingPlayer, Player otherPlayer, 
        View view)
    {
        playingPlayer.DiscardCard(selectedCard);
        playingPlayer.DrawCards();
        view.SayThatPlayerMustDiscardThisCard(playingPlayer.GetSuperstarName(), selectedCard.Title);
        view.SayThatPlayerDrawCards(playingPlayer.GetSuperstarName(), 1);
    }
}