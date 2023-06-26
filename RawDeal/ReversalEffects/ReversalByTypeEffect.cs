using RawDealView;

namespace RawDeal;

public class ReversalByTypeEffect : ReversalEffect
{
    private readonly string _targetType;

    public ReversalByTypeEffect(string targetType)
    {
        _targetType = targetType;
    }

    public override bool CanReverse(NormalCard cardToReverse, string playedAs)
    {
        return playedAs == _targetType;
    }

    public override void ApplyEffect(NormalCard reversalCard, Player activePlayer, Player opponentPlayer, View view)
    {
    }
}