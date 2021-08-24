using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts
{
  public class TimedSpinner : MonoBehaviour, ITimedAction
  {
    Vector2 previousMousePosition;

    void Update()
    {
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
      Vector2 mousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
      Vector2 centerBallPosition = new Vector2(transform.position.x, transform.position.y);

      Vector2 oldMouseVector = previousMousePosition - centerBallPosition;
      Vector2 currentMouseVector = mousePosition - centerBallPosition;

      var angleOfRotation = Vector3.SignedAngle(oldMouseVector, currentMouseVector, Vector3.forward);

      transform.Rotate(Vector3.forward, angleOfRotation);

      previousMousePosition = mousePosition;
    }
    public IEnumerator StartAction()
    {
      throw new NotImplementedException();
    }

    public IEnumerator AnimateThenDestroySelf()
    {
      throw new NotImplementedException();
    }

    public void Resolve(ResolveState resolveState)
    {
      throw new NotImplementedException();
    }

    public int EvaluatePoints()
    {
      throw new NotImplementedException();
    }
  }
}
