namespace RawDeal;

public class AfterCertainCard : IPlayCondition
{
    private readonly string _cardName;

    public AfterCertainCard(string cardName)
    {
        _cardName = cardName;
    }
    public bool CanPlayCard(Player playingPlayer)
    {
        LastPlay lastPlay = playingPlayer.LastPlay;
        return lastPlay.PlayedInSameTurn && lastPlay.CardPlayed.Title == _cardName;
    }
}