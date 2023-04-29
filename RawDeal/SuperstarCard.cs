using RawDealView;

namespace RawDeal;

public class SuperstarCard
{
    public string Name { get; }
    public string Logo { get; }
    public int HandSize { get; }
    public int SuperstarValue { get; }
    public string SuperstarAbility { get; }
    public ISuperstarAbility Ability { get; private set; }

    public SuperstarCard(string name, string logo, int handSize, int superstarValue, string superstarAbility)
    {
        Name = name;
        Logo = logo;
        HandSize = handSize;
        SuperstarValue = superstarValue;
        SuperstarAbility = superstarAbility;
        AssignAbilityBasedOnName();
    }
    
    public bool HasAbility(string name)
    {
        return Name.Equals(name, StringComparison.OrdinalIgnoreCase);
    }
    
    private void AssignAbilityBasedOnName()
    {
        switch (Name.ToUpper())
        {
            case "THE UNDERTAKER":
                Ability = new UndertakerAbility();
                break;
            case "CHRIS JERICHO":
                Ability = new JerichoAbility();
                break;
            case "STONE COLD STEVE AUSTIN":
                Ability = new StoneColdSteveAustinAbility();
                break;
            case "MANKIND":
                Ability = new MankindAbility();
                break;
            case "THE ROCK":
                Ability = new TheRockAbility();
                break;
            default:
                Ability = new NoAbility();
                break;
        }
    }

}
