using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    

    [Header("Info")]
    public StoryScene currentScene;

    [Header("References")]
    public BottomBarController bottomBar;
    public SpriteSwitcher backgroundController;

    private bool greyScreenOn;

    private void Start()
    {
        bottomBar.PlayScene(currentScene);
        backgroundController.SetImage(currentScene.background);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            if (bottomBar.IsCompleted() && !bottomBar.pause)
            {

                if (bottomBar.IsLastSentence())
                {   
                    if (currentScene.nextScene == null)
                    {
                        SceneManager.LoadScene("Menu");
                    }
                    else
                    {

                        currentScene = currentScene.nextScene;
                        bottomBar.PlayScene(currentScene);

                        if (currentScene.perspective)
                        {
                            bottomBar.greyScreen.GetComponent<Animator>().SetTrigger("FadeIn");
                            greyScreenOn = true;
                        }
                        else if (!currentScene.perspective && greyScreenOn)
                        {
                            bottomBar.greyScreen.GetComponent<Animator>().SetTrigger("FadeOut");
                            greyScreenOn = false;
                        }

                        backgroundController.SwitchImage(currentScene.background);

                    }
                }
                else
                {
                    bottomBar.PlayNextSentence();
                }
            }
        }
        else
        {
            if (!bottomBar.isPlayingAnimation)
            {
                bottomBar.PlayAnimation();
            }
        }
    }

}

