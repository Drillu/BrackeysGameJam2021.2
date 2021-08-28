using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace penguinRenderer
{
  public class PenguinRenderer : MonoBehaviour
  {
    [SerializeField]
    List<GameObject> penguins = new List<GameObject>();

    public void SetPenguin(int penguinNumber)
    {
      for (int i = 0; i < penguins.Count; i ++)
      {
        if (i == penguinNumber)
        {
          penguins[i].SetActive(true);
        }
        else
        {
          penguins[i].SetActive(false);
        }

      }
    }
  }
}
