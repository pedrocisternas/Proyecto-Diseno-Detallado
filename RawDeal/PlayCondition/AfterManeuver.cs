namespace RawDeal;

public class AfterManeuver : IPlayCondition
{
    public bool CanPlayCard(Player playingPlayer)
    {
        LastPlay lastPlay = playingPlayer.LastPlay;
        return lastPlay.PlayedAs == "Maneuver";
    }
}
