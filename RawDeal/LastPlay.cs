namespace RawDeal;

public class LastPlay
{
    public NormalCard CardPlayed;
    public string PlayedAs;
    public int DamageMade;
    public bool PlayedInSameTurn;

    public LastPlay(NormalCard cardPlayed, string playedAs)
    {
        CardPlayed = cardPlayed;
        PlayedAs = playedAs;
        PlayedInSameTurn = false;
    }

    public void AddDamage(int damage)
    {
        DamageMade = damage;
    }
    
    public void AddCard(NormalCard card, string playedAs)
    {
        CardPlayed = card;
        PlayedAs = playedAs;
        PlayedInSameTurn = true;
    }
    
    public void FinishTurn()
    {
        PlayedInSameTurn = false;
    }
}