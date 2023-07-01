namespace RawDeal;

public class DamageResult
{
    public List<NormalCard> OverturnedCards { get; }
    public int AppliedDamage { get; }
    public NormalCard ReversalCard { get; }

    public DamageResult(List<NormalCard> overturnedCards, int appliedDamage, 
        NormalCard reversalCard)
    {
        OverturnedCards = overturnedCards;
        AppliedDamage = appliedDamage;
        ReversalCard = reversalCard;
    }
    
    public bool WasReversed()
    {
        return !(ReversalCard is NullCard);
    }
}