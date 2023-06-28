using RawDealView;

namespace RawDeal;

public class StoneColdSteveAustinAbility : BaseSuperstarAbility
{
    public override void Execute(Player activePlayer, Player opponentPlayer, View view)
    {
        AnnounceAbilityUsage(activePlayer, view);
        
        if (activePlayer.GetHandCount() > 0)
        {
            DrawCardFromDeck(activePlayer, view);
        }

        ReturnCardFromHandToDeck(activePlayer, view);
    }
    
    private void AnnounceAbilityUsage(Player activePlayer, View view)
    {
        view.SayThatPlayerIsGoingToUseHisAbility(activePlayer.GetSuperstarName(), 
            activePlayer.SuperstarCard.SuperstarAbility);
    }

    private void DrawCardFromDeck(Player activePlayer, View view)
    {
        NormalCard drawnCard = activePlayer.RemoveTopCardFromDeck();
        activePlayer.AddCardToHand(drawnCard);
        view.SayThatPlayerDrawCards(activePlayer.GetSuperstarName(), 1);
    }

    private void ReturnCardFromHandToDeck(Player activePlayer, View view)
    {
        List<string> formattedCardsInHand = CardUtils.GetFormattedCards(activePlayer.GetHandCards());
        int chosenCardId = 
            view.AskPlayerToReturnOneCardFromHisHandToHisArsenal(
                activePlayer.GetSuperstarName(), formattedCardsInHand);
        NormalCard selectedCard = activePlayer.GetACardFromHand(chosenCardId);
        MoveCardFromHandToDeck(activePlayer, selectedCard);
    }

    public override bool CanUseAbility(Player activePlayer, Player opponentPlayer)
    {
        return activePlayer.GetDeckCount() > 0;
    }
}
