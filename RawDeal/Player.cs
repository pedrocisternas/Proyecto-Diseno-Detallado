using RawDealView;
using System.Collections.Generic;
namespace RawDeal;

public class Player
{
    public SuperstarCard SuperstarCard { get; }
    private List<NormalCard> Deck { get; }
    private List<NormalCard> Hand { get; }
    private List<NormalCard> Ringside { get; }
    private List<NormalCard> RingArea { get; }
    private int Fortitude { get; set; }
    private bool HasUsedAbilityThisTurn { get; set; }

    public Player(SuperstarCard superstarCard, List<NormalCard> deck)
    {
        SuperstarCard = superstarCard;
        Deck = deck;
        Hand = new List<NormalCard>();
        Ringside = new List<NormalCard>();
        RingArea = new List<NormalCard>();
        Fortitude = 0;
        HasUsedAbilityThisTurn = false;
    }
    public int GetFortitude() => Fortitude;
    public void IncreaseFortitude(int amount) => Fortitude += amount;
    public int GetHandCount() => Hand.Count;
    public List<NormalCard> GetHandCards() => Hand;
    public NormalCard GetACardFromHand(int index) => Hand[index];
    public void RemoveCardFromHand(NormalCard card) => Hand.Remove(card);
    public void AddCardToHand(NormalCard card) => Hand.Add(card);
    public int GetDeckCount() => Deck.Count;
    public List<NormalCard> GetDeckCards() => Deck;
    public NormalCard RemoveTopCardFromDeck()
    {
        int lastIndex = Deck.Count - 1;
        NormalCard topCard = Deck[lastIndex];
        Deck.RemoveAt(lastIndex);
        return topCard;
    }
    public void AddCardToDeck(NormalCard card) => Deck.Insert(0, card);
    public int GetRingsideCount() => Ringside.Count;
    public List<NormalCard> GetRingsideCards() => Ringside;
    public NormalCard GetACardFromRingside(int index) => Ringside[index];
    public void RemoveCardFromRingside(NormalCard card) => Ringside.Remove(card);
    public void AddCardToRingside(NormalCard card) => Ringside.Add(card);
    public List<NormalCard> GetRingAreaCards() => RingArea;
    public void AddCardToRingArea(NormalCard card) => RingArea.Add(card);

    public bool PlayerCanUseAbilityAndNeedsMenu(Player opponentPlayer)
    {
        return !HasUsedAbilityThisTurn && SuperstarCard.Ability.CanUseAbility(this, opponentPlayer) && !SuperstarCard.Ability.IsAutomatic();
    }

    public void ExecuteAbility(Player opponentPlayer, View view)
    {
        SuperstarCard.Ability.Execute(this, opponentPlayer, view);
        HasUsedAbilityThisTurn = true;
    }
    public void EndTurn() => HasUsedAbilityThisTurn = false;
    
    // Ver si puedo sacar el retorno bool en estas dos funciones (ya que están en negro)
    public bool DrawStartingCards()
    {
        for (int i = 0; i < SuperstarCard.HandSize && Deck.Count > 0; i++)
        {
            NormalCard drawnCard = RemoveTopCardFromDeck();
            AddCardToHand(drawnCard);
        }

        return Deck.Count == 0;
    }
    
    public void DrawCards(int numberOfCards)
    {
        for (int i = 0; i < numberOfCards; i++)
        {
            NormalCard drawnCard = RemoveTopCardFromDeck();
            AddCardToHand(drawnCard);
        }
    }
    public bool DrawOneCard()
    {
        int numberOfCardsToDraw = SuperstarCard.Ability.GetNumberOfCardsToDraw(this);
        for (int i = 0; i < numberOfCardsToDraw; i++)
        {
            if (Deck.Count > 0)
            {
                NormalCard drawnCard = RemoveTopCardFromDeck();
                AddCardToHand(drawnCard);
            }
            else
            {
                return true;
            }
        }

        return false;
    }

    // Arreglar este nombre y el de la función de arriba
    public void DrawOnlyOneCard()
    {
        NormalCard drawnCard = RemoveTopCardFromDeck();
        AddCardToHand(drawnCard);
    }
    
    public void DiscardCard(NormalCard card)
    {
        RemoveCardFromHand(card);
        AddCardToRingside(card);
    }
    
    // Ver si puedo fusionar estos dos métodos (este y el de abajo). Si no cambiar nombres
    public DamageResult ReceiveDamage(int damage, NormalCard damageCard)
    {
        int modifiedDamage = SuperstarCard.Ability.ModifyIncomingDamage(damage);
        List<NormalCard> overturnedCards = new List<NormalCard>();
        bool wasReversed = false;

        for (int i = 0; i < modifiedDamage && Deck.Count > 0; i++)
        {
            NormalCard overturnedCard = RemoveTopCardFromDeck();
            AddCardToRingside(overturnedCard);
            overturnedCards.Add(overturnedCard);
            if (overturnedCard.CanReverse(damageCard))
            {
                wasReversed = true;
                break;
            }
        }

        return new DamageResult(overturnedCards, modifiedDamage, wasReversed);
    }
    public DamageResult ReceiveDirectDamage(int damage)
    {
        List<NormalCard> overturnedCards = new List<NormalCard>();

        for (int i = 0; i < damage && Deck.Count > 0; i++)
        {
            NormalCard overturnedCard = RemoveTopCardFromDeck();
            AddCardToRingside(overturnedCard);
            overturnedCards.Add(overturnedCard);
        }

        // Revisar el último hardcode
        return new DamageResult(overturnedCards, damage, false);
    }
}