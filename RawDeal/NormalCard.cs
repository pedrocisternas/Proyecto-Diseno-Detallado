using RawDealView.Formatters;
namespace RawDeal;
using System.Collections.Generic;

public class NormalCard : IViewableCardInfo
{
    public string Title { get; }
    public List<string> Types { get; }
    public List<string> Subtypes { get; }
    public string Fortitude { get; }
    public string Damage { get; }
    public string StunValue { get; }
    public string CardEffect { get; }
    public List<ReversalEffect> ReversalEffects { get; }
    public NormalCard(string title, List<string> types, List<string> subtypes, string fortitude, string damage, string stunValue, string cardEffect)
    {
        Title = title;
        Types = types;
        Subtypes = subtypes;
        Fortitude = fortitude;
        Damage = damage;
        StunValue = stunValue;
        CardEffect = cardEffect;
        ReversalEffects = new List<ReversalEffect>();
        if (types.Contains("Reversal"))
        {
            AddReversalEffects(subtypes);
        }
    }

    public NormalCard Clone()
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
    
    private void AddReversalEffects(List<string> subtypes)
    {
        foreach (string subtype in subtypes)
        {
            switch (subtype)
            {
                case "ReversalStrike":
                    ReversalEffects.Add(new ReversalBySubtypeEffect("Strike"));
                    break;
                case "ReversalGrapple":
                    ReversalEffects.Add(new ReversalBySubtypeEffect("Grapple"));
                    break;
                case "ReversalSubmission":
                    ReversalEffects.Add(new ReversalBySubtypeEffect("Submission"));
                    break;
                case "ReversalAction":
                    ReversalEffects.Add(new ReversalByTypeEffect("Action"));
                    break;
            }
        }
    }
}


