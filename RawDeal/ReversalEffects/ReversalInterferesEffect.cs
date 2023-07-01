using RawDealView;

namespace RawDeal;

public class ReversalInterferesEffect : ReversalEffect
{
    private readonly int _cardsToDiscard;

    public ReversalInterferesEffect(int cardsToDiscard)
    {
        _cardsToDiscard = cardsToDiscard;
    }

    public override bool CanReverse(NormalCard cardToReverse, string playedAs)
    {
        return playedAs == "Maneuver";
    }

    public override void ApplyEffect(NormalCard reversalCard, Player activePlayer, 
        Player opponentPlayer, View view)
    {
        opponentPlayer.DrawCards(_cardsToDiscard);
        view.SayThatPlayerDrawCards(opponentPlayer.GetSuperstarName(), _cardsToDiscard);
    }
}