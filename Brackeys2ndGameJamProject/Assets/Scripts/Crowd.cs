using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scenes
{
  public class Crowd : MonoBehaviour
  {
    [SerializeField]
    Animator crowdAnimator;

    public void Start()
    {
      // FindObjectOfType is horrible performance wise and this would normally be handled by some
      // singleton management system but.. game jam so :shrug:
      var mainThread = FindObjectOfType<MainThread>();
    }

    public void Update()
    {
      //ScoreManager.GetScore();
      //Animator.SetBool()
    }
  }
}
