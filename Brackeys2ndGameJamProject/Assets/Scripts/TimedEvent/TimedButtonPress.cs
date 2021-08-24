using Assets.Scripts;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace timedButton
{
  public class TimedButtonPress : TimedEvent
  {
    [SerializeField]
    TMP_Text keyValueText = null;

    [SerializeField]
    Image collapsingRing = null;

    private string key = "";

    public void instantiateInstance(float maxDuration, string key)
    {
      this.maxDuration = maxDuration;
      this.key = key;
      keyValueText.text = key;
    }

    protected void Update()
    {
      if (!ShouldUpdate())
      {
        return;
      }

      if (Input.GetKeyDown(key))
      {
        resolved = true;
      }
    }

    protected override void updateTimerVisuals(float timeElapsed)
    {
      var maxScale = 2f;
      collapsingRing.rectTransform.localScale = Vector3.one * (maxScale - Mathf.Clamp(timeElapsed / maxDuration, 0, 1));
    }

    public override IEnumerator AnimateThenDestroySelf()
    {
      Destroy(this.gameObject);
      yield return null;
    }

    public override Scores.Scores EvaluateScore()
    {
      throw new NotImplementedException();
    }
  }
}
