using RawDealView;

namespace RawDeal;

public class HybridEffect : IActionAndManeuverEffect
{
    public void ApplyEffect(NormalCard selectedCard, Player playingPlayer, Player otherPlayer, View view)
    {
        playingPlayer.DiscardCard(selectedCard);
        playingPlayer.DrawCards();
        view.SayThatPlayerMustDiscardThisCard(playingPlayer.SuperstarCard.Name, selectedCard.Title);
        view.SayThatPlayerDrawCards(playingPlayer.SuperstarCard.Name, 1);
    }
}