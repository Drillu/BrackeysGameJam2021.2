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
    TMP_Text buttonText = null;

    [SerializeField]
    GameObject tutorial = null;

    [SerializeField]
    GameObject gameEnd = null;

    private bool continued = false;
    private bool isTutorial = false;

    public void instantiateCurtainEvent(bool isTutorial)
    {
      this.isTutorial = isTutorial;
      tutorial.SetActive(isTutorial);
      gameEnd.SetActive(!isTutorial);
      continued = false;
    }

    public IEnumerator RunEvent()
    {
      if (!isTutorial)
      {
        animator.SetTrigger(CloseParameter);
      }
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
