namespace RawDeal;

public class DamageResult
{
    public List<NormalCard> OverturnedCards { get; }
    public int AppliedDamage { get; }
    public bool WasReversed { get; }

    public DamageResult(List<NormalCard> overturnedCards, int appliedDamage, bool wasReversed)
    {
        OverturnedCards = overturnedCards;
        AppliedDamage = appliedDamage;
        WasReversed = wasReversed;
    }
}