using Assets.Scenes;
using audio;
using performanceBulbs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace blockingEvent
{
  public class CurtainBlockingEvent : MonoBehaviour, IBlockingEvent
  {
    private readonly string OpenParameter = "Open";
    private readonly string CloseParameter = "Close";

    [SerializeField]
    Animator animator = null;

    private bool continued = false;

    public void instantiateCurtainEvent()
    {
      continued = false;
    }

    public IEnumerator RunEvent()
    {
      animator.SetTrigger(CloseParameter);
      while (!continued)
      {
        yield return null;
      }

      animator.SetTrigger(OpenParameter);
      FindObjectOfType<AudioManager>().PlayGameMusic();
      FindObjectOfType<PerformanceBulbs>().SetCurrentPerformancePercentage(1);
      yield return new WaitForSeconds(2f);
    }

    public void Event_Continue()
    {
      continued = true;
    }
  }
}
