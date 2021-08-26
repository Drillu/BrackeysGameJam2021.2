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

    MainThread mainThread = null;
    int cachedVersion = -1;

    public void Start()
    {
      // FindObjectOfType is horrible performance wise and this would normally be handled by some
      // singleton management system but.. game jam so :shrug:
      mainThread = FindObjectOfType<MainThread>();
      cachedVersion = mainThread.ScoreVersion;
    }

    public void Update()
    {
      if (mainThread.ScoreVersion != cachedVersion)
      {
        // Set it to something here, not sure what the states will be but 
        //crowdAnimator.SetFloat()
      }
    }
  }
}
