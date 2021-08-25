using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace fadeOnCommand
{
  public class FadeOnCommand : MonoBehaviour
  {
    [SerializeField]
    Image[] imagesToFade;

    [SerializeField]
    TMP_Text[] textToFade;

    public IEnumerator FadeElements(float timeToFade)
    {
      var cachedTime = 0f;
      while (cachedTime < timeToFade)
      {
        cachedTime += Time.deltaTime;

        foreach (var image in imagesToFade)
        {
          image.color = GetColorAlphaForPercentage(image.color, cachedTime / timeToFade);
        }
        foreach (var text in textToFade)
        {
          text.color = GetColorAlphaForPercentage(text.color, cachedTime / timeToFade);
        }

        cachedTime += Time.deltaTime;
        yield return null;
      }
    }

    private static Color GetColorAlphaForPercentage(Color color, float percentage)
    {
      return new Color(color.r, color.g, color.b, 1 - percentage);
    }
  }
}
