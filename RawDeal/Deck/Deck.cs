namespace RawDeal;

public class Deck
{
    public SuperstarCard SuperstarCard { get; private set; }
    public List<NormalCard> NormalCards { get; private set; }

    public Deck(string deckPath)
    {
        LoadDeck(deckPath);
    }

    private void LoadDeck(string deckPath)
    {
        List<string> lines = ReadDeckFile(deckPath);
        string superstarName = GetSuperstarName(lines);
        lines.RemoveAt(0);

        LoadSuperstarCard(superstarName);
        LoadNormalCards(lines);
    }

    private List<string> ReadDeckFile(string deckPath)
    {
        return File.ReadAllLines(deckPath).ToList();
    }

    private string GetSuperstarName(List<string> lines)
    {
        return lines[0].Replace(" (Superstar Card)", "");
    }

    private void LoadSuperstarCard(string superstarName)
    {
        string superstarJsonPath = Path.Combine("data", "superstar2.json");
        List<SuperstarCard> allSuperstars = CardLoader.LoadAllSuperstarCards(superstarJsonPath);
        SuperstarCard = allSuperstars.FirstOrDefault(s => 
            s.Name.Equals(superstarName, StringComparison.OrdinalIgnoreCase));
    }

    private void LoadNormalCards(List<string> lines)
    {
        string cardsJsonPath = Path.Combine("data", "cards.json");
        List<NormalCard> allNormalCards = CardLoader.LoadAllNormalCards(cardsJsonPath);

        NormalCards = new List<NormalCard>();
        foreach (string cardName in lines)
        {
            NormalCard card = allNormalCards.FirstOrDefault(c => 
                c.Title.Equals(cardName, StringComparison.OrdinalIgnoreCase));
            if (card != null)
            {
                NormalCards.Add(card.Clone());
            }
        }
    }

    public bool IsValid()
    {
        DeckValidator deckValidator = new DeckValidator();
        return deckValidator.Validate(NormalCards);
    }
}
