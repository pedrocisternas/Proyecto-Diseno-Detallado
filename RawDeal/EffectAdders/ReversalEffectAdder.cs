namespace RawDeal;

public class ReversalEffectAdder
{
    private readonly NormalCard _card;
    private readonly List<ReversalEffect> _reversalEffects;

    public ReversalEffectAdder(NormalCard card)
    {
        _card = card;
        _reversalEffects = card.ReversalEffects;
    }

    public void AddReversalEffects()
    {
        foreach (string subtype in _card.Subtypes)
        {
            switch (subtype)
            {
                case "ReversalStrike":
                    _reversalEffects.Add(new ReversalBySubtypeEffect("Maneuver", "Strike"));
                    break;
                case "ReversalGrapple":
                    _reversalEffects.Add(new ReversalBySubtypeEffect("Maneuver", "Grapple"));
                    break;
                case "ReversalSubmission":
                    _reversalEffects.Add(new ReversalBySubtypeEffect("Maneuver", "Submission"));
                    break;
                case "ReversalAction":
                    _reversalEffects.Add(new ReversalByTypeEffect("Action"));
                    break;
                case "ReversalGrappleSpecial":
                    _reversalEffects.Add(new ReversalBySubtypeEffect("Maneuver", "Grapple"));
                    _reversalEffects.Add(new ReversalByDamageLimitEffect(7));
                    break;
                case "ReversalStrikeSpecial":
                    _reversalEffects.Add(new ReversalBySubtypeEffect("Maneuver", "Strike"));
                    _reversalEffects.Add(new ReversalByDamageLimitEffect(7));
                    break;
                case "ReversalSpecial":
                    AddReversalSpecialEffects();
                    break;
            }
        }
    }
    
    private void AddReversalSpecialEffects()
    {
        switch (_card.Title)
        {
            case "Elbow to the Face":
                _reversalEffects.Add(new ReversalByTypeEffect("Maneuver"));
                _reversalEffects.Add(new ReversalByDamageLimitEffect(7));
                break;
            case "Manager Interferes":
                _reversalEffects.Add(new ReversalInterferesEffect(1));
                break;
            case "Chyna Interferes":
                _reversalEffects.Add(new ReversalInterferesEffect(2));
                break;
            case "Clean Break":
                _reversalEffects.Add(new ReversalCleanBreakEffect("Jockeying for Position"));
                break;
            case "Jockeying for Position":
                _reversalEffects.Add(new ReversalJockeyingForPositionEffect("Jockeying for Position"));
                break;
            case "Belly to Belly Suplex":
                _reversalEffects.Add(new ReversalByTitleEffect("Belly to Belly Suplex"));
                break;
            case "Vertical Suplex":
                _reversalEffects.Add(new ReversalByTitleEffect("Vertical Suplex"));
                break;
            case "Belly to Back Suplex":
                _reversalEffects.Add(new ReversalByTitleEffect("Belly to Back Suplex"));
                break;
            case "Ensugiri":
                _reversalEffects.Add(new ReversalByTitleEffect("Kick"));
                break;
            case "Drop Kick":
                _reversalEffects.Add(new ReversalByTitleEffect("Drop Kick"));
                break;
            case "Double Arm DDT":
                _reversalEffects.Add(new ReversalByTitleEffect("Back Body Drop"));
                break;
            case "Irish Whip":
                _reversalEffects.Add(new ReversalIrishWhipEffect("Irish Whip"));
                break;
        }
    }
}