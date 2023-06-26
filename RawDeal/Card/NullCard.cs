namespace RawDeal;

public class NullCard : NormalCard
{
    public NullCard() : base("", new List<string>(),
        new List<string>(), "0", "0", "0", "")
    {
    }
    
    public override NormalCard Clone()
    {
        return this;
    }
    
    public override NormalCard CloneWithModifiedDamage(Player player)
    {
        return this;
    }

    public override bool CanReverse(NormalCard cardToReverse, string playedAs)
    {
        return false;
    }
}
