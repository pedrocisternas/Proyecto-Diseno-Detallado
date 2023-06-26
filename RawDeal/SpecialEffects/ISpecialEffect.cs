namespace RawDeal;

public interface ISpecialEffect
{
    bool CanApply(string playedAs);
    NormalCard Apply(NormalCard card);
}