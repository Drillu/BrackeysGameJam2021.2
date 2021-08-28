using fadeOnCommand;
using ScorableAction;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using timedEvent;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace timedSpinner
{
  public class TimedSpinner : TimedEvent, IScorableAction
  {
    [SerializeField]
    Slider completionSlider = null;

    [SerializeField]
    Image collapsingRing = null;

    [SerializeField]
    Transform spinner = null;

    [SerializeField]
    FadeOnCommand fadeOnCommand = null;

    [SerializeField]
    TMP_Text scoreValue = null;

    [SerializeField]
    Image VFXImage = null;

    [SerializeField]
    Animator animator = null;


    Vector2 previousMousePosition;

    float completionRotationAmount = 720f;

    private float totalRotationSoFar = 0f;

    Scores.Scores score = Scores.Scores.F;

    RectTransform rectTransform;

    public float ScoreModifier { get; set; }

    public void instantiateSpinner(float completionRotationAmount, float timeLimit)
    {
      maxDuration = timeLimit;
      this.completionRotationAmount = completionRotationAmount;
      rectTransform = GetComponent<RectTransform>();
    }

    protected void Update()
    {
      if (!base.ShouldUpdate())
      {
        return;
      }

      if (Math.Abs(totalRotationSoFar) > completionRotationAmount)
      {
        Resolve();
        return;
      }

      if (!Input.GetMouseButton(0))
      {
        return;
      }

      //This is horribly expensive but its a game jam so :shrug:
      PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
      pointerEventData.position = Input.mousePosition;

      List<RaycastResult> raycastResultsList = new List<RaycastResult>();
      EventSystem.current.RaycastAll(pointerEventData, raycastResultsList);

      foreach (var raycast in raycastResultsList)
      {
        if (raycast.gameObject.GetComponent<TimedSpinner>() == this)
        {
          UpdateSpinnerPosition();
        }
      }
    }

    private void UpdateSpinnerPosition()
    {
      //Calculates the vectors in relation to the center of the circle,
      //Then determines how much it should be rotated based the angle between
      Vector2 mousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
      Debug.Log(mousePosition);
      Vector3 pointToTravel = Camera.main.WorldToScreenPoint(transform.position);
      Vector2 centerBallPosition = new Vector2(pointToTravel.x, pointToTravel.y);
      Debug.Log(centerBallPosition);

      Vector2 oldMouseVector = previousMousePosition - centerBallPosition;
      Vector2 currentMouseVector = mousePosition - centerBallPosition;

      var angleOfRotation = Vector3.SignedAngle(oldMouseVector, currentMouseVector, Vector3.forward);

      spinner.Rotate(Vector3.forward, angleOfRotation);

      totalRotationSoFar += angleOfRotation;

      previousMousePosition = mousePosition;
    }

    public override IEnumerator AnimateThenDestroySelf()
    {
      animator.SetBool("Fade", true);
      yield return new WaitForSeconds(.5f);
      Destroy(this.gameObject);
      yield return null;
    }

    public override Scores.Scores EvaluateScore()
    {   
      // SS if the spin was completed
      score = Scores.ScoreHelpers.GetScoreForPercentageAmount(Math.Abs(totalRotationSoFar) / (float)completionRotationAmount);
      scoreValue.text = Scores.ScoreHelpers.ScoreToFlavorString(score);
      var newColor = Scores.ScoreHelpers.ScoreToColor(score);
      scoreValue.color = newColor;
      collapsingRing.color = newColor;

      if (score != Scores.Scores.F)
      {
        VFXImage.gameObject.SetActive(true);
        VFXImage.color = newColor;
      }

      return score;
    }

    protected override void updateTimerVisuals(float timeElapsed)
    {
      completionSlider.value = Math.Abs(totalRotationSoFar) / (float)completionRotationAmount;
      float collapsePercentage = Mathf.Clamp(maxDuration / timeElapsed, 0, 4f);
      collapsingRing.rectTransform.localScale = Vector3.one * (collapsePercentage);

      //Fade the circle
      var tempColor = collapsingRing.color;
      tempColor.a = 0.8f * timeElapsed / maxDuration;
      collapsingRing.color = tempColor;
    }
  }
}
