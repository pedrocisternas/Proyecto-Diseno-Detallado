using RawDealView;

namespace RawDeal;

public class JerichoAbility : BaseSuperstarAbility
{
    public override void Execute(Player activePlayer, Player opponentPlayer, View view)
    {
        AnnounceAbilityUsage(activePlayer, view);
        
        DiscardCardFromHand(activePlayer, view, activePlayer.SuperstarCard.Name);
        DiscardCardFromHand(opponentPlayer, view, opponentPlayer.SuperstarCard.Name);
    }

    private void AnnounceAbilityUsage(Player activePlayer, View view)
    {
        view.SayThatPlayerIsGoingToUseHisAbility(activePlayer.SuperstarCard.Name,
            activePlayer.SuperstarCard.SuperstarAbility);
    }

    private void DiscardCardFromHand(Player player, View view, string playerName)
    {
        player.DiscardCardsFromHand(1, view);
    }

    public override bool CanUseAbility(Player activePlayer, Player opponentPlayer)
    {
        return activePlayer.GetHandCount() >= 1;
    }
}
