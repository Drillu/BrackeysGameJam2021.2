using blockingEvent;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace bannerBlock
{
  public class BannerBlockingEvent : MonoBehaviour, IBlockingEvent
  {
    private readonly string Intro = "Intro";
    private readonly string Outro = "Outro";

    [SerializeField]
    Animator animator = null;
    public IEnumerator RunEvent()
    {
      animator.SetTrigger(Intro);
      yield return new WaitForSeconds(3);
      animator.SetTrigger(Outro);
      yield return new WaitForSeconds(2f);
    }
  }
}
