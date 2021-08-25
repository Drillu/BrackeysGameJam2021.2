using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.TimedEvent
{
  public class TimedEventTextRenderer
  {
    [SerializeField]
    TMP_Text textField = null;

    public void UpdateText(Scores.Scores score)
    {
      textField.text = Scores.ScoreHelpers.ScoreToFlavorString(score);
    }
  }
}
