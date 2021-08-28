using Assets.Scripts;
using listExtensions;
using ScorableAction;
using System.Collections;
using System.Collections.Generic;
using timedButton;
using UnityEngine;
using System.Linq;
using System;
using timedSpinner;
using blockingEvent;
using timedClick;

namespace Assets.Scenes
{
  // This is inteded to be the base class for executing things that are happening within the game.
  public class MainThread : MonoBehaviour
  {
    [SerializeField]
    private TimedButtonPress keyPressPrototype = null;

    [SerializeField]
    RectTransform keySpawnContainer = null;

    [SerializeField]
    private TimedSpinner timedSpinnerPrototype = null;

    [SerializeField]
    Transform spinnerContainer = null;

    [SerializeField]
    private TimedClickEvent clickEventPrototype = null;

    [SerializeField]
    RectTransform clickEventContainer = null;

    [SerializeField]
    CurtainBlockingEvent curtainBlockingEvent = null;

    private List<string> validKeys = new List<string> { "a", "s", "d", "f" };

    private int totalPossibleScore = 0;
    private int currentScore = 0;

    private bool levelResult = false;

    public Scores.Scores CurrentScore => Scores.ScoreHelpers.GetScoreForPercentageAmount(currentScore / totalPossibleScore);
    public int ScoreVersion { get; private set; }

    List<ITimedAction> actionQueue = new List<ITimedAction>();

    ITimedAction nextAction = null;

    float scorePercentage;

    public IEnumerator Start()
    {
      scorePercentage = 100f;
      yield return RunGame();
    }

    private IEnumerator RunGame()
    {
      var level1StartTime = 0;
      var level1Duration = 30;
      // Level 1 (Introduction to the keys!)
      curtainBlockingEvent.instantiateCurtainEvent(1, true, false);
      yield return curtainBlockingEvent.RunEvent();
      generateKeys(level1StartTime, level1Duration, 1f, 1);
      generateButtons(level1StartTime, level1Duration, 1f, 1, 2);
      generateSpinners(level1StartTime, level1Duration, 720f, 5, 1);
      yield return RunLevel();

      var level2StartTime = 0;
      var level2Duration = 30;
      // Level 2 (Introduction to the spinner!)
      curtainBlockingEvent.instantiateCurtainEvent(2, true, false);
      yield return curtainBlockingEvent.RunEvent();
      generateKeys(level2StartTime, level2Duration, .5f, 1);
      yield return RunLevel();
      //generateSpinners(level1StartTime, level1Duration, 3000, 5, .2f);
      // Level 3 (Heating up the spinner + button difficulty a bit!)
    }

    // Takes the current actionQueue and runs each item inside of it according to how much time has passed.
    private IEnumerator RunLevel()
    {
      actionQueue = actionQueue.OrderBy(item => item.StartTime).ToList();
      nextAction = actionQueue.Count > 0 ? actionQueue.PopAt(0) : null;
      float timePassed = 0f;
      ITimedAction lastAction = null;

      while (nextAction != null)
      {
        //The user has gameOvered!
        if (scorePercentage < 0)
        {
          levelResult = false;
          yield break;
        }
        if (nextAction.StartTime <= timePassed)
        {
          StartCoroutine(StartTimedAction(nextAction));
          lastAction = nextAction;
          nextAction = actionQueue.Count > 0 ? actionQueue.PopAt(0) : null;
        }

        timePassed += Time.deltaTime;
        yield return null;
      }

      if (lastAction == null)
      {
        yield break;
      }

      while (!lastAction.Resolved)
      {
        yield return null;
      }

      levelResult = scorePercentage > 0;
    }

    private void generateSpinners(float startTime, float timeFrame, float requiredRotation, float duration, float scoreModifier, float maxTimeBetween = 0f)
    {
      float tempTime = startTime;
      int totalAdded = 0;
      while (tempTime < timeFrame)
      {
        totalAdded += 1;

        var timedSpinner = Instantiate(timedSpinnerPrototype, spinnerContainer);
        timedSpinner.StartTime = tempTime;
        timedSpinner.ScoreModifier = scoreModifier;
        timedSpinner.instantiateSpinner(requiredRotation, duration);
        timedSpinner.gameObject.SetActive(false);
        tempTime += duration + maxTimeBetween + generateRandomTimeAmount(0, 3);
        actionQueue.Add(timedSpinner);
      }
    }

    // TODO we probably eventually want to support overlapping press keys, will need a quick algorithm to make 
    // sure we don't generate the same key in the same timeframe
    private void generateKeys(float startTime, float timeFrame, float duration, float scoreModifier, float timeBetween = .1f)
    {
      float tempTime = startTime;
      while (tempTime < timeFrame)
      {
        var randomNumber = UnityEngine.Random.Range(0, validKeys.Count);
        var randomKey = validKeys[randomNumber];

        var timedKeyEvent = Instantiate(keyPressPrototype, keySpawnContainer);
        timedKeyEvent.GetComponent<RectTransform>().anchoredPosition = generateRandomPointWithinBounds(keySpawnContainer);

        timedKeyEvent.StartTime = tempTime;
        timedKeyEvent.ScoreModifier = scoreModifier;

        //For now just giving it an arbitrary offset time, to see how it feels
        timedKeyEvent.instantiateInstance(duration, randomKey);

        timedKeyEvent.gameObject.SetActive(false);
        tempTime += duration + timeBetween;
        actionQueue.Add(timedKeyEvent);
      }
    }

    private void generateButtons(float startTime, float timeFrame, float duration, float scoreModifier, float timeBetween = .1f)
    {
      float tempTime = startTime;
      while (tempTime < timeFrame)
      {
        var randomNumber = UnityEngine.Random.Range(0, validKeys.Count);
        var randomKey = validKeys[randomNumber];

        var timedKeyEvent = Instantiate(clickEventPrototype, clickEventContainer);
        timedKeyEvent.GetComponent<RectTransform>().anchoredPosition = generateRandomPointWithinBounds(clickEventContainer);

        timedKeyEvent.StartTime = tempTime;
        timedKeyEvent.ScoreModifier = scoreModifier;

        //For now just giving it an arbitrary offset time, to see how it feels
        timedKeyEvent.instantiateInstance(duration, randomKey);

        timedKeyEvent.gameObject.SetActive(false);
        tempTime += timeBetween;
        actionQueue.Add(timedKeyEvent);
      }
    }

    private IEnumerator StartTimedAction(ITimedAction action)
    {
      action.StartAction();

      while (!action.Resolved)
      {
        yield return null;
      }

      if (action is IScorableAction scoredAction)
      {
        var scoreRank = scoredAction.EvaluateScore();
        var performancePercentageChange = Scores.ScoreHelpers.ScoreToOverallPercentage(scoreRank);
        var rawScore = Scores.ScoreHelpers.ScoreToInt(scoreRank);
        var finalScore = rawScore * scoredAction.ScoreModifier;
        var totalPossibleScore = Scores.ScoreHelpers.MaxScore * scoredAction.ScoreModifier;

        // We are losing floating point precision here, totes okay though
        UpdateTotalScore((int)finalScore, (int)totalPossibleScore, performancePercentageChange);        
      }

      StartCoroutine(action.AnimateThenDestroySelf());
    }

    public void UpdateTotalScore(int score, int totalPossible, float performancePercentageChange)
    {
      //We could do like a combo thing here! Maybe we could have a discussion about it.
      currentScore += score;
      totalPossibleScore += totalPossible;
      scorePercentage += performancePercentageChange;
    }

    public static float generateRandomTimeAmount(float minimumInSeconds, float maximumInSeconds)
    {
      //Convert seconds to ms, then grab a random number in ms
      var random = UnityEngine.Random.Range(minimumInSeconds * 1000, maximumInSeconds * 1000);
      //Convert back from ms to seconds
      return (float)random / 1000f;
    }

    public static Vector3 generateRandomPointWithinBounds(RectTransform bounds)
    {
      var x = bounds.rect.x;
      var y = bounds.rect.y;

      var randomX = UnityEngine.Random.Range(0, x);
      var randomY = UnityEngine.Random.Range(0, y);

      return new Vector3(randomX, randomY, 0);
    }
  }
}
