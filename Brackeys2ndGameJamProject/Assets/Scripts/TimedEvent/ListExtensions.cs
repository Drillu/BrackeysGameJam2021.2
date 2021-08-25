using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace listExtensions
{ 
    static class ListExtension
    {
      public static T PopAt<T>(this List<T> list, int index)
      {
        T r = list[index];
        list.RemoveAt(index);
        return r;
      }
    }
}
