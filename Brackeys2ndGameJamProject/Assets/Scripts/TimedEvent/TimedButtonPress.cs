using Assets.Scripts;
using fadeOnCommand;
using ScorableAction;
using System;
using System.Collections;
using timedEvent;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace timedButton
{
  public class TimedButtonPress : TimedEvent, IScorableAction
  {
    private readonly string Fade = "Fade";

    [SerializeField]
    TMP_Text keyValueText = null;

    [SerializeField]
    Image collapsingRing = null;

    [SerializeField]
    Image buttonImage = null;

    [SerializeField]
    Animator animator = null;

    [SerializeField]
    TMP_Text scoreValue = null;

    [SerializeField]
    FadeOnCommand fadeOnCommand = null;

    private string key = "";
    private float targetTime = 0f;
    private Scores.Scores score = Scores.Scores.F;

    public void instantiateInstance(float maxDuration, float targetTime, string key)
    {
      this.maxDuration = maxDuration;
      this.key = key;
      this.targetTime = targetTime;
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
      var maxScale = 3f;
      var shrinkScale = 2f;
      float collapsePercentage = Mathf.Clamp(timeElapsed / targetTime, 0, 1);
      collapsingRing.rectTransform.localScale = Vector3.one * (maxScale - (shrinkScale * collapsePercentage));

      //Fade the circle
      var tempColor = collapsingRing.color;
      tempColor.a = 0.8f * collapsePercentage;
      collapsingRing.color = tempColor;
    }

    public override IEnumerator AnimateThenDestroySelf()
    {
      scoreValue.text = Scores.ScoreHelpers.ScoreToFlavorString(score);
      scoreValue.color = Scores.ScoreHelpers.ScoreToColor(score);
      collapsingRing.color = Scores.ScoreHelpers.ScoreToColor(score);

      animator.SetBool(Fade, true);
      yield return fadeOnCommand.FadeElements(.8f);
      Destroy(this.gameObject);
    }

    public override Scores.Scores EvaluateScore()
    {
      var accuracy = Mathf.Abs(timeElapsed - targetTime);

      if (timeElapsed >= maxDuration)
      {
        score = Scores.Scores.F;
        return score;
      }

      switch(accuracy)
      {
        case var a when (a <= .1):
          score = Scores.Scores.SS;
          break;
        case var a when (a <= .2):
          score = Scores.Scores.S;
          break;
        case var a when (a <= .3):
          score = Scores.Scores.A;
          break;
        case var a when (a <= .4):
          score = Scores.Scores.B;
          break;
        case var a when (a <= 5):
          score = Scores.Scores.C;
          break;
        case var a when (a <= 6):
          score = Scores.Scores.F;
          break;
      }

      return score;
    }
  }
}
