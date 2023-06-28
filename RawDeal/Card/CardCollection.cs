namespace RawDeal;

public class CardCollection
{
    private readonly List<NormalCard> _cards;

    public CardCollection()
    {
        _cards = new List<NormalCard>();
    }

    public int Count => _cards.Count;

    public void Add(NormalCard card)
    {
        _cards.Add(card);
    }

    public bool Remove(NormalCard card)
    {
        return _cards.Remove(card);
    }

    public NormalCard Get(int index)
    {
        return _cards[index];
    }

    public NormalCard RemoveAt(int index)
    {
        NormalCard card = _cards[index];
        _cards.RemoveAt(index);
        return card;
    }

    public void Insert(int index, NormalCard card)
    {
        _cards.Insert(index, card);
    }

    public List<NormalCard> ToList()
    {
        return new List<NormalCard>(_cards);
    }

    public IEnumerable<NormalCard> Where(Func<NormalCard, bool> predicate)
    {
        return _cards.Where(predicate);
    }
}
