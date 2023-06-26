namespace RawDeal;

public class PlayConditionAdder
{
    private readonly NormalCard _card;
    private readonly List<IPlayCondition> _playConditions;

    public PlayConditionAdder(NormalCard card)
    {
        _card = card;
        _playConditions = card.PlayConditions;
    }

    public void AddPlayConditions()
    {
        switch (_card.Title)
        {
            case "Spit At Opponent":
                _playConditions.Add(new HasAtLeastNCardsInHand(2));
                break;
            case "Lionsault":
                _playConditions.Add(new AfterDamage(4));
                _playConditions.Add(new AfterManeuver());
                break;
            case "Austin Elbow Smash":
                _playConditions.Add(new AfterDamage(5));
                _playConditions.Add(new AfterManeuver());
                break;
            case "Back Body Drop":
                _playConditions.Add(new AfterCertainCard("Irish Whip"));
                break;
            case "Leaping Knee to the Face":
                _playConditions.Add(new AfterCertainCard("Irish Whip"));
                break;
        }
    }
}