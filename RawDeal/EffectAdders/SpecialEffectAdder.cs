namespace RawDeal;

public class SpecialEffectAdder
{
    private readonly NormalCard _card;
    private readonly List<ISpecialEffect> _specialEffects;
    
    public SpecialEffectAdder(NormalCard card)
    {
        _card = card;
        _specialEffects = card.SpecialEffects;
    }

    public void AddSpecialEffects()
    {
        switch (_card.Title)
        {
            case "Undertaker's Tombstone Piledriver":
                _specialEffects.Add(new UndertakerSpecialEffect());
                break;
        }
    }
}