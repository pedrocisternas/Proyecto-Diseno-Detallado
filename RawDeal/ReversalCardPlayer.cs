using RawDealView;
using RawDealView.Formatters;

namespace RawDeal;

public class ReversalCardPlayer
{
    private readonly Player _activePlayer;
    private readonly Player _opponentPlayer;
    private readonly View _view;
    private readonly GamePresenter _gamePresenter;
    private readonly PlayerManager _playerManager;

    public ReversalCardPlayer(PlayerManager playerManager, View view)
    {
        _activePlayer = playerManager.GetActivePlayer();
        _opponentPlayer = playerManager.GetOpponentPlayer();
        _playerManager = playerManager;
        _view = view;
        _gamePresenter = new GamePresenter(_view);
    }

    public NormalCard TryOpponentReversal(NormalCard playedCard, string playedAs)
    {
        List<NormalCard> reversalCards = 
            CardUtils.GetReversalCards(_opponentPlayer, playedCard, playedAs);
        int chosenCardIndex = SelectReversalCardToPlay(reversalCards);
        return chosenCardIndex != -1 ? reversalCards[chosenCardIndex] : new NullCard();
    }
    
    private int SelectReversalCardToPlay(List<NormalCard> reversalCards)
    {
        int chosenCardIndex = 
            _view.AskUserToSelectAReversal(_opponentPlayer.GetSuperstarName(),
                CardUtils.FormatReversalCards(reversalCards));
        return chosenCardIndex;
    }
    
    public void HandleReversal(NormalCard reversalCard, 
        NormalCard reversedCard, NormalCard modifiedReversedCard)
    {
        _gamePresenter.DisplayPlayerIsTryingToPlayCard(reversalCard, 
            "Reversal", _activePlayer, _opponentPlayer);
        _playerManager.ResetCardEffectState();
        MoveCardsInReversal(reversalCard, reversedCard);
        ApplyReversalEffects(reversalCard);
        
        HandleReversalDamage(reversalCard, modifiedReversedCard);
    }

    private void ApplyReversalEffects(NormalCard reversalCard)
    {
        if (reversalCard.ReversalEffects.Count <= 0) return;
        foreach (var effect in reversalCard.ReversalEffects)
        {
            effect.ApplyEffect(reversalCard, _activePlayer, _opponentPlayer, _view);
        }
    }

    private void MoveCardsInReversal(NormalCard reversalCard, NormalCard reversedCard)
    {
        _activePlayer.RemoveCardFromHand(reversedCard);
        _activePlayer.AddCardToRingside(reversedCard);
        _opponentPlayer.RemoveCardFromHand(reversalCard);
        _opponentPlayer.AddCardToRingArea(reversalCard);
    }

    private void HandleReversalDamage(NormalCard reversalCard, NormalCard modifiedReversedCard)
    {
        var finalReversalDamage = CheckForSpecialDamage(reversalCard, modifiedReversedCard);
        DamageResult finalDamage = 
            _activePlayer.ReceiveDirectDamage(int.Parse(finalReversalDamage), true);
        _gamePresenter.ShowAppliedDamage(finalDamage, _activePlayer);
    }

    private string CheckForSpecialDamage(NormalCard reversalCard, NormalCard modifiedReversedCard)
    {
        string finalReversalDamage = reversalCard.Damage;
        if (reversalCard.Damage == "#")
        {
            _opponentPlayer.IncreaseFortitude(0);
            finalReversalDamage = _opponentPlayer.SuperstarCard.Ability
                .ModifyIncomingDamage(int.Parse(modifiedReversedCard.Damage)).ToString();
        }
        else
        {
            _opponentPlayer.IncreaseFortitude(int.Parse(finalReversalDamage));
        }

        return finalReversalDamage;
    }

    public void ReverseExecute(NormalCard selectedCard, DamageResult damageResult)
    {
        _view.SayThatCardWasReversedByDeck(_opponentPlayer.GetSuperstarName());
        if (int.Parse(selectedCard.StunValue) > 0 && 
            damageResult.OverturnedCards.Count != damageResult.AppliedDamage)
        {
            StunValueAction(selectedCard);
        }
    }
    
    private void StunValueAction(NormalCard selectedCard)
    {
        int numberOfCardsSelected = 
            _view.AskHowManyCardsToDrawBecauseOfStunValue(_activePlayer.GetSuperstarName(),
            int.Parse(selectedCard.StunValue));
        _activePlayer.DrawCards(numberOfCardsSelected);
        if (numberOfCardsSelected > 0) 
            _view.SayThatPlayerDrawCards(_activePlayer.GetSuperstarName(), numberOfCardsSelected);
    }
}
