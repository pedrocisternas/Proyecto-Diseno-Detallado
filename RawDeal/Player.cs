using RawDealView;
using RawDealView.Formatters;

namespace RawDeal;

public class Player
{
    public SuperstarCard SuperstarCard { get; }
    private readonly List<NormalCard> _deck;
    private readonly List<NormalCard> _hand;
    private readonly List<NormalCard> _ringside;
    private readonly List<NormalCard> _ringArea;
    private int _fortitude;
    private bool _hasUsedAbilityThisTurn;
    public List<IBonus> BonusList { get; }
    public LastPlay LastPlay;

    public Player(SuperstarCard superstarCard, List<NormalCard> deck)
    {
        SuperstarCard = superstarCard;
        _deck = deck;
        _hand = new List<NormalCard>();
        _ringside = new List<NormalCard>();
        _ringArea = new List<NormalCard>();
        _fortitude = 0;
        _hasUsedAbilityThisTurn = false;
        BonusList = new List<IBonus>();
    }
    public int GetFortitude() => _fortitude;
    public void IncreaseFortitude(int amount) => _fortitude += amount;
    public int GetHandCount() => _hand.Count;
    public List<NormalCard> GetHandCards() => _hand;
    public NormalCard GetACardFromHand(int index) => _hand[index];
    public void RemoveCardFromHand(NormalCard card) => _hand.Remove(card);
    public void AddCardToHand(NormalCard card) => _hand.Add(card);
    public int GetDeckCount() => _deck.Count;
    public List<NormalCard> GetDeckCards() => _deck;
    public NormalCard RemoveTopCardFromDeck()
    {
        int lastIndex = _deck.Count - 1;
        NormalCard topCard = _deck[lastIndex];
        _deck.RemoveAt(lastIndex);
        return topCard;
    }
    public void AddCardToDeck(NormalCard card) => _deck.Insert(0, card);
    public int GetRingsideCount() => _ringside.Count;
    public List<NormalCard> GetRingsideCards() => _ringside;
    public NormalCard GetACardFromRingside(int index) => _ringside[index];
    public void RemoveCardFromRingside(NormalCard card) => _ringside.Remove(card);
    public void AddCardToRingside(NormalCard card) => _ringside.Add(card);
    public List<NormalCard> GetRingAreaCards() => _ringArea;
    public void AddCardToRingArea(NormalCard card) => _ringArea.Add(card);

    public bool PlayerCanUseAbilityAndNeedsMenu(Player opponentPlayer)
    {
        return !_hasUsedAbilityThisTurn && SuperstarCard.Ability.
            CanUseAbility(this, opponentPlayer) && !SuperstarCard.Ability.IsAutomatic();
    }

    public void ExecuteAbility(Player opponentPlayer, View view)
    {
        SuperstarCard.Ability.Execute(this, opponentPlayer, view);
        _hasUsedAbilityThisTurn = true;
    }
    public void EndTurn() => _hasUsedAbilityThisTurn = false;
    
    public void DrawStartingCards()
    {
        DrawCards(SuperstarCard.HandSize);
    }
    
    public void DrawCards(int numberOfCards = 1)
    {
        for (int i = 0; i < numberOfCards && _deck.Count > 0; i++)
        {
            NormalCard drawnCard = RemoveTopCardFromDeck();
            AddCardToHand(drawnCard);
        }
    }
    public void DrawCardStartingTurn()
    {
        int numberOfCardsToDraw = SuperstarCard.Ability.GetNumberOfCardsToDraw(this);
        for (int i = 0; i < numberOfCardsToDraw; i++)
        {
            if (_deck.Count > 0)
            {
                NormalCard drawnCard = RemoveTopCardFromDeck();
                AddCardToHand(drawnCard);
            }
        }
    }

    public void DiscardCardsFromHand(int cardsToDiscard, View view)
    {
        for (int i = 0; i < cardsToDiscard; i++)
        {
            List<NormalCard> cardsThatMightBeDiscarded = GetHandCards();
            List<string> cardsString = cardsThatMightBeDiscarded.Select(card =>
                Formatter.CardToString(card)).ToList();

            int chosenCardIndex = view.AskPlayerToSelectACardToDiscard(cardsString,
                SuperstarCard.Name,
                SuperstarCard.Name, 
                cardsToDiscard - i);
            DiscardCard(cardsThatMightBeDiscarded[chosenCardIndex]);
        }
    }

    public void DiscardCard(NormalCard card)
    {
        RemoveCardFromHand(card);
        AddCardToRingside(card);
    }

    public DamageResult ReceiveReversableDamage(NormalCard damageCard, string playedAs)
    {
        int modifiedDamage = SuperstarCard.Ability.ModifyIncomingDamage(int.Parse(damageCard.Damage));
        List<NormalCard> overturnedCards = new List<NormalCard>();
        NormalCard reversalCard = new NullCard();

        for (var i = 0; i < modifiedDamage && _deck.Count > 0; i++)
        {
            NormalCard overturnedCard = RemoveTopCardFromDeck();
            AddCardToRingside(overturnedCard);
            overturnedCards.Add(overturnedCard);
            if (!CanCardBeReversed(overturnedCard, damageCard, playedAs)) continue;
            reversalCard = overturnedCard;
            break;
        }

        return new DamageResult(overturnedCards, modifiedDamage, reversalCard);
    }

    private bool CanCardBeReversed(NormalCard overturnedCard, NormalCard damageCard, string playedAs)
    {
        return overturnedCard.CanReverse(damageCard, playedAs) 
               && int.Parse(overturnedCard.Fortitude) 
               + CalculateAdditionalFortitude(damageCard) <= GetFortitude()
               && damageCard.CanBeReversed;
    }
    
    public int CalculateAdditionalFortitude(NormalCard cardToReverse)
    {
        return BonusList.Where(bonus => 
            (bonus.ApplyTo == "Subtype" && cardToReverse.Subtypes.Contains(bonus.Value)) 
            || (bonus.ApplyTo == "Type" && cardToReverse.Types.Contains(bonus.Value)) 
            && bonus.BonusOn == "Fortitude").Sum(bonus => bonus.Quantity);
    }
    
    public int CalculateAdditionalDamageDependingOnSubtypes(NormalCard card)
    {
        return BonusList.Where(bonus => 
            (bonus.ApplyTo == "Subtype" && card.Subtypes.Contains(bonus.Value)) ||
            (bonus.ApplyTo == "Type" && card.Types.Contains(bonus.Value)) &&
            bonus.BonusOn == "Damage").Sum(bonus => bonus.Quantity);
    }

    public DamageResult ReceiveDirectDamage(int damage, bool isReversal = false)
    {
        int modifiedDamage = isReversal ? SuperstarCard.Ability.ModifyIncomingDamage(damage) : damage;
        List<NormalCard> overturnedCards = OverturnCards(modifiedDamage);

        return new DamageResult(overturnedCards, modifiedDamage, new NullCard());
    }

    private List<NormalCard> OverturnCards(int damage)
    {
        List<NormalCard> overturnedCards = new List<NormalCard>();
        for (int i = 0; i < damage && _deck.Count > 0; i++)
        {
            NormalCard overturnedCard = RemoveTopCardFromDeck();
            AddCardToRingside(overturnedCard);
            overturnedCards.Add(overturnedCard);
        }
        return overturnedCards;
    }

    public void AddLastPlay(NormalCard selectedCard, string playedAs)
    {
        LastPlay = new LastPlay(selectedCard, playedAs);
    }
    
    public void AddCardToLastPlay(NormalCard selectedCard, string playedAs)
    {
        LastPlay.AddCard(selectedCard, playedAs);
    }
    
    public void AddDamageToLastPlay(int damage)
    {
        LastPlay.AddDamage(damage);
    }

    public void EndTurnLastPlay()
    {
        LastPlay.FinishTurn();
    }
    
    public void AddBonus(IBonus bonus)
    {
        BonusList.Add(bonus);
    }
    
    public void RemoveBonus(IBonus bonus)
    {
        BonusList.Remove(bonus);
    }
    
    public void ClearBonusList()
    {
        BonusList.Clear();
    }
}