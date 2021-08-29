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

    [SerializeField]
    TMP_Text scoreText = null;

    private bool continued = false;
    private float score = 0f;

    public void instantiateCurtainEvent(float score)
    {
      this.score = score;
      scoreText.text = score.ToString();
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
      FindObjectOfType<MainThread>().ResetState();
      yield return new WaitForSeconds(3f);
    }

    public void Event_Continue()
    {
      continued = true;
    }
  }
}
