namespace RawDeal;

public class AfterDamage : IPlayCondition
{
    private readonly int _damage;

    public AfterDamage(int damage)
    {
        _damage = damage;
    }
    public bool CanPlayCard(Player playingPlayer)
    {
        LastPlay lastPlay = playingPlayer.LastPlay;
        return lastPlay.PlayedInSameTurn && lastPlay.DamageMade >= _damage;
    }
}