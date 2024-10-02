using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewStoryScene", menuName = "Data/New Story Scene")]
[System.Serializable]
public class StoryScene : ScriptableObject
{
    public List<Sentence> sentences;
    public Sprite background;
    public StoryScene nextScene;
    public bool perspective;

    [System.Serializable]
    public struct Sentence
    {   
        [Header("Data")]
        [Space()]
        public Speaker speaker;
        public string text;
        public List<Action> actions;

        [Header("Animation")]
        [Space]
        public bool animation;
        public float animationTime;
        public List<Sprite> displayImages;

        [Header("Modifiers")]
        public bool italic;
        public float blackScreenDisplay;

        [System.Serializable]
        public struct Action
        {
            public Speaker speaker;
            public int spriteIndex;
            public Vector2 coords;
            public float moveSpeed;
            public Type actionType;

            [System.Serializable]
            public enum Type
            {
                SWITCH, APPEAR, MOVE, DISAPPEAR
            }
        }

    }

}
