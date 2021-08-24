using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace timedButton
{
  public class TimedButtonPress : MonoBehaviour, ITimedAction
  {
    [SerializeField]
    TMP_Text keyValueText = null;

    [SerializeField]
    Image collapsingRing = null;

    private string key = "";
    private float maxDuration = 0f;
    private bool resolved = false;

    public void instantiateInstance(float maxDuration, string key)
    {
      this.maxDuration = maxDuration;
      this.key = key;
      keyValueText.text = key;
    }

    public IEnumerator StartAction()
    {
      yield return tickDown();
    }


    public void Update()
    {
      if (Input.GetKeyDown(key))
      {
        resolved = true;
      }
    }

    public IEnumerator tickDown()
    {
      float timeElapsed = 0f;
      var maxScale = 2f;
      while (timeElapsed < maxDuration && !resolved)
      {
        timeElapsed += Time.deltaTime;
        collapsingRing.rectTransform.localScale = Vector3.one * (maxScale - Mathf.Clamp(timeElapsed / maxDuration, 0, 1));
        yield return null;
      }
    }

    //
    public IEnumerator AnimateThenDestroySelf()
    {
      yield return null;
    }


    public int EvaluatePoints()
    {
      throw new NotImplementedException();
    }

    public void Resolve(ResolveState resolveState)
    {
    }
  }
}
