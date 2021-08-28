using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using timedButton;
using UnityEngine;
using UnityEngine.EventSystems;

namespace timedClick
{
  public class TimedClickEvent : TimedButtonPress, IPointerClickHandler
  {
    public void OnPointerClick(PointerEventData eventData)
    {
      if (started)
      {
        Resolve();
      }
    }

    protected new void Update()
    {
    }
  }
}
