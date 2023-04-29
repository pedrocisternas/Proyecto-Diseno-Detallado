using RawDealView;

namespace RawDeal
{
    public class UndertakerAbility : BaseSuperstarAbility
    {
        public override void Execute(Player activePlayer, Player opponentPlayer, View view)
        {
            view.SayThatPlayerIsGoingToUseHisAbility(activePlayer.SuperstarCard.Name, activePlayer.SuperstarCard.SuperstarAbility);

            for (int i = 0; i < 2; i++)
            {
                List<string> formattedCards = CardUtils.GetFormattedCards(activePlayer.GetHandCards());
                int chosenCardId = view.AskPlayerToSelectACardToDiscard(formattedCards, activePlayer.SuperstarCard.Name, activePlayer.SuperstarCard.Name, 2 - i);
                NormalCard selectedCard = activePlayer.GetACardFromHand(chosenCardId);
                activePlayer.RemoveCardFromHand(selectedCard);
                activePlayer.AddCardToRingside(selectedCard);
            }

            List<string> formattedRingsideCards = CardUtils.GetFormattedCards(activePlayer.GetRingsideCards());
            int chosenRingsideCardId = view.AskPlayerToSelectCardsToPutInHisHand(activePlayer.SuperstarCard.Name, 1, formattedRingsideCards);
            NormalCard selectedRingsideCard = activePlayer.GetACardFromRingside(chosenRingsideCardId);
            activePlayer.RemoveCardFromRingside(selectedRingsideCard);
            activePlayer.AddCardToHand(selectedRingsideCard);
        }

        public override bool CanUseAbility(Player activePlayer, Player opponentPlayer)
        {
            return activePlayer.GetHandCount() >= 2;
        }
    }

    public class JerichoAbility : BaseSuperstarAbility
    {
        public override void Execute(Player activePlayer, Player opponentPlayer, View view)
        {
            view.SayThatPlayerIsGoingToUseHisAbility(activePlayer.SuperstarCard.Name, activePlayer.SuperstarCard.SuperstarAbility);

            List<string> formattedActivePlayerCards = CardUtils.GetFormattedCards(activePlayer.GetHandCards());
            int chosenActivePlayerCardId = view.AskPlayerToSelectACardToDiscard(formattedActivePlayerCards, activePlayer.SuperstarCard.Name, activePlayer.SuperstarCard.Name, 1);
            NormalCard selectedActivePlayerCard = activePlayer.GetACardFromHand(chosenActivePlayerCardId);
            activePlayer.RemoveCardFromHand(selectedActivePlayerCard);
            activePlayer.AddCardToRingside(selectedActivePlayerCard);

            List<string> formattedOpponentPlayerCards = CardUtils.GetFormattedCards(opponentPlayer.GetHandCards());
            int chosenOpponentPlayerCardId = view.AskPlayerToSelectACardToDiscard(formattedOpponentPlayerCards, opponentPlayer.SuperstarCard.Name, opponentPlayer.SuperstarCard.Name, 1);
            NormalCard selectedOpponentPlayerCard = opponentPlayer.GetACardFromHand(chosenOpponentPlayerCardId);
            opponentPlayer.RemoveCardFromHand(selectedOpponentPlayerCard);
            opponentPlayer.AddCardToRingside(selectedOpponentPlayerCard);
        }

        public override bool CanUseAbility(Player activePlayer, Player opponentPlayer)
        {
            return activePlayer.GetHandCount() >= 1;
        }
    }

    public class StoneColdSteveAustinAbility : BaseSuperstarAbility
    {
        public override void Execute(Player activePlayer, Player opponentPlayer, View view)
        {
            view.SayThatPlayerIsGoingToUseHisAbility(activePlayer.SuperstarCard.Name, activePlayer.SuperstarCard.SuperstarAbility);

            if (activePlayer.GetHandCount() > 0)
            {
                NormalCard drawnCard = activePlayer.RemoveTopCardFromDeck();
                activePlayer.AddCardToHand(drawnCard);
                view.SayThatPlayerDrawCards(activePlayer.SuperstarCard.Name, 1);
            }

            List<string> formattedCardsInHand = CardUtils.GetFormattedCards(activePlayer.GetHandCards());
            int chosenCardId = view.AskPlayerToReturnOneCardFromHisHandToHisArsenal(activePlayer.SuperstarCard.Name, formattedCardsInHand);
            NormalCard selectedCard = activePlayer.GetACardFromHand(chosenCardId);
            activePlayer.RemoveCardFromHand(selectedCard);
            activePlayer.AddCardToDeck(selectedCard);
        }

        public override bool CanUseAbility(Player activePlayer, Player opponentPlayer)
        {
            return activePlayer.GetDeckCount() > 0;
        }
    }

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

    public class TheRockAbility : BaseSuperstarAbility
    {
        public override void Execute(Player activePlayer, Player opponentPlayer, View view)
        {
           if (view.DoesPlayerWantToUseHisAbility(activePlayer.SuperstarCard.Name))
           {
               List<string> formattedCards = CardUtils.GetFormattedCards(activePlayer.GetRingsideCards());
               int chosenCardId = view.AskPlayerToSelectCardsToRecover(activePlayer.SuperstarCard.Name, 1, formattedCards);
               NormalCard selectedCard = activePlayer.GetACardFromRingside(chosenCardId);
               // Este fue el que cambié por RemoveCardFromRingsideByIndex()
               activePlayer.RemoveCardFromRingside(selectedCard);
               activePlayer.AddCardToDeck(selectedCard);
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

    public class NoAbility : BaseSuperstarAbility
    {
    }
}