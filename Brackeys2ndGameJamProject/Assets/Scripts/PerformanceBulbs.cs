using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace performanceBulbs
{
  public class PerformanceBulbs : MonoBehaviour
  {
    [SerializeField]
    Animator[] animators;

    public void SetCurrentPerformancePercentage(float percentage)
    {
      for (int i = 0; i < animators.Length; i ++)
      {
        var isOn = i < percentage * 10;
        animators[i].SetBool("On", isOn);
      }
    }
  }
}
