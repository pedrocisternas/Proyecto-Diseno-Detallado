namespace RawDeal;
using RawDealView;

public abstract class BaseSuperstarAbility : ISuperstarAbility
{
    public virtual void Execute(Player activePlayer, Player opponentPlayer, View view) { }

    public virtual bool CanUseAbility(Player activePlayer, Player opponentPlayer)
    {
        return false;
    }

    public virtual int ModifyIncomingDamage(int damage)
    {
        return damage;
    }

    public virtual int GetNumberOfCardsToDraw(Player player)
    {
        return 1;
    }

    public virtual bool IsAutomatic()
    {
        return false;
    }
}
