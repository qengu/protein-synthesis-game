using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{

    [SerializeField] private GameObject blackScreen;

    public string gameScene;

    private void Awake()
    {
        blackScreen.GetComponent<Image>().color = Color.clear;
    }

    public void StartGame()
    {
        if (blackScreen != null)
        {
            blackScreen.GetComponent<Animator>().SetTrigger("FadeIn");
        }
        StartCoroutine(WaitToLoad());
    }

    public void Quit()
    {
        Application.Quit();
    }

    private IEnumerator WaitToLoad()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(gameScene, LoadSceneMode.Single);
    }
    

}
