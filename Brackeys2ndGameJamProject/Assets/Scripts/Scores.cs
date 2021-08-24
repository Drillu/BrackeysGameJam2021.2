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
  }

  public static class ScoreHelpers
  {
    // Simple helper to return what the user's current score should be based on points.
    public static Scores GetScoreForPointAmount(int points, int maxPoints)
    {
      var percentage = points / (float) maxPoints;

      switch (percentage)
      {
        case var s when (s == 1):
          return Scores.C;
        case var s when (s > .95):
          return Scores.S;
        case var s when (s > .9):
          return Scores.A;
        case var s when (s > .8):
          return Scores.B;
        default:
          return Scores.C;
      }
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
      }

      Debug.LogWarning("No valid score string found!");
      return "";
    }
  }
}
