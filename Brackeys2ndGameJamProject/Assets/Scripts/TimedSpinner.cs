using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
  public class TimedSpinner : MonoBehaviour, ITimedAction
  {
    float angleBetweenPoints(Vector2 position1, Vector2 position2)
    {
      Vector2 fromLine = position2 - position1;
      Vector2 toLine = new Vector2(1, 0);

      float angle = Vector2.Angle(fromLine, toLine);

      Vector3 cross = Vector3.Cross(fromLine, toLine);

      // did we wrap around?
      if (cross.z > 0)
      {
        angle = 360f - angle;
      }

      return angle;
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
