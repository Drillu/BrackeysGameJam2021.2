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
    Image VFXImage = null;

    [SerializeField]
    Animator animator = null;

    [SerializeField]
    TMP_Text scoreValue = null;

    [SerializeField]
    FadeOnCommand fadeOnCommand = null;

    [SerializeField]
    protected AudioSource flipSound = null;

    [SerializeField]
    protected AudioSource missSound = null;

    private string key = "";
    private float targetTime = 0f;
    private Scores.Scores score = Scores.Scores.F;

    public float ScoreModifier { get; set; }

    public virtual void instantiateInstance(float maxDuration, string key)
    {
      this.maxDuration = maxDuration;
      this.key = key;
      this.targetTime = (3 * maxDuration / 4f);
      if (keyValueText != null)
      {
        keyValueText.text = key;
      }
    }

    protected void Update()
    {
      if (!ShouldUpdate())
      {
        return;
      }

      if (Input.GetKeyDown(key))
      {
        flipSound.Play();
        resolved = true;
      }
    }

    protected override void missed()
    {
      missSound.Play();
    }

    protected override void updateTimerVisuals(float timeElapsed)
    {
      // Restrictions:
      // Needs to be '1' when timeElapsed == targetTime
      float collapsePercentage = Mathf.Clamp(targetTime / timeElapsed, 0, 4f);
      collapsingRing.rectTransform.localScale = Vector3.one * (collapsePercentage);

      //Fade the circle
      var tempColor = collapsingRing.color;
      tempColor.a = 0.8f * timeElapsed / maxDuration;
      collapsingRing.color = tempColor;

      tempColor = buttonImage.color;
      tempColor.a = 0.8f * timeElapsed / maxDuration;
      buttonImage.color = tempColor;
    }

    public override IEnumerator AnimateThenDestroySelf()
    {
      scoreValue.text = Scores.ScoreHelpers.ScoreToFlavorString(score);
      var newColor = Scores.ScoreHelpers.ScoreToColor(score);
      scoreValue.color = newColor;
      collapsingRing.color = newColor;

      if (score != Scores.Scores.F)
      {
        VFXImage.gameObject.SetActive(true);
        VFXImage.color = newColor;
      }


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
        case var a when (a <= .05):
          score = Scores.Scores.SS;
          break;
        case var a when (a <= .1):
          score = Scores.Scores.S;
          break;
        case var a when (a <= .2):
          score = Scores.Scores.A;
          break;
        case var a when (a <= .3):
          score = Scores.Scores.B;
          break;
        case var a when (a <= 4):
          score = Scores.Scores.C;
          break;
        case var a when (a <= 5):
          score = Scores.Scores.F;
          break;
      }

      return score;
    }
  }
}
