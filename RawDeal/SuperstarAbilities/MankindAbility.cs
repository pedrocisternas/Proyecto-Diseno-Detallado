namespace RawDeal;

public class MankindAbility : BaseSuperstarAbility
{
    public override int ModifyIncomingDamage(int damage)
    {
        return damage - 1;
    }

    public override int GetNumberOfCardsToDraw(Player player)
    {
        return player.GetDeckCount() > 1 ? 2 : 1;
    }
}