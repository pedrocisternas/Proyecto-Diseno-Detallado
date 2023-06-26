using RawDealView;
using RawDealView.Formatters;
namespace RawDeal;
using System.Collections.Generic;

public class NormalCard : IViewableCardInfo
{
    public string Title { get; }
    public List<string> Types { get; }
    public List<string> Subtypes { get; }
    public string Fortitude { get; set; }
    public string Damage { get; set; }
    public string StunValue { get; }
    public string CardEffect { get; }
    public List<ReversalEffect> ReversalEffects { get; }
    public List<IActionAndManeuverEffect> ActionEffects { get; }
    public List<IActionAndManeuverEffect> ManeuverEffects { get; }
    public List<IPlayCondition> PlayConditions { get; }
    public List<ISpecialEffect> SpecialEffects { get; }
    public bool CanBeReversed;
    
    public NormalCard(string title, List<string> types, List<string> subtypes, 
        string fortitude, string damage, string stunValue, string cardEffect)
    {
        Title = title;
        Types = types;
        Subtypes = subtypes;
        Fortitude = fortitude;
        Damage = damage;
        StunValue = stunValue;
        CardEffect = cardEffect;
        ReversalEffects = new List<ReversalEffect>();
        ActionEffects = new List<IActionAndManeuverEffect>();
        ManeuverEffects = new List<IActionAndManeuverEffect>();
        PlayConditions = new List<IPlayCondition>();
        SpecialEffects = new List<ISpecialEffect>();
        CanBeReversed = true;
        AddEffectsByType();
        PlayConditionAdder playConditionAdder = new PlayConditionAdder(this);
        playConditionAdder.AddPlayConditions();
        SpecialEffectAdder specialEffectAdder = new SpecialEffectAdder(this);
        specialEffectAdder.AddSpecialEffects();
    }

    private void AddEffectsByType()
    {
        if (Types.Contains("Action"))
        {
            ActionEffectAdder actionEffectAdder = new ActionEffectAdder(this);
            actionEffectAdder.AddActionEffects();
        }
        if (Types.Contains("Maneuver"))
        {
            ManeuverEffectAdder maneuverEffectAdder = new ManeuverEffectAdder(this);
            maneuverEffectAdder.AddManeuverEffects();
        }
        if (Types.Contains("Reversal"))
        {
            ReversalEffectAdder reversalEffectAdder = new ReversalEffectAdder(this);
            reversalEffectAdder.AddReversalEffects();
        }
    }

    public virtual NormalCard Clone()
    {
        return new NormalCard(
            Title,
            new List<string>(Types),
            new List<string>(Subtypes),
            Fortitude,
            Damage,
            StunValue,
            CardEffect
        );
    }
    
    public virtual NormalCard CloneWithModifiedDamage(Player player)
    {
        var additionalDamage = player.CalculateAdditionalDamageDependingOnSubtypes(this);
        var modifiedDamage = (int.Parse(Damage) + additionalDamage).ToString();

        return new NormalCard(
            Title,
            new List<string>(Types),
            new List<string>(Subtypes),
            Fortitude,
            modifiedDamage,
            StunValue,
            CardEffect
        );
    }

    public virtual bool CanReverse(NormalCard cardToReverse, string playedAs)
    {
        return ReversalEffects.Count != 0 && ReversalEffects.All(effect => 
            effect.CanReverse(cardToReverse, playedAs));
    }
}