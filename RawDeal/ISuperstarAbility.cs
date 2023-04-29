using RawDealView;
namespace RawDeal;

public interface ISuperstarAbility
{
    void Execute(Player activePlayer, Player opponentPlayer, View view);
    bool CanUseAbility(Player activePlayer, Player opponentPlayer);
    int ModifyIncomingDamage(int damage);
    int GetNumberOfCardsToDraw(Player player);
    bool IsAutomatic();
}