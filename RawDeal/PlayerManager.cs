using RawDealView;

namespace RawDeal;

public class PlayerManager
{
    private readonly View _view;
    private readonly string _deckFolder;
    private Player _player1;
    private Player _player2;
    private int _startingPlayer;
    private int _activePlayer;

    public PlayerManager(View view, string deckFolder)
    {
        _view = view;
        _deckFolder = deckFolder;
    }

    public bool InitializeGame()
    {
        try
        {
            Player player1 = CreatePlayer();
            Player player2 = CreatePlayer();

            SetPlayers(player1, player2);
        }
        catch (InvalidDeckException)
        {
            return false;
        }

        return true;
    }
    
    private Player CreatePlayer()
    {
        string deckPath = _view.AskUserToSelectDeck(_deckFolder);
        Player player = CreatePlayerWithValidDeck(deckPath);
        return player;
    }
    
    private Player CreatePlayerWithValidDeck(string deckPath)
    {
        Deck deck = new Deck(deckPath);

        if (!deck.IsValid())
        {
            _view.SayThatDeckIsInvalid();
            throw new InvalidDeckException("El mazo no es válido.");
        }

        Player player = new Player(deck.SuperstarCard, deck.NormalCards);

        return player;
    }

    private void SetPlayers(Player player1, Player player2)
    {
        _player1 = player1;
        _player2 = player2;
    }

    public Player GetActivePlayer() => _activePlayer == 1 ? _player1 : _player2;
    public Player GetOpponentPlayer() => _activePlayer == 1 ? _player2 : _player1;

    public void SwitchActivePlayer()
    {
        _activePlayer = _activePlayer == 1 ? 2 : 1;
    }

    public void DetermineStartingAndActivePlayer()
    {
        DetermineStartingPlayer();
        DetermineActivePlayer();
    }

    private void DetermineStartingPlayer()
    {
        bool player1Starts = _player1.SuperstarCard.SuperstarValue >= 
                             _player2.SuperstarCard.SuperstarValue;
        _startingPlayer = player1Starts ? 1 : 2;
    }

    private void DetermineActivePlayer()
    {
        _activePlayer = _startingPlayer;
    }
    
    public void ResetCardEffectState()
    {
        GetActivePlayer().ClearBonusList();
        GetOpponentPlayer().ClearBonusList();
    }
}
