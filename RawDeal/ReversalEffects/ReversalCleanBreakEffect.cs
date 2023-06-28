using RawDealView;
using RawDealView.Formatters;

namespace RawDeal;

public class ReversalCleanBreakEffect : ReversalEffect
{
    private readonly string _targetTitle;

    public ReversalCleanBreakEffect(string targetTitle)
    {
        _targetTitle = targetTitle;
    }

    public override bool CanReverse(NormalCard cardToReverse, string playedAs)
    {
        return cardToReverse.Title == _targetTitle;
    }
    
    public override void ApplyEffect(NormalCard reversalCard, Player activePlayer, Player opponentPlayer, View view)
    {
        activePlayer.DiscardCardsFromHand(4, view);
        opponentPlayer.DrawCards();

        view.SayThatPlayerDrawCards(opponentPlayer.GetSuperstarName(), 1);
    }
}
