using RawDealView;

namespace RawDeal;

public class ReversalIrishWhipEffect : ReversalEffect
{
    private readonly string _targetTitle;

    public ReversalIrishWhipEffect(string targetTitle)
    {
        _targetTitle = targetTitle;
    }
    public override bool CanReverse(NormalCard cardToReverse, string playedAs)
    {
        return cardToReverse.Title == _targetTitle;
    }
    public override void ApplyEffect(NormalCard selectedCard, Player playingPlayer, 
        Player otherPlayer, View view)
    {
        otherPlayer.RemoveCardFromHand(selectedCard);
        otherPlayer.AddCardToRingArea(selectedCard);
        otherPlayer.AddBonus(new Bonus("Subtype", "Strike", "Damage", 5, "Next"));
    }
}
