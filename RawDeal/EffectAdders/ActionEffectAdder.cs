namespace RawDeal;

public class ActionEffectAdder
{
    private readonly NormalCard _card;
    private readonly List<IActionAndManeuverEffect> _actionEffects;

    public ActionEffectAdder(NormalCard card)
    {
        _card = card;
        _actionEffects = card.ActionEffects;
    }

    public void AddActionEffects()
    {
        switch (_card.Title)
        {
            case "Offer Handshake":
                _actionEffects.Add(new BasicActionEffect());
                _actionEffects.Add((new PlayerCanDrawCardsEffect(3)));
                _actionEffects.Add(new PlayerDiscardCardsEffect());
                break;
            case "Jockeying for Position":
                _actionEffects.Add(new JockeyingForPositionEffect());
                break;
            case "Spit At Opponent":
                _actionEffects.Add(new BasicActionEffect());
                _actionEffects.Add(new PlayerDiscardCardsEffect());
                _actionEffects.Add(new OpponentDiscardCardsEffect(4));
                break;
            case "Chop":
                _actionEffects.Add(new HybridEffect());
                break;
            case "Arm Bar Takedown":
                _actionEffects.Add(new HybridEffect());
                break;
            case "Collar & Elbow Lockup":
                _actionEffects.Add(new HybridEffect());
                break;
            case "Undertaker's Tombstone Piledriver":
                _actionEffects.Add(new HybridEffect());
                break;
            case "Puppies! Puppies!":
                _actionEffects.Add(new BasicActionEffect());
                _actionEffects.Add(new RecoverRingsideCardsEffect(5, 2));
                break;
            case "Recovery":
                _actionEffects.Add(new BasicActionEffect());
                _actionEffects.Add(new RecoverRingsideCardsEffect(2, 1));
                break;
            case "Irish Whip":
                _actionEffects.Add(new BonusDamageEffect("Subtype", "Strike", 5));
                break;
            case "I Am the Game":
                _actionEffects.Add(new ChooseDrawOrDiscardEffect(2, true));
                break;
            case "Back Body Drop":
                _actionEffects.Add(new ChooseDrawOrDiscardEffect(2, true));
                break;
            case "Y2J":
                _actionEffects.Add(new ChooseDrawOrDiscardEffect(5, false));
                break;
            case "Roll Out of the Ring":
                _actionEffects.Add(new DiscardAndObtainRingsideCards());
                break;
            case "Undertaker’s Tombstone Piledriver":
                _actionEffects.Add(new HybridEffect());
                break;
        }
    }
}