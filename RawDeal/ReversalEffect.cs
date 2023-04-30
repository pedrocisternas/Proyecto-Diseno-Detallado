namespace RawDeal;

public abstract class ReversalEffect
{
    public abstract bool CanReverse(NormalCard reversalCard, NormalCard cardToReverse);
}

public class ReversalByTypeEffect : ReversalEffect
{
    public string TargetType { get; }

    public ReversalByTypeEffect(string targetType)
    {
        TargetType = targetType;
    }

    public override bool CanReverse(NormalCard reversalCard, NormalCard cardToReverse)
    {
        return cardToReverse.Types.Contains(TargetType);
    }
}

public class ReversalBySubtypeEffect : ReversalEffect
{
    public string TargetSubtype { get; }

    public ReversalBySubtypeEffect(string targetSubtype)
    {
        TargetSubtype = targetSubtype;
    }

    public override bool CanReverse(NormalCard reversalCard, NormalCard cardToReverse)
    {
        return cardToReverse.Subtypes.Contains(TargetSubtype);
    }
}
