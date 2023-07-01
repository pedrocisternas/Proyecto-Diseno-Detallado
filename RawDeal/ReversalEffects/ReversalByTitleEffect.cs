using RawDealView;

namespace RawDeal;

public class ReversalByTitleEffect : ReversalEffect
{
    private readonly string _targetTitle;

    public ReversalByTitleEffect(string targetTitle)
    {
        _targetTitle = targetTitle;
    }

    public override bool CanReverse(NormalCard cardToReverse, string playedAs)
    {
        return cardToReverse.Title == _targetTitle;
    }

    public override void ApplyEffect(NormalCard reversalCard, Player activePlayer, 
        Player opponentPlayer, View view)
    {
    }
}