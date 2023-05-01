namespace RawDeal;

public abstract class ReversalEffect
{
    public abstract bool CanReverse(NormalCard cardToReverse, string playedAs);
}

public class ReversalByTypeEffect : ReversalEffect
{
    public string TargetType { get; }

    public ReversalByTypeEffect(string targetType)
    {
        TargetType = targetType;
    }

    public override bool CanReverse(NormalCard cardToReverse, string playedAs)
    {
        return playedAs == TargetType;
    }
}

public class ReversalBySubtypeEffect : ReversalEffect
{
    public string TargetType { get; }
    public string TargetSubtype { get; }

    public ReversalBySubtypeEffect(string targetType, string targetSubtype)
    {
        TargetType = targetType;
        TargetSubtype = targetSubtype;
    }

    public override bool CanReverse(NormalCard cardToReverse, string playedAs)
    {
        return playedAs == TargetType && cardToReverse.Subtypes.Contains(TargetSubtype);
    }
}
