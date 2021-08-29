using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace audio
{
  public class AudioManager : MonoBehaviour
  {
    [SerializeField]
    AudioSource menuMusic = null;

    [SerializeField]
    AudioSource gameMusic = null;

    private void Start()
    {
      DontDestroyOnLoad(this.gameObject);
    }

    public void PlayMenuMusic()
    {
      menuMusic.Play();
      gameMusic.Stop();
    }

    public void PlayGameMusic()
    {
      gameMusic.Play();
      menuMusic.Stop();
    }
  }
}
