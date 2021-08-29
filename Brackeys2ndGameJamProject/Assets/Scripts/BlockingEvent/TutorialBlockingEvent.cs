using audio;
using blockingEvent;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace tutorialBlockingEvent
{
  public class TutorialBlockingEvent : MonoBehaviour, IBlockingEvent
  {
    [SerializeField]
    Animator animator = null;

    private readonly string OpenParameter = "Open";
    private readonly string ShowParameter = "Show";
    private bool continued = false;
    public IEnumerator RunEvent()
    {
      animator.SetTrigger(ShowParameter);
      while (!continued)
      {
        yield return null;
      }

      FindObjectOfType<AudioManager>().PlayGameMusic();
      animator.SetTrigger(OpenParameter);

      yield return new WaitForSeconds(2f);

    }

    public void Event_Continue()
    {
      continued = true;
    }
  }
}
