using RawDealView;
using RawDealView.Formatters;
using RawDealView.Options;

namespace RawDeal;

public class GamePresenter
{
    private readonly View _view;

    public GamePresenter(View view)
    {
        _view = view;
    }

    public void ShowPlayersInfo(Player activePlayer, Player opponentPlayer)
    {
        PlayerInfo activePlayerInfo = new PlayerInfo(activePlayer.GetSuperstarName(), 
            activePlayer.GetFortitude(), activePlayer.GetHandCount(), 
            activePlayer.GetDeckCount());
        PlayerInfo opponentPlayerInfo = new PlayerInfo(opponentPlayer.GetSuperstarName(), 
            opponentPlayer.GetFortitude(), opponentPlayer.GetHandCount(), 
            opponentPlayer.GetDeckCount());
        
        _view.ShowGameInfo(activePlayerInfo, opponentPlayerInfo);
    }
    
    public void ShowCardsWithFormat(CardSet setOfCards, Player activePlayer, Player opponentPlayer)
    {
        List<NormalCard> cardsToFormat = GetCardsByCardSet(setOfCards, activePlayer, opponentPlayer);
        List<string> formattedCards = CardUtils.GetFormattedCards(cardsToFormat);
        _view.ShowCards(formattedCards);
    }
    
    private List<NormalCard> GetCardsByCardSet(CardSet setOfCards, Player activePlayer, Player opponentPlayer)
    {
        Dictionary<CardSet, Func<List<NormalCard>>> cardSetMapping = 
            new Dictionary<CardSet, Func<List<NormalCard>>>
        {
            { CardSet.Hand, () => activePlayer.GetHandCards() },
            { CardSet.RingArea, () => activePlayer.GetRingAreaCards() },
            { CardSet.RingsidePile, () => activePlayer.GetRingsideCards() },
            { CardSet.OpponentsRingArea, () => opponentPlayer.GetRingAreaCards() },
            { CardSet.OpponentsRingsidePile, () => opponentPlayer.GetRingsideCards() }
        };

        return cardSetMapping[setOfCards]();
    }
    
    public void ShowAppliedDamage(DamageResult damageResult, Player player)
    {
        if (damageResult.AppliedDamage > 0)
        {
            _view.SayThatSuperstarWillTakeSomeDamage(player.GetSuperstarName(), damageResult.AppliedDamage);
            for (int i = 0; i < damageResult.OverturnedCards.Count; i++)
            {
                NormalCard overturnedCard = damageResult.OverturnedCards[i];
                ShowOverturnedCardInfo(overturnedCard, i + 1, damageResult.AppliedDamage);
            }
        }
    }

    private void ShowOverturnedCardInfo(NormalCard overturnedCard, int currentIndex, int damage)
    {
        IViewableCardInfo cardInfo = overturnedCard;
        string formattedCardInfo = Formatter.CardToString(cardInfo);

        _view.ShowCardOverturnByTakingDamage(formattedCardInfo, currentIndex, damage);
    }

    public void DisplayPlayerIsTryingToPlayCard(NormalCard card, 
        string cardType, Player activePlayer, Player opponentPlayer)
    {
        IViewableCardInfo cardInfo = card;
        string playedAs = cardType.ToUpper();

        PlayInfo playInfo = new PlayInfo(cardInfo, playedAs);
        string formattedPlay = Formatter.PlayToString(playInfo);

        if (cardType == "Reversal")
        {
            _view.SayThatPlayerReversedTheCard(opponentPlayer.GetSuperstarName(), formattedPlay);
        }
        else
        {
            _view.SayThatPlayerIsTryingToPlayThisCard(activePlayer.GetSuperstarName(), formattedPlay);
        }
    }
}
