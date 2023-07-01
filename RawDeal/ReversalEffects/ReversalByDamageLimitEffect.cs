using RawDealView;

namespace RawDeal;

public class ReversalByDamageLimitEffect : ReversalEffect
{
    private readonly int _damageLimit;

    public ReversalByDamageLimitEffect(int damageLimit)
    {
        _damageLimit = damageLimit;
    }

    public override bool CanReverse(NormalCard cardToReverse, string playedAs)
    {
        return int.Parse(cardToReverse.Damage) <= _damageLimit;
    }
    
    public override void ApplyEffect(NormalCard reversalCard, Player activePlayer, 
        Player opponentPlayer, View view)
    {
    }
}