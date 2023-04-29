namespace RawDeal;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

public static class CardLoader
{
    public static List<NormalCard> LoadAllNormalCards(string cardsJsonPath)
    {
        string jsonCards = File.ReadAllText(cardsJsonPath);
        List<NormalCard> allNormalCards = JsonSerializer.Deserialize<List<NormalCard>>(jsonCards);
        return allNormalCards;
    }

    public static List<SuperstarCard> LoadAllSuperstarCards(string superstarJsonPath)
    {
        string jsonSuperstars = File.ReadAllText(superstarJsonPath);
        List<SuperstarCard> allSuperstars = JsonSerializer.Deserialize<List<SuperstarCard>>(jsonSuperstars);
        return allSuperstars;
    }
}
