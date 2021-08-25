using Assets.Scripts;
using listExtensions;
using ScorableAction;
using System.Collections;
using System.Collections.Generic;
using timedButton;
using UnityEngine;
using System.Linq;

namespace Assets.Scenes
{
  // This is inteded to be the base class for executing things that are happening within the game.
  public class MainThread : MonoBehaviour
  {
    [SerializeField]
    private TimedButtonPress keyPressPrototype = null;

    [SerializeField]
    Transform buttonContainer = null;

    [SerializeField]
    private TimedSpinner timedSpinnerPrototype = null;

    [SerializeField]
    Transform spinnerContainer = null;

    private List<string> validKeys = new List<string> { "a", "s", "d", "f" };

    int score = 0;

    List<ITimedAction> actionQueue = new List<ITimedAction>();

    bool started = false;

    ITimedAction nextAction = null;

    float timePassed = 0f;

    public void Start()
    {
      populateGameFlow();
      actionQueue = actionQueue.OrderBy(item => item.StartTime).ToList();
      started = true;
    }

    public void Update()
    {
      if (!started)
      {
        return;
      }

      timePassed += Time.deltaTime;
      if (nextAction == null)
      {
        if (actionQueue.Count > 0)
        {
          nextAction = actionQueue.PopAt(0);
        }
        else
        {
          return;
        }
      }

      if (nextAction.StartTime <= timePassed)
      {
        StartCoroutine(StartTimedAction(nextAction));
        nextAction = null;
      }
    }

    private void populateGameFlow()
    {
      var level1StartTime = 0;
      var level1Duration = 60;
      //generateStartOfGame
      generateSpinners(level1StartTime, level1Duration, 3000, 5);
      generateKeys(level1StartTime, level1Duration, 1);

      //var level2StartTime = 70;
      //var level2Duration = 60;
      //generateSpinners(level1StartTime, level1Duration, 1020, 3);
      //generateKeys(level1StartTime, level1Duration, 1);

    }

    private void generateSpinners(float startTime, float timeFrame, float requiredRotation, float duration, float maxTimeBetween = 0f)
    {
      float tempTime = startTime;
      int totalAdded = 0;
      while (tempTime < timeFrame)
      {
        totalAdded += 1;

        var timedSpinner = Instantiate(timedSpinnerPrototype, spinnerContainer);
        timedSpinner.StartTime = tempTime;
        timedSpinner.instantiateSpinner(requiredRotation, duration);
        timedSpinner.gameObject.SetActive(false);
        tempTime += duration + maxTimeBetween + Random.Range(0, 3);
        actionQueue.Add(timedSpinner);
      }
    }

    // TODO we probably eventually want to support overlapping press keys, will need a quick algorithm to make 
    // sure we don't generate the same key in the same timeframe
    private void generateKeys(float startTime, float timeFrame, float duration)
    {
      float tempTime = startTime;
      while (tempTime < timeFrame)
      {
        var timedKeyEvent = Instantiate(keyPressPrototype, buttonContainer);
        timedKeyEvent.StartTime = tempTime;
        var randomKey = Random.Range(0, validKeys.Count);
        timedKeyEvent.instantiateInstance(duration, validKeys[randomKey]);
        timedKeyEvent.gameObject.SetActive(false);
        tempTime += duration + Random.Range(0, 2);
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
        score += Scores.ScoreHelpers.ScoreToInt(scoreRank);
        Debug.Log(scoreRank.ToString());
      }

      StartCoroutine(action.AnimateThenDestroySelf());
    }
  }
}
