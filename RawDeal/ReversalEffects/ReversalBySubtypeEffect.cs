using RawDealView;

namespace RawDeal;

public class ReversalBySubtypeEffect : ReversalEffect
{
    private readonly string _targetType;
    private readonly string _targetSubtype;

    public ReversalBySubtypeEffect(string targetType, string targetSubtype)
    {
        _targetType = targetType;
        _targetSubtype = targetSubtype;
    }

    public override bool CanReverse(NormalCard cardToReverse, string playedAs)
    {
        return playedAs == _targetType && cardToReverse.Subtypes.Contains(_targetSubtype);
    }
    
    public override void ApplyEffect(NormalCard reversalCard, Player activePlayer, 
        Player opponentPlayer, View view)
    {
    }
}