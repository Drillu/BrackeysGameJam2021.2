using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScorableAction
{
  public interface IScorableAction
  {
    //Assumedly this either has more parameters or the inherited members use class variables to return something.
    public Scores.Scores EvaluateScore();

    //Some actions will be worth less than others, and they can be altered point-wise here.
    public float ScoreModifier { get; set; }
  }
}
