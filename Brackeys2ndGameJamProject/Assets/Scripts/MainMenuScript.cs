using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public Animator animator;
    public float transTime = 1f;

    public void LoadNextLevel()
    {
        StartCoroutine(StartLoading(SceneManager.GetActiveScene().buildIndex +1));
    }

    public void MenuButton()
    {
        Time.timeScale = 1f;
        StartCoroutine(StartLoading(0));
    }

    IEnumerator StartLoading(int sceneBuildIndex)
    {
        animator.SetTrigger("Start");
        yield return new WaitForSeconds(transTime);
        SceneManager.LoadScene(sceneBuildIndex);
    }  
    
    public void QuitButton()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }
}
