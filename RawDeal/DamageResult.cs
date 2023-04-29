namespace RawDeal;

public class DamageResult
{
    public List<NormalCard> OverturnedCards { get; set; }
    public int AppliedDamage { get; set; }

    public DamageResult(List<NormalCard> overturnedCards, int appliedDamage)
    {
        OverturnedCards = overturnedCards;
        AppliedDamage = appliedDamage;
    }
}