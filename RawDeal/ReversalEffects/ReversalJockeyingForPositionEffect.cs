using RawDealView;
using RawDealView.Options;

namespace RawDeal;

public class ReversalJockeyingForPositionEffect : ReversalEffect
{
    private readonly string _targetTitle;

    public ReversalJockeyingForPositionEffect(string targetTitle)
    {
        _targetTitle = targetTitle;
    }
    
    public override bool CanReverse(NormalCard cardToReverse, string playedAs)
    {
        return cardToReverse.Title == _targetTitle;
    }
    
    public override void ApplyEffect(NormalCard selectedCard, Player otherPlayer, Player playingPlayer, View view)
    {
        SelectedEffect selectedEffect = 
            view.AskUserToSelectAnEffectForJockeyForPosition(playingPlayer.GetSuperstarName());
        PlaySelectedEffect(selectedEffect, playingPlayer, otherPlayer);
        
        playingPlayer.RemoveCardFromHand(selectedCard);
        playingPlayer.AddCardToRingArea(selectedCard);
    }

    private void PlaySelectedEffect(SelectedEffect selectedEffect, Player playingPlayer, Player otherPlayer)
    {
        if (selectedEffect == SelectedEffect.NextGrappleIsPlus4D)
        {
            NextGrappleIsPlus4D(playingPlayer);
        }
        else if (selectedEffect == SelectedEffect.NextGrapplesReversalIsPlus8F)
        {
            NextGrapplesReversalIsPlus8F(otherPlayer);
        }
    }

    private void NextGrappleIsPlus4D(Player playingPlayer)
    {
        playingPlayer.AddBonus(new Bonus("Subtype", "Grapple", "Damage", 4, "Next"));
    }
    
    private void NextGrapplesReversalIsPlus8F(Player otherPlayer)
    {
        otherPlayer.AddBonus(new Bonus("Subtype", "Grapple", "Fortitude", 8, "Next"));
    }
}