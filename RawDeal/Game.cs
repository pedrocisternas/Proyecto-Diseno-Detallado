using RawDealView;
using RawDealView.Options;

namespace RawDeal;

public class Game
{
    private readonly View _view;
    private readonly PlayerManager _playerManager;
    private readonly GamePresenter _gamePresenter;
    private bool _gameIsRunning;

    public Game(View view, string deckFolder)
    {
        _view = view;
        _playerManager = new PlayerManager(view, deckFolder);
        _gamePresenter = new GamePresenter(view);
        _gameIsRunning = true;
    }
    
    public void Play()
    {
        if (!_playerManager.InitializeGame())
        {
            return;
        }

        StartGame();
    }

    private void StartGame()
    {
        ActivePlayer().AddLastPlay(new NullCard(), "");
        OpponentPlayer().AddLastPlay(new NullCard(), "");
        _playerManager.DetermineStartingAndActivePlayer();
    
        _view.SayThatATurnBegins(ActivePlayer().GetSuperstarName());

        ActivePlayer().DrawStartingCards();
        OpponentPlayer().DrawStartingCards();
        RunGameLoop();
    }
    
    private void RunGameLoop()
    {
        HandleStartTurn();
        while (_gameIsRunning)
        {
            _gamePresenter.ShowPlayersInfo(ActivePlayer(), 
                OpponentPlayer());
            PlayTurnDependingOnDisplayedMenu();
        }
    }
    
    private void HandleStartTurn()
    {
        ActivateSuperstarAbility();
        ActivePlayer().DrawCardStartingTurn();
    }
    
    private void PlayTurnDependingOnDisplayedMenu()
    {
        if (ActivePlayer().
            PlayerCanUseAbilityAndNeedsMenu(OpponentPlayer()))
        {
            PlayTurn(_view.AskUserWhatToDoWhenUsingHisAbilityIsPossible());
        }
        else
        {
            PlayTurn(_view.AskUserWhatToDoWhenHeCannotUseHisAbility());
        }
    }
    
    private void PlayTurn(NextPlay userInput)
    {
        if (userInput == NextPlay.GiveUp)
        {
            HandleGiveUp();
        }
        else if (userInput == NextPlay.EndTurn)
        {
            _playerManager.ResetCardEffectState();
            HandleEndTurn();
        }
        else if (userInput == NextPlay.PlayCard)
        {
            CardPlayer cardPlayer = new CardPlayer(_view, _playerManager, 
                HandleEndTurn, EndGameAndCongratulateWinner);
            cardPlayer.HandlePlayCard();
        }
        else if (userInput == NextPlay.ShowCards)
        {
            HandleShowCards();
        }
        else if (userInput == NextPlay.UseAbility)
        {
            HandleUseAbility();
        }
    }
    
    private void HandleGiveUp()
    {
        EndGame();
        _view.CongratulateWinner(OpponentPlayer().GetSuperstarName());
    }
    
    private void HandleEndTurn()
    {
        if (!CheckGameOverAndHandle())
        {
            EndTurnAndStartNext();
        }
    }

    private void HandleShowCards()
    {
        CardSet setOfCards = _view.AskUserWhatSetOfCardsHeWantsToSee();
        _gamePresenter.ShowCardsWithFormat(setOfCards, 
            ActivePlayer(), OpponentPlayer());
    }
    
    private void HandleUseAbility()
    {
        ActivePlayer().ExecuteAbility(OpponentPlayer(), _view);
    }
    
    private void EndTurnAndStartNext()
    {
        ActivePlayer().EndTurnLastPlay();
        ActivePlayer().EndTurn();
        _playerManager.SwitchActivePlayer();
        _view.SayThatATurnBegins(ActivePlayer().GetSuperstarName());
        HandleStartTurn();
    }
    
    private void EndGame()
    {
        _gameIsRunning = false;
    }
    
    private void EndGameAndCongratulateWinner(string winnerName)
    {
        EndGame();
        _view.CongratulateWinner(winnerName);
    }

    private bool CheckGameOverAndHandle()
    {
        if (ActivePlayer().GetDeckCount() == 0)
        {
            EndGameAndCongratulateWinner(OpponentPlayer().GetSuperstarName());
            return true;
        }
        if (OpponentPlayer().GetDeckCount() == 0)
        {
            EndGameAndCongratulateWinner(ActivePlayer().GetSuperstarName());
            return true;
        }
        return false;
    }
    
    private void ActivateSuperstarAbility()
    {
        if (ActivePlayer().SuperstarCard.HasAbility("Kane"))
        {
            KaneException();
        }
        ISuperstarAbility ability = ActivePlayer().SuperstarCard.Ability;
        if (ability.IsAutomatic() && ability.CanUseAbility(
                ActivePlayer(), OpponentPlayer()))
        {
            ability.Execute(ActivePlayer(), OpponentPlayer(), _view);
        }
    }

    private Player ActivePlayer()
    {
        return _playerManager.GetActivePlayer();
    }
    
    private Player OpponentPlayer()
    {
        return _playerManager.GetOpponentPlayer();
    }

    private void KaneException()
    {
        _view.SayThatPlayerIsGoingToUseHisAbility(
            ActivePlayer().GetSuperstarName(), 
            ActivePlayer().SuperstarCard.SuperstarAbility);
        CheckGameOverAndHandle();
        DamageResult damageResult = OpponentPlayer().ReceiveDirectDamage(1);
        _gamePresenter.ShowAppliedDamage(damageResult, OpponentPlayer());
    }
}
