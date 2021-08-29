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
using bannerBlock;
using UnityEngine.UI;
using penguinRenderer;
using audio;
using tutorialBlockingEvent;

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

    [SerializeField]
    Slider performanceSlider = null;

    [SerializeField]
    PenguinRenderer penguinRenderer = null;

    [SerializeField]
    TutorialBlockingEvent tutorialBlocker = null;

    [Serializable]
    private struct Banner
    {
      public int level;
      public BannerBlockingEvent curtain;
    }

    [SerializeField]
    List<Banner> banners = new List<Banner>();

    private int totalPossibleScore = 0;
    private int currentScore = 0;

    private bool levelResult = false;

    public Scores.Scores CurrentScore => Scores.ScoreHelpers.GetScoreForPercentageAmount(currentScore / totalPossibleScore);
    public int ScoreVersion { get; private set; }

    List<ITimedAction> actionQueue = new List<ITimedAction>();

    ITimedAction nextAction = null;

    float scorePercentage;

    AudioManager audioManager;

    public IEnumerator Start()
    {
      audioManager = FindObjectOfType<AudioManager>();
      ResetState();
      // Run the tutorial on start
      yield return tutorialBlocker.RunEvent();
      yield return StartGame();
    }

    public void ResetState()
    {
      scorePercentage = 1f;
      performanceSlider.value = scorePercentage;
      penguinRenderer.SetPenguin(0);
    }
    public IEnumerator StartGame()
    {
      ResetState();
      yield return RunGame();
    }

    private IEnumerator RunGame()
    {
      var keysScoreModifier = 1f;
      var buttonsScoreModifier = 1f;
      var spinnerScoreModifier = 4f;
      var spinnerAmountNeeded = 2000f;

      var level1StartTime = 0;
      var level1Duration = 20;
      var level1Keys = new List<string> {"a"};

      // Level 1 Slow keys with one type
      //yield return banners.First(banner => banner.level == 1).curtain.RunEvent();
      generateKeys(level1StartTime, level1Duration, 2f, keysScoreModifier, level1Keys);
      generateButtons(level1StartTime, level1Duration, 1f, buttonsScoreModifier, 1.5f);


      yield return RunLevel();
      if (!levelResult)
      {
        runEndOfGameFlow();
        yield break;
      }

      // Level 2 More keys and speeding up a bit!
      var level2StartTime = 0;
      var level2Duration = 20;
      var level2Keys = new List<string> { "a" , "s" };

      yield return banners.First(banner => banner.level == 2).curtain.RunEvent();

      generateKeys(level2StartTime, level2Duration, 1f, keysScoreModifier, level2Keys);

      yield return RunLevel();
      if (!levelResult)
      {
        runEndOfGameFlow();
        yield break;
      }

      // Level 3 (Adding in some buttons!)
      var level3StartTime = 0;
      var level3Duration = 20;
      var level3Keys = new List<string> { "a", "s" };

      yield return banners.First(banner => banner.level == 3).curtain.RunEvent();

      generateKeys(level3StartTime, level3Duration, 1f, keysScoreModifier, level3Keys);
      generateButtons(level3StartTime, level3Duration, 1f, buttonsScoreModifier, 1.5f);

      yield return RunLevel();
      if (!levelResult)
      {
        runEndOfGameFlow();
        yield break;
      }

      // Level 4 (Adding in spinners)
      var level4StartTime = 0;
      var level4Duration = 20;
      var level4Keys = new List<string> { "a", "s" };

      yield return banners.First(banner => banner.level == 4).curtain.RunEvent();

      generateKeys(level4StartTime, level4Duration, 1f, keysScoreModifier, level4Keys);
      generateButtons(level4StartTime, level4Duration, 1f, buttonsScoreModifier, 1.5f);
      generateSpinners(level4StartTime, level4Duration, spinnerAmountNeeded, spinnerScoreModifier, 2f);

      yield return RunLevel();
      if (!levelResult)
      {
        runEndOfGameFlow();
        yield break;
      }


      // Level 5 (Adding more keys!)
      var level5StartTime = 0;
      var level5Duration = 40;
      var level5Keys = new List<string> { "a", "s", "d", "f" };

      yield return banners.First(banner => banner.level == 5).curtain.RunEvent();

      generateKeys(level5StartTime, level5Duration, .8f, keysScoreModifier, level5Keys);
      generateButtons(level5StartTime, level5Duration, .8f, buttonsScoreModifier, 3f);
      generateSpinners(level5StartTime, level5Duration, spinnerAmountNeeded, spinnerScoreModifier, 1f);

      yield return RunLevel();
      if (!levelResult)
      {
        runEndOfGameFlow();
        yield break;
      }

      // Ramping up difficulty!
      var infiniteLevelStartTime = 0;
      var infiniteLevelDuration = 30;
      var infiniteLevelKeys = new List<string> { "a", "s", "d", "f" };

      var keysDuration = .8f;
      var buttonsDuration = .8f;
      var spinnerDuration = 10f;

      var keysTimeBetween = 3f;
      //var buttonsTimeBetween = 2f;
      //var spinnerTimeBetween = 10f;
      while (levelResult)
      {
        generateKeys(infiniteLevelStartTime, infiniteLevelDuration, keysDuration, keysScoreModifier, level5Keys);
        generateButtons(infiniteLevelStartTime, infiniteLevelDuration, buttonsDuration, buttonsScoreModifier, keysTimeBetween);
        generateSpinners(infiniteLevelStartTime, infiniteLevelDuration, spinnerAmountNeeded, spinnerDuration, spinnerScoreModifier);
        yield return RunLevel();
        if (!levelResult)
        {
          runEndOfGameFlow();
          yield break;
        }
        else
        {
          keysDuration = Mathf.Max(keysDuration - .1f, .2f);
          buttonsDuration = Mathf.Max(buttonsDuration - .1f, .4f);
          spinnerDuration = Mathf.Max(spinnerDuration - .5f, 5f);
          keysTimeBetween = Mathf.Max(keysTimeBetween - .5f, 1f);
          spinnerAmountNeeded += 50f;
        }
      }
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
        if (scorePercentage <= 0)
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
    
    private void runEndOfGameFlow()
    {
      audioManager.PlayMenuMusic();
      StartCoroutine(endOfGame());
    }

    private IEnumerator endOfGame()
    {
      curtainBlockingEvent.instantiateCurtainEvent(false);
      yield return curtainBlockingEvent.RunEvent();
      yield return StartGame();
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
        tempTime += duration + maxTimeBetween;
        actionQueue.Add(timedSpinner);
      }
    }

    private void generateKeys(float startTime, float timeFrame, float duration, float scoreModifier, List<string> possibleKeys, float timeBetween = .1f)
    {
      float tempTime = startTime;
      while (tempTime < timeFrame)
      {
        var randomNumber = UnityEngine.Random.Range(0, possibleKeys.Count);
        var randomKey = possibleKeys[randomNumber];

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
        var timedKeyEvent = Instantiate(clickEventPrototype, clickEventContainer);
        timedKeyEvent.GetComponent<RectTransform>().anchoredPosition = generateRandomPointWithinBounds(clickEventContainer);

        timedKeyEvent.StartTime = tempTime;
        timedKeyEvent.ScoreModifier = scoreModifier;

        //For now just giving it an arbitrary offset time, to see how it feels
        timedKeyEvent.instantiateInstance(duration, "");

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
        var performancePercentageChange = Scores.ScoreHelpers.ScoreToOverallPercentage(scoreRank) * scoredAction.ScoreModifier;
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
      if (scorePercentage == 0)
      {
        return; // We already lost, nothing more to do here.
      }
      //We could do like a combo thing here! Maybe we could have a discussion about it.
      currentScore += score;
      totalPossibleScore += totalPossible;
      scorePercentage = Mathf.Clamp01(scorePercentage + performancePercentageChange);
      performanceSlider.value = scorePercentage;
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

      var randomX = UnityEngine.Random.Range(-x, x);
      var randomY = UnityEngine.Random.Range(-y, y);

      return new Vector3(randomX, randomY, 0);
    }
  }
}
