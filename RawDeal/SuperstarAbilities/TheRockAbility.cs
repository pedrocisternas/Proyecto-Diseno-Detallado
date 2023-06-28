using RawDealView;

namespace RawDeal;

public class TheRockAbility : BaseSuperstarAbility
{
    public override void Execute(Player activePlayer, Player opponentPlayer, View view)
    {
        if (view.DoesPlayerWantToUseHisAbility(activePlayer.GetSuperstarName()))
        {
            List<string> formattedCards = CardUtils.GetFormattedCards(activePlayer.GetRingsideCards());
            int chosenCardId = 
                view.AskPlayerToSelectCardsToRecover(activePlayer.GetSuperstarName(), 1, formattedCards);
            NormalCard selectedCard = activePlayer.GetACardFromRingside(chosenCardId);
            MoveCardFromRingsideToDeck(activePlayer, selectedCard);
        }
    }

    public override bool CanUseAbility(Player activePlayer, Player opponentPlayer)
    {
        return activePlayer.GetRingsideCount() > 0;
    }

    public override bool IsAutomatic()
    {
        return true;
    }
}