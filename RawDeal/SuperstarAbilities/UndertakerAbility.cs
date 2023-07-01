using RawDealView;

namespace RawDeal;

public class UndertakerAbility : BaseSuperstarAbility
{
    public override void Execute(Player activePlayer, Player opponentPlayer, View view)
    {
        AnnounceAbilityUsage(activePlayer, view);
        
        DiscardCardsFromHand(activePlayer, view, 2);
        
        MoveCardFromRingsideToHand(activePlayer, view);
    }

    private void AnnounceAbilityUsage(Player activePlayer, View view)
    {
        view.SayThatPlayerIsGoingToUseHisAbility(
            activePlayer.GetSuperstarName(), activePlayer.SuperstarCard.SuperstarAbility);
    }

    private void DiscardCardsFromHand(Player activePlayer, View view, int numberOfCardsToDiscard)
    {
        for (int i = 0; i < numberOfCardsToDiscard; i++)
        {
            NormalCard selectedCard = 
                PromptPlayerToDiscardCard(activePlayer, view, numberOfCardsToDiscard - i);
            MoveCardFromHandToRingside(activePlayer, selectedCard);
        }
    }

    private NormalCard PromptPlayerToDiscardCard(Player activePlayer, 
        View view, int remainingCardsToDiscard)
    {
        List<string> formattedCards = CardUtils.GetFormattedCards(activePlayer.GetHandCards());
        int chosenCardId = 
            view.AskPlayerToSelectACardToDiscard(formattedCards, 
                activePlayer.GetSuperstarName(), 
                activePlayer.GetSuperstarName(), remainingCardsToDiscard);
        
        return activePlayer.GetACardFromHand(chosenCardId);
    }

    private void MoveCardFromRingsideToHand(Player activePlayer, View view)
    {
        List<string> formattedRingsideCards = CardUtils.
            GetFormattedCards(activePlayer.GetRingsideCards());
        int chosenRingsideCardId = 
            view.AskPlayerToSelectCardsToPutInHisHand(
                activePlayer.GetSuperstarName(), 1, formattedRingsideCards);
        NormalCard selectedRingsideCard = activePlayer.GetACardFromRingside(chosenRingsideCardId);

        MoveCardFromRingsideToHand(activePlayer, selectedRingsideCard);
    }

    public override bool CanUseAbility(Player activePlayer, Player opponentPlayer)
    {
        return activePlayer.GetHandCount() >= 2;
    }
}
