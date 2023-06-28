namespace RawDeal;

public class ReversalOutcome
{
    public List<NormalCard> OverturnedCards { get; }
    public NormalCard ReversalCard { get; set; }

    public ReversalOutcome()
    {
        OverturnedCards = new List<NormalCard>();
        ReversalCard = new NullCard();
    }
}