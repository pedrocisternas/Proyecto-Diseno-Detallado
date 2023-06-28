using RawDealView;
using RawDealView.Formatters;
using RawDealView.Options;

namespace RawDeal;

public class CardPlayer
{
    private readonly View _view;
    private readonly Player _activePlayer;
    private readonly Player _opponentPlayer;
    private readonly GamePresenter _gamePresenter;
    private readonly ReversalCardPlayer _reversalManager;
    private readonly Action _endTurnCallback;
    private readonly Action<string> _endGameAndCongratulateWinner;

    public CardPlayer(View view, PlayerManager playerManager, 
        Action endTurnCallback, Action<string> endGameAndCongratulateWinner)
    {
        _view = view;
        _activePlayer = playerManager.GetActivePlayer();
        _opponentPlayer = playerManager.GetOpponentPlayer();
        _reversalManager = new ReversalCardPlayer(playerManager, _view);
        _endTurnCallback = endTurnCallback;
        _endGameAndCongratulateWinner = endGameAndCongratulateWinner;
        _gamePresenter = new GamePresenter(_view);
    }
    
    public void HandlePlayCard()
    {
        List<(NormalCard card, string type)> playableCardsWithType = 
            CardUtils.GetPlayableCards(_activePlayer);
        int chosenCardIndex = CardUtils.SelectCardToPlay(playableCardsWithType, _view);

        if (chosenCardIndex != -1)
        {
            (NormalCard selectedCard, string selectedType) = playableCardsWithType[chosenCardIndex];
            PlaySelectedCard(selectedCard, selectedType);
        }
    }

    private void PlaySelectedCard(NormalCard selectedCard, string selectedType)
    {
        _gamePresenter.DisplayPlayerIsTryingToPlayCard(selectedCard, selectedType, _activePlayer, _opponentPlayer);
        NormalCard modifiedSelectedCard = selectedCard.CloneWithModifiedDamage(_activePlayer);
        NormalCard reversalCard = 
            modifiedSelectedCard.CanBeReversed ? 
                _reversalManager.TryOpponentReversal(modifiedSelectedCard, selectedType) : new NullCard();

        if (!(reversalCard is NullCard))
        {
            _reversalManager.HandleReversal(reversalCard, selectedCard, modifiedSelectedCard);
            _activePlayer.LastPlay.FinishTurn();
            _endTurnCallback();
        }
        else
        {
            PlayAsActionOrManeuver(selectedCard, selectedType);
        }
    }

    private void PlayAsActionOrManeuver(NormalCard selectedCard, string selectedType)
    {
        _view.SayThatPlayerSuccessfullyPlayedACard();
        _activePlayer.AddCardToLastPlay(selectedCard, selectedType);
        if (selectedType == "Action")
        {
            PlayCardAsAction(selectedCard);
        }
        else if (selectedType == "Maneuver")
        {
            PlayCardAsManeuver(selectedCard);
        }
    }

    private void PlayCardAsAction(NormalCard selectedCard)
    {
        ApplyEffects(selectedCard, selectedCard.ActionEffects);
        BonusRemoval();
    }

    private void PlayCardAsManeuver(NormalCard selectedCard)
    {
        _activePlayer.RemoveCardFromHand(selectedCard);
        _activePlayer.AddCardToRingArea(selectedCard);
        NormalCard modifiedSelectedCard = selectedCard.CloneWithModifiedDamage(_activePlayer);
        
        ApplyEffects(selectedCard, selectedCard.ManeuverEffects);
        _activePlayer.IncreaseFortitude(int.Parse(selectedCard.Damage));
        
        DamageResult damageResult = 
            _opponentPlayer.ReceiveReversableDamage(modifiedSelectedCard, "Maneuver");
        _activePlayer.AddDamageToLastPlay(damageResult.AppliedDamage);
        ReceiveDamageAndOrReverse(selectedCard, damageResult);
        BonusRemoval();
    }

    private void ApplyEffects(NormalCard selectedCard, List<IActionAndManeuverEffect> effects)
    {
        foreach (var effect in effects)
        {
            effect.ApplyEffect(selectedCard, _activePlayer, _opponentPlayer, _view);
        }
    }

    private void BonusRemoval()
    {
        RemoveAndDeactivateBonuses(_activePlayer);
        RemoveAndDeactivateBonuses(_opponentPlayer);
    }

    private void RemoveAndDeactivateBonuses(Player player)
    {
        player.BonusList.RemoveAll(bonus => !bonus.Active);
        player.BonusList.ForEach(bonus => bonus.Active = false);
    }

    private void ReceiveDamageAndOrReverse(NormalCard selectedCard, DamageResult damageResult)
    {
        _gamePresenter.ShowAppliedDamage(damageResult, _opponentPlayer);
        if (damageResult.WasReversed())
        {
            _reversalManager.ReverseExecute(selectedCard, damageResult);
            _endTurnCallback();
        }
        CheckPinVictory(damageResult);
    }

    private void CheckPinVictory(DamageResult damageResult)
    {
        if (CheckConditionPinVictory(damageResult))
        {
            _endGameAndCongratulateWinner(_activePlayer.GetSuperstarName());
        }
    }

    private bool CheckConditionPinVictory(DamageResult damageResult)
    {
        return damageResult.AppliedDamage > damageResult.OverturnedCards.Count 
               && !damageResult.WasReversed() && 
               _opponentPlayer.GetDeckCount() == 0;
    }
}