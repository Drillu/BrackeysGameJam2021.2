using System.Collections;
using UnityEngine;

namespace timedEvent
{
  public abstract class TimedEvent : MonoBehaviour, ITimedAction
  {
    public bool Started => started;
    protected bool started = false;

    public bool Resolved => resolved;
    protected bool resolved = false;

    public float StartTime { get; set; }

    protected float maxDuration = 5f;

    protected float timeElapsed = 0f;
    public abstract IEnumerator AnimateThenDestroySelf();

    public abstract Scores.Scores EvaluateScore();

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
      gameObject.SetActive(true);
      StartCoroutine(tickDown());
    }

    protected abstract void updateTimerVisuals(float timeElapsed);
    protected virtual void missed() { }

    public virtual IEnumerator tickDown()
    {
      while (timeElapsed < maxDuration && !Resolved)
      {
        updateTimerVisuals(timeElapsed);
        timeElapsed += Time.deltaTime;
        yield return null;
      }
      updateTimerVisuals(timeElapsed);
      if (timeElapsed >= maxDuration)
      {
        missed();
      }

      resolved = true;
    }
  }
}
