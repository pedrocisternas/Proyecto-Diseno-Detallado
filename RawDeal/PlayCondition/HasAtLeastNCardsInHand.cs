namespace RawDeal;

public class HasAtLeastNCardsInHand : IPlayCondition
{
    private readonly int _number;

    public HasAtLeastNCardsInHand(int number)
    {
        _number = number;
    }

    public bool CanPlayCard(Player playingPlayer)
    {
        return playingPlayer.GetHandCount() > _number - 1;
    }
}