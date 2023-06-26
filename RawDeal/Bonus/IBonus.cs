namespace RawDeal;

public interface IBonus
{
    string ApplyTo { get; }
    string Value { get; }
    bool Active { get; set; }
    string BonusOn { get; set; }
    int Quantity { get; set; }
    string Duration { get; set; }
}