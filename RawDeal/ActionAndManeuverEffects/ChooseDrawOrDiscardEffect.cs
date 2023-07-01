using RawDealView;
using RawDealView.Formatters;
using RawDealView.Options;

namespace RawDeal;

public class ChooseDrawOrDiscardEffect : IActionAndManeuverEffect
{
    private readonly int _numberOfCards;
    private readonly bool _exact;
    
    public ChooseDrawOrDiscardEffect(int numberOfCards, bool exact)
    {
        _numberOfCards = numberOfCards;
        _exact = exact;
    }
    
    public void ApplyEffect(NormalCard selectedCard, Player playingPlayer, Player otherPlayer, 
        View view)
    {
        SelectedEffect selectedEffect = 
            view.AskUserToChooseBetweenDrawingOrForcingOpponentToDiscardCards(
                playingPlayer.GetSuperstarName());
        PlaySelectedEffect(selectedEffect, playingPlayer, otherPlayer, view);
        
        playingPlayer.RemoveCardFromHand(selectedCard); 
        playingPlayer.AddCardToRingArea(selectedCard);
        playingPlayer.AddBonus(new Bonus("Subtype", "Strike", "Damage", 
            5, "Next"));
    }
    
    private void PlaySelectedEffect(SelectedEffect selectedEffect, Player playingPlayer, 
        Player otherPlayer, View view)
    {
        if (selectedEffect == SelectedEffect.DrawCards)
        {
            DrawCards(playingPlayer, view);
        }
        else if (selectedEffect == SelectedEffect.ForceOpponentToDiscard)
        {
            ForceOpponentToDiscard(otherPlayer, view);
        }
    }

    private void DrawCards(Player playingPlayer, View view)
    {
        int cardsToDraw = _exact ? _numberOfCards : 
            view.AskHowManyCardsToDrawBecauseOfACardEffect(playingPlayer.
                GetSuperstarName(), _numberOfCards);
        view.SayThatPlayerDrawCards(playingPlayer.GetSuperstarName(), cardsToDraw);
        
        AddCardsToPlayerHandFromDeck(playingPlayer, cardsToDraw);
    }

    private void AddCardsToPlayerHandFromDeck(Player playingPlayer, int numberOfCards)
    {
        for (int i = 0; i < numberOfCards; i++)
        {
            NormalCard card = playingPlayer.RemoveTopCardFromDeck();
            playingPlayer.AddCardToHand(card);
        }
    }
    
    private void ForceOpponentToDiscard(Player otherPlayer, View view)
    {
        for (int i = 0; i < _numberOfCards && otherPlayer.GetHandCards().Count > 0; i++)
        {
            otherPlayer.DiscardCardsFromHand(_numberOfCards, view);
        }
    }
}

