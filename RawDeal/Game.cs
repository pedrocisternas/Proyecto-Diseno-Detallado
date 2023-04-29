using RawDealView;
using RawDealView.Formatters;
using RawDealView.Options;

namespace RawDeal;

public class Game
{
    private readonly View _view;
    private readonly string _deckFolder;
    private Player _player1;
    private Player _player2;
    private bool GameIsRunning { get; set; }
    private int StartingPlayer { get; set; }
    private int ActivePlayer { get; set; }
    
    public Game(View view, string deckFolder)
    {
        _view = view;
        _deckFolder = deckFolder;
        GameIsRunning = true;
    }
    public void Play()
    {
        if (!InitializeGame())
        {
            return;
        }

        StartGame();
    }
    private bool InitializeGame()
    {
        Player player1 = CreatePlayer();
        if (player1 == null) return false;

        Player player2 = CreatePlayer();
        if (player2 == null) return false;

        SetPlayers(player1, player2);
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
            return null;
        }

        Player player = new Player(deck.SuperstarCard, deck.NormalCards);

        return player;
    }
    private void SetPlayers(Player player1, Player player2)
    {
        _player1 = player1;
        _player2 = player2;
    }
    private void StartGame()
    {
        DetermineStartingPlayer();
        DetermineActivePlayer();
    
        _view.SayThatATurnBegins(GetActivePlayer().SuperstarCard.Name);
        _player1.DrawStartingCards();
        _player2.DrawStartingCards();
        RunGameLoop();
    }
    private void DetermineStartingPlayer()
    {
        bool player1Starts = _player1.SuperstarCard.SuperstarValue >= _player2.SuperstarCard.SuperstarValue;
        StartingPlayer = player1Starts ? 1 : 2;
    }
    private void DetermineActivePlayer()
    {
        ActivePlayer = StartingPlayer;
    }
    private void RunGameLoop()
    {
        HandleStartTurn();
        while (GameIsRunning)
        {
            ShowPlayersInfo();
            PlayTurnDependingOnDisplayedMenu();
        }
    }
    private void HandleStartTurn()
    {
        ActivateSuperstarAbility();
        GetActivePlayer().DrawOneCard();
    }
    private void ShowPlayersInfo()
    {
        PlayerInfo activePlayerInfo = new PlayerInfo(GetActivePlayer().SuperstarCard.Name, GetActivePlayer().GetFortitude(), GetActivePlayer().GetHandCount(), GetActivePlayer().GetDeckCount());
        PlayerInfo opponentPlayerInfo = new PlayerInfo(GetOpponentPlayer().SuperstarCard.Name, GetOpponentPlayer().GetFortitude(), GetOpponentPlayer().GetHandCount(), GetOpponentPlayer().GetDeckCount());
        
        _view.ShowGameInfo(activePlayerInfo, opponentPlayerInfo);
    }
    private void PlayTurnDependingOnDisplayedMenu()
    {
        if (GetActivePlayer().PlayerCanUseAbilityAndNeedsMenu(GetOpponentPlayer()))
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
            HandleEndTurn();
        }
        else if (userInput == NextPlay.PlayCard)
        {
            HandlePlayCard();
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
        _view.CongratulateWinner(GetOpponentPlayer().SuperstarCard.Name);
    }
    private void HandleEndTurn()
    {
        if (GetActivePlayer().GetDeckCount() == 0)
        {
            EndGameAndCongratulateWinner(GetOpponentPlayer().SuperstarCard.Name);
        }
        else
        {
            EndTurnAndStartNext();
        }
    }
    private void HandlePlayCard()
    {
        List<(NormalCard card, string type)> playableCardsWithType = CardUtils.GetPlayableCards(GetActivePlayer());
        int chosenCardIndex = SelectCardToPlay(playableCardsWithType);

        if (chosenCardIndex != -1)
        {
            (NormalCard selectedCard, string selectedType) = playableCardsWithType[chosenCardIndex];
            PlaySelectedCard(selectedCard, selectedType);
        }
    }
    private void PlaySelectedCard(NormalCard selectedCard, string selectedType)
    {
        int damage = int.Parse(selectedCard.Damage);

        DisplayPlayerIsTryingToPlayCard(selectedCard, selectedType);

        _view.SayThatPlayerSuccessfullyPlayedACard();

        GetActivePlayer().IncreaseFortitude(damage);
        GetActivePlayer().RemoveCardFromHand(selectedCard);
        GetActivePlayer().AddCardToRingArea(selectedCard);
        
        DamageResult damageResult = GetOpponentPlayer().ReceiveDamage(damage);
        ShowAppliedDamage(damageResult.OverturnedCards, damageResult.AppliedDamage);
    }
    private void HandleShowCards()
    {
        CardSet setOfCards = _view.AskUserWhatSetOfCardsHeWantsToSee();
        ShowCardsWithFormat(setOfCards);
    }
    private void HandleUseAbility()
    {
        GetActivePlayer().ExecuteAbility(GetOpponentPlayer(), _view);
    }
    private void EndTurnAndStartNext()
    {
        GetActivePlayer().EndTurn();
        SwitchActivePlayer();
        _view.SayThatATurnBegins(GetActivePlayer().SuperstarCard.Name);
        HandleStartTurn();
    }
    private void ShowCardsWithFormat(CardSet setOfCards)
    {
        List<NormalCard> cardsToFormat = GetCardsByCardSet(setOfCards);
        List<string> formattedCards = CardUtils.GetFormattedCards(cardsToFormat);
        _view.ShowCards(formattedCards);
    }
    private List<NormalCard> GetCardsByCardSet(CardSet setOfCards)
    {
        Dictionary<CardSet, Func<List<NormalCard>>> cardSetMapping = new Dictionary<CardSet, Func<List<NormalCard>>>
        {
            { CardSet.Hand, () => GetActivePlayer().GetHandCards() },
            { CardSet.RingArea, () => GetActivePlayer().GetRingAreaCards() },
            { CardSet.RingsidePile, () => GetActivePlayer().GetRingsideCards() },
            { CardSet.OpponentsRingArea, () => GetOpponentPlayer().GetRingAreaCards() },
            { CardSet.OpponentsRingsidePile, () => GetOpponentPlayer().GetRingsideCards() }
        };

        return cardSetMapping[setOfCards]();
    }
    private Player GetActivePlayer() => ActivePlayer == 1 ? _player1 : _player2;
    private Player GetOpponentPlayer() => ActivePlayer == 1 ? _player2 : _player1;
    private void SwitchActivePlayer()
    {
        if (ActivePlayer == 1)
        {
            ActivePlayer = 2;
        }
        else
        {
            ActivePlayer = 1;
        }
    }
    private void EndGame()
    {
        GameIsRunning = false;
    }
    private void EndGameAndCongratulateWinner(string winnerName)
    {
        EndGame();
        _view.CongratulateWinner(winnerName);
    }
    private int SelectCardToPlay(List<(NormalCard card, string type)> playableCardsWithType)
    {
        List<string> playableCardStrings = playableCardsWithType.Select(cardWithType =>
        {
            IViewableCardInfo cardInfo = cardWithType.card;
            // No debería ser necesario, pero chequear
            // string cardFormatted = Formatter.CardToString(cardInfo);
            string playedAs = cardWithType.type.ToUpper();
            PlayInfo playInfo = new PlayInfo(cardInfo, playedAs);
            return Formatter.PlayToString(playInfo);
        }).ToList();

        int chosenCardIndex = _view.AskUserToSelectAPlay(playableCardStrings);

        return chosenCardIndex;
    }
    private void DisplayPlayerIsTryingToPlayCard(NormalCard card, string cardType)
    {
        // string playInfo = Formatter.CardToString(card.Title,
        //     card.Fortitude, card.Damage,
        //     card.StunValue, card.Types, card.Subtypes,
        //     card.CardEffect);
        // _view.SayThatPlayerIsTryingToPlayThisCard(GetActivePlayer().SuperstarCard.Name, Formatter.PlayToString(playInfo, cardType.ToUpper()));
        IViewableCardInfo cardInfo = card;
        string playedAs = cardType.ToUpper();

        PlayInfo playInfo = new PlayInfo(cardInfo, playedAs);
        string formattedPlay = Formatter.PlayToString(playInfo);

        _view.SayThatPlayerIsTryingToPlayThisCard(GetActivePlayer().SuperstarCard.Name, formattedPlay);

    }

    private void ShowAppliedDamage(List<NormalCard> overturnedCards, int damage)
    {
        _view.SayThatOpponentWillTakeSomeDamage(GetOpponentPlayer().SuperstarCard.Name, damage);
        for (int i = 0; i < overturnedCards.Count; i++)
        {
            NormalCard overturnedCard = overturnedCards[i];
            ShowOverturnedCardInfo(overturnedCard, i + 1, damage);
        }
        CheckGameOverAndHandle();
    }
    private void ShowOverturnedCardInfo(NormalCard overturnedCard, int currentIndex, int damage)
    {
        // string overturnedCardInfo = Formatter.CardToString(overturnedCard.Title,
        //     overturnedCard.Fortitude, overturnedCard.Damage,
        //     overturnedCard.StunValue, overturnedCard.Types, overturnedCard.Subtypes,
        //     overturnedCard.CardEffect);
        // _view.ShowCardOverturnByTakingDamage(overturnedCardInfo, currentIndex, damage);
        IViewableCardInfo cardInfo = overturnedCard;
        string formattedCardInfo = Formatter.CardToString(cardInfo);

        _view.ShowCardOverturnByTakingDamage(formattedCardInfo, currentIndex, damage);

    }
    private bool CheckGameOverAndHandle()
    {
        if (GetOpponentPlayer().GetDeckCount() == 0)
        {
            EndGameAndCongratulateWinner(GetActivePlayer().SuperstarCard.Name);
            return true;
        }
        return false;
    }
    private void ActivateSuperstarAbility()
    {
        if (GetActivePlayer().SuperstarCard.HasAbility("Kane"))
        {
            _view.SayThatPlayerIsGoingToUseHisAbility(GetActivePlayer().SuperstarCard.Name, GetActivePlayer().SuperstarCard.SuperstarAbility);
            DamageResult damageResult = GetOpponentPlayer().ReceiveDirectDamage(1);
            ShowAppliedDamage(damageResult.OverturnedCards, damageResult.AppliedDamage);
        }
        ISuperstarAbility ability = GetActivePlayer().SuperstarCard.Ability;
        if (ability.IsAutomatic() && ability.CanUseAbility(GetActivePlayer(), GetOpponentPlayer()))
        {
            ability.Execute(GetActivePlayer(), GetOpponentPlayer(), _view);
        }
    }
}
