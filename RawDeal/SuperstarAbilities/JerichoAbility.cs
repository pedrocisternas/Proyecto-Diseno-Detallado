using RawDealView;

namespace RawDeal;

public class JerichoAbility : BaseSuperstarAbility
{
    public override void Execute(Player activePlayer, Player opponentPlayer, View view)
    {
        AnnounceAbilityUsage(activePlayer, view);
        
        DiscardCardFromHand(activePlayer, view, activePlayer.GetSuperstarName());
        DiscardCardFromHand(opponentPlayer, view, opponentPlayer.GetSuperstarName());
    }

    private void AnnounceAbilityUsage(Player activePlayer, View view)
    {
        view.SayThatPlayerIsGoingToUseHisAbility(activePlayer.GetSuperstarName(),
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
