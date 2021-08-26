using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace blockingEvent
{
  public interface IBlockingEvent
  {
    public IEnumerator RunEvent();
  }
}
