namespace RawDeal;

public class ManeuverEffectAdder
{
    private readonly NormalCard _card;
    private readonly List<IActionAndManeuverEffect> _maneuverEffects;

    public ManeuverEffectAdder(NormalCard card)
    {
        _card = card;
        _maneuverEffects = card.ManeuverEffects;
    }
    
    public void AddManeuverEffects()
    {
        switch (_card.Title)
        {
            case "Head Butt":
                _maneuverEffects.Add(new PlayerDiscardCardsEffect());
                break;
            case "Arm Drag":
                _maneuverEffects.Add(new PlayerDiscardCardsEffect());
                break;
            case "Arm Bar":
                _maneuverEffects.Add(new PlayerDiscardCardsEffect());
                break;
            case "Bear Hug":
                _maneuverEffects.Add(new OpponentDiscardCardsEffect(1));
                break;
            case "Choke Hold":
                _maneuverEffects.Add(new OpponentDiscardCardsEffect(1));
                break;
            case "Ankle Lock":
                _maneuverEffects.Add(new OpponentDiscardCardsEffect(1));
                break;
            case "Spinning Heel Kick":
                _maneuverEffects.Add(new OpponentDiscardCardsEffect(1));
                break;
            case "Figure Four Leg Lock":
                _maneuverEffects.Add(new OpponentDiscardCardsEffect(1));
                break;
            case "Samoan Drop":
                _maneuverEffects.Add(new OpponentDiscardCardsEffect(1));
                break;
            case "Boston Crab":
                _maneuverEffects.Add(new OpponentDiscardCardsEffect(1));
                break;
            case "Power Slam":
                _maneuverEffects.Add(new OpponentDiscardCardsEffect(1));
                break;
            case "Torture Rack":
                _maneuverEffects.Add(new OpponentDiscardCardsEffect(1));
                break;
            case "Pump Handle Slam":
                _maneuverEffects.Add(new OpponentDiscardCardsEffect(2));
                break;
            case "Bulldog":
                _maneuverEffects.Add(new PlayerDiscardCardsEffect());
                _maneuverEffects.Add(new PlayerChooseOpponentDiscardCardsEffect());
                break;
            case "Kick":
                _maneuverEffects.Add(new CollateralDamageEffect());
                break;
            case "Running Elbow Smash":
                _maneuverEffects.Add(new CollateralDamageEffect());
                break;
            case "Double Leg Takedown":
                _maneuverEffects.Add((new PlayerCanDrawCardsEffect(1)));
                break;
            case "Reverse DDT":
                _maneuverEffects.Add((new PlayerCanDrawCardsEffect(1)));
                break;
            case "Headlock Takedown":
                _maneuverEffects.Add(new OpponentDrawCardsEffect());
                break;
            case "Standing Side Headlock":
                _maneuverEffects.Add(new OpponentDrawCardsEffect());
                break;
            case "Press Slam":
                _maneuverEffects.Add(new CollateralDamageEffect());
                _maneuverEffects.Add(new OpponentDiscardCardsEffect(2));
                break;
            case "Fisherman's Suplex":
                _maneuverEffects.Add(new CollateralDamageEffect());
                _maneuverEffects.Add(new PlayerCanDrawCardsEffect(1));
                break;
            case "DDT":
                _maneuverEffects.Add(new CollateralDamageEffect());
                _maneuverEffects.Add(new OpponentDiscardCardsEffect(2));
                break;
            case "Guillotine Stretch":
                _maneuverEffects.Add(new OpponentDiscardCardsEffect(1));
                _maneuverEffects.Add(new PlayerCanDrawCardsEffect(1));
                break;
            case "Chicken Wing":
                _maneuverEffects.Add(new RecoverRingsideCardsEffect(2, 0));
                break;
            case "Lionsault":
                _maneuverEffects.Add(new OpponentDiscardCardsEffect(1));
                break;
            case "Tree of Woe":
                _maneuverEffects.Add(new CannotBeReversedEffect(_card));
                _maneuverEffects.Add(new OpponentDiscardCardsEffect(2));
                break;
            case "Austin Elbow Smash":
                _maneuverEffects.Add(new CannotBeReversedEffect(_card));
                break;
            case "Leaping Knee to the Face":
                _maneuverEffects.Add(new CannotBeReversedEffect(_card));
                _maneuverEffects.Add(new OpponentDiscardCardsEffect(1));
                break;
            case "Clothesline":
                _maneuverEffects.Add(new BonusDamageEffect("Type", "Maneuver", 2));
                break;
            case "Atomic Drop":
                _maneuverEffects.Add(new BonusDamageEffect("Type", "Maneuver", 2));
                break;
            case "Snap Mare":
                _maneuverEffects.Add(new BonusDamageEffect("Subtype", "Strike", 2));
                break;
        }
    }
}