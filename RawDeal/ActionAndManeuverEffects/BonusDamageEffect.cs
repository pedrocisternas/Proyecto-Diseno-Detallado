using RawDealView;

namespace RawDeal;

public class BonusDamageEffect : IActionAndManeuverEffect
{
    private readonly string _applyTo;
    private readonly string _value;
    private readonly int _quantity;

    public BonusDamageEffect(string applyTo, string value, int quantity)
    {
        _applyTo = applyTo;
        _value = value;
        _quantity = quantity;
    }
    
    public void ApplyEffect(NormalCard selectedCard, Player playingPlayer, Player otherPlayer, 
        View view)
    {
        playingPlayer.RemoveCardFromHand(selectedCard);
        playingPlayer.AddCardToRingArea(selectedCard);
        playingPlayer.AddBonus(new Bonus(_applyTo, _value, "Damage", 
            _quantity, "Next"));
    }
}