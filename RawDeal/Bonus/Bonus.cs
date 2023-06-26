namespace RawDeal;

public class Bonus : IBonus
{
    public string ApplyTo { get; }
    public string Value { get; }
    public bool Active { get; set; }
    public string BonusOn { get; set; }
    public int Quantity { get; set; }
    public string Duration { get; set; }

    public Bonus(string applyTo, string value, string bonusOn, int quantity, string duration)
    {
        ApplyTo = applyTo;
        Value = value;
        Active = true;
        BonusOn = bonusOn;
        Quantity = quantity;
        Duration = duration;
    }
}
