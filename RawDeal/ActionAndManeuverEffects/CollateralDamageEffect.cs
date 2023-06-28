using RawDealView;
using RawDealView.Formatters;

namespace RawDeal;

public class CollateralDamageEffect : IActionAndManeuverEffect
{
    public void ApplyEffect(NormalCard selectedCard, Player playingPlayer, Player otherPlayer, View view)
    {
        ApplyPlayerDamage(playingPlayer, view);
    }

    private void ApplyPlayerDamage(Player playingPlayer, View view)
    {
        view.SayThatPlayerDamagedHimself(playingPlayer.GetSuperstarName(), 1);
        DamageResult damageResult = playingPlayer.ReceiveDirectDamage(1, false);

        if (damageResult.AppliedDamage > 0)
        {
            ShowDamageAndOverturnedCards(playingPlayer, view, damageResult);
        }
    }

    private void ShowDamageAndOverturnedCards(Player playingPlayer, View view, DamageResult damageResult)
    {
        view.SayThatSuperstarWillTakeSomeDamage(playingPlayer.GetSuperstarName(), damageResult.AppliedDamage);
        
        for (int i = 0; i < damageResult.OverturnedCards.Count; i++)
        {
            ShowOverturnedCardInfo(damageResult.OverturnedCards[i], 
                i + 1, damageResult.AppliedDamage, view);
        }
    }

    private void ShowOverturnedCardInfo(NormalCard overturnedCard, int currentIndex, int damage, View view)
    {
        IViewableCardInfo cardInfo = overturnedCard;
        string formattedCardInfo = Formatter.CardToString(cardInfo);

        view.ShowCardOverturnByTakingDamage(formattedCardInfo, currentIndex, damage);
    }
}
