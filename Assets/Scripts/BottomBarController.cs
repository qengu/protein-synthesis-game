using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BottomBarController : MonoBehaviour
{

    [Header("References")]
    public TextMeshProUGUI barText;
    public TextMeshProUGUI personNameText;
    [SerializeField] private Animator drawingAnimator;
    [SerializeField] private Animator backgroundAniamtor;
    [SerializeField] private GameObject blackScreen;
    [SerializeField] public GameObject greyScreen;
    public Image animationImage;

    [Header("Info")]
    public StoryScene currentScene;
    [SerializeField] private float textDisplayDelay;
    public Dictionary<Speaker, SpriteController> sprites;
    public GameObject spritesPrefab;
  

    public int sentenceIndex = -1;
    public bool isPlayingAnimation = false;
    private State state = State.COMPLETED;
    private bool draw = false;
    [HideInInspector] public bool pause = false;
    
    private enum State
    {
        PLAYING, COMPLETED
    }

    private void Awake()
    {
        sprites = new Dictionary<Speaker, SpriteController>();
        blackScreen.GetComponent<Animator>().SetTrigger("FadeOut");
    }

    public void PlayScene(StoryScene scene)
    {
        currentScene = scene;
        sentenceIndex = -1;
        PlayNextSentence();
    }

    public void PlayNextSentence()
    {   

        StartCoroutine(TypeText(currentScene.sentences[++sentenceIndex].text));

        if (currentScene.sentences[sentenceIndex].italic)
        {
            barText.fontStyle = TMPro.FontStyles.Italic;
            barText.color = Color.gray;
        }
        else
        {
            barText.fontStyle = TMPro.FontStyles.Normal;
            barText.color = Color.white;
        }

        if (currentScene.sentences[sentenceIndex].blackScreenDisplay > 0f)
        {
            pause = true;
            StartCoroutine(BlackScreen(currentScene.sentences[sentenceIndex].blackScreenDisplay));
        }

        if (currentScene.sentences[sentenceIndex].animation)
        {
            if (!draw)
            {
                drawingAnimator.SetTrigger("Open");
                backgroundAniamtor.SetTrigger("Open");
                draw = true;
            }

            if (sentenceIndex + 1 <= currentScene.sentences.Count && !currentScene.sentences[sentenceIndex + 1].animation)
            {
                draw = false;
                drawingAnimator.SetTrigger("Close");
                backgroundAniamtor.SetTrigger("Close");
            }

        }

        personNameText.text = currentScene.sentences[sentenceIndex].speaker.speakerName;
        personNameText.color = currentScene.sentences[sentenceIndex].speaker.textColor;
        ActSpeakers();
    }

    public void PlayAnimation()
    {
        if (currentScene.sentences[sentenceIndex].animationTime > 0f && currentScene.sentences[sentenceIndex].displayImages.Count > 0)
        {
            isPlayingAnimation = true;
            StartCoroutine(DisplayImages(currentScene.sentences[sentenceIndex].displayImages, currentScene.sentences[sentenceIndex].animationTime));
        }
    }

    public bool IsCompleted()
    {
        return state == State.COMPLETED;
    }

    public bool IsLastSentence()
    {
        return sentenceIndex + 1 == currentScene.sentences.Count;
    }

    private IEnumerator BlackScreen(float time)
    {
        Animator anim = blackScreen.GetComponent<Animator>();
        anim.SetTrigger("FadeIn");
        yield return new WaitForSeconds(time);
        anim.SetTrigger("FadeOut");
        pause = false;
    }

    public IEnumerator DisplayImages(List<Sprite> images, float time)
    {
        for (int i = 0; i < images.Count; i++)
        {

            float aspectRatio = images[i].rect.width / images[i].rect.height;

            animationImage.sprite = images[i];
            animationImage.gameObject.GetComponent<AspectRatioFitter>().aspectRatio = aspectRatio;
            yield return new WaitForSeconds(time);
        }
        isPlayingAnimation = false;
    }

    private IEnumerator TypeText(string text)
    {
        barText.text = "";
        state = State.PLAYING;
        int wordIndex = 0;

        while (state != State.COMPLETED)
        {
            barText.text += text[wordIndex];
            yield return new WaitForSeconds(textDisplayDelay);
            if (++wordIndex == text.Length)
            {
                state = State.COMPLETED;
                break;
            }
        }

    }
    
    

    private void ActSpeakers()
    {
        List<StoryScene.Sentence.Action> actions = currentScene.sentences[sentenceIndex].actions;
        for (int i = 0; i < actions.Count; i++)
        {
            ActSpeaker(actions[i]);
        }
    }

    private void ActSpeaker(StoryScene.Sentence.Action action)
    {
        SpriteController controller = null;
        switch (action.actionType)
        {
            case StoryScene.Sentence.Action.Type.APPEAR:
                if (!sprites.ContainsKey(action.speaker))
                {
                    controller = Instantiate(action.speaker.prefab.gameObject, spritesPrefab.transform).GetComponent<SpriteController>();
                    sprites.Add(action.speaker, controller);
                }
                else
                {
                    controller = sprites[action.speaker];
                }
                controller.Setup(action.speaker.sprites[action.spriteIndex]);
                if (action.coords != Vector2.zero) controller.Show(action.coords);
                return;
            case StoryScene.Sentence.Action.Type.MOVE:
                break;
            case StoryScene.Sentence.Action.Type.DISAPPEAR:
                if (sprites.ContainsKey(action.speaker))
                {
                    controller = sprites[action.speaker];
                    controller.Hide();
                }
                return;
            case StoryScene.Sentence.Action.Type.SWITCH:
                if (sprites.ContainsKey(action.speaker))
                {
                    controller = sprites[action.speaker];
                }
                break;
        }

        if (controller != null)
        {
            controller.SwitchSprite(action.speaker.sprites[action.spriteIndex]);
        }

    }

}
