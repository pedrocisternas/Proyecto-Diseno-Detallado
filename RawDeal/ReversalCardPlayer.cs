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
        List<string> reversalCardStrings = reversalCards.Select(card =>
        {
            IViewableCardInfo cardInfo = card;
            string playedAs = "REVERSAL";
            PlayInfo playInfo = new PlayInfo(cardInfo, playedAs);
            return Formatter.PlayToString(playInfo);
        }).ToList();
        int chosenCardIndex = 
            _view.AskUserToSelectAReversal(_opponentPlayer.SuperstarCard.Name, reversalCardStrings);
        return chosenCardIndex;
    }
    
    public void HandleReversal(NormalCard reversalCard, 
        NormalCard reversedCard, NormalCard modifiedReversedCard)
    {
        _gamePresenter.DisplayPlayerIsTryingToPlayCard(reversalCard, 
            "Reversal", _activePlayer, _opponentPlayer);
        _playerManager.ResetCardEffectState();
        MoveCardsInReversal(reversalCard, reversedCard);
        
        if (reversalCard.ReversalEffects.Count > 0)
        {
            foreach (var effect in reversalCard.ReversalEffects)
            {
                effect.ApplyEffect(reversalCard, _activePlayer, _opponentPlayer, _view);
            }
        }
        
        HandleReversalDamage(reversalCard, modifiedReversedCard);
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
        string finalReversalDamage = reversalCard.Damage;
        if (reversalCard.Damage == "#")
        {
            finalReversalDamage = _opponentPlayer.SuperstarCard.Ability.
                ModifyIncomingDamage(int.Parse(modifiedReversedCard.Damage)).ToString();
            _opponentPlayer.IncreaseFortitude(0);
        }
        else
        {
            _opponentPlayer.IncreaseFortitude(int.Parse(finalReversalDamage));
        }
        DamageResult finalDamage = 
            _activePlayer.ReceiveDirectDamage(int.Parse(finalReversalDamage), true);
        _gamePresenter.ShowAppliedDamage(finalDamage, _activePlayer);
    }
    
    public void ReverseExecute(NormalCard selectedCard, DamageResult damageResult)
    {
        _view.SayThatCardWasReversedByDeck(_opponentPlayer.SuperstarCard.Name);
        if (int.Parse(selectedCard.StunValue) > 0 && 
            damageResult.OverturnedCards.Count != damageResult.AppliedDamage)
        {
            StunValueAction(selectedCard);
        }
    }
    
    private void StunValueAction(NormalCard selectedCard)
    {
        int numberOfCardsSelected = 
            _view.AskHowManyCardsToDrawBecauseOfStunValue(_activePlayer.SuperstarCard.Name,
            int.Parse(selectedCard.StunValue));
        _activePlayer.DrawCards(numberOfCardsSelected);
        if (numberOfCardsSelected > 0) 
            _view.SayThatPlayerDrawCards(_activePlayer.SuperstarCard.Name, numberOfCardsSelected);
    }
}
