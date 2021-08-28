using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using timedClick;
using UnityEngine;

namespace Assets.Scripts
{
  public class InputManager : MonoBehaviour
  {
    private void Update()
    { 
      if (Input.GetMouseButtonDown(0))
      {
        RaycastHit raycastHit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out raycastHit, 100f))
        {
          var timedClickEvent = raycastHit.transform.gameObject.GetComponent<TimedClickEvent>();
          if (timedClickEvent != null && timedClickEvent.Started == true)
          {
            timedClickEvent.Resolve();
          }
        }
      }
    }
  }
}
