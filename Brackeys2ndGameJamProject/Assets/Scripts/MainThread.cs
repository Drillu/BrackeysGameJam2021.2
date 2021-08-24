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

    private int score = 0;

    private IEnumerator CurrentLevel = null;

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
        timedSpinner.instantiateSpinner(3000, 10f);

        timedSpinner.StartAction();
        while (!timedSpinner.Resolved)
        {
          yield return null;
        }

        StartCoroutine(timedSpinner.AnimateThenDestroySelf());
        //var buttonPress = Instantiate(buttonPressPrototype, transform);
        //var index = Random.Range(0, validKeys.Count);

        //var keyToPress = validKeys[index];
        //buttonPress.instantiateInstance(1f, keyToPress);
        //buttonPress.gameObject.SetActive(true);

        //yield return buttonPress.StartAction();

        //Destroy(buttonPress.gameObject);
      }
    }

  }
}
