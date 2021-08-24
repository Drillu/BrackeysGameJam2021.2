using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using timedButton;
using UnityEngine;

namespace Assets.Scenes
{
  // This is inteded to be the base class for executing things that are happening within the game.
  public class MainThread : MonoBehaviour
  {
    [SerializeField]
    private TimedButtonPress buttonPressPrototype = null;

    [SerializeField]
    private TimedSpinner timedSpinnerPrototype = null;

    private List<string> validKeys = new List<string> { "a", "s", "d", "f" };

    List<ITimedAction> actionQueue = new List<ITimedAction>();

    public IEnumerator Start()
    {
      //Ideally this gets called after we..you know actually start the game but here is good for now
      yield return runGame();
    }

    private IEnumerator runGame()
    {
      //Prototype of level 1
      while (true)
      {
        var timedSpinner = Instantiate(timedSpinnerPrototype, transform);
        timedSpinner.instantiateSpinner(3000, 2f);

        timedSpinner.StartAction();

        while (!timedSpinner.Resolved)
        {
          yield return null;
        }

        StartCoroutine(timedSpinner.AnimateThenDestroySelf());
        Debug.Log(timedSpinner.EvaluateScore().ToString());
        yield return new WaitForSeconds(2f);
        //var buttonPress = Instantiate(buttonPressPrototype, transform);
        //var index = Random.Range(0, validKeys.Count);

        //var keyToPress = validKeys[index];
        //buttonPress.instantiateInstance(1f, keyToPress);
        //buttonPress.gameObject.SetActive(true);

        //yield return buttonPress.StartAction();

        //Destroy(buttonPress.gameObject);
      }
    }

    private void generateSpinners(float timeFrame, float requiredRotation, float duration, float maxTimeBetween = 0f)
    {
      var tempTime = 0;
      int totalAdded = 0;
      while (tempTime < timeFrame)
      {
        totalAdded += 1;
        var timedSpinner = Instantiate(timedSpinnerPrototype, transform);
        timedSpinner.instantiateSpinner(3000, 2f);
        actionQueue.Add(timedSpinner);
      }
    }

    // TODO we probably eventually want to support overlapping press keys, will need a quick algorithm to make 
    // sure we don't generate the same key in the same timeframe
    private void generateKeys(float timeFrame, float duration)
    {
      var tempTime = 0;
      while (tempTime < timeFrame)
      {
        actionQueue.Add(Instantiate(timedSpinnerPrototype, transform));
      }
    }

    //private IEnumerator runSpinners(float duration)
    //{
    //  float currentTime = 0f;
    //  while (currentTime < duration)
    //  {

    //  }

    //}

  }
}
