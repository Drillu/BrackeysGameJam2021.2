using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
  public abstract class TimedEvent : MonoBehaviour, ITimedAction
  {
    public bool Started => started;
    protected bool started = false;

    public bool Resolved => resolved;
    protected bool resolved = false;

    protected float maxDuration = 5f;

    public abstract IEnumerator AnimateThenDestroySelf();

    public abstract int EvaluatePoints();

    public void Resolve()
    {
      resolved = true;
    }

    protected virtual bool ShouldUpdate()
    { 
      if (!started || resolved)
      {
        return false;
      }

      return true;
    }


    public virtual void StartAction()
    {
      started = true;
      StartCoroutine(tickDown());
    }

    protected abstract void updateTimerVisuals(float timeElapsed);

    public virtual IEnumerator tickDown()
    {
      float timeElapsed = 0f;
      while (timeElapsed < maxDuration && !Resolved)
      {
        updateTimerVisuals(timeElapsed);
        timeElapsed += Time.deltaTime;
        yield return null;
      }

      resolved = true;
    }
  }
}
