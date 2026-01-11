using System.Collections;
using UnityEngine;

public class BaseExpansion : MonoBehaviour
{
    [SerializeField] private Base _base;
    [SerializeField] private BotFactory _botFactory;
    [SerializeField] private BaseFactory _baseFactory;
    [SerializeField] private FlagPlacer _flagPlacer;

    private Coroutine _expansionRoutine;

    private bool _isExpansionInProgress;
    private bool _isWaitingForBaseCreate;

    private Bot _expandingBot;
    private Base _createdBase;

    private void Awake()
    {
        _flagPlacer.Unset();

        _baseFactory.enabled = false;
        _botFactory.enabled = true;
    }

    private void OnEnable()
    {
        _base.ExpansionRequested += OnExpansionRequested;
        _botFactory.BotCreate += OnBotCreated;
        _baseFactory.BaseCreate += OnBaseCreated;
    }

    private void OnDisable()
    {
        _base.ExpansionRequested -= OnExpansionRequested;
        _botFactory.BotCreate -= OnBotCreated;
        _baseFactory.BaseCreate -= OnBaseCreated;

        UnsubscribeExpandingBot();

        if (_expansionRoutine != null)
        {
            StopCoroutine(_expansionRoutine);
            _expansionRoutine = null;
        }

        _isExpansionInProgress = false;
        _isWaitingForBaseCreate = false;

        _expandingBot = null;
        _createdBase = null;
    }

    private void OnBotCreated(Bot bot)
    {
        _base.AddBot(bot);
    }

    private void OnExpansionRequested(Base baseSender, Vector3 position)
    {
        if (_isExpansionInProgress == true)
        {
            return;
        }

        _isExpansionInProgress = true;

        if (_expansionRoutine != null)
        {
            StopCoroutine(_expansionRoutine);
        }

        _flagPlacer.Set(position);
        _baseFactory.SetSpawnPosition(position);

        _expansionRoutine = StartCoroutine(ExpansionRoutine());
    }

    private IEnumerator ExpansionRoutine()
    {
        Bot availableBot;

        while (_base.TryTakeFreeBot(out availableBot) == false)
        {
            yield return null;
        }

        _expandingBot = availableBot;
        _createdBase = null;

        _expandingBot.RelocationReached += OnExpandingBotReachedFlag;

        bool relocationStarted = _expandingBot.TryRelocateTo(_flagPlacer.FlagTransform);

        if (relocationStarted == false)
        {
            UnsubscribeExpandingBot();
            _base.AddBot(_expandingBot);

            FinishExpansion();
            yield break;
        }

        while (_expandingBot != null && _expandingBot.IsBusy == true)
        {
            yield return null;
        }

        if (_expandingBot == null)
        {
            FinishExpansion();
            yield break;
        }

        UnsubscribeExpandingBot();

        _botFactory.enabled = false;
        _baseFactory.enabled = true;

        _isWaitingForBaseCreate = true;
        _baseFactory.Produce();

        while (_createdBase == null)
        {
            yield return null;
        }

        _isWaitingForBaseCreate = false;

        _flagPlacer.Unset();
        _baseFactory.ClearSpawnPosition();

        _baseFactory.enabled = false;
        _botFactory.enabled = true;

        _createdBase.AddBot(_expandingBot);

        FinishExpansion();
    }

    private void OnExpandingBotReachedFlag(Bot bot)
    {
        if (_expandingBot != bot)
        {
            return;
        }

        bot.Reset();
    }

    private void OnBaseCreated(Base createdBase)
    {
        if (_isWaitingForBaseCreate == false)
        {
            return;
        }

        _createdBase = createdBase;
    }

    private void FinishExpansion()
    {
        _isExpansionInProgress = false;

        if (_expansionRoutine != null)
        {
            StopCoroutine(_expansionRoutine);
        }

        _expansionRoutine = null;
        _expandingBot = null;
        _createdBase = null;
    }

    private void UnsubscribeExpandingBot()
    {
        if (_expandingBot != null)
        {
            _expandingBot.RelocationReached -= OnExpandingBotReachedFlag;
        }
    }
}
