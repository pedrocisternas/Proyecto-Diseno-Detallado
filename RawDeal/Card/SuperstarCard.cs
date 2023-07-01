namespace RawDeal;

public class SuperstarCard
{
    public string Name { get; }
    public string Logo { get; }
    public int HandSize { get; }
    public int SuperstarValue { get; }
    public string SuperstarAbility { get; }
    public ISuperstarAbility Ability { get; }

    public SuperstarCard(string name, string logo, int handSize, int superstarValue, 
        string superstarAbility)
    {
        Name = name;
        Logo = logo;
        HandSize = handSize;
        SuperstarValue = superstarValue;
        SuperstarAbility = superstarAbility;
        Ability = AssignAbilityBasedOnName();
    }
    
    public bool HasAbility(string name)
    {
        return Name.Equals(name, StringComparison.OrdinalIgnoreCase);
    }
    
    private ISuperstarAbility AssignAbilityBasedOnName()
    {
        switch (Name.ToUpper())
        {
            case "THE UNDERTAKER":
                return new UndertakerAbility();
            case "CHRIS JERICHO":
                return new JerichoAbility();
            case "STONE COLD STEVE AUSTIN":
                return new StoneColdSteveAustinAbility();
            case "MANKIND":
                return new MankindAbility();
            case "THE ROCK":
                return new TheRockAbility();
            default:
                return new NoAbility();
        }
    }
}

