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

    [SerializeField]
    TMP_Text levelText = null;

    private bool continued = false;

    public void instantiateCurtainEvent()
    {
      continued = false;
    }

    public IEnumerator RunEvent()
    {
      while (!continued)
      {
        yield return null;
      }
      animator.SetTrigger(OpenParameter);
      yield return new WaitForSeconds(2f);

    }

    public void Event_Continue()
    {
      continued = true;
    }
  }
}
