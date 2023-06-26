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
    
    protected void MoveCardFromHandToRingside(Player player, NormalCard card)
    {
        player.RemoveCardFromHand(card);
        player.AddCardToRingside(card);
    }

    protected void MoveCardFromRingsideToHand(Player player, NormalCard card)
    {
        player.RemoveCardFromRingside(card);
        player.AddCardToHand(card);
    }
    
    protected void MoveCardFromRingsideToDeck(Player player, NormalCard card)
    {
        player.RemoveCardFromRingside(card);
        player.AddCardToDeck(card);
    }

    protected void MoveCardFromHandToDeck(Player player, NormalCard card)
    {
        player.RemoveCardFromHand(card);
        player.AddCardToDeck(card);
    }
}
