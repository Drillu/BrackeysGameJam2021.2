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

    public void instantiateCurtainEvent(int level, bool hintKeys, bool hintSpinner)
    {
      levelText.text = "Level " + level.ToString();
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
      yield return new WaitForSeconds(2f);

    }

    private IEnumerator StartCountdown()
    {
      yield return null;
    }

    public void Event_Continue()
    {
      continued = true;
    }
  }
}
