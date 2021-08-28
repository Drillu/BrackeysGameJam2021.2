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

    public static readonly int MaxScore = 300;
    public static int ScoreToInt(Scores score)
    {
      switch (score)
      {
        case Scores.SS:
          return MaxScore;
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

    public static string ScoreToFlavorString(Scores score)
    {
      switch (score)
      {
        case Scores.SS:
          return "Perfect!";
        case Scores.S:
          return "Great!";
        case Scores.A:
          return "Good";
        case Scores.B:
          return "Okay";
        case Scores.C:
          return "Oof";
        case Scores.F:
          return "Miss";
      }

      Debug.LogWarning("No valid score string found!");
      return "";
    }

    public static float ScoreToOverallPercentage(Scores score)
    {
      switch (score)
      {
        case Scores.SS:
          return .1f;
        case Scores.S:
          return .05f;
        case Scores.A:
          return 0f;
        case Scores.B:
          return -.05f;
        case Scores.C:
          return -.1f;
        case Scores.F:
          return -.15f;
      }

      return 0;
    }

    public static Color ScoreToColor(Scores score)
    {
      switch (score)
      {
        case Scores.SS:
          return new Color(.9f, .26f, .85f); //Purple
        case Scores.S:
          return new Color(.2f, .9f, .27f); //Green
        case Scores.A:
          return new Color(.38f, .38f, 1f); //Blue
        case Scores.B:
          return new Color(.86f, .81f, .39f); //Yellow
        case Scores.C:
          return new Color(.88f, .60f, .25f); //Orange
        case Scores.F:
          return new Color(.6f, .15f, .01f); //Red
        default:
          return new Color(1, 1, 1); //White
      }

    }
  }
}
