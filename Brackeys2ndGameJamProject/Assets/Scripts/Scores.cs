using UnityEngine;

namespace Scores
{
  public enum Scores
  {
    SS,
    S,
    A,
    B,
    C,
    F
  }

  public static class ScoreHelpers
  {
    // Simple helper to return what the user's current score should be based on points.
    public static Scores GetScoreForPercentageAmount(float percentage)
    {
      switch (percentage)
      {
        case var s when (s >= 1):
          return Scores.SS;
        case var s when (s > .95):
          return Scores.S;
        case var s when (s > .9):
          return Scores.A;
        case var s when (s > .8):
          return Scores.B;
        case var s when (s > .5):
          return Scores.C;
        default:
          return Scores.F;
      }
    }

    public static int ScoreToInt(Scores score)
    {
      switch (score)
      {
        case Scores.SS:
          return 300;
        case Scores.S:
          return 250;
        case Scores.A:
          return 200;
        case Scores.B:
          return 150;
        case Scores.C:
          return 100;
        case Scores.F:
          return 0;
      }

      Debug.LogWarning("No valid score int found!");
      return 0;
    }

    public static string ScoreToString(Scores score)
    {
      switch (score)
      {
        case Scores.SS:
          return "SS";
        case Scores.S:
          return "S";
        case Scores.A:
          return "A";
        case Scores.B:
          return "B";
        case Scores.C:
          return "C";
        case Scores.F:
          return "F";
      }

      Debug.LogWarning("No valid score string found!");
      return "";
    }
  }
}
