using RawDealView;

namespace RawDeal;

public abstract class ReversalEffect
{
    public abstract bool CanReverse(NormalCard cardToReverse, string playedAs);
    public abstract void ApplyEffect(NormalCard reversalCard, Player activePlayer, Player opponentPlayer, View view);
}