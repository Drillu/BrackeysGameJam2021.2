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
    [Serializable]
    private struct CrowdReactions
    {
      public Scores.Scores score;
      public GameObject gameObject;
    }

    [SerializeField]
    List<CrowdReactions> reactions = new List<CrowdReactions>();

    public void SetCrowdReaction(Scores.Scores score)
    {
      foreach (var reaction in reactions)
      {
        reaction.gameObject.SetActive(score == reaction.score);
      }
    }
  }
}
