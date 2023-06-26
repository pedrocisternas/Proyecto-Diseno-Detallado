namespace RawDeal;

public class UndertakerSpecialEffect : ISpecialEffect
{
    public bool CanApply(string playedAs)
    {
        return playedAs == "Action";
    }
    
    public NormalCard Apply(NormalCard card)
    {
        var clonedCard = card.Clone();
        clonedCard.Fortitude = (int.Parse(card.Fortitude) - 30).ToString();
        clonedCard.Damage = (int.Parse(card.Damage) - 25).ToString();
        
        return clonedCard;
    }
}