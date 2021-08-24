using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts
{
  public class TimedSpinner : TimedEvent
  {
    [SerializeField]
    Slider completionSlider = null;

    [SerializeField]
    Transform spinner = null;

    Vector2 previousMousePosition;

    float completionRotationAmount = 720f;

    private float totalRotationSoFar = 0f;

    public void instantiateSpinner(float completionRotationAmount, float timeLimit)
    {
      maxDuration = timeLimit;
      this.completionRotationAmount = completionRotationAmount;
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
          Debug.Log("Here!");
          UpdateSpinnerPosition();
        }
      }
    }

    private void UpdateSpinnerPosition()
    {
      //Calculates the vectors in relation to the center of the circle,
      //Then determines how much it should be rotated based the angle between
      Vector2 mousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
      Vector2 centerBallPosition = new Vector2(transform.position.x, transform.position.y);

      Vector2 oldMouseVector = previousMousePosition - centerBallPosition;
      Vector2 currentMouseVector = mousePosition - centerBallPosition;

      var angleOfRotation = Vector3.SignedAngle(oldMouseVector, currentMouseVector, Vector3.forward);

      spinner.Rotate(Vector3.forward, angleOfRotation);

      totalRotationSoFar += angleOfRotation;

      previousMousePosition = mousePosition;
    }

    public override IEnumerator AnimateThenDestroySelf()
    {
      Destroy(this.gameObject);
      yield return null;
    }

    public override int EvaluatePoints()
    {
      throw new NotImplementedException();
    }

    protected override void updateTimerVisuals(float timeElapsed)
    {
      completionSlider.value = Math.Abs(totalRotationSoFar) / (float)completionRotationAmount;
    }
  }
}
